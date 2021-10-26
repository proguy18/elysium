using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPopulator : MonoBehaviour
{
    static int WALL = 1;
	static int FLOOR = 0;
    public int spaceBetweenLights;
    
    MapGenerator mapGenerator;
    MapPopulatorI mapPopulator;
    int[,] _map;
    bool hasMap = false;
	private void Awake() {
	
		mapGenerator = GetComponent<MapGenerator>();
	}

    // Update is called once per frame
    void tryMap(){
        _map = mapGenerator.getBordedMap();
    }
	public void reload(){
		KillMap();
		tryMap();
		Debug.Log(_map);
		mapPopulator = new MapPopulatorI(_map);
        hasMap = true;
	}

	void KillMap(){
		mapPopulator = null;
		_map = null;
	}
    public Vector3 getPlayerSpawn() {
        if (mapPopulator != null) { 
			MapGenerator.Coord coord = mapPopulator.getPlayerSpawn();
			mapPopulator.fillCoord(coord);
            return CoordToWorldPoint(coord);
        }
        return new Vector3(0,0,0);
    }
    public List<Vector3> getRandomSpawns(int number, float height = 0){
		if (mapPopulator == null){
			return null;
		}
		List<MapGenerator.Coord> locations = mapPopulator.generateRandLocationList(number);
		if (locations == null) Debug.Log("Locations is null");
		if (locations.Count == 0) Debug.Log("Locations is empty");
		List<Vector3> realWorldPositions = new List<Vector3>();

		for (int i = 0; i < locations.Count; i ++){

			realWorldPositions.Add(CoordToWorldPoint(locations[i]));
			mapPopulator.fillCoord(locations[i]);
		}

		return realWorldPositions;
	}
    public List<Vector3> GenerateLightPoints(){


        if (!hasMap) return null;
		List<Vector3> poss = new List<Vector3>();
		for (int i = 0; i < _map.GetLength(0); i += spaceBetweenLights){
			for (int j = spaceBetweenLights/2; j < _map.GetLength(1); j += spaceBetweenLights){
				if (_map[i,j] != WALL){
					poss.Add(CoordToWorldPoint(new MapGenerator.Coord(i , j), 1.64f));
				}
			}
		}
        // List<MapGenerator.Coord> edgeTiles = mapGenerator.getEdgeTiles();
        // if (edgeTiles == null){
        //     return null;
        // }
        // //First try getting every 20th and return it

        // List<Vector3> lessTiles = new List<Vector3>();
        // int j = spaceBetweenLights/2;
        // foreach(MapGenerator.Coord tile in edgeTiles){
        //     j ++;
        //     if (j == spaceBetweenLights && !mapPopulator.filledCoord(tile)){
        //         lessTiles.Add(CoordToWorldPoint(tile, 1.64f));
		// 		mapPopulator.filledCoord(tile);
        //         j = 0;
        //     }
            
        // }
       return poss;
    }
    public Vector3 getEndSpawn(){
        if (getPlayerSpawn() == new Vector3(0,0,0)){
            return new Vector3(0,0,0);
        }
        Vector3 playerSpawn = getPlayerSpawn();
        List<MapGenerator.Coord> edgeTiles = mapGenerator.getEdgeTiles();
        List<MapGenerator.Coord> randomTiles = new List<MapGenerator.Coord>();
        System.Random random = new System.Random();
        for (int i = 0; i < 10; i ++){
			int index = random.Next(0, edgeTiles.Count - 1);
			if (!mapPopulator.filledCoord(edgeTiles[index])){
				randomTiles.Add(edgeTiles[index]);
			}
            
        }

        float max = float.MinValue;
        Vector3 finalPos = new Vector3(0,0,0);
		MapGenerator.Coord finalCoord = new MapGenerator.Coord(0,0);
        foreach (MapGenerator.Coord pos in randomTiles){
            Vector3 offset = CoordToWorldPoint(pos) - playerSpawn;
            float mag2 = offset.sqrMagnitude;

            if (mag2 > max && !mapPopulator.filledCoord(pos)){
                max = mag2;
                finalPos = CoordToWorldPoint(pos);
				finalCoord = pos;
            }
        }
		mapPopulator.fillCoord(finalCoord);
        return finalPos;
    }


    Vector3 CoordToWorldPoint(MapGenerator.Coord tile, float y = 0){
		return new Vector3(-_map.GetLength(0)/2 +.5f + tile.tileX, y, -_map.GetLength(1)/2 + .5f + tile.tileY);
	}
    class MapPopulatorI {
		MapGenerator.Coord spawnPoint; 
		MapGenerator.Coord endPoint; 
		HashSet<MapGenerator.Coord> filledCoords;	
		List<MapGenerator.Coord> unfilledFloor;
		int[,] map;
		public MapPopulatorI(int[,] _map){
			filledCoords = new HashSet<MapGenerator.Coord>();
			unfilledFloor = new List<MapGenerator.Coord>();
			map = _map;
			for (int x = 0; x < map.GetLength(0); x ++){
				for (int y = 0; y < map.GetLength(1); y ++){
					if (map[x,y] == FLOOR){
						unfilledFloor.Add(new MapGenerator.Coord(x, y));
					}
				}
			}
			
			spawnPoint = GenerateSpawnPoint();
			endPoint = GenerateEndPoint();
		}

		public List<MapGenerator.Coord> generateRandLocationList(int amount, bool onWalls = false, List<MapGenerator.Coord> reducedList = null, bool onEdge = false, bool notOnEdge = false){
			//generates a list of random, unfilled coordinates on the map or in a smaller set of locations
			List<MapGenerator.Coord> locations = new List<MapGenerator.Coord>();
			List<MapGenerator.Coord> possibleLocations; 
			if (reducedList != null) {
				possibleLocations = reducedList;
			} else {
				possibleLocations = unfilledFloor;
			}
			
			System.Random rand = new System.Random();
			int index = rand.Next(0, possibleLocations.Count  - 1); // -1 to account for indexing 
			MapGenerator.Coord possLocation; 
			int maxIterations = possibleLocations.Count;
			int i = 0;
			while (locations.Count < amount && i <= possibleLocations.Count){
				possLocation = possibleLocations[index];
				if (filledCoords.Contains(possLocation)){
					index = rand.Next(0, possibleLocations.Count  - 1);
					continue;
				}
				locations.Add(possLocation);
                index = rand.Next(0, possibleLocations.Count  - 1);
				i++;
			}
            foreach (MapGenerator.Coord coord in locations){
                filledCoords.Add(coord);
            }
			return locations;
		}
        
		MapGenerator.Coord GenerateSpawnPoint(){

			// List<Coord> location = generateRandLocationList(1, false, smallestRoom.tiles, false, true);
			List<MapGenerator.Coord> location = generateRandLocationList(1);
			if (location.Count == 1){
				return location[0];
			} 
			location = generateRandLocationList(1);
			if (location.Count == 1){
				return location[0];
			}
			
			return new MapGenerator.Coord(-1, -1);
		}
		MapGenerator.Coord GenerateEndPoint(){
			return new MapGenerator.Coord(-1, -1);
		}
		public MapGenerator.Coord getPlayerSpawn(){
			return spawnPoint;
		}
		public void fillCoord(MapGenerator.Coord coord){
			filledCoords.Add(coord);
		}
		public bool filledCoord(MapGenerator.Coord coord){
			return filledCoords.Contains(coord);
		}
	}
}
