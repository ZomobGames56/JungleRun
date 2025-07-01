using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinSpawn_Jumping : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;

    void OnEnable(){
        int rando = Random.Range(0,100);
        if(rando < 50){
            SpawnCoins();
        }
    }

    void SpawnCoins(){
        if(transform.GetChild(0).childCount == 0)
            Instantiate(coinPrefab, transform.GetChild(0));
    }
}
