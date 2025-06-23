using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyBridge_BridgeLevel : MonoBehaviour
{
    public void DestroyBridge(Transform obj)
    {
        StartCoroutine(Destroying(obj));
    }

    IEnumerator Destroying(Transform obj){
        for(int j=0; j < obj.childCount; j++){
            obj.GetChild(j).gameObject.AddComponent<Rigidbody>();
            Collider col = obj.GetChild(j).GetComponent<Collider>();
            if(col != null) Destroy(col);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
