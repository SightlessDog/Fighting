using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private TMP_Text volumeText = null;
    [SerializeField] private float _defaultVolume = 0.5f;
	[SerializeField] private TMP_Dropdown qualityDropdown;
	[SerializeField] private Toggle fullScreenToggle;

    void Awake()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            float localVolume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = localVolume;
			volumeText.text = localVolume.ToString("0.0");
			AudioListener.volume = localVolume;
        }
        else
        {
            AudioListener.volume = _defaultVolume;
            volumeSlider.value = _defaultVolume;
            volumeText.text = _defaultVolume.ToString("0.0");
        }

        if (PlayerPrefs.HasKey("quality"))
        {
            int localQuality = PlayerPrefs.GetInt("quality");
			qualityDropdown.value = localQuality;
			QualitySettings.SetQualityLevel(localQuality);
        }
        else
        {
            QualitySettings.SetQualityLevel(1);
        }

        if (PlayerPrefs.HasKey("fullScreen"))
        {
            int localFullScreen = PlayerPrefs.GetInt("fullScreen");
				if ( localFullScreen == 1 ) {
					Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
					localFullScreen = 0;
					fullScreenToggle.isOn = true;			
				} else { 
					Screen.fullScreenMode = FullScreenMode.Windowed;
					localFullScreen = 1;
					fullScreenToggle.isOn = false;
				}
		}
    }

}
