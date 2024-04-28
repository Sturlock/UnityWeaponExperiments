using System;
using UnityEngine;

namespace Owl
{
	public class WeaponSway : MonoBehaviour
	{
		[SerializeField] private Single _Intensity = 1.0f;
		[SerializeField] private Single _Smoothness = 1.0f;

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

			Quaternion xAdjustment = Quaternion.AngleAxis(-_Intensity * input.x, Vector3.up);
			Quaternion yAdjustment = Quaternion.AngleAxis(_Intensity * input.y, Vector3.right);
			Quaternion targetRotation = _OriginRotation * xAdjustment * yAdjustment;

			transform.localRotation = Quaternion.Lerp(transform.localRotation , targetRotation, Time.deltaTime * _Smoothness);
		}
	}
}