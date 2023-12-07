using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singelton
    public static GameManager Instance;
	private void Awake()
    {
        Instance = this;
    }

    // GameState
    public GameState gameState;
    public static event Action<GameState> OnGameStateChanged;

    // Pause
    public static event Action<bool> Paused;
    [HideInInspector]
    // isPaused is equal to false when the game is active and equalt to true when isPaused
    public bool isPaused;

    // PlayFab / Leaderboard
    [SerializeField]
    PlayFapManager playFapManager;

    //  UI + Spawner
    [SerializeField]
    MenuManager menuManager;
    [SerializeField]
    SpawnFrog spawner;
    public TextMeshProUGUI spawnRateText;
    private double spawnRate;

    public TextMeshProUGUI timeBetweenSpawnText;

    // camera Shake
    Animator cameraAnimator;

    [HideInInspector]
    public int m_score;
    int m_health;
    int m_highScore;

    [SerializeField]
    private BackgroundManager backgroundManager;

    void Start()
    {
        cameraAnimator = Camera.main.GetComponent<Animator>();
        isPaused = false;
        UpdateGameState(GameState.Beginn);
    }

    public void UpdateGameState(GameState newGameState)
    {
        gameState = newGameState;

        switch (newGameState)
        {
            case GameState.Beginn:
				//m_highScore = PlayerPrefs.GetInt("HighScore", 0);
                m_highScore = playFapManager.playerStatisticValue;
                menuManager.UpdateHighScore(m_highScore, false);
                StartCoroutine(getHighScore());
                break;
            case GameState.Gameplay:
				m_score = 0;
                m_health = 3;
                menuManager.UpdateHealth(m_health);
                menuManager.UpdateScore(m_score);
                spawner.time = 0.8f;
                UpdateSpawnRate();
				break;
            case GameState.End:
				if (m_score > m_highScore)
                {
                    m_highScore = m_score;    
                }
                playFapManager.SendLeaderBoard(m_score);
                menuManager.UpdateHighScore(m_highScore, m_highScore == m_score);
                break;
        }
        OnGameStateChanged?.Invoke(gameState);
    }

    private IEnumerator getHighScore()
    {
        yield return new WaitForSeconds(1f);
        m_highScore = playFapManager.playerStatisticValue;
        menuManager.UpdateHighScore(m_highScore, false);
    }

    public void Beginn() {
        UpdateGameState(GameState.Gameplay);
    }

    private void UpdateSpawnRate()
    {
		spawnRate = Math.Round(1 / spawner.time, 2);
		spawnRateText.text = spawnRate.ToString() + "/s";
	}

    public void ToggelPause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            timeBetweenSpawnText.text = "Time Between Spawn: " + spawner.time.ToString()+"s";
        }
        else
        {
            Time.timeScale = 1;
		}
		isPaused = !isPaused;
        Paused?.Invoke(isPaused);
	}

    public void IncreasScore()
    {
        m_score++;
        menuManager.UpdateScore(m_score);
        AudioManager.instance.PlaySound("KillFrog");
        // increase spawn rate
        if(m_score % 10 == 0)
        {
            if (m_score >= 120) 
            {
                spawner.time -= 0.03f;
            }
            else
            {
                spawner.time -= 0.05f;
            }
            backgroundManager.SwitchBackground(m_score);
        }
		UpdateSpawnRate();
	}

	public void DecreaseHealth()
    {
        m_health--;
        menuManager.UpdateHealth(Math.Clamp(m_health, 0, 3));
        AudioManager.instance.PlaySound("Hurt");
        // spawn rate
        spawner.time += 0.075f;
		UpdateSpawnRate();
		cameraAnimator.SetTrigger("shakeTrigger");
        if (m_health <= 0) 
        {
            UpdateGameState(GameState.End);
        }
    }
}

public enum GameState
{
    Beginn,
    Gameplay,
    End,
}