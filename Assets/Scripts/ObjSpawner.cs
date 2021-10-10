using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSpawner : MonoBehaviour
{
    public string nameStem = "rock";
    
    private List<GameObject> objects;

    public GameObject mapGeneratorOb;
    private List<Vector3> spawnPoints = null;

    public int number = 100; 
    public float z = 0;

    private void Awake() {
        objects = new List<GameObject>();
        bool moreObjects = true;
        int i = 1;
        while(moreObjects){
            var _temp = Resources.Load(nameStem + i.ToString());
            if (_temp != null){
                objects.Add(_temp as GameObject);
            }
            else {
                moreObjects = false;
            }
            i ++;
        }

        
    }
    void Update()
    {
        trySpawnPoints();
        if (Input.GetKeyDown("k")){
            if (spawnPoints != null){
                System.Random random = new System.Random();
                int rand = random.Next(0, objects.Count);
                foreach(Vector3 spawnPoint in spawnPoints){
                    GameObject objectToSpawn = objects[rand];
                    Vector3 sp = new Vector3(spawnPoint.x, spawnPoint.y + z, spawnPoint.z);
                    Instantiate(objectToSpawn, sp, transform.rotation);
                    rand = random.Next(0, objects.Count);
                }
            }
        }
    }
    private void trySpawnPoints(){
        if (spawnPoints == null){
            MapPopulator mapPopulator = mapGeneratorOb.GetComponent<MapPopulator>();
            if (mapPopulator != null){
                spawnPoints = mapPopulator.getRandomSpawns(number);
                
            }
        }
    }
}
