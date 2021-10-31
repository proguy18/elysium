using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningSafely : MonoBehaviour
{
    // Start is called before the first frame update
    private MaterialControllerSkinned skinned = null;
    private float timer;
    private CharacterStats stats;
    private bool onStart = true;
    
    void Start()
    {
        skinned = gameObject.GetComponentInChildren<MaterialControllerSkinned>();
        stats = gameObject.GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onStart)
		{
			timer += Time.deltaTime;
			if (skinned != null )skinned.changeToTransparent();
			spawnProtection();
		}
    }
    private void spawnProtection()
	{
		
		if (timer > 5f)
		{
			gameObject.GetComponentInChildren<MaterialControllerSkinned>().changeToDefault();
			stats.Heal(stats.maxHealth.GetValue());
			onStart = false;
		}
        else {
            stats.Heal(500);
        }

	}
}
