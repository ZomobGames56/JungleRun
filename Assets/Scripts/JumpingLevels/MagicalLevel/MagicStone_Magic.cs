using UnityEngine;
using System;

public class MagicStone_Magic : MonoBehaviour
{
    public static event Action magicObstacle_Magic;
    bool once = false;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !once)
        {
            magicObstacle_Magic?.Invoke();
            once = true;
        }
    }
}
