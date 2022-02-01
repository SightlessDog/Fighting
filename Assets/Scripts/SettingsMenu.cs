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


    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        _volumeText.text = volume.ToString("0.0");
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("volume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }
    
    public void ResetVolume()
    {
        AudioListener.volume = _defaultVolume;
        _volumeSlider.value = _defaultVolume;
        _volumeText.text = _defaultVolume.ToString("0.0");
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
