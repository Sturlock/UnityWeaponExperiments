using System;
using System.Collections.Generic;
using UnityEngine;

namespace Owl.Raycast
{
	public class CurvedRaycast : MonoBehaviour
	{
		private const Int32 RESOLUTION = 50;
		private const Single GRAVITY = -9.81f;

		public static Vector3[] CalculateParabolicPath(Vector3 startPosition, Vector3 forwardDirection, Single speed, Vector3 angle)
		{
			List<Vector3> points = new List<Vector3>();
			Vector3 currentPosition = startPosition;

			// Calculate the initial velocity vector
			Vector3 initialVelocity = Quaternion.Euler(angle) * forwardDirection * speed;

			Single timeStep = RESOLUTION / speed;
			Single time = 0f;
			points.Add(currentPosition);

			while (true)
			{
				time += timeStep;
				Single x = initialVelocity.x * time;
				Single y = initialVelocity.y * time + 0.5f * GRAVITY * Mathf.Pow(time, 2);
				Single z = initialVelocity.z * time;
				Vector3 nextPosition = startPosition + new Vector3(x, y, z);

				// Perform a raycast between the current position and the next position
				if (Physics.Raycast(currentPosition, nextPosition - currentPosition, out RaycastHit hit, Vector3.Distance(currentPosition, nextPosition)))
				{
					points.Add(hit.point);
					Debug.Log("Hit: " + hit.collider.name);
					break; // Stop calculation after a hit
				}

				points.Add(nextPosition);
				currentPosition = nextPosition;
			}

			return points.ToArray();
		}


		public static Boolean PerformCurvedRaycast(Vector3[] path, out RaycastHit hit)
		{
			hit = default;
			for (Int32 i = 0; i < path.Length - 1; i++)
			{
				Debug.DrawLine(path[i], path[i + 1], Color.magenta, 5f);
			}

			for (Int32 i = 0; i < path.Length - 1; i++)
			{
				Vector3 start = path[i];
				Vector3 end = path[i + 1];

				if (Physics.Raycast(start, end - start, out hit, Vector3.Distance(start, end)))
				{
					return true; // Stop raycasting after a hit
				}
			}

			return false;
		}
	}
}