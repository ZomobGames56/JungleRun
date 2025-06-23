using UnityEngine;

public class EnvironmentCheck_Jumping : MonoBehaviour
{
    private EnvironmentSpawner_Jumping environmentSpawner_Jumping;

    void Start(){
        environmentSpawner_Jumping = GameObject.Find("EnvirSpawner").GetComponent<EnvironmentSpawner_Jumping>();
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            environmentSpawner_Jumping.Spawn();
        }
    }
}
