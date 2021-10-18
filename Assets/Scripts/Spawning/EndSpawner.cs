using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToSpawn;
    public GameObject mapGeneratorOb;
    private Vector3 spawnPoint;
    public float z = 0;

    // Update is called once per frame
    void Update()
    {
        trySpawnPoint();
        if (Input.GetKeyDown("k")){
            Debug.Log(spawnPoint);
            if (!spawnPoint.Equals(new Vector3(0,0,0))){
                Instantiate(objectToSpawn, new Vector3(spawnPoint.x, z, spawnPoint.z), transform.rotation);
            }
        }
    }
    private void trySpawnPoint(){
        MapPopulator mapPopulator = mapGeneratorOb.GetComponent<MapPopulator>();
        if (mapPopulator != null){
            spawnPoint = mapPopulator.getEndSpawn();
        }
    }
}
