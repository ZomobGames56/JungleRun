using UnityEngine;

public class Explosion_Lava : MonoBehaviour
{
    void Start()
    {
        Invoke("DestroyObj", 2f);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }
    
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            GameManager_Jumping.Instance.EndGame();
            SoundManager_Jumping.Instance.PlayLoseSound("fire");
        }
    }
}
