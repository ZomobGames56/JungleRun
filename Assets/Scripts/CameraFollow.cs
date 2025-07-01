using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;
    [SerializeField] string levelType;
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    public Vector3 initialCameraRotation = new Vector3(45f, 0, 0);
    [SerializeField]
    public Vector3 offsetFromPlayer = new Vector3(0, 10, -10);

    public float cameraFollowSpeed = 5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (playerTransform == null)
        {
            Debug.Log("Player transform is missing");
            playerTransform = GameObject.FindGameObjectWithTag("HY_Player").transform;
        }
    }


    void LateUpdate()
    {
        if (levelType == "Jumping")
        {
            if (!GameManager_Jumping.Instance.startGame || GameManager_Jumping.Instance.playerDead || GameManager_Jumping.Instance.paused) return;
        }
        else if (levelType == "Tutorial")
        {
            if (!GameManager_Jumping_Tutorial.Instance.startGame || GameManager_Jumping_Tutorial.Instance.playerDead || GameManager_Jumping_Tutorial.Instance.paused) return;
        }
        else
        {
            if (!GameManager_BridgeLevel.Instance.startGame || GameManager_BridgeLevel.Instance.playerDead || GameManager_BridgeLevel.Instance.paused) return;
        }

        transform.rotation = Quaternion.Euler(initialCameraRotation);
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + offsetFromPlayer, cameraFollowSpeed * Time.deltaTime);
    }
}
