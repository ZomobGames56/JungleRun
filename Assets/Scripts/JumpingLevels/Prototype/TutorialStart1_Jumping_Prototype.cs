using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialStart1_Jumping_Prototype : MonoBehaviour
{
    Tutorial1_Jumping_Prototype tutorial1_Jumping_Prototype;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tutorial1_Jumping_Prototype = GetComponent<Tutorial1_Jumping_Prototype>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            tutorial1_Jumping_Prototype.enabled = true;
            tutorial1_Jumping_Prototype.UpTutorial();
        }
    }

    void Update()
    {
        HandleFallCheck();
        if (tutorial1_Jumping_Prototype.done)
        {
            tutorial1_Jumping_Prototype.done = false;
            tutorial1_Jumping_Prototype.enabled = false;
        }
    }
    
    void HandleFallCheck(){
        if(player.position.y < -5f){
            GameManager_Jumping_Prototype.Instance.EndGame();
        }
    }
}
