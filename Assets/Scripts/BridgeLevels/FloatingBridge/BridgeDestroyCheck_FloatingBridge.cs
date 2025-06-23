using UnityEngine;

public class BridgeDestroyCheck_FloatingBridge : MonoBehaviour
{
    private BridgeSpawner_FloatingBridge bridgeSpawner_FloatingBridge;

    void Start(){
        bridgeSpawner_FloatingBridge = GameObject.Find("BridgeSpawner").GetComponent<BridgeSpawner_FloatingBridge>();
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            bridgeSpawner_FloatingBridge.SpawnBridge();
        }
    }
}
