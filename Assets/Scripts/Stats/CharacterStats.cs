using System;
using UnityEngine;

/* Contains all the stats for a character. */

public class CharacterStats : MonoBehaviour 
{

	public Stat maxHealth;			// Maximum amount of health
	public int currentHealth {get;protected set;}	// Current amount of health

	public Stat damage;
	public Stat armor;

	private float timer;
	private bool onStart = true;

	public event System.Action OnHealthReachedZero;
	public event System.Action OnDamaged;
	public virtual void Awake() {
		currentHealth = maxHealth.GetValue();
	}

	// Start with max HP.
	public virtual void Start ()
	{
	}

	public void Update()
	{
		if (onStart)
		{
			timer += Time.deltaTime;
			// currentHealth = 100000;
			// gameObject.GetComponentInChildren<MaterialControllerSkinned>().changeToTransparent();
			spawnProtection();
		}
	}

	// Damage the character
	public void TakeDamage (int damage)
	{
		// Subtract the armor value - Make sure damage doesn't go below 0.
		damage -= armor.GetValue();
		damage = Mathf.Clamp(damage, 0, int.MaxValue);

		// Subtract damage from health
		currentHealth -= damage;
		Debug.Log(transform.name + " takes " + damage + " damage.");

		// If we hit 0. Die.
		if (currentHealth <= 0)
		{
			if (OnHealthReachedZero != null)
				OnHealthReachedZero();
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

	private void spawnProtection()
	{
		
		if (timer > 5f)
		{
			gameObject.GetComponentInChildren<MaterialControllerSkinned>().changeToDefault();
			currentHealth = 500;
			onStart = false;
		}

		

	}



}