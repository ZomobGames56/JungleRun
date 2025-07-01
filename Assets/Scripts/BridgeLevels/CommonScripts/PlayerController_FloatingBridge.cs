using UnityEngine;
using System;
using System.Collections;

// public enum SIDE
// {
//     right, left
// }

public class PlayerController_FloatingBridge : MonoBehaviour
{
    public SIDE currSide = SIDE.right;
    float xPos = 0.8f;
    public float xVal;
    public float leftRightSpeed;
    public float jumpPower;
    public float fwdSpeed;
    private float x = 0.8f;
    private float y;

    private CharacterController characterController;
    private Animator anim;

    public bool inJump;
    public bool inRoll;
    bool swipeDetected = false;
    bool isFalling = false;
    bool onceCalled = false;
    [SerializeField] Transform checkForGround;
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] float length;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded = false;

    private float colHeight;
    private float colCenter;
    Transform player;

    // Swipe detection variables
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private float swipeThreshold = 10f;

    // Events
    public static event Action<QuestType, int> questUpdater;

    internal float rollCounter;

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
        if (a == "Right")
        {
            currSide = SIDE.right;
        }
        else if (a == "Left")
        {
            currSide = SIDE.left;
        }
    }

    void UpdatePlayer(GameObject a, Animator b)
    {
        player = a.transform;
        anim = b;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        colHeight = characterController.height;
        colCenter = characterController.center.y;
        player = transform.GetChild(transform.childCount - 2);
        anim = player.GetComponent<Animator>();
        // transform.position = 0.8f;
    }

    void Update()
    {
        if (!GameManager_BridgeLevel.Instance.startGame || GameManager_BridgeLevel.Instance.playerDead || GameManager_BridgeLevel.Instance.paused) return;
        DetectSwipe();
        HandleGroundCheck();
        HandleFallCheck();

        x = Mathf.Lerp(x, xPos, Time.deltaTime * leftRightSpeed);
        Vector3 moveVector = new Vector3(x - transform.position.x, y * Time.deltaTime, fwdSpeed * Time.deltaTime);
        characterController.Move(moveVector * GameManager_BridgeLevel.Instance.speedModifier);

        Jump();
        Roll();
    }

    void HandleGroundCheck()
    {
        Collider[] colliders = Physics.OverlapBox(checkForGround.position, new Vector3(width, height, length), Quaternion.identity, groundLayer);
        isGrounded = colliders.Length > 0;
        anim.SetBool("isFalling", !isGrounded);
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
    }

    void DetectSwipe()
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
                SwipeLogic(touch.position);
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                swipeDetected = false;
            }
        }
    }

    void SwipeLogic(Vector2 currentTouch)
    {
        endTouchPos = currentTouch;
        Vector2 deltaSwipe = endTouchPos - startTouchPos;

        if (deltaSwipe.magnitude > swipeThreshold)
        {
            float x = Mathf.Abs(deltaSwipe.x);
            float y = Mathf.Abs(deltaSwipe.y);

            if (x > y)
            {
                if (deltaSwipe.x > 0)
                    OnSwipeRight();
                else
                    OnSwipeLeft();
            }
            else
            {
                if (deltaSwipe.y > 0)
                    OnSwipeUp();
                else
                    OnSwipeDown();
            }
            swipeDetected = true;
        }
    }

    void OnSwipeLeft()
    {
        // if (inRoll) return;

        if (currSide == SIDE.right)
        {
            xPos = -xVal;
            currSide = SIDE.left;
            player.rotation = Quaternion.Euler(0f, -20f, 0f);
            Invoke("Straighten", 0.3f);
            // anim.Play("Left");
            characterController.center = new Vector3(0, colCenter, 0);
            characterController.height = colHeight;
        }
    }

    void OnSwipeRight()
    {
        // if (inRoll) return;

        if (currSide == SIDE.left)
        {
            xPos = xVal;
            currSide = SIDE.right;
            player.rotation = Quaternion.Euler(0f, 20f, 0f);
            Invoke("Straighten", 0.3f);
            // anim.Play("Right");
            characterController.center = new Vector3(0, colCenter, 0);
            characterController.height = colHeight;
        }
    }

    void OnSwipeUp()
    {
        if (isGrounded)
        // if (characterController.isGrounded)
        {
            y = jumpPower;
            anim.CrossFadeInFixedTime("Jump", 0.1f);
            inJump = true;
            inRoll = false;
            questUpdater?.Invoke(QuestType.Jumps, 1);
        }
    }

    void OnSwipeDown()
    {
        if (inRoll) return;

        rollCounter = 1.5f;
        y -= 10f;
        characterController.center = new Vector3(0, colCenter / 2f, 0);
        characterController.height = colHeight / 2f;
        // anim.CrossFadeInFixedTime("Slide", 0.1f);
        questUpdater?.Invoke(QuestType.Slide, 1);
        anim.Play("Slide");
        inRoll = true;
        inJump = false;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            inJump = false;
            isFalling = false;
            // anim.SetBool("isFalling", );
            anim.SetBool("isFalling", false);
        }
        else
        {
            y -= jumpPower * 2 * Time.deltaTime;
            if (!isFalling)
            // if (characterController.velocity.y < -0.1f && !isFalling)
            {
                // anim.Play("Falling");
                anim.SetBool("isFalling", true);
                Debug.Log("AAAAAAAAa");
                // anim.SetTrigger("Falling");
                isFalling = true;
            }
        }
    }

    public void Roll()
    {
        rollCounter -= Time.deltaTime;
        if (rollCounter <= 0f)
        {
            rollCounter = 0f;
            characterController.center = new Vector3(0, colCenter, 0);
            characterController.height = colHeight;
            inRoll = false;
        }
    }
    
    void Straighten()
    {
        player.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
