using UnityEngine;

public class IdleCheck_BridgeLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            GameManager_BridgeLevel.Instance.EndGame();
            SoundManager_BridgeLevel.Instance.PlayLoseSound("");
        }
    }
}
