using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialStartJump_BridgeLevel : MonoBehaviour
{
    TutorialJump_BridgeLevel tutorialJump_BridgeLevel;
    public string levelType;
    Transform player;
    PlayerMovement_FloatingBridge playerMovement_FloatingBridge;
    CoinSpawn_FloatingBridge coinSpawn_FloatingBridge;
    PlayerMovement_Ice playerMovement_Ice;
    CoinSpawn_Ice coinSpawn_Ice;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tutorialJump_BridgeLevel = GetComponent<TutorialJump_BridgeLevel>();
        if (levelType == "FloatingBridge" || levelType == "Horror")
        {
            playerMovement_FloatingBridge = player.GetComponent<PlayerMovement_FloatingBridge>();
            coinSpawn_FloatingBridge = player.GetComponent<CoinSpawn_FloatingBridge>();
        }
        else
        {
            playerMovement_Ice = player.GetComponent<PlayerMovement_Ice>();
            coinSpawn_Ice = player.GetComponent<CoinSpawn_Ice>();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            tutorialJump_BridgeLevel.enabled = true;
            tutorialJump_BridgeLevel.StartTutorial();
        }
    }

    void Update()
    {
        HandleFallCheck();
        if (tutorialJump_BridgeLevel.done)
        {
            tutorialJump_BridgeLevel.done = false;
            tutorialJump_BridgeLevel.enabled = false;
            Time.timeScale = 1f;
            GameManager_BridgeLevel.Instance.tutorialStarted = false;
            PlayerPrefs.SetInt("Tutorial - " + levelType, 0);
            PlayerPrefs.Save();
            GameManager_BridgeLevel.startTutorial = false;
            if (levelType == "FloatingBridge" || levelType == "Horror")
            {
                playerMovement_FloatingBridge.enabled = true;
                playerMovement_FloatingBridge.currDir = "Right";
                coinSpawn_FloatingBridge.enabled = true;
            }
            else
            {
                playerMovement_Ice.enabled = true;
                playerMovement_Ice.currDir = "Right";
                coinSpawn_Ice.enabled = true;
            }
        }
    }

    void HandleFallCheck(){
        if (player.position.y < -5f)
        {
            GameManager_BridgeLevel.Instance.EndGame();
        }
    }
}
