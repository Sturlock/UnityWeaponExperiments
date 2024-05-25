using System;
using UnityEngine;

namespace Owl
{
	public class WeaponSway : MonoBehaviour
	{
		[Header("Position"), SerializeField] private Vector2 _MovementAmount = new Vector2(0.01f, 0.01f);
		[SerializeField] private Single _MaxMovementAmount = 0.1f;
		[SerializeField] private Single _MovementSmoothness = 6f;

		[Header("Tilt"), SerializeField] private Vector2 _TiltAmount = new Vector2(4, 4);
		[SerializeField] private Single _MaxTiltAmount = 1;
		[SerializeField] private Single _TiltSmoothness = 1;
		[Space, SerializeField] private Boolean _TiltX = true;
		[SerializeField] private Boolean _TiltY = true;
		[SerializeField] private Boolean _TiltZ = true;

		[Header("Rotation"), SerializeField] private Vector2 _RotationIntensity = new Vector2(1, 1);
		[SerializeField] private Single _RotationSmoothness = 1;

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
			RotateSway(input);
		}

		private void MoveSway(Vector2 input)
		{
			Single xAdjustment = Mathf.Clamp(_MovementAmount.x * input.x, -_MaxMovementAmount, _MaxMovementAmount);
			Single yAdjustment = Mathf.Clamp(_MovementAmount.y * input.y, -_MaxMovementAmount, _MaxMovementAmount);

			Vector3 targetPosition = new Vector3(xAdjustment, yAdjustment, 0);

			transform.localPosition = Vector3.Lerp(transform.localPosition, _OriginPosition + targetPosition, Time.deltaTime * _MovementSmoothness);
		}

		private void TiltSway(Vector2 input)
		{
			Single yAdjustment = Mathf.Clamp(input.x * _TiltAmount.x, -_MaxTiltAmount, _MaxTiltAmount);
			Single xAdjustment = Mathf.Clamp(input.y * _MovementAmount.y, -_MaxTiltAmount, _MaxTiltAmount);

			Single targetX = _TiltX ? -xAdjustment : 0;
			Single targetY = _TiltY ? yAdjustment : 0;
			Single targetZ = _TiltZ ? yAdjustment : 0;
			Quaternion targetRotation = Quaternion.Euler(new Vector3(targetX, targetY, targetZ));

			transform.localRotation = Quaternion.Slerp(transform.localRotation, _OriginRotation * targetRotation, Time.deltaTime * _TiltSmoothness);
		}

		private void RotateSway(Vector2 input)
		{
			Quaternion xAdjustment = Quaternion.AngleAxis(-_RotationIntensity.x * input.x, Vector3.up);
			Quaternion yAdjustment = Quaternion.AngleAxis(_RotationIntensity.y * input.y, Vector3.right);

			Quaternion targetRotation = _OriginRotation * xAdjustment * yAdjustment;

			transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * _RotationSmoothness);
		}

		private static Vector2 CalculateSway()
		{
			return -PlayerInputHandler.PlayerLook;
		}
	}
}