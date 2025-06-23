using UnityEngine;

public class RiverCheck_Jumping : MonoBehaviour
{
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            GameManager_Jumping.Instance.EndGame();
            SoundManager_Jumping.Instance.PlayLoseSound("");
        }
    }
}
