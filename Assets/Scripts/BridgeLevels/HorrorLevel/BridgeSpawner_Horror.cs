using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner_Horror : MonoBehaviour
{
    private GameObject bridgePrefab;

    public List<GameObject> bridges = new List<GameObject>();
    public List<GameObject> obstacleBridges = new List<GameObject>();

    public List<GameObject> activeBridges = new List<GameObject>();
    [SerializeField] List<GameObject> deactiveBridges = new List<GameObject>();
    [SerializeField] List<GameObject> deactiveObstacleBridges = new List<GameObject>();

    int gap = 2;
    int countBtwObs = 0;
    private int count = 0;

    private Transform spawn;
    public Transform lastBridgeSpawn;

    void Start()
    {
        spawn = transform;
        gap = Random.Range(2, 4);
        for (int i = 0; i < 20; i++)
        {
            SpawnInitialBridge();
        }
    }

    private void SpawnInitialBridge()
    {
        if (gap == countBtwObs)
        {
            bridgePrefab = obstacleBridges[Random.Range(0, obstacleBridges.Count)];
            countBtwObs = 0;
            gap = Random.Range(1, 3);
        }
        else
        {
            bridgePrefab = bridges[Random.Range(0, bridges.Count)];
            countBtwObs++;
        }

        GameObject newBridge = Instantiate(bridgePrefab, spawn.position, Quaternion.identity);
        activeBridges.Add(newBridge);
        spawn = newBridge.transform.GetChild(0);
    }

    public void SpawnBridge()
    {
        GameObject oldest = activeBridges[0];
        activeBridges.RemoveAt(0);

        oldest.SetActive(false);
        if (IsObstacleBridge(oldest))
            deactiveObstacleBridges.Add(oldest);
        else
            deactiveBridges.Add(oldest);

        bool spawnObstacle = (gap == countBtwObs);
        if (spawnObstacle)
        {
            countBtwObs = 0;
            gap = Random.Range(1, 3);
        }
        else
        {
            countBtwObs++;
        }

        GameObject newBridge;
        if (spawnObstacle)
        {
            if (deactiveObstacleBridges.Count > 0)
            {
                int r = Random.Range(0, deactiveObstacleBridges.Count);
                newBridge = deactiveObstacleBridges[r];
                deactiveObstacleBridges.RemoveAt(r);
            }
            else
            {
                newBridge = Instantiate(obstacleBridges[Random.Range(0, obstacleBridges.Count)]);
            }
        }
        else
        {
            if (deactiveBridges.Count > 0)
            {
                int r = Random.Range(0, deactiveBridges.Count);
                newBridge = deactiveBridges[r];
                deactiveBridges.RemoveAt(r);
            }
            else
            {
                newBridge = Instantiate(bridges[Random.Range(0, bridges.Count)]);
            }
        }

        newBridge.transform.position = spawn.position;
        newBridge.SetActive(true);
        activeBridges.Add(newBridge);
        spawn = newBridge.transform.GetChild(0);

        count++;
        if (count % 20 == 0)
        {
            Time.timeScale += 0.1f * Time.timeScale;
            if (Time.timeScale > 1.75f)
                Time.timeScale = 1.75f;

            var player = GameObject.FindGameObjectWithTag("Player").transform;
            int childIndex = player.childCount - 2;
            if (childIndex >= 0)
            {
                Animator anim = player.GetChild(childIndex).GetComponent<Animator>();
                if (anim != null) anim.speed = 1 / Time.timeScale;
            }
            Debug.Log(Time.timeScale);
        }
    }

    private bool IsObstacleBridge(GameObject bridge)
    {
        foreach (var obs in obstacleBridges)
        {
            if (bridge.name.Contains(obs.name))
                return true;
        }
        return false;
    }
}