using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentSpawner_FloatingBridge : MonoBehaviour
{
    [SerializeField] List<GameObject> environments = new List<GameObject>();
    [SerializeField] List<GameObject> activeEnvironmentsList = new List<GameObject>();
    int count = 0;

    void Start()
    {
        for(int i=0;i<4;i++){
            Spawn();
        }    
    }

    public void Spawn(){
        int j = Random.Range(0,environments.Count-1);
        GameObject t = Instantiate(environments[j],new Vector3(0f,-25f,(54.28495f * count)), Quaternion.Euler(0f,90f,0f));
        count++;
        activeEnvironmentsList.Add(t);
        if(activeEnvironmentsList.Count > 6){
            Destroy(activeEnvironmentsList[0]);
            activeEnvironmentsList.RemoveAt(0);
        }
    }
}
