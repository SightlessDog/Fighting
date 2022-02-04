using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadCurrentScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenuScene(){
        SceneManager.LoadScene(0);
    }

    public void LoadPlayScene()
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
