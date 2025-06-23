using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomStoneSpawn_Lava : MonoBehaviour
{
    [SerializeField] private List<GameObject> stones = new List<GameObject>();
    public bool multipleStones;
    Vector3 spawn1;
    Vector3 spawn2;
    Vector3 spawnVector;

    void Start(){
        int rando = Random.Range(0,stones.Count);
        switch(transform.name){
            case "Stone - Left + Middle(Clone)":
                spawn1 = new Vector3(-5f, -0.5f, 0f);
                spawn2 = new Vector3(0f, -0.5f, 0f);
                break;
            
            case "Stone - Left + Right(Clone)":
                spawn1 = new Vector3(-5f, -0.5f, 0f);
                spawn2 = new Vector3(5f, -0.5f, 0f);
                break;
            
            case "Stone - Left(Clone)":
                spawn1 = new Vector3(-5f, -0.5f, 0f);
                break;
            
            case "Stone - Middle(Clone)":
                spawn1 = new Vector3(0f, -0.5f, 0f);
                break;
            
            case "Stone - Right + Middle(Clone)":
                spawn1 = new Vector3(0f, -0.5f, 0f);
                spawn2 = new Vector3(5f, -0.5f, 0f);
                break;
            
            case "Stone - Right(Clone)":
                spawn1 = new Vector3(5f, -0.5f, 0f);
                break;

            default:
                break;
        }
        Vector3 pos = new Vector3(0f, -0.8f, 0f);
        if (multipleStones)
        {
            GameObject stone1 = Instantiate(stones[rando], spawn1, Quaternion.Euler(0f, 90f, 0f), transform);
            if (stone1.name == "FlatStone(Clone)") stone1.transform.position = pos;
            stone1.transform.localPosition = spawn1;
            rando = Random.Range(0, stones.Count);
            GameObject stone2 = Instantiate(stones[rando], spawn2, Quaternion.Euler(0f, 90f, 0f), transform);
            if (stone2.name == "FlatStone(Clone)") stone2.transform.position = pos;
            stone2.transform.localPosition = spawn2;
        }
        else
        {
            GameObject stone1 = Instantiate(stones[rando], spawn1, Quaternion.Euler(0f, 90f, 0f), transform);
            if (stone1.name == "FlatStone(Clone)") stone1.transform.position = pos;
            stone1.transform.localPosition = spawn1;
        }

        // transform.GetComponent<ObstacleSpawner_Jungle>().enabled = true;
    }
}
