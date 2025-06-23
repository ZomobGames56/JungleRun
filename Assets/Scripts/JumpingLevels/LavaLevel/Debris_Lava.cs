using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Debris_Lava : MonoBehaviour
{
    [SerializeField] GameObject explosion;

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Ground")){
            GameObject explo = Instantiate(explosion, transform.position, transform.rotation);
            explo.transform.SetParent(null);
            SoundManager_Jumping.Instance.PlayExplosionSound();
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject explo = Instantiate(explosion, transform.position, transform.rotation);
            explo.transform.SetParent(null);
            SoundManager_Jumping.Instance.PlayExplosionSound();
            Destroy(gameObject);
        }
    }
}
