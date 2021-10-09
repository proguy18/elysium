using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPopulator : MonoBehaviour
{
    static int WALL = 1;
	static int FLOOR = 0;
    
    MapGenerator mapGenerator;
    MapPopulatorI mapPopulator;
    int[,] _map;

    // Update is called once per frame
    void Update()
    {
        mapGenerator = GetComponent<MapGenerator>();
        tryMap();
        if (_map != null){
            mapPopulator = new MapPopulatorI(_map);
        }
    }
    void tryMap(){
        _map = mapGenerator.getBordedMap();
    }
    public Vector3 getPlayerSpawn() {
        if (mapPopulator != null) { 
            return CoordToWorldPoint(mapPopulator.getPlayerSpawn());
        }
        return new Vector3(0,0,0);
    }
    public List<Vector3> getRandomSpawns(int number, float height = 0){

		// if (mapGenerator.isMapOperational()){
        //     Debug.Log("No map operational");
		// 	return null;
		// }
		List<Coord> locations = mapPopulator.generateRandLocationList(number);
		List<Vector3> realWorldPositions = new List<Vector3>();
		
		for (int i = 0; i < locations.Count; i ++){
			realWorldPositions.Add(CoordToWorldPoint(locations[i]));
		}
		return realWorldPositions;
	}

    Vector3 CoordToWorldPoint(Coord tile, float y = 0){
		return new Vector3(-_map.GetLength(0)/2 +.5f + tile.tileX, y, -_map.GetLength(1)/2 + .5f + tile.tileY);
	}
    class MapPopulatorI {
		Coord spawnPoint; 
		Coord endPoint; 
		HashSet<Coord> filledCoords;	
		List<Coord> unfilledFloor;
		int[,] map;
		public MapPopulatorI(int[,] _map){
			Debug.Log("here we are");
			filledCoords = new HashSet<Coord>();
			unfilledFloor = new List<Coord>();
			map = _map;
			for (int x = 0; x < map.GetLength(0); x ++){
				for (int y = 0; y < map.GetLength(1); y ++){
					if (map[x,y] == FLOOR){
						unfilledFloor.Add(new Coord(x, y));
					}
				}
			}
			
			spawnPoint = GenerateSpawnPoint();
			endPoint = GenerateEndPoint();
		}

		public List<Coord> generateRandLocationList(int amount, bool onWalls = false, List<Coord> reducedList = null, bool onEdge = false, bool notOnEdge = false){
			//generates a list of random, unfilled coordinates on the map or in a smaller set of locations
			List<Coord> locations = new List<Coord>();
			List<Coord> possibleLocations; 
			if (reducedList != null) {
				possibleLocations = reducedList;
			} else {
				possibleLocations = unfilledFloor;
			}
			
			System.Random rand = new System.Random();
			int index = rand.Next(0, possibleLocations.Count  - 1); // -1 to account for indexing 
			Debug.Log(possibleLocations.Count);
			Coord possLocation; 
			int maxIterations = possibleLocations.Count;
			int i = 0;
			while (locations.Count < amount && i <= possibleLocations.Count){
				possLocation = possibleLocations[index];
				if (filledCoords.Contains(possLocation)){
					index = rand.Next(0, possibleLocations.Count  - 1);
					continue;
				}
				// if (onWalls){
				// 	//check condition
				// }
				// if (onEdge){
				// }
				// if (notOnEdge && GetNumberOfNeighbors(possLocation.tileX, possLocation.tileY, map) > 0){
				// 	index = rand.Next(0, possibleLocations.Count  - 1);
				// 	continue;
				// }
				locations.Add(possLocation);
                index = rand.Next(0, possibleLocations.Count  - 1);
				i++;
			}
            foreach (Coord coord in locations){
                filledCoords.Add(coord);
            }
			return locations;
		}
		Coord GenerateSpawnPoint(){

			// List<Coord> location = generateRandLocationList(1, false, smallestRoom.tiles, false, true);
			List<Coord> location = generateRandLocationList(1);
			if (location.Count == 1){
				return location[0];
			} 
			location = generateRandLocationList(1);
			if (location.Count == 1){
				return location[0];
			}
			
			return new Coord(-1, -1);
		}
		Coord GenerateEndPoint(){
			return new Coord(-1, -1);
		}
		public Coord getPlayerSpawn(){
			return spawnPoint;
		}
	}
    struct Coord{
		public int tileX;
		public int tileY;
		public Coord(int x, int y){
			tileX = x;
			tileY = y;
		}
	}
}
