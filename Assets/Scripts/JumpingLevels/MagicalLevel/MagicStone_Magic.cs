using UnityEngine;
using System;

public class MagicStone_Magic : MonoBehaviour
{
    public static event Action magicObstacle_Magic;
    bool once = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !once)
        {
            magicObstacle_Magic?.Invoke();
            once = true;
        }
    }
}
