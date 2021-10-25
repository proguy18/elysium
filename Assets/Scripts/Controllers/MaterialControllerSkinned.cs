using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialControllerSkinned : MonoBehaviour
{
    public Material defaultMaterial;
    public Material transparentMaterial;

    public SkinnedMeshRenderer skinnedMeshRenderer;

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
                Debug.Log(skinnedMeshRenderer.material);
                if (!alt)
                {
                    Debug.Log("changing material");
                    skinnedMeshRenderer.material = transparentMaterial;
                    alt = true;
                }
                else
                {
                    Debug.Log("not changing material");
                    skinnedMeshRenderer.material = defaultMaterial;
                    alt = false;
                }

                cooldown = 1f;
            }
        }
    }
}
