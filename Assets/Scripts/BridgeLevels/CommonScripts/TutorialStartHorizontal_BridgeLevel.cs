using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialStartHorizontal_BridgeLevel : MonoBehaviour
{
    public string direction;
    TutorialHorizontal_BridgeLevel tutorialHorizontal_BridgeLevel;

    void Start()
    {
        tutorialHorizontal_BridgeLevel = GetComponent<TutorialHorizontal_BridgeLevel>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (direction == "Right")
            {
                tutorialHorizontal_BridgeLevel.enabled = true;
                tutorialHorizontal_BridgeLevel.RightTutorial();
            }

            if (direction == "Left")
            {
                tutorialHorizontal_BridgeLevel.enabled = true;
                tutorialHorizontal_BridgeLevel.LeftTutorial();
            }
        }
    }

    void Update()
    {
        if (tutorialHorizontal_BridgeLevel.done)
        {
            tutorialHorizontal_BridgeLevel.done = false;
            tutorialHorizontal_BridgeLevel.enabled = false;
        }
    }
}
