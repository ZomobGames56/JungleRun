using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin_BridgeLevel : MonoBehaviour
{
    private Transform player;
    private Tween upDown;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Invoke("DestroyObject", 8f);
        upDown = transform.DOMoveY(transform.position.y + 0.3f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    void DestroyObject(){
        Destroy(gameObject);
    }

   void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            GameManager_BridgeLevel.Instance.UpdateCoins("Gain", 1);
            SoundManager_BridgeLevel.Instance.PlayCoinsSound();
            if (upDown != null && upDown.IsActive())
            {
                upDown.Kill();
            }
            Destroy(gameObject);
        }
    }

    public void Movement(Transform pos){
        StartCoroutine(Move(pos));
    }

    IEnumerator Move(Transform pos){
        while(Vector3.Distance(transform.position, pos.position) > 0.01f){
            transform.position = Vector3.MoveTowards(transform.position, pos.position, 2f * Time.deltaTime);
            yield return null;
        }
    }
}
