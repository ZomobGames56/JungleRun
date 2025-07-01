using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ObstacleSpawner_Jungle : MonoBehaviour
{
    // GameObjects
    [Header("GameObjects")]
    [SerializeField] GameObject spikes;
    [SerializeField] GameObject firePrefab;
    [SerializeField] GameObject fireCannon;

    // Spawns
    [Header("Spawns")]
    [SerializeField] Transform fireSpawn;
    [SerializeField] Transform fireSpawn2;

    // Others
    [Header("Others")]
    [SerializeField] AudioClip spikeAudio;
    [SerializeField] float leftrightSpeed;
    public bool stoneMoved = false;
    public bool multipleStones;

    // Private variables
    GameObject stoneToMove;
    string dirToMove;
    GameObject player;
    bool allowTrap = false;
    string trap = "";
    bool onceCalled = false;
    Transform spawn = null;
    float originalPos;
    GameObject fire;
    GameObject sp;

    public void Start()
    {
        stoneToMove = transform.GetChild(0).gameObject;
        player = GameObject.Find("Player");
        if (transform.position.z > 52)
        {
            if (!multipleStones)
            {
                int rando = Random.Range(0, 100);
                if (rando < 33)
                {
                    // CheckMovement();
                    trap = "Move";
                }
                else if (rando >= 33 && rando < 66)
                {
                    SpawnFire(false);
                    trap = "Fire";
                }
                else
                {
                    trap = "Spikes";
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
                else if (rando >= 50 && rando < 85)
                {
                    SpawnFire(false);
                    trap = "Fire";
                }
                else
                {
                    trap = "Spikes";
                }
            }
        }
        // if(fireSpawn != null) spawn = fireSpawn;
        // else spawn = fireSpawn2;
    }

    void Update()
    {
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

                case "Fire":
                    SpawnFire(true);
                    break;

                case "Spikes":
                    MoveSpikes();
                    break;

                default:
                    break;
            }
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
                    stoneToMove = transform.GetChild(2).gameObject;
                    dirToMove = "right";
                    break;

                case "Stone - Left + Right(Clone)":
                    int a = Random.Range(0, 2);
                    if (a == 1)
                    {
                        stoneToMove = transform.GetChild(3).gameObject;
                        dirToMove = "left";
                    }
                    else
                    {
                        stoneToMove = transform.GetChild(2).gameObject;
                        dirToMove = "right";
                    }
                    break;

                case "Stone - Left(Clone)":
                    stoneToMove = transform.GetChild(1).gameObject;
                    dirToMove = "right";
                    break;

                case "Stone - Middle(Clone)":
                    stoneToMove = transform.GetChild(0).gameObject;
                    int b = Random.Range(0, 2);
                    if (b == 1) dirToMove = "right";
                    else dirToMove = "left";
                    break;

                case "Stone - Right + Middle(Clone)":
                    stoneToMove = transform.GetChild(1).gameObject;
                    dirToMove = "left";
                    break;

                case "Stone - Right(Clone)":
                    stoneToMove = transform.GetChild(1).gameObject;
                    dirToMove = "left";
                    break;

                default:
                    break;
            }
            stoneMoved = true;
            // StartCoroutine(MoveTo(dirToMove));
            float dir = 0f;
            if (dirToMove == "left")
            {
                dir = -4f;
            }
            else
            {
                dir = 4f;
            }
            SoundManager_Jumping.Instance.PlayRiverSound();
            originalPos = stoneToMove.transform.position.x;
            stoneToMove.transform.DOMoveX(stoneToMove.transform.position.x + dir, leftrightSpeed).SetEase(Ease.InOutSine);
        }
    }

    void MoveSpikes()
    {
        int rando = Random.Range(0, 100);
        if (rando < 85)
        {
            int rando2 = Random.Range(0, 2);
            Transform child;
            if (transform.name == "Stone - Left + Right(Clone)") child = transform.GetChild(2).GetChild(1);
            else if (transform.name == "Stone - Middle(Clone)") child = transform.GetChild(0).GetChild(1);
            else child = transform.GetChild(1).GetChild(1);

            if (rando2 == 1 && multipleStones)
            {
                if (transform.name == "Stone - Left + Right(Clone)") child = transform.GetChild(3).GetChild(1);
                else child = transform.GetChild(2).GetChild(1);
            }

            if (child.name == "MagicStoneSpikes") return;
            sp = Instantiate(spikes, child.position, child.rotation, child);
            sp.GetComponent<AudioSource>().PlayOneShot(spikeAudio);
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

    void SpawnFire(bool secondCall)
    {
        if (transform.name == "Stone - Middle(Clone)") return;
        int rando = Random.Range(0, 100);
        if (!secondCall)
        {
            if (rando < 85)
            {
                onceCalled = true;
                switch (transform.name)
                {
                    case "Stone - Left + Middle(Clone)":
                        spawn = fireSpawn;
                        break;

                    case "Stone - Left + Right(Clone)":
                        int a = Random.Range(0, 2);
                        if (a == 1)
                        {
                            spawn = fireSpawn;
                        }
                        else
                        {
                            spawn = fireSpawn2;
                        }
                        break;

                    case "Stone - Left(Clone)":
                        spawn = fireSpawn;
                        break;

                    case "Stone - Middle(Clone)":
                        return;

                    case "Stone - Right + Middle(Clone)":
                        spawn = fireSpawn2;
                        break;

                    case "Stone - Right(Clone)":
                        spawn = fireSpawn2;
                        break;

                    default:
                        break;
                }
                GameObject cannon = Instantiate(fireCannon, spawn.position, spawn.rotation, spawn);
                cannon.transform.localPosition = new Vector3(0f, -1f, -0.4f);
                cannon.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
                onceCalled = false;
            }
        }
        else if (secondCall && onceCalled) StartCoroutine(StartFire(spawn));
    }

    IEnumerator StartFire(Transform spawn)
    {
        fire = Instantiate(firePrefab, spawn.position, spawn.rotation, spawn);
        yield return new WaitForSeconds(5f);
        Destroy(fire);
    }
    
    void OnDisable()
    {
        StopAllCoroutines();
        if (trap == "Move" && stoneToMove != null)
        {
            stoneToMove.transform.DOKill();
            stoneToMove.transform.position = new Vector3(originalPos, stoneToMove.transform.position.y, stoneToMove.transform.position.z);
            stoneMoved = false;
        }

        if (trap == "Disappear" && stoneToMove != null)
        {
            stoneToMove.transform.DOKill();
            stoneToMove.transform.localPosition = new Vector3(stoneToMove.transform.localPosition.x, -0.554f, stoneToMove.transform.localPosition.z);
        }

        if (trap == "Fire")
        {
            if (fire != null)
            {
                Destroy(fire);
                fire = null;
            }
            onceCalled = true;
        }

        if (trap == "Spikes")
        {
            if (sp != null)
            {
                Destroy(sp);
                sp = null;
            }
        }
        allowTrap = false;
        stoneToMove = null;
    }
}
