using System;
using UnityEngine;

namespace Owl
{
	public class WeaponSway : MonoBehaviour
	{
		[Header("Position"), SerializeField] private Vector2 _MovementAmount = new Vector2(0.01f, 0.01f);
		[SerializeField] private Single _MaxMovementAmount = 0.1f;
		[SerializeField] private Single _MovementSmoothness = 6f;

		[Header("Rotation"), SerializeField] private Vector2 _RotationAmount;
		[SerializeField] private Single _MaxRotationAmount;
		[SerializeField] private Single _RotationSmoothness;
		[Space, SerializeField] private Boolean _RotationX = true;
		[SerializeField] private Boolean _RotationY = true;
		[SerializeField] private Boolean _RotationZ = true;

		private Vector3 _OriginPosition;
		private Quaternion _OriginRotation;

		private void Start()
		{
			_OriginPosition = transform.localPosition;
			_OriginRotation = transform.localRotation;
		}

		private void Update()
		{
			Vector2 input = CalculateSway();

			MoveSway(input);
			TiltSway(input);
		}

		private void TiltSway(Vector2 input)
		{
			Single xAdjustment = Mathf.Clamp(_MovementAmount.x * input.x, -_MaxMovementAmount, _MaxMovementAmount);
			Single yAdjustment = Mathf.Clamp(_MovementAmount.y * input.y, -_MaxMovementAmount, _MaxMovementAmount);

			Vector3 targetPosition = new Vector3(xAdjustment, yAdjustment, 0);

			transform.localPosition = Vector3.Lerp(transform.localPosition, _OriginPosition + targetPosition, Time.deltaTime * _MovementSmoothness);
		}

		private void MoveSway(Vector2 input)
		{
			Single yAdjustment = Mathf.Clamp(input.x * _RotationAmount.x, -_MaxRotationAmount, _MaxRotationAmount);
			Single xAdjustment = Mathf.Clamp(input.y * _MovementAmount.y, -_MaxRotationAmount, _MaxRotationAmount);

			Single targetX = _RotationX ? -xAdjustment : 0;
			Single targetY = _RotationY ? yAdjustment : 0;
			Single targetZ = _RotationZ ? yAdjustment : 0;
			Quaternion targetRotation = Quaternion.Euler(new Vector3(targetX, targetY, targetZ));

			transform.localRotation = Quaternion.Slerp(transform.localRotation, _OriginRotation * targetRotation, Time.deltaTime * _RotationSmoothness);
		}

		private static Vector2 CalculateSway()
		{
			return -PlayerInputHandler.PlayerLook;
		}
	}
}