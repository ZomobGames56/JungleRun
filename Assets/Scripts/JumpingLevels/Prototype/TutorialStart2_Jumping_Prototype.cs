using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialStart2_Jumping_Prototype : MonoBehaviour
{
    Tutorial2_Jumping_Prototype tutorial2_Jumping_Prototype;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tutorial2_Jumping_Prototype = GetComponent<Tutorial2_Jumping_Prototype>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            tutorial2_Jumping_Prototype.enabled = true;
            tutorial2_Jumping_Prototype.UpTutorial();
        }
    }

    void Update()
    {
        HandleFallCheck();
        if (tutorial2_Jumping_Prototype.done)
        {
            tutorial2_Jumping_Prototype.done = false;
            tutorial2_Jumping_Prototype.enabled = false;
        }
    }
    
    void HandleFallCheck(){
        if(player.position.y < -5f){
            GameManager_Jumping_Prototype.Instance.EndGame();
        }
    }
}
