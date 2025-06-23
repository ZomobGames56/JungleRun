using UnityEngine;

public class BridgeDestroyCheck_Horror : MonoBehaviour
{
    private BridgeSpawner_Horror bridgeSpawner_Horror;

    void Start(){
        bridgeSpawner_Horror = GameObject.Find("BridgeSpawner").GetComponent<BridgeSpawner_Horror>();
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            bridgeSpawner_Horror.SpawnBridge();
        }
    }
}
