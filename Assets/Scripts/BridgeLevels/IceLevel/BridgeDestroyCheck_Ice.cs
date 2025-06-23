using UnityEngine;

public class BridgeDestroyCheck_Ice : MonoBehaviour
{
    private BridgeSpawner_Ice bridgeSpawner_Ice;

    void Start(){
        bridgeSpawner_Ice = GameObject.Find("BridgeSpawner").GetComponent<BridgeSpawner_Ice>();
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            bridgeSpawner_Ice.SpawnBridge();
        }
    }
}
