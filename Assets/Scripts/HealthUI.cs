using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;

    Transform ui;
    Image healthSlider;

    // Start is called before the first frame update
    void Start()
    {
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
}
