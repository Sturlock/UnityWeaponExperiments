using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Owl.Raycast
{
	public class CurvedRaycast : MonoBehaviour
	{
		private const Single GRAVITY = -9.81f;
		private const Single POINT_SPACING = 1f; // The distance between each calculated point along the path

		public static (Vector3[] path, RaycastHit? hit) CalculateParabolicPath(Vector3 startPosition, Vector3 forwardDirection, Single speed, Vector3 angle, Single maxDistance, LayerMask layerMask)
		{
			NativeList<Vector3> path = new(Allocator.TempJob);

			CalculateParabolicPathJob job = new()
			{
				StartPosition = startPosition,
				ForwardDirection = forwardDirection,
				Speed = speed,
				Angle = angle,
				Gravity = GRAVITY,
				PointSpacing = POINT_SPACING,
				MaxDistance = maxDistance,
				Path = path
			};

			JobHandle handle = job.Schedule();
			handle.Complete();

			List<Vector3> pathList = new(path.Length);
			foreach (Vector3 item in path)
			{
				pathList.Add(item);
			}


			RaycastHit? hit = null;
			for (Int32 i = 0; i < pathList.Count - 1; i++)
			{
				Vector3 start = pathList[i];
				Vector3 end = pathList[i + 1];
				if (!Physics.Raycast(start, end - start, out RaycastHit hitInfo, Vector3.Distance(start, end), layerMask)) continue;

				hit = hitInfo;
				pathList = pathList.GetRange(0, i + 2); // Trim the path up to the hit point
				pathList[pathList.Count - 1] = hitInfo.point; // Replace last point with the hit point
				break;
			}

			path.Dispose();

			return (pathList.ToArray(), hit);
		}
	}

	[BurstCompile]
	public struct CalculateParabolicPathJob : IJob
	{
		public Vector3 StartPosition;
		public Vector3 ForwardDirection;
		public Single Speed;
		public Vector3 Angle;
		public Single Gravity;
		public Single PointSpacing;
		public Single MaxDistance;

		public NativeList<Vector3> Path;

		public void Execute()
		{
			Vector3 currentPosition = StartPosition;
			Vector3 initialVelocity = Quaternion.Euler(Angle) * ForwardDirection * Speed;

			Single timeStep = PointSpacing / Speed;
			Single time = 0f;
			Path.Add(currentPosition);

			Single totalDistance = 0f;

			while (totalDistance <= MaxDistance)
			{
				time += timeStep;
				Single x = initialVelocity.x * time;
				Single y = initialVelocity.y * time + 0.5f * Gravity * Mathf.Pow(time, 2);
				Single z = initialVelocity.z * time;
				Vector3 nextPosition = StartPosition + new Vector3(x, y, z);

				Single segmentDistance = Vector3.Distance(currentPosition, nextPosition);
				totalDistance += segmentDistance;

				Path.Add(nextPosition);
				currentPosition = nextPosition;
			}
		}
	}


}