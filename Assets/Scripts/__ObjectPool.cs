using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class __ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] List<GameObject> activeObj = new List<GameObject>();
    [SerializeField] List<GameObject> deactiveObj = new List<GameObject>();
    float spawnPos = 0f;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject a = Instantiate(prefab, new Vector3(0f, 0f, spawnPos), Quaternion.identity);
            activeObj.Add(a);
            spawnPos = a.transform.position.z + 10f;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Despawn();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Spawn();
        }
    }

    void Despawn()
    {
        GameObject t = activeObj[0];
        t.SetActive(false);
        deactiveObj.Add(t);
        activeObj.RemoveAt(0);
    }

    void Spawn()
    {
        if (deactiveObj.Count != 0)
        {
            GameObject t = deactiveObj[0];
            t.SetActive(true);
            t.transform.position = new Vector3(0f, 0f, spawnPos);
            spawnPos = t.transform.position.z + 10f;
            activeObj.Add(t);
            deactiveObj.RemoveAt(0);
        }
        else
        {
            GameObject t = Instantiate(prefab, new Vector3(0f, 0f, spawnPos), Quaternion.identity);
            t.SetActive(true);
            t.transform.position = new Vector3(0f, 0f, spawnPos);
            spawnPos = t.transform.position.z + 10f;
            activeObj.Add(t);
        }
    }
}
