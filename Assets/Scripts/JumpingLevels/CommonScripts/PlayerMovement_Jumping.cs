using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement_Jumping : MonoBehaviour
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
    [SerializeField] string levelType;
    [SerializeField] LayerMask groundLayer;
    string currDir = "Left";

    // Private Variables
    Transform player;
    Rigidbody rb;
    Animator anim;
    Vector3 startTouchPos;
    Vector3 endTouchPos;
    bool jumpAllowed = true;
    bool swipeDetected = false;
    bool swipeAllowed = false;
    float dir = 0f;
    float dirVal = 0f;
    bool onceCalled = false;

    // Events
    public static event Action<QuestType, int> questUpdater;

    void Start(){
        if(levelType == "Jungle") dirVal = 4f;
        else dirVal = 5f;
        rb = GetComponent<Rigidbody>();
        player = transform.GetChild(transform.childCount - 2);
        anim = player.GetComponent<Animator>();
    }

    void OnEnable()
    {
        GameManager_Jumping.playerMovement += ToggleScript;
        GameManager_Jumping.updatePlayerMove += UpdatePlayer;
    }

    void OnDisable()
    {
        GameManager_Jumping.playerMovement -= ToggleScript;
        GameManager_Jumping.updatePlayerMove -= UpdatePlayer;
    }

    void ToggleScript(bool enable, string a)
    {
        this.enabled = enable;
        currDir = a;
    }

    void UpdatePlayer(GameObject a, Animator b)
    {
        player = a.transform;
        anim = b;
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawCube(checkForGround.position, new Vector3(width, height, length));
    }

    void Update(){
        if(!GameManager_Jumping.Instance.startGame || GameManager_Jumping.Instance.playerDead || GameManager_Jumping.Instance.paused) return;

        HandleGroundCheck();
        HandleFallCheck();
        HandleTouchInput();
    }

    void HandleGroundCheck(){
        Collider[] colliders = Physics.OverlapBox(checkForGround.position, new Vector3(width, height, length), Quaternion.identity, groundLayer);
        jumpAllowed = colliders.Length > 0;
        anim.SetBool("isFalling", !jumpAllowed);
    }

    void HandleFallCheck(){
        if (transform.position.y < -0.7f && !onceCalled)
        {
            SoundManager_Jumping.Instance.PlayLoseSound("");
            onceCalled = true;
        }

        if (transform.position.y < -5f)
        {
            GameManager_Jumping.Instance.EndGame();
        }
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
            if (GameManager_Jumping.Instance.isInverted)
            {
                dir = dir == "Right" ? "Left" : "Right";
            }
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
                SoundManager_Jumping.Instance.PlayJumpingSound();
                StartCoroutine(Jump());
            }
        }
    }

    void CheckSwipe(string dirToMove){
        if(currDir == "Right"){
            if(dirToMove == "Left"){
                dir = 0f;  
                currDir = "Centre";
                swipeAllowed = true;
            }
        }
        else if(currDir == "Left"){
            if(dirToMove == "Right"){
                dir = 0f;  
                currDir = "Centre";
                swipeAllowed = true;
            }
        }
        else if(currDir == "Centre"){
            if(dirToMove == "Right"){
                dir = dirVal;  
                currDir = "Right";
                swipeAllowed = true;
            }
            else if(dirToMove == "Left"){
                dir = -dirVal;  
                currDir = "Left";
                swipeAllowed = true;
            }
        }
    }

    IEnumerator HorizontalMovement(string direction){
        rb.useGravity = true;

        Quaternion playerRotations = player.rotation;
        if(direction == "Right") playerRotations = Quaternion.Euler(0f, 20f, 0f);
        else playerRotations = Quaternion.Euler(0f, -20f, 0f);
        player.rotation = playerRotations;

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

    IEnumerator Jump(){
        rb.useGravity = false;
        Quaternion playerRotations = player.rotation;
        playerRotations = Quaternion.Euler(0f, 0f, 0f);
        player.rotation = playerRotations;
        anim.Play("Jump", 0, 0f);
        questUpdater?.Invoke(QuestType.Jumps, 1);

        Vector3 target = new Vector3(transform.position.x, transform.position.y + yDis, transform.position.z + zDis);
        while(Mathf.Abs(transform.position.z - target.z) > 0.1f){
            Vector3 moveVector = (target.z - transform.position.z) * Vector3.up * jumpSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveVector);
            yield return null;
        }

        rb.useGravity = true;
        anim.SetBool("isFalling", true);
        anim.SetTrigger("OnGround");
    }
}
