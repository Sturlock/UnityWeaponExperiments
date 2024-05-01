using System;
using UnityEngine;

namespace Owl
{
	public class WeaponSway : MonoBehaviour
	{
		[SerializeField] private Vector2 _Intensity;
		[SerializeField] private Single _Smoothness = 1f;

		private Quaternion _OriginRotation;
		[SerializeField] private Quaternion _Rotation;
		[SerializeField] private Quaternion _XAdjustment;
		[SerializeField] private Quaternion _YAdjustment;

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

			_XAdjustment = Quaternion.AngleAxis(-_Intensity.x * input.x, Vector3.up);
			_YAdjustment = Quaternion.AngleAxis(_Intensity.y * input.y, Vector3.right);
			Quaternion targetRotation = _OriginRotation * _XAdjustment * _YAdjustment;

			_Rotation = Quaternion.Lerp(transform.localRotation , targetRotation, Time.deltaTime * _Smoothness);
			transform.localRotation = _Rotation;
		}
	}
}