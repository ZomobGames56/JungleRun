using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawner_Lava : MonoBehaviour
{
    // GameObjects
    [Header("GameObjects")]
    [SerializeField] GameObject firePrefab;
    [SerializeField] GameObject geyserPrefab;
    [SerializeField] GameObject debrisPrefab;

    // Spawns
    [Header("Spawns")]
    Transform fireSpawn;
    Transform geyserSpawn;

    // Others
    [Header("Others")]
    // [SerializeField] AudioClip spikeAudio;
    [SerializeField] float leftrightSpeed;
    [SerializeField] float debrisSpeed;
    public bool stoneMoved = false;
    public bool multipleStones;

    // Private variables
    GameObject stoneToMove;
    string dirToMove;
    GameObject player;
    bool allowTrap = false;
    string trap = "";

    public void Start()
    {
        // stoneToMove = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        if (transform.position.z > 52)
        {
            if (!multipleStones)
            {
                int rando = Random.Range(0, 100);
                if (rando < 50)
                {
                    // CheckMovement();
                    trap = "Move";
                }
                else
                {
                    // SpawnFire(false);
                    trap = "Lava";
                }

            }
            else
            {
                int rando = Random.Range(0, 100);
                if (rando < 25)
                {
                    // CheckMovement();
                    trap = "Move";
                }
                else if (rando >= 25 && rando < 50)
                {
                    // Disappear();
                    trap = "Disappear";
                }
                else if (rando >= 50 && rando < 75)
                {
                    // SpawnFire(false);
                    trap = "Lava";
                }
                else
                {
                    trap = "Debris";
                }
            }
        }
        // if(fireSpawn != null) spawn = fireSpawn;
        // else spawn = fireSpawn2;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
        }

        if (GameManager_Jumping.Instance.paused) return;
        if (Mathf.Abs(transform.position.z - player.transform.position.z) < 16 && !allowTrap)
        {
            switch (trap)
            {
                case "Move":
                    CheckMovement();
                    break;

                case "Disappear":
                    Disappear();
                    break;

                case "Lava":
                    SpawnGeyser();
                    break;

                default:
                    break;
            }
            allowTrap = true;
        }
        if (Mathf.Abs(transform.position.z - player.transform.position.z) < 28 && !allowTrap && trap == "Debris")
        {
            SpawnDebris();
            Debug.Log("DEEBBBRRISSS");
            allowTrap = true;
        }
    }

    void CheckMovement()
    {
        int rando = Random.Range(0, 100);
        if (rando < 75)
        {
            switch (transform.name)
            {
                case "Stone - Left + Middle(Clone)":
                    stoneToMove = transform.GetChild(1).gameObject;
                    dirToMove = "right";
                    break;

                case "Stone - Left + Right(Clone)":
                    int a = Random.Range(0, 2);
                    if (a == 1)
                    {
                        stoneToMove = transform.GetChild(0).gameObject;
                        dirToMove = "left";
                    }
                    else
                    {
                        stoneToMove = transform.GetChild(1).gameObject;
                        dirToMove = "right";
                    }
                    break;

                case "Stone - Left(Clone)":
                    stoneToMove = transform.GetChild(0).gameObject;
                    dirToMove = "right";
                    break;

                case "Stone - Middle(Clone)":
                    stoneToMove = transform.GetChild(0).gameObject;
                    int b = Random.Range(0, 2);
                    if (b == 1) dirToMove = "right";
                    else dirToMove = "left";
                    break;

                case "Stone - Right + Middle(Clone)":
                    stoneToMove = transform.GetChild(0).gameObject;
                    dirToMove = "left";
                    break;

                case "Stone - Right(Clone)":
                    stoneToMove = transform.GetChild(0).gameObject;
                    dirToMove = "left";
                    break;

                default:
                    break;
            }
            stoneMoved = true;
            StartCoroutine(MoveTo(dirToMove));
        }
    }

    IEnumerator MoveTo(string dirToMove)
    {
        float dir = 0f;
        if (dirToMove == "left")
        {
            dir = -5f;
        }
        else
        {
            dir = 5f;
        }
        Vector3 target = new Vector3(stoneToMove.transform.position.x + dir, stoneToMove.transform.position.y, stoneToMove.transform.position.z);
        while (Mathf.Abs(stoneToMove.transform.position.x - dir) > 0.1f)
        {
            stoneToMove.transform.position = Vector3.Lerp(stoneToMove.transform.position, target, leftrightSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void SpawnDebris()
    {
        int rando = Random.Range(0, 100);
        if (rando < 75)
        {
            switch (transform.name)
            {
                case "Stone - Left + Middle(Clone)":
                case "Stone - Left + Right(Clone)":
                case "Stone - Right + Middle(Clone)":
                    int a = Random.Range(0, 2);
                    if (a == 1)
                    {
                        stoneToMove = transform.GetChild(2).gameObject;
                    }
                    else
                    {
                        stoneToMove = transform.GetChild(3).gameObject;
                    }
                    break;

                case "Stone - Left(Clone)":
                case "Stone - Middle(Clone)":
                case "Stone - Right(Clone)":
                    stoneToMove = transform.GetChild(2).gameObject;
                    break;

                default:
                    break;
            }
        }
        GameObject debris = Instantiate(debrisPrefab, new Vector3(stoneToMove.transform.position.x, stoneToMove.transform.position.y + 50f, stoneToMove.transform.position.z + 50f), Quaternion.Euler(-15f, 0f, 0f));
        StartCoroutine(DebrisFalling(debris));
    }

    IEnumerator DebrisFalling(GameObject debris)
    {
        Vector3 target = new Vector3(stoneToMove.transform.position.x, stoneToMove.transform.position.y, stoneToMove.transform.position.z + 2f);
        while (Vector3.Distance(target, debris.transform.position) > 0.1f)
        {
            debris.transform.position = Vector3.Lerp(debris.transform.position, target, debrisSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void Disappear()
    {
        int rando = Random.Range(0, 100);
        if (rando < 75)
        {
            StartCoroutine(DrownBlock());
        }
    }

    IEnumerator DrownBlock()
    {
        int rando = Random.Range(0, 2);
        Transform child = transform.GetChild(0);
        SoundManager_Jumping.Instance.PlayLoseSound("");
        if (rando == 1) child = transform.GetChild(1);
        Vector3 target = new Vector3(child.position.x, -10f, child.position.z);
        while (Mathf.Abs(child.position.y - target.y) > 0.1f)
        {
            child.position = Vector3.Lerp(child.position, target, 0.5f * Time.unscaledDeltaTime);
            yield return null;
        }
    }

    void SpawnFire()
    {
        int rando = Random.Range(0, 100);
        if (rando < 75)
        {
            if (multipleStones)
            {
                int rando2 = Random.Range(0, 2);
                fireSpawn = transform.GetChild(rando2).GetChild(1);
            }
            else
            {
                fireSpawn = transform.GetChild(0).GetChild(1);
            }
            Instantiate(firePrefab, fireSpawn);
        }
    }

    void SpawnGeyser()
    {
        int rando = Random.Range(0, 100);
        if (rando < 75)
        {
            int rando2 = Random.Range(0, 2);
            geyserSpawn = transform.GetChild(rando2);
            GameObject geyser = Instantiate(geyserPrefab, geyserSpawn);
            geyser.transform.SetParent(null);
            Invoke("PlayExploSound", 1f);
        }
    }

    void PlayExploSound()
    {
        SoundManager_Jumping.Instance.PlayExplosionSound();
    }
}
