using UnityEngine;

public class Ghost_Horror : MonoBehaviour
{
    [SerializeField] float minCooldown;
    [SerializeField] float maxCooldowm;
    [SerializeField] GameObject ghostPrefab;
    [SerializeField] Transform spawn;
    float rando = 0f;
    float timer = 0f;

    void Start()
    {
        rando = Random.Range(minCooldown, maxCooldowm);
    }

    void Update()
    {
        if (GameManager_BridgeLevel.Instance.startGame && !GameManager_BridgeLevel.Instance.paused)
        {
            timer += Time.deltaTime;
            if (timer >= rando)
            {
                Instantiate(ghostPrefab, spawn);
                timer = 0f;
                rando = Random.Range(minCooldown, maxCooldowm);
            }
        }
    }
}
