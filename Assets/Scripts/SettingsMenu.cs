using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider = null;
    [SerializeField] private TMP_Text _volumeText = null;
    [SerializeField] private GameObject confirmationPrompt = null;
    [SerializeField] private float _defaultVolume = 0.5f;

	[SerializeField] private TMP_Dropdown qualityDropdown;
	[SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
		_qualityLevel = qualityIndex;
		Debug.Log(_qualityLevel);
    }
    
    public void SetFullScreen(bool isFullScreen)
    {
		if ( _isFullScreen ) {
			Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
			_isFullScreen = false;
	} 
		else { 
			Screen.fullScreenMode = FullScreenMode.Windowed;
			_isFullScreen = true;
		}    
	}

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        _volumeText.text = volume.ToString("0.0");
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("volume", AudioListener.volume);
                
        PlayerPrefs.SetInt("quality", _qualityLevel);

        PlayerPrefs.SetInt("fullscreen", (_isFullScreen ? 0 : 1));

        StartCoroutine(ConfirmationBox());
    }
    
    public void ResetSettings()
    {
        AudioListener.volume = _defaultVolume;
        _volumeSlider.value = _defaultVolume;
        _volumeText.text = _defaultVolume.ToString("0.0");
		
		qualityDropdown.value = 1;
		QualitySettings.SetQualityLevel(1);
		
        fullScreenToggle.isOn = true;
		Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    }

    public IEnumerator ConfirmationBox()
    {
		Debug.Log("CONFIRMATION!");
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
