using UnityEngine;

public class StoneDestroyCheck_Jumping : MonoBehaviour
{
    StoneSpawner_Jumping stoneSpawner_Jumping;

    private void Start()
    {
        stoneSpawner_Jumping = GameObject.Find("StoneSpawner").GetComponent<StoneSpawner_Jumping>();
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            stoneSpawner_Jumping.NextStone();
            GameManager_Jumping.Instance.UpdateTimeSpeed();
        }
    }
}
