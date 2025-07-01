using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner_FloatingBridge : MonoBehaviour
{
    private GameObject bridgePrefab;
    public GameObject fullBridge;
    public List<GameObject> obstacleBridges = new List<GameObject>();
    public List<GameObject> sideEnvirs = new List<GameObject>();
    public List<GameObject> activeBridges = new List<GameObject>();
    public List<GameObject> deactiveBridges = new List<GameObject>();
    public List<GameObject> deactiveObstacleBridges = new List<GameObject>();
    int gap = 2;
    int countBtwObs = 0;
    private int count = 0;
    private Transform spawn;
    public Transform lastBridgeSpawn;
    private float bridgeGap = 4f;
    private bool isGap = false;
    private DestroyBridge_BridgeLevel destroyBridge_BridgeLevel;
    bool obstacleToBeSpawned = false;

    void OnEnable()
    {
        GameManager_BridgeLevel.bridgeSpawnerFloatingBridge += StartSpawning;
    }

    void OnDisable()
    {
        GameManager_BridgeLevel.bridgeSpawnerFloatingBridge -= StartSpawning;
    }

    void StartSpawning()
    {
        destroyBridge_BridgeLevel = GameObject.Find("DestroyBridge").transform.GetComponent<DestroyBridge_BridgeLevel>();
        spawn = transform;
        gap = Random.Range(2, 5);

        for (int i = 0; i < 15; i++)
        {
            SpawnBridge();
        }
    }

    public void SpawnBridge()
    {
        if (gap == countBtwObs)
        {
            int rando = Random.Range(0, obstacleBridges.Count);
            bridgePrefab = obstacleBridges[rando];
            countBtwObs = 0;
            obstacleToBeSpawned = true;
            gap = Random.Range(1, 3);
        }
        else
        {
            bridgePrefab = fullBridge;
            obstacleToBeSpawned = false;
            countBtwObs++;
        }

        if (count % 4 == 0)
        {
            isGap = true;
        }

        GameObject newBridge;

        if (activeBridges.Count > 15)
        {
            GameObject oldBridge = activeBridges[0];
            activeBridges.RemoveAt(0);

            oldBridge.SetActive(false);

            if (oldBridge.name.Contains("FullBridge"))
            {
                deactiveBridges.Add(oldBridge);
            }
            else
            {
                deactiveObstacleBridges.Add(oldBridge);
            }

            if (obstacleToBeSpawned)
            {
                if (deactiveObstacleBridges.Count > 0)
                {
                    int r = Random.Range(0, deactiveObstacleBridges.Count);
                    newBridge = deactiveObstacleBridges[r];
                    deactiveObstacleBridges.RemoveAt(r);
                }
                else
                {
                    newBridge = Instantiate(bridgePrefab);
                }
            }
            else
            {
                if (deactiveBridges.Count > 0)
                {
                    newBridge = deactiveBridges[0];
                    deactiveBridges.RemoveAt(0);
                }
                else
                {
                    newBridge = Instantiate(fullBridge);
                }
            }

            newBridge.transform.position = spawn.position;
            if (isGap)
            {
                newBridge.transform.position += Vector3.forward * bridgeGap;
                isGap = false;
            }

            newBridge.SetActive(true);
            activeBridges.Add(newBridge);

            if (newBridge.transform.childCount > 0)
            {
                spawn = newBridge.transform.GetChild(0);
            }
        }
        else
        {
            newBridge = Instantiate(bridgePrefab, spawn.position, Quaternion.identity);

            if (isGap)
            {
                newBridge.transform.position += Vector3.forward * bridgeGap;
                isGap = false;
            }

            activeBridges.Add(newBridge);

            if (newBridge.transform.childCount > 0)
            {
                spawn = newBridge.transform.GetChild(0);
            }
        }

        count++;

        if (count % 8 == 0 && sideEnvirs.Count > 0 && activeBridges.Count > 0)
        {
            int r = Random.Range(0, sideEnvirs.Count);
            Instantiate(sideEnvirs[r], activeBridges[0].transform);
        }

        if (count % 20 == 0)
        {
            GameManager_BridgeLevel.Instance.UpdateSpeed();
            bridgeGap = Mathf.Min(bridgeGap + 1f, 10f);
        }
    }
}
