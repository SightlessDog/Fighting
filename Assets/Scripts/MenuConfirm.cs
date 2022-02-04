using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;


public class MenuConfirm : MonoBehaviour
{
    [SerializeField] private GameObject confirmationPrompt = null;

    public void ConfirmReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void KeepPlaying()
    {
        confirmationPrompt.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            confirmationPrompt.SetActive(true);
        }
    }
}