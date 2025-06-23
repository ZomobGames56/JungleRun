using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn_FloatingBridge : MonoBehaviour
{
    [SerializeField] GameObject magnetPrefab;
    [SerializeField] GameObject gliderCollectiblePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] Transform spawnPoint;
    private float timer;
    private float cooldown;
    public float minCooldown;
    public float maxCooldown;
    float spawnHeight;

    void OnEnable()
    {
        GameManager_BridgeLevel.coinSpawnFloatingBridge += ToggleScript;
    }

    void OnDisable()
    {
        GameManager_BridgeLevel.coinSpawnFloatingBridge -= ToggleScript;
    }

    void ToggleScript(bool enable)
    {
        this.enabled = enable;
    }

    private void Update()
    {
        if (GameManager_BridgeLevel.Instance.startGame)
        {
            timer += Time.deltaTime;
            cooldown = Random.Range(minCooldown, maxCooldown);
            if (timer > cooldown)
            {
                if (GameManager_BridgeLevel.Instance.powerUpAllowed)
                {
                    StartCoroutine(SpawnPowerup());
                    GameManager_BridgeLevel.Instance.powerUpAllowed = false;
                }
                else StartCoroutine(SpawnCoins());
                timer = 0;
            }
        }
    }

    IEnumerator SpawnCoins(){
        for(int i = 0; i < Random.Range(1, 6); i++){
            int a = Random.Range(0, 2);
            float spawnX = 0;
            if(a == 1){
                spawnX = -0.8f;
            }
            else{
                spawnX = 0.8f;
            }
            if (GameManager_BridgeLevel.Instance.startGlide) spawnHeight = 3.5f;
            else spawnHeight = 0.5f;
            GameObject spawnedCoin = Instantiate(coinPrefab, new Vector3(spawnX, spawnHeight, spawnPoint.position.z), Quaternion.Euler(0f, coinPrefab.transform.rotation.y, coinPrefab.transform.rotation.z));
            spawnedCoin.transform.SetParent(null);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator SpawnPowerup(){
        int a = Random.Range(0, 2);
        float spawnX = 0;
        if(a == 1){
            spawnX = -0.8f;
        }
        else{
            spawnX = 0.8f;
        }
        // GameObject prefab = gliderCollectiblePrefab; 
        GameObject prefab = null; 
        a = Random.Range(0,2);
        if(a == 1 || GameManager_BridgeLevel.Instance.startGlide){
            prefab = magnetPrefab;
        }
        else{
            prefab = gliderCollectiblePrefab;
        }
        GameObject spawnedPowerup = Instantiate(prefab, new Vector3(spawnX, spawnHeight, spawnPoint.position.z), prefab.transform.rotation);
        spawnedPowerup.transform.SetParent(null);
        yield return new WaitForSeconds(1f);
    }
}
