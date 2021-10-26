using UnityEngine;

/* The base item class. All items should derive from this. */

public class Item : MonoBehaviour {
	public enum Type {Sword, Ring, Potion};
	public string name = "New Item";
	public Sprite icon = null;	
	public Type type;

	public Item(string name, Sprite icon, Type type){
		this.name = name;
		this.icon = icon;
		this.type = type;
	}


}
