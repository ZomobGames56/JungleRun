using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial2_Jumping_Prototype : MonoBehaviour
{
    // TUTORIAL 2

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
    [SerializeField] float dir;

    // Tutorial Essentials
    [Header("Tutorial Essentials")]
    [SerializeField] GameObject fingerObj;
    [SerializeField] Animator fingerAnim;
    ForwardMovement_Jumping_Prototype forwardMovement_Jumping_Prototype;
    PlayerMovement_Jumping_Prototype playerMovement_Jumping_Prototype;

    // Private Variables
    Transform playerParent;
    Transform player;
    Rigidbody rb;
    Animator anim;
    Vector3 startTouchPos;
    Vector3 endTouchPos;
    bool jumpAllowed = true;
    bool swipeDetected = false;
    bool rightAllowed = false;
    public bool done = false;

    public void UpTutorial()
    {
        playerParent = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerParent.GetChild(0);
        rb =  playerParent.GetComponent<Rigidbody>();
        anim = player.GetComponent<Animator>();
        forwardMovement_Jumping_Prototype = playerParent.GetComponent<ForwardMovement_Jumping_Prototype>();
        playerMovement_Jumping_Prototype = playerParent.GetComponent<PlayerMovement_Jumping_Prototype>();
        fingerObj.SetActive(true);
        fingerAnim.Play("Up");
        forwardMovement_Jumping_Prototype.enabled = false;
    }

    void RightTutorial(){
        fingerObj.SetActive(true);
        fingerAnim.Play("Right");
    }

    void Update()
    {
        HandleGroundCheck();
        HandleTouchInput();
    }

    void HandleGroundCheck(){
        Collider[] colliders = Physics.OverlapBox(checkForGround.position, new Vector3(width, height, length), Quaternion.identity, groundLayer);
        jumpAllowed = colliders.Length > 0;
        anim.SetBool("isFalling", !jumpAllowed);
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPos = touch.position;
            }

            if (touch.phase == TouchPhase.Moved && !swipeDetected)
            {
                DetectSwipe(touch.position);
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                swipeDetected = false;
            }
        }
    }

    void DetectSwipe(Vector2 currentTouch)
    {
        endTouchPos = currentTouch;
        Vector2 swipePos = endTouchPos - startTouchPos;

        float horizThreshold = Screen.width * 0.015f;
        float vertiThreshold = Screen.height * 0.005f;

        if (Mathf.Abs(swipePos.x) > horizThreshold && Mathf.Abs(swipePos.x) > Mathf.Abs(swipePos.y))
        {
            swipeDetected = true;
            if (swipePos.x > 0)
            {
                if (rightAllowed)
                {
                    StopAllCoroutines();
                    forwardMovement_Jumping_Prototype.enabled = true;
                    Time.timeScale = 1f;
                    fingerObj.SetActive(false);
                    rightAllowed = false;
                    StartCoroutine(HorizontalMovement("Right"));
                    done = true;
                }
            }
        }
        else if (Mathf.Abs(swipePos.y) > vertiThreshold && Mathf.Abs(swipePos.y) > Mathf.Abs(swipePos.x) && jumpAllowed)
        {
            swipeDetected = true;
            StopAllCoroutines();
            rightAllowed = true;
            StartCoroutine(Jump());
            fingerObj.SetActive(false);
            Invoke("RightTutorial", 0.5f);
        }
    }

    IEnumerator HorizontalMovement(string direction){
        rb.useGravity = true;

        Quaternion playerRotations = player.rotation;
        playerRotations = Quaternion.Euler(0f, 20f, 0f);
        player.rotation = playerRotations;

        while(Mathf.Abs(playerParent.position.x - dir) > 0.05f){
            Vector3 moveVector = (dir - playerParent.position.x) * Vector3.right * horizontalSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveVector); 
            yield return null;
        }

        playerRotations = Quaternion.Euler(0f, 0f, 0f);
        player.rotation = playerRotations;

        anim.SetTrigger("OnGround");
    }

    IEnumerator Jump(){
        forwardMovement_Jumping_Prototype.enabled = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        Quaternion playerRotations = player.rotation;
        playerRotations = Quaternion.Euler(0f, 0f, 0f);
        player.rotation = playerRotations;
        anim.Play("JumpRoll", 0, 0f);
        Time.timeScale = 0.5f;

        Vector3 target = new Vector3(playerParent.position.x, playerParent.position.y + yDis, playerParent.position.z + zDis);
        while(Mathf.Abs(playerParent.position.z - target.z) > 0.5f){
            Vector3 moveVector = (target.z - playerParent.position.z) * Vector3.up * jumpSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveVector);
            yield return null;
        }

        anim.SetBool("isFalling", true);
        anim.SetTrigger("OnGround");
        rb.useGravity = true;
    }
}
