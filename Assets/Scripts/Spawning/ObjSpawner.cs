using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjSpawner : MonoBehaviour, ISpawnable 
{
    public string nameStem = "rock";
    
    private List<GameObject> objects;

    private GameObject mapGeneratorOb;
    public List<GameObject> clones;
    private List<Vector3> spawnPoints = null;

    public int number = 100; 
    public float z = 0;
    public int levelType = 0;

    public bool isEnemySpawner = false;
    private GameObject parent;

    private void Awake() {
        objects = new List<GameObject>();
        parent = GameObject.Find("SpawnerParent");
        mapGeneratorOb = GameObject.Find("MapGenerator");
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
    private void trySpawnPoints(){
        
        MapPopulator mapPopulator = mapGeneratorOb.GetComponent<MapPopulator>();
        if (mapPopulator != null){
            spawnPoints = mapPopulator.getRandomSpawns(number);
        }
        
    }
    public void kill(){
        foreach (GameObject ob in clones){
            Destroy(ob);
        }
    }
    public void spawn(){
        trySpawnPoints();
        
        if (spawnPoints != null){
            clones = new List<GameObject>();
            System.Random random = new System.Random();
            int rand = random.Next(0, objects.Count);
            foreach(Vector3 spawnPoint in spawnPoints){
                GameObject objectToSpawn = objects[rand];
                Vector3 sp = new Vector3(spawnPoint.x, spawnPoint.y + z, spawnPoint.z);
                GameObject instance = Instantiate(objectToSpawn, sp, Quaternion.Euler(new Vector3(0,random.Next(0,360),0 )));
                clones.Add(instance);
                instance.transform.parent = parent.transform;
                rand = random.Next(0, objects.Count);
                if (isEnemySpawner){
                    instance.layer = 6;
                }
            }
        }
        if (isEnemySpawner){
            Debug.Log("Total mobs " + clones.Count);
        }
        
    }
    public void spawn(float multiplier){
        int _number = number;
        number = Convert.ToInt32(Math.Floor(multiplier* number));
        Debug.Log("Asking for " + number);
        spawn();
        Debug.Log("Original number" + _number);
        number = _number;
    }
    public bool isEnemy(){
        return isEnemySpawner;
    }

}
