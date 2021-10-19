using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpawner : MonoBehaviour
{
    
    public GameObject myLight;

    public GameObject mapGeneratorOb;
    private List<Vector3> spawnPoints = null;


    void Update()
    {
        trySpawnPoints();
        if (Input.GetKeyDown("k")){
            if (spawnPoints != null){
                Debug.Log(spawnPoints.Count);
                foreach(Vector3 spawnPoint in spawnPoints){
                    Instantiate(myLight, spawnPoint, transform.rotation);
                }
            }
        }
    }
    private void trySpawnPoints(){
        if (spawnPoints == null){
            MapPopulator mapPopulator = mapGeneratorOb.GetComponent<MapPopulator>();
            if (mapPopulator != null){
                spawnPoints = mapPopulator.GenerateLightPoints();
            }
        }
    }
}
