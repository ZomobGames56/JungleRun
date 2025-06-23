using UnityEngine;

public class StoneDestroyCheck_Jumping_Prototype : MonoBehaviour
{
    StoneSpawner_Jumping_Prototype stoneSpawner_Jumping_Prototype;

    private void Start()
    {
        stoneSpawner_Jumping_Prototype = GameObject.Find("StoneSpawner").GetComponent<StoneSpawner_Jumping_Prototype>();
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            stoneSpawner_Jumping_Prototype.NextStone();
            GameManager_Jumping_Prototype.Instance.UpdateTimeSpeed();
        }
    }
}
