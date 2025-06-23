using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnvironmentSpawner_Jumping : MonoBehaviour
{
    [SerializeField] private List<GameObject> environments = new List<GameObject>();
    [SerializeField] private List<GameObject> activeEnvironmentsList = new List<GameObject>();
    int count = 0;

    void Start(){
        for(int i=0;i<6;i++){
            Spawn();
        }
    }

    public void Spawn(){
        int j = Random.Range(0,environments.Count);
        Vector3 spawn = Vector3.zero;
        if(activeEnvironmentsList.Count > 0) spawn = activeEnvironmentsList[activeEnvironmentsList.Count-1].transform.GetChild(0).position;
        else spawn = transform.position;
        GameObject temp = Instantiate(environments[j], new Vector3(0f, environments[j].transform.position.y, spawn.z), environments[j].transform.rotation);
        count++;
        activeEnvironmentsList.Add(temp);
        if(activeEnvironmentsList.Count > 8){
            Destroy(activeEnvironmentsList[0]);
            activeEnvironmentsList.RemoveAt(0);
        }
    }
}
