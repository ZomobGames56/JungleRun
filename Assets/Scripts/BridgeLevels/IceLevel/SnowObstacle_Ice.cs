using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnowObstacle_Ice : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    GameObject snowScreen;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Instantiate(explosion, transform);
            SoundManager_BridgeLevel.Instance.PlaySnowSound();
            snowScreen = GameObject.Find("SnowScreenCanvas");
            int rando = Random.Range(0, snowScreen.transform.childCount);
            GameManager_BridgeLevel.Instance.EnableSnowScreen(snowScreen.transform.GetChild(rando).gameObject);
            Invoke("DestroyObj", 2f);
        }
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }
}
