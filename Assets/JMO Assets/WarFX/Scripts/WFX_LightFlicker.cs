using System;
using System.Collections;
using UnityEngine;

/**
 * *	Rapidly sets a light on/off.
 * *
 * *	(c) 2015, Jean Moreno
 * *
 */
[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
	public Single time = 0.05f;

	private Single timer;

	void Start()
	{
		timer = time;
		StartCoroutine("Flicker");
	}

	IEnumerator Flicker()
	{
		while (true)
		{
			GetComponent<Light>().enabled = !GetComponent<Light>().enabled;

			do
			{
				timer -= Time.deltaTime;
				yield return null;
			} while (timer > 0);

			timer = time;
		}
	}
}