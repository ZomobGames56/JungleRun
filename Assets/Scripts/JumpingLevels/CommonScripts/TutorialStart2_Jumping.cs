using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialStart2_Jumping : MonoBehaviour
{
    Tutorial2_Jumping tutorial2_Jumping;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tutorial2_Jumping = GetComponent<Tutorial2_Jumping>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            tutorial2_Jumping.enabled = true;
            tutorial2_Jumping.UpTutorial();
        }
    }

    void Update()
    {
        HandleFallCheck();
        if (tutorial2_Jumping.done)
        {
            tutorial2_Jumping.done = false;
            tutorial2_Jumping.enabled = false;
        }
    }
    
    void HandleFallCheck(){
        if(player.position.y < -5f){
            GameManager_Jumping_Tutorial.Instance.EndGame();
        }
    }
}
