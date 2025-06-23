using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Coin_Jumping_Prototype : MonoBehaviour
{
    void Start(){
        StartCoroutine(UpDown());
    }

    IEnumerator UpDown(){
        while(true){
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
            yield return new WaitForSeconds(0.3f);
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            GameManager_Jumping_Prototype.Instance.UpdateCoins("Gain", 1);
            SoundManager_Jumping_Prototype.Instance.PlayCoinsSound();
            Destroy(gameObject);
        }
    }
}
