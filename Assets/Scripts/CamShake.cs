using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Owl
{
	public class CamShake : MonoBehaviour
	{
		public IEnumerator Shake(Single duration, Single magnitude)
		{
			Vector3 originalPos = transform.localPosition;

			Single elapsed = 0.0f;

			while (elapsed < duration)
			{
				Single x = Random.Range(-1f, 1f) * magnitude;
				Single y = Random.Range(-1f, 1f) * magnitude;

				transform.localPosition = new Vector3(x, y, originalPos.z);

				elapsed += Time.deltaTime;

				yield return null;
			}

			transform.localPosition = originalPos;
		}
	}
}