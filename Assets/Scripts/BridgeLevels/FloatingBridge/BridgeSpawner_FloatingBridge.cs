using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner_FloatingBridge : MonoBehaviour
{
    private GameObject bridgePrefab;
    public List<GameObject> bridges = new List<GameObject>();
    public List<GameObject> obstacleBridges = new List<GameObject>();
    public List<GameObject> sideEnvirs = new List<GameObject>();
    public List<GameObject> activeBridges = new List<GameObject>();
    int gap = 2;
    int countBtwObs = 0;
    private int count = 0;
    private Transform spawn;
    public Transform lastBridgeSpawn;
    private DestroyBridge_BridgeLevel destroyBridge_BridgeLevel;

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
        if (!GameManager_BridgeLevel.startTutorial) spawn = transform;
        else spawn = lastBridgeSpawn;
        gap = Random.Range(2, 5);
        for (int i = 0; i < 20; i++)
        {
            if (gap == countBtwObs)
            {
                int rando = Random.Range(0, obstacleBridges.Count);
                bridgePrefab = obstacleBridges[rando];
                GameObject t = Instantiate(bridgePrefab, spawn);
                t.transform.SetParent(null);
                activeBridges.Add(t);
                spawn = activeBridges[activeBridges.Count - 1].transform.GetChild(0);
                countBtwObs = 0;
                gap = Random.Range(2, 6);
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

    public void SpawnBridge()
    {
        Destroy(activeBridges[0]);
        activeBridges.RemoveAt(0);
        if (gap == countBtwObs)
        {
            int rando = Random.Range(0, obstacleBridges.Count);
            bridgePrefab = obstacleBridges[rando];
            GameObject t = Instantiate(bridgePrefab, spawn);
            t.transform.SetParent(null);
            activeBridges.Add(t);
            spawn = activeBridges[activeBridges.Count - 1].transform.GetChild(0);
            countBtwObs = 0;
            gap = Random.Range(2, 6);
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
        count++;
        if (count % 4 == 0)
        {
            if (activeBridges[6].name != "EmptyBridge(Clone)" && activeBridges[4].name != "EmptyBridge(Clone)" && activeBridges[6].name != "EmptyBridge2(Clone)" && activeBridges[4].name != "EmptyBridge2(Clone)" && activeBridges[6].name != "MiddleGapBridge(Clone)" && activeBridges[4].name != "MiddleGapBridge(Clone)")
            {
                StopAllCoroutines();
                // StartCoroutine(Destroying(activeBridges[5].transform));
                destroyBridge_BridgeLevel.DestroyBridge(activeBridges[5].transform);
            }
        }

        if (count % 8 == 0)
        {
            int rando = Random.Range(0, sideEnvirs.Count);
            Instantiate(sideEnvirs[rando], activeBridges[activeBridges.Count - 1].transform);
        }

        if (count % 20 == 0)
        {
            GameManager_BridgeLevel.Instance.UpdateSpeed();
        }
    }

    IEnumerator Destroying(Transform obj){
        for(int j=0; j < obj.childCount; j++){
            obj.GetChild(j).gameObject.AddComponent<Rigidbody>();
            Collider col = obj.GetChild(j).GetComponent<Collider>();
            if(col != null) Destroy(col);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
