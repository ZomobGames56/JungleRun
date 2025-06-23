using UnityEngine;

public class SnowSpawn_Ice : MonoBehaviour
{
    [SerializeField] Transform spawn;
    [SerializeField] Transform spawn2;
    [SerializeField] Transform spawn3;
    [SerializeField] GameObject snow;

    void Start()
    {
        int rando = Random.Range(0, 100);
        if (rando < 10)
        {
            Instantiate(snow, spawn);
            Instantiate(snow, spawn2);
            Instantiate(snow, spawn3);
        }
    }
}
