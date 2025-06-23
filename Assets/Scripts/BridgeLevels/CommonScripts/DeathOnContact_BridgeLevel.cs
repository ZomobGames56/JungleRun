using UnityEngine;

public class DeathOnContact_BridgeLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("BBBBBBBBBBB");
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("AAAAAAAAAAAAAAA");
            GameManager_BridgeLevel.Instance.EndGame();
            SoundManager_BridgeLevel.Instance.PlayLoseSound("");
        }
    }
}
