using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
 
    public void ButtonScore()
    {
        // Hide other buttons (easy if grouped in editor)
        // Show Score screen (enable score group)
    }

    public void ButtonSettings()
    {
        // Hide other buttons(easy if grouped in editor)
        // Show Settings screen(enable Settings group)
    }
}
