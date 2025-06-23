using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialSlide_BridgeLevel : MonoBehaviour
{
    // Private Variables
    Transform playerParent;
    Transform player;
    Rigidbody rb;
    Animator anim;
    Vector3 startTouchPos;
    Vector3 endTouchPos;
    bool jumpAllowed = true;
    [SerializeField] GameObject fingerObj;
    [SerializeField] Animator fingerAnim;
    ForwardMovement_BridgeLevel forwardMovement_BridgeLevel;
    bool swipeDetected = false;
    public bool done = false;

    public void StartTutorial()
    {
        fingerObj.SetActive(true);
        fingerAnim.Play("Down");
        playerParent = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerParent.GetChild(playerParent.childCount - 2);
        rb = playerParent.GetComponent<Rigidbody>();
        anim = player.GetComponent<Animator>();
        forwardMovement_BridgeLevel = playerParent.GetComponent<ForwardMovement_BridgeLevel>();
        forwardMovement_BridgeLevel.enabled = false;
    }

    void Update(){
        HandleTouchInput();
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

        if(Mathf.Abs(swipePos.y) > vertiThreshold && Mathf.Abs(swipePos.y) > Mathf.Abs(swipePos.x)){
            swipeDetected = true;
            if (swipePos.y > 0 && jumpAllowed) { }
            else if (swipePos.y < 0)
            {
                StartCoroutine(Slide());
                fingerObj.SetActive(false);
                done = true;
            }
        }
    }

    IEnumerator Slide(){
        forwardMovement_BridgeLevel.enabled = true;
        rb.useGravity = true;
        anim.SetTrigger("Slide");
        CapsuleCollider col = playerParent.GetComponent<CapsuleCollider>();
        col.center = new Vector3(col.center.x, 0.5770407f, col.center.z);
        col.height = 1.160505f;
        yield return new WaitForSeconds(0.75f);
        col.center = new Vector3(col.center.x, 0.9765382f, col.center.z);
        col.height = 1.9595f;
    }
}
