using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject previousPlayer;
    int levelCount = 0;
    private MapGenerator mapGenerator;
    void Start()
    {
        mapGenerator = GetComponentInChildren<MapGenerator>();
        newLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void newLevel(){
        //Save player stats inc inventory
        //Delete old Map
        // mapGenerator.KillMap();
        
        //Generate New Map
        mapGenerator.GenMap();
        //Populate new map - possibly differently
        levelCount ++;
    }
}
