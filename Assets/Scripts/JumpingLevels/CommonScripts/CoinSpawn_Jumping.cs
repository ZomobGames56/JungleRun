using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinSpawn_Jumping : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;

    void Start(){
        int rando = Random.Range(0,100);
        if(rando < 10){
            SpawnCoins();
        }
    }

    void SpawnCoins(){
        Instantiate(coinPrefab, transform.GetChild(0));
    }
}
