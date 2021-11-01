using System;
using UnityEngine;

/* Contains all the stats for a character. */

public class CharacterStats : MonoBehaviour 
{

	public Stat maxHealth;			// Maximum amount of health
	public int currentHealth {get;protected set;}	// Current amount of health

	public Stat damage;
	public Stat armor;
	

	public event System.Action OnHealthReachedZero;
	public event System.Action OnDamaged;
	private int minimumDamage; 
	private int baseHp; 
	private int baseDammage; 
	private int baseArmour;
	

	public virtual void Awake() {
		currentHealth = maxHealth.GetValue();
		minimumDamage = maxHealth.GetValue() / 100; // 1% of base health
		baseHp = currentHealth;
		baseArmour = armor.GetValue();
		baseDammage = damage.GetValue();;
		
	}
	public int getBaseHp(){
		return baseHp;
	}
	public int getBaseDamage(){
		return baseDammage;
	}
	public int getBaseArmor(){
		return baseArmour;
	}


	// Start with max HP.


	// Damage the character
	public void TakeDamage (int damage)
	{
		damage -= (damage * armor.GetValue() / 100); // armour modifier (eg. 50 armour = 50% damage reduction)
		damage = Mathf.Clamp(damage, minimumDamage, currentHealth); // damage must be between 1% of base health and 100% of current health 

		// Subtract damage from health
		currentHealth -= damage;

		// If we hit 0. Die.
		if (currentHealth <= 0)
		{
			if (OnHealthReachedZero != null)
				OnHealthReachedZero();
				if (gameObject.layer == 6){
					gameObject.SendMessageUpwards("incrementKillCount", gameObject.transform.position);
				}
		}
		else
		{
			if (OnDamaged != null)
				OnDamaged();
		}
	}

	// Heal the character.
	public void Heal (int amount)
	{
		currentHealth += amount;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
	}

}