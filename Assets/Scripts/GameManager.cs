using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject pausedScreenPrefab;
    [SerializeField] private GameObject healthBarPrefab;
    private GameObject pausedScreen;
    public bool isPaused { get; private set; }
    private Vector2 checkpoint;
    private int checkPointHp;
    private int checkPointMaxHp;


    private void Awake()
    {
        if (GetInstance() != this) Destroy(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckAndAddPauseScreen();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If checkpoint is active, move player to it's position and set it's health to the time of activation
        if (checkpoint.magnitude != 0)
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (player != null)
            {
                player.transform.position = checkpoint + new Vector2(0, 3);
                player.LoadCheckpoint(checkPointHp, checkPointMaxHp);
            }
        }

        Instantiate(healthBarPrefab);
    }


    private void CheckAndAddPauseScreen()
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
    
    public void DisableCheckpoint()
    {
        checkpoint = Vector2.zero;
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

    public void SetCheckPoint(Vector2 checkpoint, int hp, int maxHp)
    {
        this.checkpoint = checkpoint;
        checkPointHp = hp;
        checkPointMaxHp = maxHp;
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
