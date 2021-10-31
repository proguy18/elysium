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
        cam = Camera.main.transform;
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if(c.renderMode == RenderMode.WorldSpace) 
            {
                Debug.Log("Canvas name is " + c.name);
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
        // StartCoroutine(DestroyHealthUIIn(1f));
    }

    // private IEnumerator DestroyHealthUIIn(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     Destroy(HealthUITransform.gameObject);
    //     HealthUITransform = null;
    // }

    // Update is called once per frame
    void Update()
    {
        if (HealthUITransform == null)
            return;
        HealthUITransform.position = target.position;
        healthSlider.fillAmount = GetHealthPercent();
    }

    private void LateUpdate()
    {
        if (HealthUITransform == null)
            return;
        var camPosition = cam.position;
        Debug.Log("Name of camera is " + cam.name + " position is " + cam.position);
        HealthUITransform.LookAt (
            new Vector3(camPosition.x,transform.position.y,camPosition.z), Vector3.down);
        
    }
    float GetHealthPercent() {
        return Mathf.Clamp01(stats.currentHealth / (float)stats.maxHealth.GetValue ());
    }
}
