using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tutorial1_Jumping : MonoBehaviour
{
    // TUTORIAL 1

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
    ForwardMovement_Jumping_Tutorial forwardMovement_Jumping_Tutorial;

    // Private Variables
    Transform playerParent;
    Transform player;
    Rigidbody rb;
    Animator anim;
    Vector3 startTouchPos;
    Vector3 endTouchPos;
    bool jumpAllowed = true;
    bool swipeDetected = false;
    bool leftAllowed = false;
    public bool done = false;

    public void UpTutorial()
    {
        playerParent = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerParent.GetChild(playerParent.childCount - 2);
        rb = playerParent.GetComponent<Rigidbody>();
        anim = player.GetComponent<Animator>();
        forwardMovement_Jumping_Tutorial = playerParent.GetComponent<ForwardMovement_Jumping_Tutorial>();
        fingerObj.SetActive(true);
        fingerAnim.Play("Up");
        forwardMovement_Jumping_Tutorial.enabled = false;
    }

    void LeftTutorial()
    {
        fingerObj.SetActive(true);
        fingerAnim.Play("Left");
        leftAllowed = true;
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
            if (swipePos.x > 0) { }
            else
            {
                if (leftAllowed)
                {
                    StopAllCoroutines();
                    fingerObj.SetActive(false);
                    Time.timeScale = 1f;
                    leftAllowed = false;
                    StartCoroutine(HorizontalMovement("Left"));
                    done = true;
                }
            }
        }
        else if (Mathf.Abs(swipePos.y) > vertiThreshold && Mathf.Abs(swipePos.y) > Mathf.Abs(swipePos.x) && jumpAllowed)
        {
            swipeDetected = true;
            StopAllCoroutines();
            // leftAllowed = true;
            StartCoroutine(Jump());
            fingerObj.SetActive(false);
            Invoke("LeftTutorial", 0.5f);
        }
    }

    IEnumerator HorizontalMovement(string direction){
        rb.useGravity = true;
        fingerObj.SetActive(false);

        Quaternion playerRotations = player.rotation;
        playerRotations = Quaternion.Euler(0f, -20f, 0f);
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
        forwardMovement_Jumping_Tutorial.enabled = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        Quaternion playerRotations = player.rotation;
        playerRotations = Quaternion.Euler(0f, 0f, 0f);
        player.rotation = playerRotations;
        anim.Play("JumpRoll", 0, 0f);
        // Time.timeScale = 0.5f;

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
