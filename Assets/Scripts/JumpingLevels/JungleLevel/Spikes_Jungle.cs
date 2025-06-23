using UnityEngine;

public class Spikes_Jungle : MonoBehaviour
{
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            GameManager_Jumping.Instance.EndGame();
            SoundManager_Jumping.Instance.PlayLoseSound("stab");
        }
    }
}
