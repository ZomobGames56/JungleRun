using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialStart3_Jumping : MonoBehaviour
{
    Tutorial3_Jumping tutorial3_Jumping;
    Transform player;
    PlayerMovement_Jumping playerMovement_Jumping;
    [SerializeField] string levelType;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement_Jumping = player.GetComponent<PlayerMovement_Jumping>();
        tutorial3_Jumping = GetComponent<Tutorial3_Jumping>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            tutorial3_Jumping.enabled = true;
            tutorial3_Jumping.UpTutorial();
        }
    }

    void Update()
    {
        HandleFallCheck();
        if (tutorial3_Jumping.done)
        {
            tutorial3_Jumping.done = false;
            tutorial3_Jumping.enabled = false;
            Time.timeScale = 1.3f;
            GameManager_Jumping.Instance.tutorialStarted = false;
            PlayerPrefs.SetInt("Tutorial - " + levelType, 0);
            PlayerPrefs.Save();
            GameManager_Jumping.startTutorial = false;
            playerMovement_Jumping.enabled = true;
        }
    }
    
    void HandleFallCheck(){
        if(player.position.y < -5f){
            GameManager_Jumping.Instance.EndGame();
        }
    }
}
