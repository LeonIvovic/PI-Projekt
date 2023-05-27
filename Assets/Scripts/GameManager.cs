using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    // Prefabs
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private GameObject pausedScreenPrefab;
    [SerializeField] private GameObject scoreDisplayPrefab;

    private DataHolder data;

    // Instances
    private GameObject healthBar;
    private GameObject pausedScreen;
    private TMPro.TMP_Text scoreDisplay;

    // Paused check
    public bool isPaused { get; private set; }

    // Checkpoint location
    private Vector2 checkpoint;

    private void Awake()
    {
        // Additional check just in case
        if (GetInstance() != this) Destroy(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        CheckAndAddPauseScreen();

        data = ScriptableObject.CreateInstance<DataHolder>();
        data.scores = new int[SceneManager.sceneCountInBuildSettings];
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

        // Display score and increase it over time
        data.currentLevelScore += (Time.deltaTime * data.scorePerSecond);
        scoreDisplay.text = data.currentLevelScore.ToString("0"); // "0" - Don't show decimals
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Load players health from checkpoint or last level
        // If checkpoint was active move the player to its position
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            if (checkpoint.magnitude != 0)
            {
                player.transform.position = checkpoint + new Vector2(0, 3);
            }
            player.LoadSaved(data.savedHp, data.savedMaxHp);
        }

        if (healthBar == null) healthBar = Instantiate(healthBarPrefab, transform);
        if (scoreDisplay == null) scoreDisplay = Instantiate(scoreDisplayPrefab, transform).GetComponentInChildren<TMPro.TMP_Text>();

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
        // Save current health and max health so we can load them in next level
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (player != null)
        {
            data.savedHp = player.GetHealth();
            data.savedMaxHp = player.GetMaxHealth();
        }

        // Disable checkpoint or the player will be moved to previous level's checkpoint position
        DisableCheckpoint();

        // Save score for current level and reset the coutner for the next one
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        data.scores[currentLevelIndex] = (int)data.currentLevelScore;
        data.currentLevelScore = 0;

        // If next scene is a level - load next scene from build
        string nextScene = SceneUtility.GetScenePathByBuildIndex(currentLevelIndex + 1);
        if (nextScene != null && nextScene.Contains("Level"))
        {
            SceneManager.LoadScene(currentLevelIndex + 1);
        }
        // Else if this was the Last level - upload score and show leaderboard
        else
        {
            Destroy(this.gameObject);
            ScoreManager.UploadScore(data.scores.Sum());
            SceneManager.LoadScene("Leaderboard");
        }
    }

    public void PlayerDeath()
    {
        // Score increase on death
        data.currentLevelScore += data.scorePerDeath;

        RestartLevel();
    }

    public void SetCheckPoint(Vector2 checkpoint, int hp, int maxHp)
    {
        this.checkpoint = checkpoint;
        data.savedHp = hp;
        data.savedMaxHp = maxHp;
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
