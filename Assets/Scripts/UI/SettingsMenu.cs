using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    private Resolution[] m_Resolutions;
    public TMPro.TMP_Dropdown resolutionsDropDown;
    public TMPro.TMP_Dropdown qualityDropDown;
    public TMPro.TMP_Dropdown displayDropDown;
    
    void Start()
    {
        //Sets default display mode
        if (Screen.fullScreenMode.Equals(FullScreenMode.ExclusiveFullScreen))
            displayDropDown.value = 0;
        else if (Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow))
            displayDropDown.value = 1;
        else if (Screen.fullScreenMode.Equals(FullScreenMode.Windowed))
            displayDropDown.value = 2;
        displayDropDown.RefreshShownValue();

        //Sets default graphics quality to 'High'
        qualityDropDown.value = 3; 
        qualityDropDown.RefreshShownValue();
        
        // Sets default resolution settings
        m_Resolutions = Screen.resolutions;
        resolutionsDropDown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < m_Resolutions.Length; i++)
        {
            // Only accept refresh rates of 60 Or 120hz
            if (m_Resolutions[i].refreshRate == 120 || m_Resolutions[i].refreshRate == 60)
            {
                string option = m_Resolutions[i].width + " x " + m_Resolutions[i].height + " (" + m_Resolutions[i].refreshRate + "Hz)";
                options.Add(option);
            
                if(m_Resolutions[i].width == Screen.currentResolution.width &&
                   m_Resolutions[i].height == Screen.currentResolution.height &&
                   m_Resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
                    currentResolutionIndex = i;
            }
        }
        
        resolutionsDropDown.AddOptions(options);
        resolutionsDropDown.value = currentResolutionIndex;
        resolutionsDropDown.RefreshShownValue();
    }
    public void SetVolume(float volume)
    {
        // Adjust Volume 
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
    public void SetDisplay(int displayIndex)
    {
        Screen.fullScreenMode = displayIndex switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.FullScreenWindow,
            2 => FullScreenMode.Windowed,
            _ => Screen.fullScreenMode
        };
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = m_Resolutions[resolutionIndex];
        
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }
}
