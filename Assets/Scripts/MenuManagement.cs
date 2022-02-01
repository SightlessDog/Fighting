using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;


public class MenuManagement : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Quit()
    {
        // Following line only for development mode
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
