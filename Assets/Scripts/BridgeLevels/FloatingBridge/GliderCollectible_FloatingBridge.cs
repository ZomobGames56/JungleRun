using UnityEngine;

public class GliderCollectible_FloatingBridge : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            // collider.gameObject.GetComponent<PowerupsBridge>().startGlide = true;
            GameManager_BridgeLevel.Instance.gliderCollected = true;
            SoundManager_BridgeLevel.Instance.PlayCoinsSound();
            Debug.Log("Pick Up Glider");
            Destroy(gameObject);
        }
    }
}
