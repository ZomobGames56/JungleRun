using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

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
    GameObject debrisInstance;
    GameObject geyserInstance;
    float originalPosX;

    public void Start()
    {
        // stoneToMove = transform.GetChild(0).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        if (transform.position.z > 52)
        {
            if (!multipleStones)
            {
                int rando = Random.Range(0, 100);
                if (rando < 25)
                {
                    // CheckMovement();
                    trap = "Move";
                }
                else if (rando >= 50 && rando < 75)
                {
                    // SpawnFire(false);
                    trap = "Debris";
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
        if (rando < 85)
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
            float dir = 0f;
            if (dirToMove == "left")
            {
                dir = -5f;
            }
            else
            {
                dir = 5f;
            }
            originalPosX = stoneToMove.transform.position.x;
            stoneToMove.transform.DOMoveX(stoneToMove.transform.position.x + dir, leftrightSpeed).SetEase(Ease.InOutSine);
        }
    }

    void SpawnDebris()
    {
        int rando = Random.Range(0, 100);
        if (rando < 85)
        {
            switch (transform.name)
            {
                case "Stone - Left + Middle(Clone)":
                case "Stone - Left + Right(Clone)":
                case "Stone - Right + Middle(Clone)":
                    int a = Random.Range(0, 2);
                    if (a == 1)
                    {
                        stoneToMove = transform.GetChild(2).GetChild(0).gameObject;
                    }
                    else
                    {
                        stoneToMove = transform.GetChild(3).GetChild(0).gameObject;
                    }
                    break;

                case "Stone - Left(Clone)":
                case "Stone - Middle(Clone)":
                case "Stone - Right(Clone)":
                    stoneToMove = transform.GetChild(2).GetChild(0).gameObject;
                    break;

                default:
                    break;
            }
        }
        if (stoneToMove != null)
        {
            Debug.Log(gameObject.name);
            debrisInstance = Instantiate(debrisPrefab, new Vector3(stoneToMove.transform.position.x, stoneToMove.transform.position.y + 20f, stoneToMove.transform.position.z + 100f), Quaternion.Euler(-15f, 0f, 0f));
            Vector3 target = new Vector3(stoneToMove.transform.position.x, stoneToMove.transform.position.y, stoneToMove.transform.position.z);
            debrisInstance.transform.DOMove(target, debrisSpeed).SetEase(Ease.InOutSine);
            // StartCoroutine(DebrisFalling(debris));
        }
    }

    void Disappear()
    {
        int rando = Random.Range(0, 100);
        if (rando < 85)
        {
            // StartCoroutine(DrownBlock());
            int rando2 = Random.Range(0, 2);
            Transform child = transform.GetChild(transform.childCount - 1);
            if (rando2 == 1) child = transform.GetChild(transform.childCount - 2);
            SoundManager_Jumping.Instance.PlayLoseSound("");
            stoneToMove = child.gameObject;
            child.DOMoveY(-10f, 4f).SetEase(Ease.InOutSine);
        }
    }

    void SpawnGeyser()
    {
        int rando = Random.Range(0, 100);
        if (rando < 85)
        {
            int rando2 = Random.Range(0, 2);
            geyserSpawn = transform.GetChild(rando2);
            geyserInstance = Instantiate(geyserPrefab, geyserSpawn);
            geyserInstance.transform.SetParent(null);
            Invoke("PlayExploSound", 1f);
        }
    }

    void PlayExploSound()
    {
        SoundManager_Jumping.Instance.PlayExplosionSound();
    }

    void OnDisable()
    {
        StopAllCoroutines();

        // Reset moving stone
        if (trap == "Move" && stoneToMove != null)
        {
            stoneToMove.transform.DOKill();
            stoneToMove.transform.position = new Vector3(originalPosX, stoneToMove.transform.position.y, stoneToMove.transform.position.z);
            stoneMoved = false;
        }

        // Remove debris
        if (trap == "Debris" && debrisInstance != null)
        {
            debrisInstance.transform.DOKill();
            Destroy(debrisInstance);
            debrisInstance = null;
        }

        // Remove geyser
        if (trap == "Lava" && geyserInstance != null)
        {
            geyserInstance.transform.DOKill();
            Destroy(geyserInstance);
            geyserInstance = null;
        }

        // For disappear trap, re-set block Y to original
        if (trap == "Disappear" && stoneToMove != null)
        {
            stoneToMove.transform.DOKill();
            stoneToMove.transform.localPosition = new Vector3(stoneToMove.transform.localPosition.x, -0.5f, stoneToMove.transform.localPosition.z);
        }

        allowTrap = false;
        stoneToMove = null;
    }

    void OnDestroy()
    {
        DOTween.Kill(gameObject);
    }
}
