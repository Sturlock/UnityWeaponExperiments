﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Owl
{
	public class WeaponFireControl : MonoBehaviour
	{
		[SerializeField] private Single _Rpm = 120;
		[SerializeField] private GameObject _Barrel;
		[SerializeField] private LayerMask _LayerMask;
		[SerializeField] private Double _Timer;
		[SerializeField] private Single _FireRate;
		private Camera _Camera;
		private InputAction.CallbackContext _FireCallback;

		private void Start()
		{
			_Camera = Camera.main;

			const Single MINUTE_IN_SECONDS = 60;
			Single rps = _Rpm / MINUTE_IN_SECONDS;
			_FireRate = 1/rps;

			PlayerInputHandler.WeaponFireAction += context => _FireCallback = context;
		}

		private void Update()
		{
			if (_Timer < 0)
			{
				if (_FireCallback.phase is not InputActionPhase.Performed) return;

				Vector3 barrelPoint = _Barrel.transform.position;
				Vector2 direction = new(0.5F, 0.5F);
				Ray shotRay = _Camera.ViewportPointToRay(direction);
				Ray shot = new(barrelPoint, shotRay.direction);

				if (Physics.Raycast(shot, out RaycastHit hit, Mathf.Infinity, _LayerMask)) Debug.DrawRay(barrelPoint, shot.direction * 10, Color.magenta, 5f);

				_Timer = _FireRate;
			}
			else
			{
				_Timer -= Time.fixedDeltaTime;
			}
		}
	}
}