using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomStoneSpawn_Magic : MonoBehaviour
{
    private List<GameObject> stones = new List<GameObject>();
    [SerializeField] private List<GameObject> stonesWithMagic = new List<GameObject>();
    [SerializeField] private List<GameObject> stonesWithoutMagic = new List<GameObject>();
    public bool multipleStones;
    Vector3 spawn1;
    Vector3 spawn2;
    Vector3 spawnVector;

    void Start(){
        switch(transform.name){
            case "Stone - Left + Middle(Clone)":
                spawn1 = new Vector3(-4f, -1.3f, 1f);
                spawn2 = new Vector3(0f, -1.3f, 1f);
                break;
            
            case "Stone - Left + Right(Clone)":
                spawn1 = new Vector3(-4f, -1.3f, 1f);
                spawn2 = new Vector3(4f, -1.3f, 1f);
                break;
            
            case "Stone - Left(Clone)":
                spawn1 = new Vector3(-4f, -1.3f, 1f);
                break;
            
            case "Stone - Middle(Clone)":
                spawn1 = new Vector3(0f, -1.3f, 1f);
                break;
            
            case "Stone - Right + Middle(Clone)":
                spawn1 = new Vector3(0f, -1.3f, 1f);
                spawn2 = new Vector3(4f, -1.3f, 1f);
                break;
            
            case "Stone - Right(Clone)":
                spawn1 = new Vector3(4f, -1.3f, 1f);
                break;

            default:
                break;
        }

        if (GameManager_Jumping.Instance.magicStoneSpawned)
        {
            stones = stonesWithoutMagic;
        }
        else
        {
            stones = stonesWithMagic;
        }
        int rando = Random.Range(0,stones.Count);
        if(multipleStones){
            GameObject stone1 = Instantiate(stones[rando], spawn1, Quaternion.Euler(0f, 90f, 0f), transform);
            if (stone1.name == "MagicStone(Clone)")
            {
                GameManager_Jumping.Instance.magicStoneSpawned = true;
            }
            stone1.transform.localPosition = spawn1;
            rando = Random.Range(0,stones.Count);
            GameObject stone2 = Instantiate(stones[rando], spawn2, Quaternion.Euler(0f, 90f, 0f), transform);
            if(stone2.name == "MagicStone(Clone)"){
                GameManager_Jumping.Instance.magicStoneSpawned = true;
            }
            stone2.transform.localPosition = spawn2;
        }
        else{
            GameObject stone1 = Instantiate(stones[rando], spawn1, Quaternion.Euler(0f, 90f, 0f), transform);
            if(stone1.name == "MagicStone(Clone)"){
                GameManager_Jumping.Instance.magicStoneSpawned = true;
            }
            stone1.transform.localPosition = spawn1;
        }

        transform.GetComponent<ObstacleSpawner_Jungle>().enabled = true;
    }
}
