using UnityEngine;

public class RiverSpawnCheck_Jumping : MonoBehaviour
{
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            GameObject.Find("EnvirSpawner").GetComponent<RiverSpawner_Jumping>().Spawn();
        }
    }
}
