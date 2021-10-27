using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpawner : MonoBehaviour, ISpawnable
{
    
    public GameObject myLight;
    private List<GameObject> instances = null;
    public GameObject mapGeneratorOb;
    private List<Vector3> spawnPoints = null;



    private void trySpawnPoints(){
        
        MapPopulator mapPopulator = mapGeneratorOb.GetComponent<MapPopulator>();
        if (mapPopulator != null){
            spawnPoints = mapPopulator.GenerateLightPoints();
        }

    }
    public void spawn(){
        trySpawnPoints();
        if (spawnPoints != null){
            instances = new List<GameObject>();
            foreach(Vector3 spawnPoint in spawnPoints){
                
                instances.Add(Instantiate(myLight, spawnPoint, transform.rotation));
            }
        }
    }
    public void kill(){
        if (instances != null){
            foreach (GameObject instance in instances){
                Destroy(instance);
            }
            instances = null;
        }
    }
}
