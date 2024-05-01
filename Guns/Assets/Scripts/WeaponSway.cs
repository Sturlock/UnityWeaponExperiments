using System;
using UnityEngine;

namespace Owl
{
	public class WeaponSway : MonoBehaviour
	{
		[SerializeField] private Vector2 _Intensity;
		[SerializeField] private Single _Smoothness = 1f;

		private Quaternion _OriginRotation;

		private void Start()
		{
			_OriginRotation = transform.localRotation;
		}

		private void Update()
		{
			UpdateSway();
		}

		private void UpdateSway()
		{
			Vector2 input = PlayerInputHandler.PlayerLook;

			Quaternion xAdjustment = Quaternion.AngleAxis(-_Intensity.x * input.x, Vector3.up);
			Quaternion yAdjustment = Quaternion.AngleAxis(_Intensity.y * input.y, Vector3.right);
			Quaternion targetRotation = _OriginRotation * xAdjustment * yAdjustment;

			Quaternion rotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * _Smoothness);
			transform.localRotation = rotation;
		}
	}
}