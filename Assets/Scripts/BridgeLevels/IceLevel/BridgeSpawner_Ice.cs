using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner_Ice : MonoBehaviour
{
    private GameObject bridgePrefab;

    public GameObject geyserPrefab;

    public List<GameObject> bridges = new List<GameObject>();
    public List<GameObject> obstacleBridges = new List<GameObject>();
    public List<GameObject> doubleObstacleBridges = new List<GameObject>();
    public List<GameObject> sideEnvirs = new List<GameObject>();

    public List<GameObject> activeBridges = new List<GameObject>();
    [SerializeField] List<GameObject> deactiveBridges = new List<GameObject>();
    [SerializeField] List<GameObject> deactiveObstacleBridges = new List<GameObject>();
    [SerializeField] List<GameObject> deactiveDoubleObstacleBridges = new List<GameObject>();

    private int gap = 2;
    private int countBtwObs = 0;
    private int count = 0;
    private bool isGap = false;
    private float bridgeGap = 4f;
    private Transform spawn;
    public Transform lastBridgeSpawn;
    private Vector3 baseGravity = new Vector3(0, -18f, 0);

    private DestroyBridge_BridgeLevel destroyBridge_BridgeLevel;

    void OnEnable()
    {
        GameManager_BridgeLevel.bridgeSpawnerIce += StartSpawning;
    }

    void OnDisable()
    {
        GameManager_BridgeLevel.bridgeSpawnerIce -= StartSpawning;
    }

    void StartSpawning()
    {
        destroyBridge_BridgeLevel = GameObject.Find("DestroyBridge").GetComponent<DestroyBridge_BridgeLevel>();

        spawn = transform;
        gap = Random.Range(2, 3);

        for (int i = 0; i < 20; i++)
        {
            SpawnBridgeInternal();
        }
    }

    public void SpawnBridge()
    {
        GameObject oldest = activeBridges[0];
        activeBridges.RemoveAt(0);

        oldest.SetActive(false);
        AddToPool(oldest);

        SpawnBridgeInternal();
    }

    private void SpawnBridgeInternal()
    {
        bool spawnObstacle = (gap == countBtwObs);
        bool spawnDouble = (spawnObstacle && count >= 60);

        if (spawnObstacle)
        {
            countBtwObs = 0;
            gap = Random.Range(1, 3);
        }
        else
        {
            countBtwObs++;
        }

        if (spawnDouble)
        {
            bridgePrefab = doubleObstacleBridges[Random.Range(0, doubleObstacleBridges.Count)];
        }
        else if (spawnObstacle)
        {
            bridgePrefab = obstacleBridges[Random.Range(0, obstacleBridges.Count)];
        }
        else
        {
            bridgePrefab = bridges[Random.Range(0, bridges.Count)];
        }

        if (count % 4 == 0)
        {
            isGap = true;
            StartCoroutine(SpawnGeyser(spawn.position));
        }

        GameObject newBridge = GetFromPoolOrInstantiate(spawnObstacle, spawnDouble);

        newBridge.transform.position = spawn.position;
        if (isGap)
        {
            newBridge.transform.position += Vector3.forward * bridgeGap;
            isGap = false;
        }

        newBridge.SetActive(true);
        activeBridges.Add(newBridge);
        spawn = newBridge.transform.GetChild(0);

        count++;
        if (count % 8 == 0 && sideEnvirs.Count > 0)
        {
            int r = Random.Range(0, sideEnvirs.Count);
            Instantiate(sideEnvirs[r], newBridge.transform);
        }

        if (count % 20 == 0)
        {
            Time.timeScale += 0.1f * Time.timeScale;
            if (Time.timeScale > 1.75f) Time.timeScale = 1.75f;
            bridgeGap += 1f;

            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            if (player.childCount > 0)
            {
                player.GetChild(0).GetComponent<Animator>().speed = 1 / Time.timeScale;
            }
            Debug.Log($"TimeScale: {Time.timeScale}");
        }
    }

    private GameObject GetFromPoolOrInstantiate(bool isObstacle, bool isDouble)
    {
        if (isDouble)
        {
            if (deactiveDoubleObstacleBridges.Count > 0)
            {
                GameObject b = deactiveDoubleObstacleBridges[0];
                deactiveDoubleObstacleBridges.RemoveAt(0);
                return b;
            }
            return Instantiate(doubleObstacleBridges[Random.Range(0, doubleObstacleBridges.Count)]);
        }

        if (isObstacle)
        {
            if (deactiveObstacleBridges.Count > 0)
            {
                GameObject b = deactiveObstacleBridges[0];
                deactiveObstacleBridges.RemoveAt(0);
                return b;
            }
            return Instantiate(obstacleBridges[Random.Range(0, obstacleBridges.Count)]);
        }

        if (deactiveBridges.Count > 0)
        {
            GameObject b = deactiveBridges[0];
            deactiveBridges.RemoveAt(0);
            return b;
        }

        return Instantiate(bridges[Random.Range(0, bridges.Count)]);
    }

    private void AddToPool(GameObject bridge)
    {
        if (IsInList(bridge.name, doubleObstacleBridges))
        {
            deactiveDoubleObstacleBridges.Add(bridge);
        }
        else if (IsInList(bridge.name, obstacleBridges))
        {
            deactiveObstacleBridges.Add(bridge);
        }
        else
        {
            deactiveBridges.Add(bridge);
        }
    }

    private bool IsInList(string name, List<GameObject> list)
    {
        foreach (var prefab in list)
        {
            if (name.Contains(prefab.name))
                return true;
        }
        return false;
    }

    IEnumerator SpawnGeyser(Vector3 pos)
    {
        yield return new WaitForSeconds(23.5f);

        int rando = Random.Range(0, 3);
        float spawnX = 0f;
        float spawnX_2 = 0f;

        if (rando == 0)
        {
            spawnX = 1.9f;
            spawnX_2 = Random.Range(0, 2) == 0 ? 0.05f : -1.8f;
        }
        else if (rando == 1)
        {
            spawnX = -1.8f;
            spawnX_2 = Random.Range(0, 2) == 0 ? 0.05f : 1.9f;
        }
        else
        {
            spawnX = 0.05f;
            spawnX_2 = Random.Range(0, 2) == 0 ? 1.9f : -1.8f;
        }

        if (count >= 60)
        {
            Instantiate(geyserPrefab, new Vector3(spawnX, 0f, pos.z + 2f), Quaternion.identity);
            Instantiate(geyserPrefab, new Vector3(spawnX_2, 0f, pos.z + 2f), Quaternion.identity);
        }
        else
        {
            Instantiate(geyserPrefab, new Vector3(spawnX, 0f, pos.z + 2f), Quaternion.identity);
        }

        yield return new WaitForSeconds(1.75f);
        SoundManager_BridgeLevel.Instance.PlayExploSound();
    }
}
