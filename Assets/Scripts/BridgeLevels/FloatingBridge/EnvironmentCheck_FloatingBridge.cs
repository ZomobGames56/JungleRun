using UnityEngine;

public class EnvironmentCheck_FloatingBridge : MonoBehaviour
{
    private EnvironmentSpawner_FloatingBridge envirSpawner;

    void Start(){
        envirSpawner = GameObject.Find("EnvironmnetSpawner").GetComponent<EnvironmentSpawner_FloatingBridge>();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            envirSpawner.Spawn();
        }
    }
}
