using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject previousPlayer;
    int levelCount = 0;
    private MapGenerator mapGenerator;
    private ObjSpawner[] spawners;

    void Start()
    {
        mapGenerator = GetComponentInChildren<MapGenerator>();
        spawners = GetComponentsInChildren<ObjSpawner>();
        newLevel();
    }

    // Update is called once per frame
    void newLevel(){
        //Save player stats inc inventory
        //Delete old Map
        if (levelCount > 0) {
            foreach(ObjSpawner spawner in spawners){
                spawner.kill();
            }
            mapGenerator.KillMap();
        }
        
        //Generate New Map
        if (levelCount == 0){
        mapGenerator.GenMap();
        //Populate new map - possibly differently
        levelCount ++;
        foreach(ObjSpawner spawner in spawners){
            spawner.spawn();
        }
        }

    }
    public void playerFinishedLevel(){
        newLevel();
    }

          
}
