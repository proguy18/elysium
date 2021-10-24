using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;

    private Transform ui;
    private Image healthSlider;
    private Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if(c.renderMode == RenderMode.WorldSpace) 
            {
                ui = Instantiate(uiPrefab, c.transform).transform;
                healthSlider = ui.GetChild(0).GetComponent<Image>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
        ui.position = target.position;
        
    }

    private void LateUpdate()
    {
        var camPosition = cam.position;
        transform.LookAt (new Vector3(camPosition.x,transform.position.y,camPosition.z), Vector3.down);
    }
}
