using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerSpawn : MonoBehaviour
{
    private Vector3 spawnPoint;
    public GameObject mapGeneratorOb;

    
    void Update()
    {
        if (Input.GetKeyDown("k")){
            Debug.Log(spawnPoint);
            if (!spawnPoint.Equals(new Vector3(0,0,0))){
                transform.position = new Vector3(spawnPoint.x, transform.position.y, spawnPoint.z);
            }
        }
    }
    private void trySpawnPoint(){
        MapPopulator mapPopulator = mapGeneratorOb.GetComponent<MapPopulator>();
        if (mapPopulator != null){
            spawnPoint = mapPopulator.getPlayerSpawn();
        }
    }
}
