using UnityEngine; 

public class Grid {
    private int width; 
    private int height; 
    private int[,] map;

    public Grid(int width, int height){
        this.width = width;
        this.height = height;
        map = new int[width, height];

    }

    public void setPoint(int x, int y, int data){
        map[x, y] = data;
    }
    public int getPoint(int x, int y){
        return map[x, y];
    }
    public void debug(){
        for (int i = 0; i < width ; i ++){
			for (int j = 0; j < height; j ++){
				Debug.Log("( " + i + ", " + j +"): " +map[i,j]);
			}
		}
    }
    public Grid copy(){
        Grid cpy = new Grid(this.width, this.height);
        for (int i = 0; i < width ; i ++){
			for (int j = 0; j < height; j ++){
				cpy.setPoint(i,j,this.getPoint(i,j));
			}
		}
        return cpy;
    }
    public bool isWithinMapBounds(int x, int y){
        if (x >= width || x <= 0 || y <= 0 || y >= height) return false;
        return true;
    }
    public int[,] getMap(){
        return map;
    }
    
}