using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    
    public GameObject rock1;
    public GameObject rock2;
    public GameObject rock3;
    public GameObject rock4;

    public GameObject mapGeneratorOb;
    private List<Vector3> spawnPoints;

    public int number = 100; 
    private int NUM_ROCKS = 4; 

    private void Awake() {

        var _rock1 = Resources.Load("Rock1");
        var _rock2 = Resources.Load("Rock2");
        var _rock3 = Resources.Load("Rock3");
        var _rock4 = Resources.Load("Rock4");
        rock1 = _rock1 as GameObject;
        rock2 = _rock2 as GameObject;
        rock3 = _rock3 as GameObject;
        rock4 = _rock4 as GameObject;

        
    }
    void Update()
    {
        trySpawnPoints();
        if (Input.GetKeyDown("k")){
            if (!spawnPoints.Equals(null)){
                System.Random random = new System.Random();
                int rand = random.Next(0, NUM_ROCKS);
                GameObject[] objs = new GameObject[] {rock1, rock2, rock3, rock4};
                foreach(Vector3 spawnPoint in spawnPoints){
                    GameObject objectToSpawn = objs[rand];
                    Instantiate(objectToSpawn, spawnPoint, transform.rotation);
                    rand = random.Next(0, NUM_ROCKS);
                }
            }
        }
    }
    private void trySpawnPoints(){
        if (spawnPoints == null){
            MapGenerator mapGenerator = mapGeneratorOb.GetComponent<MapGenerator>();
            spawnPoints = mapGenerator.getRandomSpawns(number);
        }
    }
}
