using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow_BridgeLevel : MonoBehaviour
{
    [SerializeField] float yDis;
    [SerializeField] float xDis;
    Transform player;

    void Start(){
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if(GameManager_BridgeLevel.Instance.startGame){
            Vector3 a = transform.position;
            a.y = yDis;
            a.x = xDis;
            transform.position = a;
        }
    }
}
