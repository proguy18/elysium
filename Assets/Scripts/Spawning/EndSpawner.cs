using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSpawner : MonoBehaviour, ISpawnable
{
    // Start is called before the first frame update
    public GameObject objectToSpawn;
    private GameObject mapGeneratorOb;
    private GameObject instance = null;
    private Vector3 spawnPoint;
    public float z = 0;

    private void Awake() {
        mapGeneratorOb = GameObject.Find("MapGenerator");
    }

    private void trySpawnPoint(){
        MapPopulator mapPopulator = mapGeneratorOb.GetComponent<MapPopulator>();
        if (mapPopulator != null){
            Debug.Log("Spawning here!");
            spawnPoint = mapPopulator.getEndSpawn();
        }
    }
    public void kill(){
        Destroy(instance);
        instance = null;
    }
    public void spawn(){
        trySpawnPoint();
        if (!spawnPoint.Equals(new Vector3(0,0,0))){

            instance = Instantiate(objectToSpawn, new Vector3(spawnPoint.x, z, spawnPoint.z), transform.rotation);
           
        }
        
    }
}
