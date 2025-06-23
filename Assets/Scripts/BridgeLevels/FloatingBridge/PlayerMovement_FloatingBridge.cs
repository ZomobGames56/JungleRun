using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement_FloatingBridge : MonoBehaviour
{
    // Movement
    [Header("Movement")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float yDis;
    [SerializeField] float zDis;
    [SerializeField] float horizontalSpeed;

    // Ground Check
    [Header("Ground Check")]
    [SerializeField] Transform checkForGround;
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] float length;

    // Inputs
    [Header("Inputs")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float idleThreshold;
    [SerializeField] float idleTimeAllowed;
    [SerializeField] Animator gliderAnim;

    // Private Variables
    Transform player;
    Rigidbody rb;
    Animator anim;
    float lastPos;
    Vector3 startTouchPos;
    Vector3 endTouchPos;
    bool jumpAllowed = true;
    bool swipeDetected = false;
    bool swipeAllowed = false;
    public string currDir = "Right";
    float dir = 0f;
    float idleTimer;
    float distanceCheckInterval = 1f;
    float intervalTimer = 0f;
    float intervalDistance = 0f;
    bool onceCalled = false;

    // Events
    public static event Action<QuestType, int> questUpdater;

    void OnEnable()
    {
        GameManager_BridgeLevel.playerMovementFloatingBridge += ToggleScript;
        GameManager_BridgeLevel.updatePlayerMoveFloatingBridge += UpdatePlayer;
    }

    void OnDisable()
    {
        GameManager_BridgeLevel.playerMovementFloatingBridge -= ToggleScript;
        GameManager_BridgeLevel.updatePlayerMoveFloatingBridge -= UpdatePlayer;
    }

    void ToggleScript(bool enable, string a)
    {
        this.enabled = enable;
        Debug.Log(enable);
        currDir = a;
    }

    void UpdatePlayer(GameObject a, Animator b)
    {
        player = a.transform;
        anim = b;
    }

    void Start(){
        rb = GetComponent<Rigidbody>();
        player = transform.GetChild(transform.childCount - 2);
        anim = player.GetComponent<Animator>();
        lastPos = transform.position.z;
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawCube(checkForGround.position, new Vector3(width, height, length));
    }

    void Update(){
        if(!GameManager_BridgeLevel.Instance.startGame || GameManager_BridgeLevel.Instance.playerDead || GameManager_BridgeLevel.Instance.paused) return;

        HandleGroundCheck();
        HandleFallCheck();
        HandleTouchInput();
        // HandleIdleDetection();
    }

    void HandleGroundCheck(){
        Collider[] colliders = Physics.OverlapBox(checkForGround.position, new Vector3(width, height, length), Quaternion.identity, groundLayer);
        jumpAllowed = colliders.Length > 0;
        anim.SetBool("isFalling", !jumpAllowed);
    }

    void HandleFallCheck(){
        if (transform.position.y < -5f)
        {
            GameManager_BridgeLevel.Instance.EndGame();
        }
        else if (transform.position.y < 0f)
        {
            anim.Play("Falling");
            if (!onceCalled)
            {
                SoundManager_BridgeLevel.Instance.PlayLoseSound("");
                onceCalled = true;
            }
        }
    }

    void HandleIdleDetection(){
        float currPos = transform.position.z;
        float movedDistance = Mathf.Abs(currPos - lastPos);
        intervalDistance += movedDistance;
        intervalTimer += Time.unscaledDeltaTime;
        if (intervalTimer >= distanceCheckInterval)
        {
            if (intervalDistance < idleThreshold)
            {
                idleTimer += intervalTimer;
                if (idleTimer >= idleTimeAllowed)
                {
                    GameManager_BridgeLevel.Instance.EndGame();
                }
            }
            else
            {
                idleTimer = 0f;
            }
            intervalTimer = 0f;
            intervalDistance = 0f;
        }
        lastPos = currPos;
    }

    void HandleTouchInput(){
        if(Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began){
                startTouchPos = touch.position;
            }

            if(touch.phase == TouchPhase.Moved && !swipeDetected){
                DetectSwipe(touch.position);
            }

            if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled){
                swipeDetected = false;
            }
            // switch(touch.phase){
            //     case TouchPhase.Began:
            //         break;

            //     case TouchPhase.Moved:
            //         if(!swipeDetected) DetectSwipe(touch.position);
            //         break;

            //     case TouchPhase.Ended:
            //     case TouchPhase.Canceled:
            //         swipeDetected = false;
            //         break;
            // }
        }
    }

    void DetectSwipe(Vector2 currentTouch){
        endTouchPos = currentTouch;
        Vector2 swipePos = endTouchPos - startTouchPos;

        float horizThreshold = Screen.width * 0.015f;
        float vertiThreshold = Screen.height * 0.005f;

        if(Mathf.Abs(swipePos.x) > horizThreshold && Mathf.Abs(swipePos.x) > Mathf.Abs(swipePos.y)){
            swipeDetected = true;
            string dir = swipePos.x > 0 ? "Right" : "Left";
            CheckSwipe(dir);
            if(swipeAllowed){
                StopAllCoroutines();
                StartCoroutine(HorizontalMovement(dir));
            }
        }
        else if(Mathf.Abs(swipePos.y) > vertiThreshold && Mathf.Abs(swipePos.y) > Mathf.Abs(swipePos.x)){
            swipeDetected = true;
            if(swipePos.y > 0 && jumpAllowed){
                StopAllCoroutines();
                if (!GameManager_BridgeLevel.Instance.gliderCollected)
                {
                    SoundManager_BridgeLevel.Instance.PlayJumpingSound();
                    StartCoroutine(Jump());
                }
                else if (GameManager_BridgeLevel.Instance.gliderCollected && !GameManager_BridgeLevel.Instance.startGlide)
                {
                    StartCoroutine(Jump());
                }
            }
            else if(swipePos.y < 0){
                if (!GameManager_BridgeLevel.Instance.gliderCollected)
                {
                    StartCoroutine(Slide());
                }
                else if (GameManager_BridgeLevel.Instance.gliderCollected && !GameManager_BridgeLevel.Instance.startGlide)
                {
                    StartCoroutine(Slide());
                }
            }
        }
    }

    void CheckSwipe(string direction){
        if(currDir == "Right"){
            if(direction == "Left"){
                dir = -0.8f;  
                currDir = "Left";
                swipeAllowed = true;
            }
        }
        else if(currDir == "Left"){
            if(direction == "Right"){
                dir = 0.8f;  
                currDir = "Right";
                swipeAllowed = true;
            }
        }
    }

    IEnumerator HorizontalMovement(string direction){
        CapsuleCollider col = transform.GetComponent<CapsuleCollider>();
        col.center = new Vector3(col.center.x, 0.9765382f, col.center.z);
        col.height = 1.9595f;
        if (!GameManager_BridgeLevel.Instance.gliderCollected)
        {
            rb.useGravity = true;
        }
        else if (GameManager_BridgeLevel.Instance.gliderCollected && !GameManager_BridgeLevel.Instance.startGlide)
        {
            rb.useGravity = true;
        }

        Quaternion playerRotations = player.rotation;
        if(GameManager_BridgeLevel.Instance.startGlide){
            anim.SetTrigger("Glide" + direction);
            gliderAnim.SetTrigger(direction);
        }
        else{
            if(direction == "Right") playerRotations = Quaternion.Euler(0f, 20f, 0f);
            else playerRotations = Quaternion.Euler(0f, -20f, 0f);
            player.rotation = playerRotations;
        }

        while(Mathf.Abs(transform.position.x - dir) > 0.05f){
            Vector3 moveVector = (dir - transform.position.x) * Vector3.right * horizontalSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveVector); 
            yield return null;
        }

        playerRotations = Quaternion.Euler(0f, 0f, 0f);
        player.rotation = playerRotations;

        anim.SetTrigger("OnGround");
        swipeAllowed = false;
    }

    IEnumerator Jump()
    {
        CapsuleCollider col = transform.GetComponent<CapsuleCollider>();
        col.center = new Vector3(col.center.x, 0.9765382f, col.center.z);
        col.height = 1.9595f;
        rb.useGravity = false;
        Quaternion playerRotations = player.rotation;
        playerRotations = Quaternion.Euler(0f, 0f, 0f);
        player.rotation = playerRotations;
        anim.Play("JumpRoll", 0, 0f);
        questUpdater?.Invoke(QuestType.Jumps, 1);

        // float zToUse = zDis/GameManager_BridgeLevel.Instance.speedModifier;
        Vector3 target = new Vector3(transform.position.x, transform.position.y + yDis, transform.position.z + zDis);
        while (Mathf.Abs(transform.position.z - target.z) > 0.1f)
        {
            Vector3 moveVector = (target.z - transform.position.z) * Vector3.up * jumpSpeed * GameManager_BridgeLevel.Instance.speedModifier * Time.deltaTime;
            rb.MovePosition(rb.position + moveVector);
            yield return null;
        }

        anim.SetBool("isFalling", true);
        rb.useGravity = true;
    }

    IEnumerator Slide(){
        if (!GameManager_BridgeLevel.Instance.gliderCollected)
        {
            rb.useGravity = true;
        }
        else if (GameManager_BridgeLevel.Instance.gliderCollected && !GameManager_BridgeLevel.Instance.startGlide)
        {
            rb.useGravity = true;
        }
        anim.SetTrigger("Slide");
        questUpdater?.Invoke(QuestType.Slide, 1);
        CapsuleCollider col = transform.GetComponent<CapsuleCollider>();
        col.center = new Vector3(col.center.x, 0.5770407f, col.center.z);
        col.height = 1.160505f;
        yield return new WaitForSeconds(0.75f);
        col.center = new Vector3(col.center.x, 0.9765382f, col.center.z);
        col.height = 1.9595f;
    }
}
