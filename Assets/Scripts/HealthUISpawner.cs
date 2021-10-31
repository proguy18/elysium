using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUISpawner : MonoBehaviour
{
    public GameObject uiPrefab;
    public Transform target;

    private Transform HealthUITransform;
    private Image healthSlider;
    private Transform cam;
    private CharacterStats stats;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = PlayerCamera.Camera.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if(c.renderMode == RenderMode.WorldSpace) 
            {
                HealthUITransform = Instantiate(uiPrefab, c.transform).transform;
                healthSlider = HealthUITransform.GetChild(0).GetComponent<Image>();
                break;
            }
        }
    }
    private void OnEnable()
    {
        stats.OnHealthReachedZero += DestroyHealthUI;
    }

    private void OnDisable()
    {
        stats.OnHealthReachedZero -= DestroyHealthUI;
    }
    private void OnDestroy() {
        DestroyHealthUI();
    }

    private void DestroyHealthUI()
    {
        if (HealthUITransform == null)
            return;
        Destroy(HealthUITransform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthUITransform == null)
            return;
        healthSlider.fillAmount = GetHealthPercent();
    }

    private void LateUpdate()
    {
        if (HealthUITransform == null)
            return;
        var camPosition = cam.position;
        HealthUITransform.LookAt (
            new Vector3(camPosition.x,camPosition.y,camPosition.z), Vector3.down);
        HealthUITransform.position = target.position;
        if(name.Contains("Player"))
            Debug.Log("target position = " + target.position);
    }
    float GetHealthPercent() {
        return Mathf.Clamp01(stats.currentHealth / (float)stats.maxHealth.GetValue ());
    }
}
