using UnityEngine;

public class CoinMagnet_BridgeLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PowerupsBridge_BridgeLevel>().startMagnet = true;
            other.gameObject.GetComponent<PowerupsBridge_BridgeLevel>().timer = 0f;
            SoundManager_BridgeLevel.Instance.PlayCoinsSound();
            Debug.Log("Pick Up Magnet");
            Destroy(gameObject);
        }
    }
}
