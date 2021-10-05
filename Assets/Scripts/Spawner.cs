using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject objectToSpawn;
    public GameObject mapGeneratorOb;
    private Vector3 spawnPoint;

    // Update is called once per frame
    void Update()
    {
        trySpawnPoint();
        if (Input.GetKeyDown("k")){
            Debug.Log(spawnPoint);
            if (!spawnPoint.Equals(new Vector3(0,0,0))){
                Instantiate(objectToSpawn, spawnPoint, transform.rotation);
            }
        }
    }
    private void trySpawnPoint(){
        MapGenerator mapGenerator = mapGeneratorOb.GetComponent<MapGenerator>();
        spawnPoint = mapGenerator.getSpawnPoint();
        Debug.Log("Spawn point is " + spawnPoint);
    }
}
