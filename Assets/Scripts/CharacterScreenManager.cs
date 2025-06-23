using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterScreenManager : MonoBehaviour
{
    [SerializeField] List<GameObject> characters = new List<GameObject>();
    [SerializeField] List<int> cost = new List<int>();
    int currChar = 0;
    GameObject player;
    GameObject child;
    GameObject childRagdoll;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        child = Instantiate(characters[0], player.transform);
        childRagdoll = Instantiate(characters[1], player.transform);
    }

    public void ChangeChar(string charName)
    {
        switch (charName)
        {
            case "Bear":
                currChar = 0;
                break;

            case "Lion":
                currChar = 2;
                break;

            case "Deer":
                currChar = 4;
                break;
        }
        Destroy(child);
        Destroy(childRagdoll);
        child = Instantiate(characters[currChar], player.transform);
        childRagdoll = Instantiate(characters[currChar + 1], player.transform);
    } 
}
