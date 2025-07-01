using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialStart3_Jumping : MonoBehaviour
{
    Tutorial3_Jumping tutorial3_Jumping;
    Transform player;
    [SerializeField] string levelType;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
            tutorial3_Jumping.enabled = false;
            Time.timeScale = 1f;
            PlayerPrefs.SetInt("Tutorial", 0);
            PlayerPrefs.Save();
            tutorial3_Jumping.done = false;
        }
    }
    
    void HandleFallCheck(){
        if(player.position.y < -5f){
            GameManager_Jumping_Tutorial.Instance.EndGame();
        }
    }
}
