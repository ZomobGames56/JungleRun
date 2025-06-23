using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Coin_Jumping : MonoBehaviour
{
    private Tween upDown;
    void Start()
    {
        upDown = transform.DOMoveY(transform.position.y + 0.3f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            GameManager_Jumping.Instance.UpdateCoins(1);
            SoundManager_Jumping.Instance.PlayCoinsSound();
            if (upDown != null && upDown.IsActive())
            {
                upDown.Kill();
            }
            Destroy(gameObject);
        }
    }
}
