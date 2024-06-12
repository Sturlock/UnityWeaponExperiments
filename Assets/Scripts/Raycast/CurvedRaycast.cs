using System;
using UnityEngine;

namespace Owl.Raycast
{
	public class CurvedRaycast : MonoBehaviour
	{
		public static Int32 Resolution = 30;
		public static Single Gravity = -9.81f;

		public static Vector3[] CalculateParabolicPath(Vector3 startPosition, Single speed, Vector3 angle)
		{
			Vector3[] points = new Vector3[Resolution];
			Vector3 radianAngle = new(angle.x.Deg2Rad(), angle.y.Deg2Rad(), angle.z.Deg2Rad());

			for (Int32 index = 0; index < Resolution; index++)
			{
				Single normalizedTime = (Single)index / (Resolution - 1); // Normalized time (0 to 1)
				Single x = speed * normalizedTime * Mathf.Cos(radianAngle.x) * Mathf.Cos(radianAngle.y);
				Single y = speed * normalizedTime * Mathf.Sin(radianAngle.x) + 0.5f * Gravity * Mathf.Pow(normalizedTime, 2);
				Single z = speed * normalizedTime * Mathf.Cos(radianAngle.x) * Mathf.Sin(radianAngle.y);
				points[index] = startPosition + new Vector3(x, y, z);
			}
			return points;
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