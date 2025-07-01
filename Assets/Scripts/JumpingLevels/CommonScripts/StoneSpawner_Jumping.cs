using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner_Jumping : MonoBehaviour
{
    [SerializeField] private List<GameObject> stones = new List<GameObject>();
    GameObject prefab;
    [SerializeField] private List<GameObject> activeStonesList = new List<GameObject>();
    [SerializeField] private List<GameObject> deactiveStonesList = new List<GameObject>();
    public float spawnPos = 60f;
    float gapBtwStones = 10.5f;
    bool left = false;
    int count = 0;
    int normalCount = 0;

    void OnEnable()
    {
        GameManager_Jumping.stoneSpawning += StartSpawning;
        GameManager_Jumping.stoneGaps += StoneGap;
    }

    void OnDisable()
    {
        GameManager_Jumping.stoneSpawning -= StartSpawning;
        GameManager_Jumping.stoneGaps -= StoneGap;
    }

    public void StartSpawning(bool start, float pos)
    {
        spawnPos = pos;
        if (start)
        {
            // CreateStone();
            for (int i = 0; i < 10; i++)
            {
                CreateStone();
            }
        }
    }

    void StoneGap() {
        // gapBtwStones += 1f;
    }

    public void NextStone() {
        // Destroy(activeStonesList[0]);
        GameObject t = activeStonesList[0];
        deactiveStonesList.Add(t);
        // t.GetComponent<ObstacleSpawner_Jungle>().Deactived();
        t.SetActive(false);
        activeStonesList.RemoveAt(0);
        CreateStone();
    }

    void CreateStone()
    {
        if (!GameManager_Jumping.Instance.obstacleRush)
        {
            
            if (deactiveStonesList.Count > 5)
            {
                int rando = Random.Range(0, deactiveStonesList.Count - 1);
                GameObject t = deactiveStonesList[rando];
                deactiveStonesList.RemoveAt(rando);
                t.SetActive(true);
                t.transform.position = new Vector3(0f, 0f, spawnPos);
                activeStonesList.Add(t);
            }
            else
            {
                int rando = Random.Range(0, stones.Count - 1);
                prefab = stones[rando];
                GameObject t = Instantiate(prefab, new Vector3(0f, 0f, spawnPos), Quaternion.identity);
                activeStonesList.Add(t);
            }
            spawnPos += gapBtwStones;
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
                spawnPos += gapBtwStones;
                activeStonesList.Add(t);
                left = true;
            }
            else
            {
                GameObject t = Instantiate(stones[1], new Vector3(0f, 0f, spawnPos), Quaternion.identity);
                spawnPos += gapBtwStones;
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