using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator1 : MonoBehaviour
{
    //FROM SEBASTIAN LAGUE VIDEOS https://www.youtube.com/watch?v=yOgIncKp0BE
    public SquareGrid squareGrid;
    public MeshFilter walls;
    public MeshFilter cave;
    public int wallHeight  = 5;
    List<Vector3> vertices;
    List<int> triangles;
    Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
    Mesh mesh;
    
    // for texture scaling i hope
    [Range(0, 1000000)] public float x_scale = 10000;
    [Range(0, 1000000)] public int wallTiles = 20;

    float squareSize; 
    int[,] map;

    List<List<int>> outlines = new List<List<int>>();
    HashSet<int> checkedVerts = new HashSet<int>();
    public void GenerateMesh(int[,] map, float squareSize){
        outlines.Clear();
        checkedVerts.Clear();
        triangleDictionary.Clear();
        this.squareSize = squareSize;
        this.map = map;

        squareGrid = new SquareGrid(map, squareSize);
        vertices = new List<Vector3>();
        triangles = new List<int>();
        for (int x = 0; x < squareGrid.squares.GetLength(0); x++){
            for (int y = 0; y < squareGrid.squares.GetLength(1); y ++){
                TriangulateSquare(squareGrid.squares[x,y]);
            }
        }
        mesh = new Mesh();
        cave.mesh = mesh;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        int tileAmount = 10;
        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 0; i < vertices.Count; i ++){
            float percentX = Mathf.InverseLerp(-map.GetLength(0)/2*squareSize, map.GetLength(0)/2*squareSize, vertices[i].x)* tileAmount;
            float percentY = Mathf.InverseLerp(-map.GetLength(1)/2*squareSize, map.GetLength(1)/2*squareSize, vertices[i].z)* tileAmount;
            uvs[i] = new Vector2(percentX, percentY);
        }
        mesh.uv = uvs;
        CreateWallMesh();
    }
    public void ClearMesh(){
        cave.mesh.Clear();
        walls.mesh.Clear();
    }
    void CreateWallMesh(){

        CalculateOutlines();
        List<Vector3> wallVerts = new List<Vector3>();
        List<int> wallTriangles = new List<int>(); 
        Mesh wallMesh = new Mesh();

        foreach( List<int> outline in outlines){
            for (int i = 0; i < outline.Count - 1; i ++){
                int startInd = wallVerts.Count;
                wallVerts.Add(vertices[outline[i]]); //left
                wallVerts.Add(vertices[outline[i + 1]]); //right
                wallVerts.Add(vertices[outline[i]]  - Vector3.up*wallHeight); //bottom left
                wallVerts.Add(vertices[outline[i  + 1]] - Vector3.up*wallHeight); //bottom right

                wallTriangles.Add(startInd + 0);
                wallTriangles.Add(startInd + 2);
                wallTriangles.Add(startInd + 3);

                wallTriangles.Add(startInd + 3);
                wallTriangles.Add(startInd + 1);
                wallTriangles.Add(startInd + 0);

            }
        }
        wallMesh.vertices = wallVerts.ToArray();
        wallMesh.triangles = wallTriangles.ToArray();
        wallMesh.RecalculateNormals();
       
        if (walls.gameObject.GetComponent<MeshCollider>() != null){
            Destroy(walls.gameObject.GetComponent<MeshCollider>());
        }
        MeshCollider wallCollider = walls.gameObject.AddComponent<MeshCollider> ();
        
        // From here code effects textures
        //float textureScale = walls.gameObject.GetComponentInChildren<MeshRenderer>().material.mainTextureScale.x;
        float textureScaleX = x_scale;
        float increment = (textureScaleX/map.GetLength(1));
        Vector2[] uvs = new Vector2[wallMesh.vertices.Length];
        float[] uvEntries = new float[]{0.5f, increment};

        for (int i = 0; i < wallMesh.vertices.Length; i ++)
        {
            float map_length = map.GetLength(0);
            float fraction = wallHeight / map_length;
            Vector3 wallmeshvertice = wallMesh.vertices[i];
            float value = wallMesh.vertices[i].y*wallTiles;
            value = value * fraction;
            float tmp = (-wallHeight) * squareSize;
            float percentY = Mathf.InverseLerp((-wallHeight)*squareSize, 0, value);
            //float percentY = Mathf.InverseLerp((-wallHeight)*squareSize, 0, wallMesh.vertices[i].y)*wallTiles*(wallHeight/map.GetLength(1));
            float percentX = Mathf.InverseLerp((-wallHeight)*squareSize, 0, wallMesh.vertices[i].y)*wallTiles*(wallHeight/map.GetLength(0));
            //float percentX = Mathf.InverseLerp(-map.GetLength(0)/2*squareSize, map.GetLength(0)/2*squareSize, vertices[i].x)* tileAmount;
            //float percentY = Mathf.InverseLerp(-map.GetLength(1)/2*squareSize, map.GetLength(1)/2*squareSize, vertices[i].z)* tileAmount;
            //float percentY = textureScaleY;
            uvs[i] = new Vector2(uvEntries[i%2], percentY);
            uvs[i] = new Vector2(percentX, percentY);
        }
        wallMesh.uv = uvs; 
        wallCollider.sharedMesh = wallMesh;
        //End here
        walls.mesh = wallMesh;
        

    }
    void TriangulateSquare(Square square) {
		switch (square.config) {
		case 0:
			break;

		// 1 points:
		case 1:
			MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
			break;
		case 2:
			MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
			break;
		case 4:
			MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
			break;
		case 8:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
			break;

		// 2 points:
		case 3:
			MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
			break;
		case 6:
			MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
			break;
		case 9:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
			break;
		case 12:
			MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
			break;
		case 5:
			MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
			break;
		case 10:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
			break;

		// 3 point:
		case 7:
			MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
			break;
		case 11:
			MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
			break;
		case 13:
			MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
			break;
		case 14:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
			break;

		// 4 point:
		case 15:
			MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
			checkedVerts.Add(square.topLeft.vertexIndex);
			checkedVerts.Add(square.topRight.vertexIndex);
			checkedVerts.Add(square.bottomRight.vertexIndex);
			checkedVerts.Add(square.bottomLeft.vertexIndex);
			break;
		}

	}

    void MeshFromPoints(params Node[] points){
        AssignVertices(points);
        if (points.Length >= 3){
            CreateTriangle(points[0], points[1], points[2]);
        }
        if (points.Length >= 4){
            CreateTriangle(points[0], points[2], points[3]);
        }
        if (points.Length >= 5){
            CreateTriangle(points[0], points[3], points[4]);
        }
        if (points.Length >= 6){
            CreateTriangle(points[0],  points[4], points[5]);
        }

    }

    void AssignVertices(Node[] points){
        for (int i= 0; i < points.Length; i ++){
            if (points[i].vertexIndex == -1){
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }
    void CreateTriangle(Node a, Node b, Node c){
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);

        Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
        AddTriangleToDictionary(a.vertexIndex, triangle);
        AddTriangleToDictionary(b.vertexIndex, triangle);
        AddTriangleToDictionary(c.vertexIndex, triangle);
    }

    void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle){
        if (triangleDictionary.ContainsKey(vertexIndexKey)){
            triangleDictionary[vertexIndexKey].Add(triangle);
        } else {
            List<Triangle> triangles = new List<Triangle>();
            triangles.Add(triangle);
            triangleDictionary.Add(vertexIndexKey, triangles);
        }
    }

    int getConnectedOutlineVertex(int vertexIndex){
        List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];
        for (int i = 0; i <trianglesContainingVertex.Count; i ++){
            Triangle triangle = trianglesContainingVertex[i];

            for (int j = 0; j < 3; j ++){
                int vertexB = triangle[j];
                if (vertexB != vertexIndex && !checkedVerts.Contains(vertexB)){
                    if (isOutLineEdge(vertexIndex, vertexB)){
                        return vertexB;
                    }
                }
            }

        }
        return -1;
    }

    bool isOutLineEdge(int vertexA, int vertexB){
        List<Triangle> Atrinagles = triangleDictionary[vertexA];
        int sharedTrigales = 0;

        for (int i = 0; i < Atrinagles.Count; i ++){
            if (Atrinagles[i].containsIndex(vertexB)){
                sharedTrigales ++;
                if (sharedTrigales > 1) break;
            }
        }
        return sharedTrigales == 1;
    }
    void CalculateOutlines(){
        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex ++){
            if (!checkedVerts.Contains(vertexIndex)){
                int newOutlineVert = getConnectedOutlineVertex(vertexIndex);
                if (newOutlineVert != -1){
                    checkedVerts.Add(vertexIndex);
                    List<int> newOutline = new List<int>();
                    newOutline.Add(vertexIndex);
                    outlines.Add(newOutline);
                    followOutline(newOutlineVert, outlines.Count - 1);
                    outlines[outlines.Count - 1].Add(vertexIndex);
                }
            }
        }
    }

    void followOutline(int vertexInd, int outlineIndex){
        outlines[outlineIndex].Add(vertexInd);
        checkedVerts.Add(vertexInd);
        int nextVertexIndex = getConnectedOutlineVertex(vertexInd);

        if (nextVertexIndex != -1){
            followOutline(nextVertexIndex, outlineIndex);
        }
    }

    struct Triangle{
        public int vertexA;
        public int vertexB; 
        public int vertexC;

        int[] vertices;

        public Triangle(int a, int b, int c){
            vertexA = a; 
            vertexB = b;
            vertexC = c;

            vertices = new int[3];
            vertices[0] = a;
            vertices[1] = b;
            vertices[2] = c;

        }
        public int this[int i] {
            get {
                return vertices[i];
            }
        }

        public bool containsIndex(int index){
            if (index == vertexA || index == vertexB || index == vertexC){
                return true;
            }
            return false;
        }
    }

    public class SquareGrid {
        public Square[,] squares;
        public SquareGrid(int[,] map, float squareSize){
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float mapWidth = nodeCountX *squareSize;
            float mapHeight = nodeCountY*squareSize;

            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++){
                for (int y = 0; y < nodeCountY; y ++){
                    Vector3 pos = new Vector3(-mapWidth/2 + x*squareSize + squareSize/2, 0, -mapHeight/2 + y*squareSize + squareSize/2);
                    controlNodes[x,y] = new ControlNode(pos, map[x,y] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++){
                for (int y = 0; y < nodeCountY - 1; y ++){
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x+ 1, y + 1], controlNodes[x + 1, y], controlNodes[x,y]);
                }
            }

        }
    }
    public class Square {
        public ControlNode topLeft, topRight, bottomRight, bottomLeft; 
        public Node centreTop, centreRight, centreBottom, centreLeft;
        public int config;
        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft){
            topLeft = _topLeft;
            topRight = _topRight;
            bottomRight = _bottomRight;
            bottomLeft = _bottomLeft;

            centreTop = topLeft.right;
            centreRight = bottomRight.above;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.above;

            if (topLeft.active) config += 8;
            if (topRight.active) config += 4;
            if (bottomRight.active) config += 2;
            if (bottomLeft.active) config += 1;
        }

    }
    
    public class Node {
		public Vector3 position;
		public int vertexIndex = -1;

		public Node(Vector3 _pos) {
			position = _pos;
		}
	}

	public class ControlNode : Node {

		public bool active;
		public Node above, right;

		public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos) {
			active = _active;
			above = new Node(position + Vector3.forward * squareSize/2f);
			right = new Node(position + Vector3.right * squareSize/2f);
		}

	}
}
