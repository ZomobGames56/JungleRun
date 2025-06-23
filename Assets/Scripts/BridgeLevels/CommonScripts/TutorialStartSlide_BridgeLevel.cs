using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialStartSlide_BridgeLevel : MonoBehaviour
{
    TutorialSlide_BridgeLevel tutorialSlide_BridgeLevel;

    void Start()
    {
        tutorialSlide_BridgeLevel = GetComponent<TutorialSlide_BridgeLevel>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            tutorialSlide_BridgeLevel.enabled = true;
            tutorialSlide_BridgeLevel.StartTutorial();
        }
    }

    void Update()
    {
        if (tutorialSlide_BridgeLevel.done)
        {
            tutorialSlide_BridgeLevel.done = false;
            tutorialSlide_BridgeLevel.enabled = false;
        }
    }
}
