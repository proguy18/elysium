using UnityEngine;
using System.Collections.Generic;

public class Item : MonoBehaviour {
	public enum Type {Sword, Ring, Potion};	
	public Sprite icon = null;	
	public Type type;
	public int damageBuff;
	public int maxHealthBuff;
	public int armourBuff;
	public enum Effect {none, refillHealth}
    public Effect potionEffect = Effect.none;
	public int effectValue;

	public Item(Sprite icon, Type type){
		this.icon = icon;
		this.type = type;
	}


}
