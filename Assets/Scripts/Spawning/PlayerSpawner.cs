using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour, ISpawnable
{

    private GameObject instance = null;
    public GameObject objectToSpawn;
    public GameObject mapGeneratorOb;
    private Vector3 spawnPoint;

    // Update is called once per frame
    private void trySpawnPoint(){
        MapPopulator mapPopulator = mapGeneratorOb.GetComponent<MapPopulator>();
        if (mapPopulator != null){
            spawnPoint = mapPopulator.getPlayerSpawn();
        }
    }
    public void kill(){
        instance.SetActive(false);
    }
    public void spawn(){
        trySpawnPoint();
        if (!spawnPoint.Equals(new Vector3(0,0,0))){
            if (instance != null){
                instance.transform.position = spawnPoint;
                instance.SetActive(true);
            }else{
                instance = Instantiate(objectToSpawn, spawnPoint, transform.rotation);
            }
        }
            
    }
}
