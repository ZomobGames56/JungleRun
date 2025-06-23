using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner_Jumping_Prototype : MonoBehaviour
{
    [SerializeField] private List<GameObject> stones = new List<GameObject>();
    GameObject prefab;
    [SerializeField] private List<GameObject> activeStonesList = new List<GameObject>();
    public float spawnPos = 60f;

    public void StartSpawning()
    {
        CreateStone();
        for (int i = 0; i < 10; i++)
        {
            CreateStone();
        }
    }

    public void NextStone(){
        Destroy(activeStonesList[0]);
        activeStonesList.RemoveAt(0);
        CreateStone();
    }

    void CreateStone()
    {
        int rando = Random.Range(0, stones.Count - 1);
        prefab = stones[rando];
        GameObject t = Instantiate(prefab, new Vector3(0f, 0f, spawnPos), Quaternion.identity);
        spawnPos += 12f;
        activeStonesList.Add(t);
    }
}