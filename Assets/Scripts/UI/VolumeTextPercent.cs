using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VolumeTextPercent : MonoBehaviour
{
    public TMP_Text volumeTextPercent;
    // Start is called before the first frame update
    void Start()
    {
        volumeTextPercent = GetComponent<TMP_Text>();
    }
    
    public void UpdateTextPercent(float volume)
    {
        volumeTextPercent.text = Mathf.RoundToInt(volume * 100) + "%";
    }
}
