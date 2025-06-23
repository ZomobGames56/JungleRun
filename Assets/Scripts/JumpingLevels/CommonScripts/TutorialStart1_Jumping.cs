using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialStart1_Jumping : MonoBehaviour
{
    Tutorial1_Jumping tutorial1_Jumping;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tutorial1_Jumping = GetComponent<Tutorial1_Jumping>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            tutorial1_Jumping.enabled = true;
            tutorial1_Jumping.UpTutorial();
        }
    }

    void Update()
    {
        HandleFallCheck();
        if (tutorial1_Jumping.done)
        {
            tutorial1_Jumping.done = false;
            tutorial1_Jumping.enabled = false;
        }
    }
    
    void HandleFallCheck(){
        if(player.position.y < -5f){
            GameManager_Jumping.Instance.EndGame();
        }
    }
}
