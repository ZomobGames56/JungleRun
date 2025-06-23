using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialStart3_Jumping_Prototype : MonoBehaviour
{
    Tutorial3_Jumping_Prototype tutorial3_Jumping_Prototype;
    Transform player;
    PlayerMovement_Jumping_Prototype playerMovement_Jumping_Prototype;
    [SerializeField] string levelType;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement_Jumping_Prototype = player.GetComponent<PlayerMovement_Jumping_Prototype>();
        tutorial3_Jumping_Prototype = GetComponent<Tutorial3_Jumping_Prototype>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            tutorial3_Jumping_Prototype.enabled = true;
            tutorial3_Jumping_Prototype.UpTutorial();
        }
    }

    void Update()
    {
        HandleFallCheck();
        if (tutorial3_Jumping_Prototype.done)
        {
            tutorial3_Jumping_Prototype.done = false;
            tutorial3_Jumping_Prototype.enabled = false;
            Time.timeScale = 1.3f;
            GameManager_Jumping_Prototype.Instance.tutorialStarted = false;
            PlayerPrefs.SetInt("Tutorial - " + levelType, 0);
            PlayerPrefs.Save();
            GameManager_Jumping_Prototype.startTutorial = false;
            playerMovement_Jumping_Prototype.enabled = true;
        }
    }
    
    void HandleFallCheck(){
        if(player.position.y < -5f){
            GameManager_Jumping_Prototype.Instance.EndGame();
        }
    }
}
