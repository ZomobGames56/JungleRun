using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovement_FloatingBridge_Test : MonoBehaviour
{
    public float jumpSpeed, playerForce, directionForce;
    public float minimumSwipeDistance = 50f;

    private CharacterController controller;
    private Vector3 vel;
    private float gravity = -9.81f;

    private bool moveLeft, moveRight;
    private Vector3 movementVector;

    private InputSystem inputActions;
    private Vector2 swipeDelta;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new InputSystem();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Update()
    {
        swipeDelta = inputActions.Move.Swipe.ReadValue<Vector2>();
        if (swipeDelta.magnitude >= minimumSwipeDistance)
        {
            ProcessSwipe(swipeDelta);
        }

        UpdateMovement();
    }

    void ProcessSwipe(Vector2 delta)
    {
        float x = Mathf.Abs(delta.x);
        float y = Mathf.Abs(delta.y);

        if (x > y)
        {
            if (delta.x > 0)
                MoveRight();
            else
                MoveLeft();
        }
        else
        {
            if (delta.y > 0 && controller.isGrounded)
                Jump();
        }
    }

    void MoveRight()
    {
        moveRight = true;
    }

    void MoveLeft()
    {
        moveLeft = true;
    }

    void Jump()
    {
        vel.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
    }

    void UpdateMovement()
    {
        if (controller.isGrounded && vel.y < 0)
        {
            vel.y = -2f;
        }

        vel.y += gravity * Time.deltaTime;

        movementVector = transform.forward * playerForce;
        if (moveLeft)
        {
            movementVector += -transform.right * directionForce;
            moveLeft = false;
        }
        if (moveRight)
        {
            movementVector += transform.right * directionForce;
            moveRight = false;
        }

        controller.Move((movementVector + vel) * Time.deltaTime);
    }



















    // [Header("Movement")]
    // [SerializeField] float jumpSpeed;
    // [SerializeField] float yDis;
    // [SerializeField] float zDis;
    // [SerializeField] float horizontalSpeed;

    // [Header("Ground Check")]
    // [SerializeField] Transform checkForGround;
    // [SerializeField] float width;
    // [SerializeField] float height;
    // [SerializeField] float length;

    // [Header("Inputs")]
    // [SerializeField] LayerMask groundLayer;
    // [SerializeField] float idleThreshold;
    // [SerializeField] float idleTimeAllowed;
    // [SerializeField] Animator gliderAnim;

    // Transform player;
    // CharacterController controller;
    // Animator anim;
    // float lastPos;
    // Vector3 startTouchPos;
    // Vector3 endTouchPos;
    // bool jumpAllowed = true;
    // bool swipeDetected = false;
    // bool swipeAllowed = false;
    // string currDir = "Right";
    // float dir = 0f;
    // float idleTimer;
    // float distanceCheckInterval = 1f;
    // float intervalTimer = 0f;
    // float intervalDistance = 0f;

    // Vector3 velocity;
    // float gravity = -20f;
    // bool isJumping = false;

    // void Start()
    // {
    //     controller = GetComponent<CharacterController>();
    //     player = transform.GetChild(0);
    //     anim = player.GetComponent<Animator>();
    //     lastPos = transform.position.z;
    // }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawCube(checkForGround.position, new Vector3(width, height, length));
    // }

    // void Update()
    // {
    //     if (!GameManager_BridgeLevel.Instance.startGame || GameManager_BridgeLevel.Instance.playerDead || GameManager_BridgeLevel.Instance.paused)
    //         return;

    //     HandleGroundCheck();
    //     HandleFallCheck();
    //     HandleTouchInput();

    //     // Apply gravity
    //     if (!controller.isGrounded)
    //         velocity.y += gravity * Time.deltaTime;
    //     else if (!isJumping)
    //         velocity.y = -1f;

    //     // Move with gravity
    //     controller.Move(velocity * Time.deltaTime);
    // }

    // void HandleGroundCheck()
    // {
    //     Collider[] colliders = Physics.OverlapBox(checkForGround.position, new Vector3(width, height, length), Quaternion.identity, groundLayer);
    //     jumpAllowed = colliders.Length > 0;
    //     anim.SetBool("isFalling", !jumpAllowed);
    // }

    // void HandleFallCheck()
    // {
    //     if (transform.position.y < -5f)
    //     {
    //         GameManager_BridgeLevel.Instance.EndGame();
    //     }
    //     else if (transform.position.y < 0f)
    //     {
    //         anim.Play("Falling");
    //     }
    // }

    // void HandleTouchInput()
    // {
    //     if (Input.touchCount > 0)
    //     {
    //         Touch touch = Input.GetTouch(0);
    //         if (touch.phase == TouchPhase.Began)
    //             startTouchPos = touch.position;

    //         if (touch.phase == TouchPhase.Moved && !swipeDetected)
    //             DetectSwipe(touch.position);

    //         if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
    //             swipeDetected = false;
    //     }
    // }

    // void DetectSwipe(Vector2 currentTouch)
    // {
    //     endTouchPos = currentTouch;
    //     Vector2 swipePos = endTouchPos - startTouchPos;

    //     float horizThreshold = Screen.width * 0.015f;
    //     float vertiThreshold = Screen.height * 0.005f;

    //     if (Mathf.Abs(swipePos.x) > horizThreshold && Mathf.Abs(swipePos.x) > Mathf.Abs(swipePos.y))
    //     {
    //         swipeDetected = true;
    //         string direction = swipePos.x > 0 ? "Right" : "Left";
    //         CheckSwipe(direction);
    //         if (swipeAllowed)
    //         {
    //             StopAllCoroutines();
    //             StartCoroutine(HorizontalMovement(direction));
    //         }
    //     }
    //     else if (Mathf.Abs(swipePos.y) > vertiThreshold && Mathf.Abs(swipePos.y) > Mathf.Abs(swipePos.x))
    //     {
    //         swipeDetected = true;
    //         if (swipePos.y > 0 && jumpAllowed)
    //         {
    //             StopAllCoroutines();
    //             StartCoroutine(Jump());
    //         }
    //         else if (swipePos.y < 0)
    //         {
    //             StopAllCoroutines();
    //             StartCoroutine(Slide());
    //         }
    //     }
    // }

    // void CheckSwipe(string direction)
    // {
    //     if (currDir == "Right" && direction == "Left")
    //     {
    //         dir = -0.8f;
    //         currDir = "Left";
    //         swipeAllowed = true;
    //     }
    //     else if (currDir == "Left" && direction == "Right")
    //     {
    //         dir = 0.8f;
    //         currDir = "Right";
    //         swipeAllowed = true;
    //     }
    // }

    // IEnumerator HorizontalMovement(string direction)
    // {
    //     Quaternion playerRotations = player.rotation;
    //     if (GameManager_BridgeLevel.Instance.startGlide)
    //     {
    //         anim.SetTrigger("Glide" + direction);
    //         gliderAnim.SetTrigger(direction);
    //     }
    //     else
    //     {
    //         player.rotation = direction == "Right" ? Quaternion.Euler(0f, 20f, 0f) : Quaternion.Euler(0f, -20f, 0f);
    //     }

    //     while (Mathf.Abs(transform.position.x - dir) > 0.05f)
    //     {
    //         float moveAmount = (dir - transform.position.x) * horizontalSpeed * Time.deltaTime;
    //         Vector3 move = new Vector3(moveAmount, 0f, 0f);
    //         controller.Move(move);
    //         yield return null;
    //     }

    //     player.rotation = Quaternion.Euler(0f, 0f, 0f);
    //     anim.SetTrigger("OnGround");
    //     swipeAllowed = false;
    // }

    // IEnumerator Jump()
    // {
    //     isJumping = true;
    //     anim.Play("JumpRoll", 0, 0f);
    //     Vector3 target = new Vector3(transform.position.x, transform.position.y + yDis, transform.position.z + zDis);
    //     float jumpProgress = 0f;

    //     while (jumpProgress < 1f)
    //     {
    //         Vector3 move = new Vector3(0f, yDis, zDis) * jumpSpeed * Time.deltaTime;
    //         controller.Move(move);
    //         jumpProgress += jumpSpeed * Time.deltaTime;
    //         yield return null;
    //     }

    //     anim.SetBool("isFalling", true);
    //     isJumping = false;
    // }

    // IEnumerator Slide()
    // {
    //     anim.SetTrigger("Slide");

    //     CapsuleCollider col = GetComponent<CapsuleCollider>();
    //     col.center = new Vector3(col.center.x, 0.5770407f, col.center.z);
    //     col.height = 1.160505f;
    //     yield return new WaitForSeconds(0.75f);
    //     col.center = new Vector3(col.center.x, 0.9765382f, col.center.z);
    //     col.height = 1.9595f;
    // }
}
