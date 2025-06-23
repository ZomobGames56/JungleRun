using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RiverSpawner_Jumping : MonoBehaviour
{
    [SerializeField] private List<GameObject> activeRiver = new List<GameObject>();
    [SerializeField] private GameObject river;
    [SerializeField] private Vector3 pos;
    [SerializeField] private float yRot;
    int count = 0;

    void Start()
    {
        for(int i=0;i<3;i++){
            Spawn();
        }
    }

    public void Spawn(){
        GameObject temp = Instantiate(river,new Vector3(pos.x, pos.y, (pos.z * count)), Quaternion.Euler(0f, yRot, 0f));
        count++;
        activeRiver.Add(temp);
        if(activeRiver.Count > 5){
            Destroy(activeRiver[0]);
            activeRiver.RemoveAt(0);
        }
    }
}
