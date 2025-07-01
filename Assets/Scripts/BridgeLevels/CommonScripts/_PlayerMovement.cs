using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EndlessRunnerPlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float laneSwitchSpeed = 10f;
    public float jumpHeight = 8f;
    public float gravity = 20f;

    [Header("Lane Settings")]
    public float[] laneXPositions = new float[] { -1.8f, 0.05f, 1.9f };
    private int currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
    private float targetX;

    [Header("Ground Check")]
    public Transform checkForGround;
    public LayerMask groundLayer;
    public float width = 0.3f, height = 0.1f, length = 0.3f;

    private CharacterController controller;
    private Animator anim;

    private Vector3 moveDirection = Vector3.zero;
    private float verticalVelocity;
    private bool jumpAllowed = false;
    private bool swipeDetected = false;
    private bool onceCalled = false;

    // Swipe input
    private Vector2 startTouchPos, endTouchPos;

    void Start()
    {
        Invoke("StartMethod", 0.1f);
    }

    void StartMethod()
    {
        controller = GetComponent<CharacterController>();
        anim = transform.GetChild(transform.childCount - 2).GetComponent<Animator>();
        targetX = laneXPositions[currentLane];
    }

    void Update()
    {
        if (!GameManager_BridgeLevel.Instance.startGame || GameManager_BridgeLevel.Instance.playerDead || GameManager_BridgeLevel.Instance.paused)
            return;

        HandleGroundCheck();
        HandleFallCheck();
        HandleTouchInput();
    }

    void HandleGroundCheck()
    {
        Collider[] colliders = Physics.OverlapBox(checkForGround.position, new Vector3(width, height, length), Quaternion.identity, groundLayer);
        jumpAllowed = colliders.Length > 0;
        // anim.SetBool("isFalling", !jumpAllowed);
    }

    void HandleFallCheck()
    {
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

        if (controller.isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
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
        }
        else if (Mathf.Abs(swipePos.y) > vertiThreshold && Mathf.Abs(swipePos.y) > Mathf.Abs(swipePos.x))
        {
            swipeDetected = true;
            if (swipePos.y > 0 && jumpAllowed)
            {
                StartCoroutine(Jump());
            }
            else if (swipePos.y < 0)
            {
                StartCoroutine(Slide());
            }
        }
    }

    void CheckSwipe(string direction)
    {
        if (direction == "Left" && currentLane > 0)
        {
            currentLane--;
            targetX = laneXPositions[currentLane];
        }
        else if (direction == "Right" && currentLane < laneXPositions.Length - 1)
        {
            currentLane++;
            targetX = laneXPositions[currentLane];
        }
    }

    IEnumerator Jump()
    {
        anim.Play("Jumping");
        verticalVelocity = jumpHeight;
        yield return null;
    }

    IEnumerator Slide()
    {
        anim.Play("Sliding");
        controller.height = 1f;
        yield return new WaitForSeconds(1f);
        controller.height = 2f;
    }
}
