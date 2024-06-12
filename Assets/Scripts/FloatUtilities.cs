using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Owl
{
	public static class FloatUtilities
	{
		public static Single Range(this Single single)
		{
			return Random.Range(-single, single);
		}

		public static Single Deg2Rad(this Single single)
		{
			return Mathf.Deg2Rad * single;
		}
	}
}