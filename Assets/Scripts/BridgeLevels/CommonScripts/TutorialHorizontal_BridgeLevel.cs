using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialHorizontal_BridgeLevel : MonoBehaviour
{
    [SerializeField] float horizontalSpeed;

    Transform playerParent;
    Transform player;
    Rigidbody rb;
    Animator anim;
    Vector3 startTouchPos;
    Vector3 endTouchPos;
    bool swipeDetected = false;
    bool swipeAllowed = false;
    string currDir = "Right";
    float dir = 0f;
    [SerializeField] GameObject fingerObj;
    [SerializeField] Animator fingerAnim;
    [SerializeField] float dirValLeft;
    [SerializeField] float dirValRight;
    ForwardMovement_BridgeLevel forwardMovement_BridgeLevel;
    public bool done = false;

    public void LeftTutorial()
    {
        fingerObj.SetActive(true);
        fingerAnim.Play("Left");
        playerParent = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerParent.GetChild(playerParent.childCount - 2);
        rb = playerParent.GetComponent<Rigidbody>();
        anim = player.GetComponent<Animator>();
        forwardMovement_BridgeLevel = playerParent.GetComponent<ForwardMovement_BridgeLevel>();
        forwardMovement_BridgeLevel.enabled = false;
        currDir = "Right";
    }

    public void RightTutorial()
    {
        fingerObj.SetActive(true);
        fingerAnim.Play("Right");
        playerParent = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerParent.GetChild(playerParent.childCount - 2);
        rb = playerParent.GetComponent<Rigidbody>();
        anim = player.GetComponent<Animator>();
        forwardMovement_BridgeLevel = playerParent.GetComponent<ForwardMovement_BridgeLevel>();
        forwardMovement_BridgeLevel.enabled = false;
        currDir = "Left";
    }

    void Update()
    {
        HandleTouchInput();
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
            string dir = swipePos.x > 0 ? "Right" : "Left";
            CheckSwipe(dir);
            if (swipeAllowed)
            {
                StopAllCoroutines();
                StartCoroutine(HorizontalMovement(dir));
                fingerObj.SetActive(false);
                done = true;
            }
        }
    }

    void CheckSwipe(string direction){
        if(currDir == "Right"){
            if(direction == "Left"){
                dir = dirValLeft;  
                currDir = "Left";
                swipeAllowed = true;
            }
        }
        else if(currDir == "Left"){
            if(direction == "Right"){
                dir = dirValRight;  
                currDir = "Right";
                swipeAllowed = true;
            }
        }
    }
    
    IEnumerator HorizontalMovement(string direction){
        forwardMovement_BridgeLevel.enabled = true;
        rb.useGravity = true;

        Quaternion playerRotations = player.rotation;
        if(direction == "Right") playerRotations = Quaternion.Euler(0f, 20f, 0f);
        else playerRotations = Quaternion.Euler(0f, -20f, 0f);
        player.rotation = playerRotations;

        while(Mathf.Abs(playerParent.position.x - dir) > 0.05f){
            Vector3 moveVector = (dir - playerParent.position.x) * Vector3.right * horizontalSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + moveVector); 
            yield return null;
        }

        playerRotations = Quaternion.Euler(0f, 0f, 0f);
        player.rotation = playerRotations;

        anim.SetTrigger("OnGround");
        swipeAllowed = false;
    }
}
