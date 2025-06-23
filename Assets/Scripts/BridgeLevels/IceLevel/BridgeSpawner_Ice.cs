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
    int gap = 2;
    int countBtwObs = 0;
    private int count = 0;
    private Transform spawn;
    private Vector3 baseGravity = new Vector3(0, -18f, 0);
    private DestroyBridge_BridgeLevel destroyBridge_BridgeLevel;
    public Transform lastBridgeSpawn;

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
        destroyBridge_BridgeLevel = GameObject.Find("DestroyBridge").transform.GetComponent<DestroyBridge_BridgeLevel>();
        if (!GameManager_BridgeLevel.startTutorial) spawn = transform;
        else spawn = lastBridgeSpawn;
        gap = Random.Range(2, 5);
        for (int i = 0; i < 20; i++)
        {
            if (gap == countBtwObs)
            {
                if (count >= 60)
                {
                    int rando = Random.Range(0, doubleObstacleBridges.Count);
                    bridgePrefab = doubleObstacleBridges[rando];
                }
                else
                {
                    int rando = Random.Range(0, obstacleBridges.Count);
                    bridgePrefab = obstacleBridges[rando];
                }
                GameObject t = Instantiate(bridgePrefab, spawn);
                t.transform.SetParent(null);
                activeBridges.Add(t);
                spawn = activeBridges[activeBridges.Count - 1].transform.GetChild(0);
                countBtwObs = 0;
                gap = Random.Range(2, 5);
            }
            else
            {
                int rando = Random.Range(0, bridges.Count);
                bridgePrefab = bridges[rando];
                GameObject t = Instantiate(bridgePrefab, spawn);
                t.transform.SetParent(null);
                activeBridges.Add(t);
                spawn = activeBridges[activeBridges.Count - 1].transform.GetChild(0);
                countBtwObs++;
            }
        }
    }

    public void SpawnBridge(){
        Destroy(activeBridges[0]);
        activeBridges.RemoveAt(0);
        if(gap == countBtwObs){
            if (count >= 60)
            {
                int rando = Random.Range(0, doubleObstacleBridges.Count);
                bridgePrefab = doubleObstacleBridges[rando];
            }
            else
            {
                int rando = Random.Range(0, obstacleBridges.Count);
                bridgePrefab = obstacleBridges[rando];
            }
            countBtwObs = 0;
            gap = Random.Range(2,5);
        }
        else{
            int rando = Random.Range(0,bridges.Count);
            bridgePrefab = bridges[rando];
            countBtwObs++;
        }
        GameObject t = Instantiate(bridgePrefab, spawn);
        t.transform.SetParent(null);
        activeBridges.Add(t);
        spawn = activeBridges[activeBridges.Count - 1].transform.GetChild(0);
        count++;
        if(count%4 == 0){
            if (activeBridges[4].name == "FullBridge(Clone)")
            {
                // StopAllCoroutines();
                destroyBridge_BridgeLevel.DestroyBridge(activeBridges[5].transform);
                StartCoroutine(SpawnGeyser(activeBridges[4].transform.GetChild(0).position));
            }
        }

        if (count % 8 == 0)
        {
            int rando = Random.Range(0, sideEnvirs.Count);
            Instantiate(sideEnvirs[rando], activeBridges[activeBridges.Count - 1].transform);
        }

        if (count % 20 == 0)
        {
            Time.timeScale += 0.1f * Time.timeScale;
            Debug.Log(Time.timeScale);
            if (Time.timeScale > 1.75f) Time.timeScale = 1.75f;
            GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<Animator>().speed = 1 / (Time.timeScale);
            // Physics.gravity = baseGravity / Time.timeScale;
            // Debug.Log($"TimeScale: {Time.timeScale}, Gravity: {Physics.gravity}");
        }
    }

    IEnumerator SpawnGeyser(Vector3 pos)
    {
        yield return new WaitForSeconds(4f);
        int rando = Random.Range(0, 3);
        float spawnX = 0f;
        float spawnX_2 = 0f;
        if (rando == 0)
        {
            spawnX = 1.9f;
            int rando2 = Random.Range(0, 2);
            if (rando2 == 0) spawnX_2 = 0.05f;
            else spawnX_2 = -1.8f;
        }
        else if (rando == 1)
        {
            spawnX = -1.8f;
            int rando2 = Random.Range(0, 2);
            if (rando2 == 0) spawnX_2 = 0.05f;
            else spawnX_2 = 1.9f;
        }
        else
        {
            spawnX = 0.05f;
            int rando2 = Random.Range(0, 2);
            if (rando2 == 0) spawnX_2 = 1.9f;
            else spawnX_2 = -1.8f;
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
