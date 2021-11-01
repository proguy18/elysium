using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningSafely : MonoBehaviour
{
    // Start is called before the first frame update
    private MaterialControllerSkinned skin = null;
    private GameObject[] weapons;
    private float timer;
    private CharacterStats stats;
    private bool onStart = true;
    
    void Start()
    {
        skin = gameObject.GetComponentInChildren<MaterialControllerSkinned>();
        weapons = GameObject.FindGameObjectsWithTag("weapon");
        stats = gameObject.GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onStart)
		{
			timer += Time.deltaTime;
			if (skin != null ) skin.changeToTransparent();
			weaponInvisibility(true);
			spawnProtection();
		}
    }
    private void spawnProtection()
	{
		
		if (timer > 5f)
		{
			skin.changeToDefault();
			weaponInvisibility(false);
			stats.Heal(stats.maxHealth.GetValue());
			onStart = false;
		}
        else {
            stats.Heal(500);
        }

	}

    private void weaponInvisibility(bool isInvisible)
    {
	    foreach (GameObject material in weapons)
	    {
		    if (isInvisible) material.GetComponent<MaterialController>().changeToTransparent();
		    
		    else material.GetComponent<MaterialController>().changeToDefault();
		    
	    }
    }
}
