using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow_BridgeLevel : MonoBehaviour
{
    [SerializeField] float yDis;
    Transform player;

    void Start(){
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if(GameManager_BridgeLevel.Instance.startGame && !GameManager_BridgeLevel.Instance.playerDead){
            Vector3 a = transform.position;
            a.y = yDis;
            a.x = 0f;
            transform.position = a;
        }
    }
}
