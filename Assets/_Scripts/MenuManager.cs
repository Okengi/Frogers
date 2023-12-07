using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	[Header("Screens")]

	[SerializeField] GameObject leaderboardScreen;
	[SerializeField] GameObject pauseScreen;

	[Header("Texts")]

	[SerializeField] TextMeshProUGUI scoreText;
	[SerializeField] TextMeshProUGUI healthText;
	[SerializeField] GameObject highScoreText;

	[Header("Buttons")]

	[SerializeField] GameObject pauseButton;
	[SerializeField] GameObject startGameButton;

	private void Awake()
	{
		GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
		GameManager.Paused += GameManagerPaused;
	}

	private void OnDestroy()
	{
		GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
		GameManager.Paused -= GameManagerPaused;
	}

	private void Start()
	{
		pauseScreen.SetActive(false);
	}

	private void GameManagerOnGameStateChanged(GameState newState)
	{
		highScoreText.SetActive(newState == GameState.Beginn || newState == GameState.End);
		leaderboardScreen.SetActive(newState == GameState.Beginn || newState == GameState.End);
		pauseButton.SetActive(newState == GameState.Gameplay);
		switch (newState)
		{
			case GameState.Beginn:
				startGameButton.SetActive(true);
				startGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Beginn";
				break;
			case GameState.Gameplay:
				startGameButton.SetActive(false);
				break;
			case GameState.End:
				startGameButton.SetActive(true);
				startGameButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
				break;
		}
	}

	private void GameManagerPaused(bool isPaused)
	{
		pauseScreen.SetActive(isPaused);
	}

	public void UpdateScore(int score)
	{
		scoreText.text = score.ToString();
	}

	public void UpdateHealth(int health)
	{
		healthText.text = "Health: " + health.ToString();
	}

	public void UpdateHighScore(int _highScore, bool newOne)
	{
		if (newOne)
		{
			highScoreText.GetComponent<TextMeshProUGUI>().text = "New Highscore\n" + _highScore.ToString();
		}
		else
		{
			highScoreText.GetComponent<TextMeshProUGUI>().text = "Highscore\n" + _highScore.ToString();
		}
	}
}
