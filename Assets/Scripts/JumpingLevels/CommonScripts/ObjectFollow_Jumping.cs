using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow_Jumping : MonoBehaviour
{
    [SerializeField] float yDis;
    Transform player;

    void Start(){
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        // if(GameManager_Jumping_Prototype.Instance.startGame){
        if(GameManager_Jumping.Instance.startGame){
            Vector3 a = transform.position;
            a.y = yDis;
            a.x = 0f;
            transform.position = a;
        }
    }
}
