using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFrog : MonoBehaviour
{
    [SerializeField]
    GameObject frog;

    [HideInInspector]
    public float time = 0.8f;

    private List<GameObject> frogs = new List<GameObject>();

    IEnumerator spawnCoroutine;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void Start()
    {
        spawnCoroutine = spawningFrogs();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged(GameState newState)
    {
        if(newState == GameState.Gameplay)
        {
            StartCoroutine(spawnCoroutine);
        }
        if(newState == GameState.End) 
        {
            StopCoroutine(spawnCoroutine);
            StartCoroutine(getRideOfFrogs());
        }
    }

    IEnumerator getRideOfFrogs()
    {
		for (int i = frogs.Count; i > GameManager.Instance.m_score + 3; i--)
		{
			frogs[i - 1].GetComponent<KillFrog>().killForg();
            yield return new WaitForSeconds(0.1f);
		}
	}

    IEnumerator spawningFrogs()
    {
        yield return new WaitForSeconds(0.25f);
        while (true)
        {
            yield return new WaitForSeconds(time);

            float posY = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
            float posX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x + 1, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - 1);
            Vector2 spawnPoint = new Vector2(posX, posY + Random.Range(0.5f,2));

            frogs.Add(Instantiate(frog, spawnPoint, Quaternion.identity));
        }
    }
}
