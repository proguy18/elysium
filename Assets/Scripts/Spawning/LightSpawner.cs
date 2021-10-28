using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpawner : MonoBehaviour, ISpawnable
{
    
    public GameObject myLight;
    private List<GameObject> instances = null;
    private GameObject mapGeneratorOb;
    private List<Vector3> spawnPoints = null;
    private GameObject parent;

    private void Awake() {
        mapGeneratorOb = GameObject.Find("MapGenerator");
        parent = GameObject.Find("SpawnerParent");


    }
    private void Update() {
        if (Input.GetKeyDown("m")){
            WallAttach[] wallAttaches = GetComponentsInChildren<WallAttach>();
            foreach (WallAttach wallAttach in wallAttaches){
                wallAttach.MoveLights();
            }
        }
    }

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
                GameObject spawn = Instantiate(myLight, spawnPoint, transform.rotation);
                instances.Add(spawn);
                spawn.transform.parent = parent.transform;
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
