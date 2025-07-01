using UnityEngine;

public class CameraFollow_BridgeLevel : MonoBehaviour
{
    [SerializeField] float yDis = 2f;
    [SerializeField] float zOffset = -5f;
    private Transform player;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void LateUpdate()
    {
        if (GameManager_BridgeLevel.Instance.startGame && !GameManager_BridgeLevel.Instance.playerDead)
        {
            Vector3 targetPos = new Vector3(0f, yDis, player.position.z + zOffset);
            transform.position = targetPos;
        }
    }
}
