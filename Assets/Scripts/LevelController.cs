using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class LevelController : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshSurface surface;
    private GameObject previousPlayer;
    private int levelCount = 0;
    private MapGenerator mapGenerator;
    private ObjSpawner[] spawners;
    private List<ObjSpawner> option0;
    private List<ObjSpawner> option1;
    private List<ObjSpawner> option2;
    private LightSpawner lightSpawner;
    private PlayerSpawner playerSpawner;
    private MapPopulator mapPopulator;
    private EndSpawner endSpawner;
    private bool newMap = false;
    int count = 3;

    private void Awake() {
        mapGenerator = GetComponentInChildren<MapGenerator>();
        spawners = GetComponentsInChildren<ObjSpawner>();
        option1 = new List<ObjSpawner>();
        option0 = new List<ObjSpawner>();
        option2 = new List<ObjSpawner>();
        foreach (ObjSpawner spawner in spawners){
            if (spawner.levelType == 0 || spawner.levelType == -1){
                option0.Add(spawner);
            }
            if (spawner.levelType == 1 || spawner.levelType == -1){
                option1.Add(spawner);
            }
            if (spawner.levelType == 2 || spawner.levelType == -1){
                option2.Add(spawner);
            }
        }
        lightSpawner = GetComponentInChildren<LightSpawner>();
        playerSpawner = GetComponentInChildren<PlayerSpawner>();
        mapPopulator = GetComponentInChildren<MapPopulator>();
        endSpawner = GetComponentInChildren<EndSpawner>();
    }
    private void Start() {
        newLevel();
    }
    private void LateUpdate() {
        if (Input.GetKeyDown("i")){
            newLevel();
            
        }   
        if (newMap) count --;

        if (count == 0 && newMap){ 
            moveWalledObjects();
            newMap = false;
            count = 3;
        }

    }

    void newLevel(){
        //Delete old Map
        
        if (levelCount > 0) {
            foreach(ObjSpawner spawner in spawners){
                spawner.kill();
            }
            endSpawner.kill();
            lightSpawner.kill();
            playerSpawner.kill();
            mapGenerator.KillMap();
        }
        
        //Generate New Map
    
        mapGenerator.GenMap();
        mapPopulator.reload();
        //Populate new map - possibly differently
        levelCount ++;
        playerSpawner.spawn();
        endSpawner.spawn();
        int levelInd = 0;
        if (levelCount <= 3){
            levelInd = levelCount;
        }
        else {
            var rand = new System.Random();
            levelInd = rand.Next(1, 4);
            Debug.Log(levelInd);
        }
        if (levelInd == 1){
            foreach(ObjSpawner spawner in option0){
                spawner.spawn();
            }
        }
        else if (levelInd == 2) {
            foreach(ObjSpawner spawner in option1){
                spawner.spawn();
            }
        } else if (levelInd == 3){
            foreach(ObjSpawner spawner in option2){
                spawner.spawn();
            }
        }
        
        lightSpawner.spawn();
        moveWalledObjects();
        newMap = true;
        surface.BuildNavMesh();
    }
    public void playerFinishedLevel(){
        newLevel();
    }
    void moveWalledObjects(){
        
        WallAttach[] wallAttaches = GetComponentsInChildren<WallAttach>();
        foreach (WallAttach wallAttach in wallAttaches){
            wallAttach.MoveLights();
        }
    }

    public int GetLevel(){
        return levelCount;
    }

          
}
