using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFrog : MonoBehaviour
{
    [SerializeField]
    GameObject SmokeEffect;

    [SerializeField]
    GameObject WhaterEffect;

	private void OnMouseDown()
    {
        if (GameManager.Instance.isPaused) { return; }
        if (GameManager.Instance.gameState != GameState.End)
        {
            GameManager.Instance.IncreasScore();
        }
        killForg();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "lake")
        {
            GameManager.Instance.DecreaseHealth();
            Instantiate(WhaterEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void killForg()
    {
		Instantiate(SmokeEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
        AudioManager.instance.PlaySound("Hurt");
	}
}
