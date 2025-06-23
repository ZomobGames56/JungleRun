using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner_Horror : MonoBehaviour
{
    private GameObject bridgePrefab;
    public List<GameObject> bridges = new List<GameObject>();
    public List<GameObject> obstacleBridges = new List<GameObject>();
    public List<GameObject> activeBridges = new List<GameObject>();
    int gap = 2;
    int countBtwObs = 0;
    private int count = 0;
    private Transform spawn;
    public Transform lastBridgeSpawn;

    void Start()
    {
        if (!GameManager_BridgeLevel.startTutorial) spawn = transform;
        else spawn = lastBridgeSpawn;
        gap = Random.Range(2, 4);
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
                gap = Random.Range(2, 4);
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
            gap = Random.Range(2, 4);
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

        if (count % 20 == 0)
        {
            Time.timeScale += 0.1f * Time.timeScale;
            Debug.Log(Time.timeScale);
            if (Time.timeScale > 1.75f) Time.timeScale = 1.75f;
            GameObject.FindGameObjectWithTag("Player").transform.GetChild(GameObject.FindGameObjectWithTag("Player").transform.childCount - 2).GetComponent<Animator>().speed = 1 / (Time.timeScale);
            // GameManager_BridgeLevel.Instance.UpdateSpeed();
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
