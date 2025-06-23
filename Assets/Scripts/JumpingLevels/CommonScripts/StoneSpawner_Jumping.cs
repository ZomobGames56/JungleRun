using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner_Jumping : MonoBehaviour
{
    [SerializeField] private List<GameObject> stones = new List<GameObject>();
    GameObject prefab;
    [SerializeField] private List<GameObject> activeStonesList = new List<GameObject>();
    public float spawnPos = 60f;
    bool left = false;
    int count = 0;
    int normalCount = 0;

    void OnEnable()
    {
        GameManager_Jumping.stoneSpawning += StartSpawning;
    }

    void OnDisable()
    {
        GameManager_Jumping.stoneSpawning -= StartSpawning;
    }

    public void StartSpawning(bool start, float pos)
    {
        spawnPos = pos;
        if (start)
        {
            CreateStone();
            for (int i = 0; i < 10; i++)
            {
                CreateStone();
            }
        }
    }

    public void NextStone(){
        Destroy(activeStonesList[0]);
        activeStonesList.RemoveAt(0);
        CreateStone();
    }

    void CreateStone()
    {
        if (!GameManager_Jumping.Instance.obstacleRush)
        {
            int rando = Random.Range(0, stones.Count - 1);
            prefab = stones[rando];
            GameObject t = Instantiate(prefab, new Vector3(0f, 0f, spawnPos), Quaternion.identity);
            spawnPos += 12f;
            activeStonesList.Add(t);
            normalCount++;
            if (normalCount == 10)
            {
                GameManager_Jumping.Instance.magicStoneSpawned = false;
                normalCount = 0;
            }
        }
        else
        {
            if (!left)
            {
                GameObject t = Instantiate(stones[0], new Vector3(0f, 0f, spawnPos), Quaternion.identity);
                spawnPos += 12f;
                activeStonesList.Add(t);
                left = true;
            }
            else
            {
                GameObject t = Instantiate(stones[1], new Vector3(0f, 0f, spawnPos), Quaternion.identity);
                spawnPos += 12f;
                activeStonesList.Add(t);
                left = false;
            }
            if (count == 10)
            {
                GameManager_Jumping.Instance.obstacleRush = false;
                count = 0;
            }
            count++;
        }
    }
}