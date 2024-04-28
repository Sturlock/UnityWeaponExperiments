using System;
using UnityEngine;

namespace Owl
{
	public class PlayerCameraController : MonoBehaviour
	{
		[SerializeField] private Transform _PlayerTransform;
		[SerializeField] private Vector2 _MouseSensitivity;
		private Single _XRotation;

		// Start is called before the first frame update
		private void Start()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}

		// Update is called once per frame
		private void Update()
		{
			Vector2 mouseValues = PlayerInputHandler.PlayerLook * _MouseSensitivity * Time.deltaTime;

			_XRotation -= mouseValues.y;
			_XRotation = Mathf.Clamp(_XRotation, -90, 90);

			transform.localRotation = Quaternion.Euler(_XRotation, 0, 0);
			_PlayerTransform.Rotate(Vector3.up * mouseValues.x);
		}
	}
}