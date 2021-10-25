using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    public Material defaultMaterial;
    public Material transparentMaterial;

    public MeshRenderer meshRenderer;

    // Just for debugging
    public KeyCode changeMaterial = KeyCode.F;
    
    private bool alt = false;
    
    private float cooldown;


    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (Input.GetKey(changeMaterial))
        {
            if (cooldown <= 0f)
            {
                if (!alt)
                {
                    meshRenderer.material = transparentMaterial;
                    alt = true;
                }
                else
                {
                    meshRenderer.material = defaultMaterial;
                    alt = false;
                }

                cooldown = 1f;
            }
        }
    }
}
