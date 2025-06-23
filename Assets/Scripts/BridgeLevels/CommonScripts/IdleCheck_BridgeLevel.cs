using UnityEngine;

public class IdleCheck_BridgeLevel : MonoBehaviour
{
    float idleTimer = 0f;
    float idleTimeAllowed = 0.5f;

    void OnCollisionStay(Collision collision)
    {
        idleTimer += Time.unscaledDeltaTime;
        if (idleTimer >= idleTimeAllowed)
        {
            GameManager_BridgeLevel.Instance.EndGame();
            SoundManager_BridgeLevel.Instance.PlayLoseSound("");
            idleTimer = 0f;
        }
    }
}
