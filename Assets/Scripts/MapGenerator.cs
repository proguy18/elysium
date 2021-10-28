using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class MapGenerator : MonoBehaviour {

	public int width;
	public int height;
	public int iterations; 
	

	public string seed;
	public bool useRandomSeed;

	[Range(0,100)]
	public int randomFillPercent;

	int[,] map;
	int[,] borderedMap;

	static int WALL = 1;
	static int FLOOR = 0;

	public int passageRadius = 1;

	MeshGenerator meshGenerator;

	List<Room> finalRooms;

	bool mapOperational = false;

	List<Coord> edgeTiles = null; 
	public int TILE_BORDER = 30;
	public void GenMap(){
		mapOperational = true;
		GenerateMap();
	}
	public void KillMap(){
		mapOperational = false;
		meshGenerator.ClearMesh();
	}
	void GenerateMap(){
		makeNoiseGrid();
		applyCellularAutomation(iterations);
		ProcessMap();
		
		int borderSize = TILE_BORDER; 
		borderedMap = new int[width + borderSize*2, height + borderSize*2];
		for (int i = 0; i < borderedMap.GetLength(0) ; i ++){
			for (int j = 0; j < borderedMap.GetLength(1); j ++){
				if (i >= borderSize && i < width + borderSize && j >= borderSize && j < borderSize + height){
					borderedMap[i,j] = map[i - borderSize, j - borderSize];
				} else {
					borderedMap[i,j] = WALL;
				}
			}
		}
		List<Coord> allTiles = getAllFloorTiles(map);
		Room bigRoom = new Room(allTiles, map);
		edgeTiles = bigRoom.edgeTiles;
		meshGenerator = GetComponent<MeshGenerator>();
		meshGenerator.GenerateMesh(borderedMap, 1);
		mapOperational = true;
	}
	void makeNoiseGrid(){

		if (useRandomSeed){
			seed = Time.time.ToString();
		}
		map = new int[width, height];
		
		System.Random rand = new System.Random();

        float random = rand.Next(0, 100);
		for (int i = 0; i < width ; i ++){
			for (int j = 0; j < height; j ++){
				if (i == 0 || i == width - 1 || j == 0 || j == height - 1) {
					map[i, j] = WALL;
				}
				else if (random < randomFillPercent){
					map[i, j] = WALL;
				} else map[i, j]= FLOOR;
				random = rand.Next(0, 100);
			}
		}
    }

	public bool isMapOperational(){
		return mapOperational;
	}

	List<Coord> getAllFloorTiles(int[,] map){
		//n^2 implementation. Flood Fill would be better but this is for correctness. 
		List<Coord> allTiles = new List<Coord>();
		for (int x = 0; x < map.GetLength(0); x ++){
			for (int y = 0; y < map.GetLength(1); y ++){
				if (map[x,y] == FLOOR) allTiles.Add(new Coord(x, y));
			}
		}
		return allTiles;
	}
	void applyCellularAutomation(int count){
		
		for (int i = 0; i < count; i ++){
			int[,] cpy = new int[width, height];
			for (int j = 0; j < width; j ++){
				for (int k = 0; k < height; k ++){
					cpy[i,j] = map[i,j];
				}
			}
			for (int j = 0; j < width; j ++){
				for (int k = 0; k < height; k ++){
					int neighbourWallCount = GetNumberOfNeighbors(j, k, map);
					if (neighbourWallCount > 4){
						map[j, k] = WALL;
					} else if (neighbourWallCount < 4) {
						map[j, k] =  FLOOR;
					}
			
				}
			}
		}
	}
	/// <summary>
    /// Returns the number of neighbors for a cell at X,Y.
    /// </summary>
    /// <param name="x">X coordinate of cell to check</param>
    /// <param name="y">Y coordinate of cell to check</param>
    /// <returns>number of neighbors</returns>
    public static int GetNumberOfNeighbors(int gridX, int gridY, int[,] cpy) {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
                if (neighbourX >= 0 && neighbourX < cpy.GetLength(0) && neighbourY >= 0 && neighbourY < cpy.GetLength(1)) {
                    if (neighbourX != gridX || neighbourY != gridY) {
                        wallCount += cpy[neighbourX,neighbourY];
                    }
                }
                else {
                    wallCount ++;
                }
            }
        }

        return wallCount;
    }

	void ProcessMap(){
		List<List<Coord>> wallRegions = GetRegions(1);
		int wallThresholdSize = 20;
		foreach (List<Coord> wallRegion in wallRegions){

			if (wallRegion.Count < wallThresholdSize){
				foreach (Coord tile in wallRegion){
					map[tile.tileX, tile.tileY] = FLOOR;
				}
			}
		}

		List<List<Coord>> roomRegions = GetRegions (0);
		List<Room> survivingRooms = new List<Room>();
        int roomThresholdSize = 20;

        foreach (List<Coord> roomRegion in roomRegions) {
            if (roomRegion.Count < roomThresholdSize) {
                foreach (Coord tile in roomRegion) {
                    map[tile.tileX,tile.tileY] = WALL;
                }
            } else {
				survivingRooms.Add(new Room(roomRegion, map));
			}
        }
		survivingRooms.Sort();
		if (survivingRooms.Count > 0){
			survivingRooms[0].isMainRoom = true;
			survivingRooms[0].isAccessibleFromMainRoom = true;
		}

		ConnectClosestRooms(survivingRooms);
		survivingRooms.Sort();
		finalRooms = survivingRooms;
	}
	public List<Coord> getEdgeTiles(){
		List<Coord> fixedEgdeTiles = new List<Coord>();
		foreach (Coord tile in edgeTiles){
			fixedEgdeTiles.Add(new Coord(tile.tileX + TILE_BORDER, tile.tileY + TILE_BORDER));
		}
		return fixedEgdeTiles;
	}
	void ConnectClosestRooms(List<Room> allRooms, bool forceAccesibilityFromMainRoom = false){
		List<Room> roomListA = new List<Room>();
		List<Room> roomListB = new List<Room>();

		if (forceAccesibilityFromMainRoom) {
			foreach (Room room in allRooms){
				if (room.isAccessibleFromMainRoom) {
					roomListB.Add(room);
				} else{
					roomListA.Add(room);
				}
			}
		}
		else{
			roomListA = allRooms;
			roomListB = allRooms;
		}

		bool connectionFound = false;
		float bestDistance = 0;
		Coord bestTileA = new Coord();
		Coord bestTileB = new Coord();
		Room bestRoomA = new Room();
		Room bestRoomB = new Room();
		foreach( Room roomA in roomListA){
			
			if (!forceAccesibilityFromMainRoom){
				connectionFound = false;
				if (roomA.connectedRooms.Count > 0) continue;

			}
		
			foreach(Room roomB in roomListB){
				if (roomA == roomB || roomA.IsConnected(roomB)) continue; 


				for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA ++){
					for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB ++){
						Coord tileA = roomA.edgeTiles[tileIndexA];
						Coord tileB = roomB.edgeTiles[tileIndexB];
						float distanceBetweenRooms2 =  Mathf.Pow(tileB.tileY - tileA.tileY, 2) + Mathf.Pow(tileB.tileX - tileA.tileX, 2);
						if (distanceBetweenRooms2 < bestDistance || !connectionFound){
							bestDistance = distanceBetweenRooms2;
							bestRoomA = roomA;
							bestRoomB = roomB;
							bestTileA = tileA;
							bestTileB = tileB;
							connectionFound = true;
						}
					}
				}
			}
			if (connectionFound && !forceAccesibilityFromMainRoom){
				CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
			}
		}

		if (connectionFound && forceAccesibilityFromMainRoom){
			CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
			ConnectClosestRooms(allRooms, true);
		}

		if (!forceAccesibilityFromMainRoom){
			ConnectClosestRooms(allRooms, true);
		}
		
	}

	void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB){
		Room.ConnectRooms(roomA, roomB);

		List<Coord> line = getLine(tileA, tileB);
		foreach (Coord c in line){
			DrawCricle(c, passageRadius);
		}


	}
	void DrawCricle(Coord c, int r){
		for (int x = -r; x <= r; x ++){
			for (int y = -r; y <= r; y ++){
				if (x*x + y*y <= r*r){
					int realX = c.tileX + x;
					int realY = c.tileY + y;
					if (IsInMapRange(realX, realY)){
						map[realX, realY] = FLOOR;
					}
				}
			}
		}
	}
	List<Coord> getLine(Coord start, Coord end){
		List<Coord> line = new List<Coord>();
		int y = start.tileY;
		int x = start.tileX;
		bool inverted = false;

		int dx = end.tileX - start.tileX;
		int dy = end.tileY - start.tileY;

		//Assuming dx >= dy
		int step = Math.Sign(dx);
		int gradStep = Math.Sign(dy);

		int longest = Math.Abs(dx);
		int shortest = Math.Abs(dy);

		if (longest < shortest){
			inverted = true;
			longest = Math.Abs(dy);
			shortest = Math.Abs(dx);
			step = Math.Sign(dy);
			gradStep = Math.Sign(dx);
		}

		int gradAccumilation = longest/2;
		for (int i = 0; i < longest; i++){
			line.Add(new Coord(x, y));
			if (inverted){
				y += step;
			} else{
				x += step;
			}
			gradAccumilation += shortest;
			if (gradAccumilation >= longest){
				if (inverted){
					x += gradStep;
				} else {
					y += gradStep;
				}
				gradAccumilation -= longest;
			}
		}
		return line;
	}
	public int[,] getBordedMap(){
		if (mapOperational){
			return borderedMap;
		}
		return null;
	}

	Vector3 CoordToWorldPoint(Coord tile){
		return new Vector3(-width/2 +.5f + tile.tileX, 0, -height/2 + .5f + tile.tileY);
	}

	List<List<Coord>> GetRegions(int tileType) {
		List<List<Coord>> regions = new List<List<Coord>>();
		int[,] mapFlags = new int[width, height];
		for (int i = 0; i < width; i ++){
			for (int j = 0; j < height; j ++){
				if (mapFlags[i,j] == 0 && map[i,j] == tileType){
					List<Coord> newRegion = GetRegionTiles(i,j);
					regions.Add(newRegion);
					foreach (Coord tile in newRegion){
						mapFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}
		return regions;
	}
	List<Coord> GetRegionTiles(int startX, int startY){
		List<Coord> tiles = new List<Coord>();
		int[,] mapFlags = new int[width, height];
		int tileType = map[startX, startY];
		Queue<Coord> queue = new Queue<Coord>();
		queue.Enqueue(new Coord(startX, startY));
		mapFlags[startX, startY] = 1;

		while(queue.Count > 0){
			Coord tile = queue.Dequeue();
			tiles.Add(tile);

			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x ++){
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y ++){
					if (IsInMapRange(x,y) && (y == tile.tileY || x == tile.tileX)){
						if (mapFlags[x,y] == 0 && map[x,y] == tileType){
							mapFlags[x,y] = 1;
							queue.Enqueue(new Coord(x, y));
						}

					}
				}
			}
		}
		return tiles;
	}

	bool IsInMapRange(int x, int y){
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	public struct Coord{
		public int tileX;
		public int tileY;
		public Coord(int x, int y){
			tileX = x;
			tileY = y;
		}
	}


	class Room : IComparable<Room>{
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<Room> connectedRooms;
		public int roomSize;
		public bool isAccessibleFromMainRoom;
		public bool isMainRoom;

		public Room(){
		}
		public Room(List<Coord> roomTiles, int[,] map){
			tiles = roomTiles;
			roomSize = tiles.Count;
			connectedRooms = new List<Room>();
			edgeTiles = new List<Coord>();
			foreach (Coord tile in tiles){
				for (int x = tile.tileX - 1; x<= tile.tileX + 1;x ++){
					for( int y = tile.tileY - 1; y <= tile.tileY + 1; y++){
						if (x == tile.tileX || y == tile.tileY){
							if ( x>= 0 && x< map.GetLength(0) && y >=0 && y < map.GetLength(1) && map[x,y] == WALL) {
								edgeTiles.Add(tile);
							}
						}
					}
				}
			}
		}
		public static void ConnectRooms(Room roomA, Room roomB) {
			if (roomA.isAccessibleFromMainRoom) {
				roomB.SetAccessibleFromMainRoom(); 
			} else if (roomB.isAccessibleFromMainRoom){
				roomA.SetAccessibleFromMainRoom();
			}

			roomA.connectedRooms.Add(roomA);
			roomB.connectedRooms.Add(roomB);
		}

		public bool IsConnected(Room otherRoom){
			return connectedRooms.Contains(otherRoom);
		}
		
		public int CompareTo(Room otherRoom){
			return otherRoom.roomSize.CompareTo(roomSize);
		}
		public void SetAccessibleFromMainRoom(){
			if (!isAccessibleFromMainRoom){
				isAccessibleFromMainRoom = true;
				foreach (Room room in connectedRooms){
					room.isAccessibleFromMainRoom = true;
				}
			}
		}

	}
	

}
