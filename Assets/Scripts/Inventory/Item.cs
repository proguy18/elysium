using UnityEngine;
using System.Collections.Generic;
using System;

public class Item : MonoBehaviour {
	public enum Type {Sword, Ring, Potion};	
	public Sprite icon = null;	
	public Type type;
	public int damageModifier;
	public int maxHealthModifier;
	public int armorModifier;
	public enum Effect {none, refillHealth}
    public Effect potionEffect = Effect.none;
	public int effectValue;

	public void modifyStats(float multiplier){
		float hpMod = multiplier*(1 + getGaussian());
		float dammageMod = multiplier*(1 + getGaussian());
		float armourMod = multiplier*(1 + getGaussian());
		maxHealthModifier = Convert.ToInt32(Math.Ceiling(maxHealthModifier*hpMod));
		damageModifier = Convert.ToInt32(Math.Ceiling(dammageMod*damageModifier));
		armorModifier = Convert.ToInt32(Math.Ceiling(armourMod*armorModifier));
	}
	float getGaussian(){
		//returns a number greater than -1 
		Gaussian gaussian = new Gaussian();
		float dist = Convert.ToSingle(gaussian.RandomGauss(0, 0.33));
		while (dist < - 1){
			dist = Convert.ToSingle(gaussian.RandomGauss(0, 0.33));
		}
		return dist;
	}
}
