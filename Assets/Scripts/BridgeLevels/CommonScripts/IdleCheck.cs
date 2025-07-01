using UnityEngine;

public class IdleCheck : MonoBehaviour
{
    public float idleTimeAllowed = 0.5f;
    private float idleTimer = 0f;
    private Vector3 lastPosition;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        if(!GameManager_BridgeLevel.Instance.startGame || GameManager_BridgeLevel.Instance.playerDead || GameManager_BridgeLevel.Instance.paused) return;
        Vector3 currentPosition = transform.position;

        Vector2 horizontalMove = new Vector2(
            currentPosition.x - lastPosition.x,
            currentPosition.z - lastPosition.z
        );

        if (horizontalMove.magnitude < 0.05f)
        {
            idleTimer += Time.unscaledDeltaTime;
            if (idleTimer >= idleTimeAllowed)
            {
                Debug.Log("Player is idle");
                GameManager_BridgeLevel.Instance.EndGame();
                SoundManager_BridgeLevel.Instance.PlayLoseSound("");
                idleTimer = 0f;
            }
        }
        else
        {
            idleTimer = 0f;
        }

        lastPosition = currentPosition;
    }
}
