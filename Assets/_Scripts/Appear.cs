using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appear : MonoBehaviour
{
	public float delay = 0.5f;
	public float time = 0.3f;
	// Update is called once per frame
	private void OnEnable()
	{
		Vector3 pos = transform.localScale;
		transform.localScale = new Vector3(0, 0, 0);
		LeanTween.scale(gameObject, pos, time).setDelay(delay);
	}
}