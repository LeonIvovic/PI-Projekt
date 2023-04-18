using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject pausedScreenPrefab;
    private GameObject pausedScreen;
    public bool isPaused { get; private set; }

    private void Awake()
    {
        pausedScreen = GameObject.FindGameObjectWithTag("PausedScreen");
        if (pausedScreen == null)
        {
            pausedScreen = Instantiate(pausedScreenPrefab, transform);
        }

        Unpause();
    }

    private void Update()
    {
        // Pause / Unpause with Escape or P
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void RestartLevel()
    {
        // Reloads the current active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        // Loads next scene from build order
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayerDeath()
    {
        // TODO expand later
        RestartLevel();
    }

    public void Pause()
    {
        pausedScreen.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Unpause()
    {
        pausedScreen.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
}
