using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BackgroundManager : MonoBehaviour
{
	public GameObject background;

	public Color backgroundColorOne;
	public Color backgroundColorTwo;
	public Color backgroundColorThree;

	public float speed = 0.2f;

	private SpriteRenderer backgroundRenderer;
	private void Awake()
	{
		GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
	}
	private void Start()
	{
		backgroundRenderer = background.GetComponent<SpriteRenderer>();
	}
	private void OnDestroy()
	{
		GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
	}

	public void SwitchBackground(int score)
    {
		if (score == 100)
		{
			StopAllCoroutines();
			StartCoroutine(LerpToColor(backgroundColorTwo));
		}
		else if (score == 200)
		{
			StopAllCoroutines();
			StartCoroutine(LerpToColor(backgroundColorThree));
		}
    }

	private void GameManagerOnGameStateChanged(GameState newState)
	{
		if (newState == GameState.Gameplay || newState == GameState.End)
		{
			StopAllCoroutines();
			StartCoroutine(LerpToColor(backgroundColorOne));
		}
	}

	IEnumerator LerpToColor(Color newColor)
	{
		while (backgroundRenderer.color != newColor)
		{
			backgroundRenderer.color = Color.Lerp(backgroundRenderer.color, newColor, Time.deltaTime);
			yield return new WaitForSeconds(0);
		}
		Debug.Log("LerpToColor finished");
	}
}
