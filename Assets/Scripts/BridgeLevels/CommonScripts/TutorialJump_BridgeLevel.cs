using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialJump_BridgeLevel : MonoBehaviour
{
    // Movement
    [Header("Movement")]
    [SerializeField] float jumpSpeed;
    [SerializeField] float yDis;
    [SerializeField] float zDis;

    // Ground Check
    [Header("Ground Check")]
    [SerializeField] Transform checkForGround;
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] float length;

    // Inputs
    [Header("Inputs")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject fingerObj;
    [SerializeField] Animator fingerAnim;

    // Private Variables
    Transform playerParent;
    Transform player;
    Rigidbody rb;
    Animator anim;
    Vector3 startTouchPos;
    Vector3 endTouchPos;
    bool jumpAllowed = true;
    bool swipeDetected = false;
    ForwardMovement_BridgeLevel forwardMovement_BridgeLevel;
    public bool done = false;

    public void StartTutorial()
    {
        fingerObj.SetActive(true);
        fingerAnim.Play("Up");
        playerParent = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerParent.GetChild(playerParent.childCount - 2);
        rb = playerParent.GetComponent<Rigidbody>();
        anim = player.GetComponent<Animator>();
        forwardMovement_BridgeLevel = playerParent.GetComponent<ForwardMovement_BridgeLevel>();
        forwardMovement_BridgeLevel.enabled = false;
    }

    void Update()
    {
        HandleGroundCheck();
        HandleTouchInput();
    }

    void HandleGroundCheck()
    {
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

        if (Mathf.Abs(swipePos.y) > vertiThreshold && Mathf.Abs(swipePos.y) > Mathf.Abs(swipePos.x))
        {
            swipeDetected = true;
            if (swipePos.y > 0 && jumpAllowed)
            {
                StopAllCoroutines();
                StartCoroutine(Jump());
                fingerObj.SetActive(false);
                done = true;
            }
        }
    }
    
    IEnumerator Jump()
    {
        forwardMovement_BridgeLevel.enabled = true;
        rb.useGravity = false;
        Quaternion playerRotations = player.rotation;
        playerRotations = Quaternion.Euler(0f, 0f, 0f);
        player.rotation = playerRotations;
        anim.Play("JumpRoll", 0, 0f);

        Vector3 target = new Vector3(playerParent.position.x, playerParent.position.y + yDis, playerParent.position.z + zDis);
        while (Mathf.Abs(playerParent.position.z - target.z) > 0.1f)
        {
            Vector3 moveVector = (target.z - playerParent.position.z) * Vector3.up * jumpSpeed * GameManager_BridgeLevel.Instance.speedModifier * Time.deltaTime;
            rb.MovePosition(rb.position + moveVector);
            yield return null;
        }

        anim.SetBool("isFalling", true);
        rb.useGravity = true;
    }
}
