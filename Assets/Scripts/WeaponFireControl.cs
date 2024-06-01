using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Owl
{
	/// <summary>
	///    A class for handing how a weapon fires
	/// </summary>
	public class WeaponFireControl : MonoBehaviour
	{
		[SerializeField] private Single _Rpm = 120;
		[SerializeField] private GameObject _Barrel;
		[SerializeField] private LayerMask _LayerMask;
		[SerializeField] private Boolean _FullAuto;
		[SerializeField] private Single _Damage;
		private Camera _Camera;
		private InputAction.CallbackContext _FireCallback;
		private Single _FireRate;
		private Magazine _Magazine;
		private Int32 _ShotsFired;
		private Single _Timer;

		private void Start()
		{
			//This is safe to do as there should only be one Magazine per weapon.
			_Magazine = GetComponentInChildren<Magazine>();
			_Camera = Camera.main;

			const Single MINUTE_IN_SECONDS = 60;
			Single rps = _Rpm / MINUTE_IN_SECONDS;
			_FireRate = 1 / rps;
		}

		private void Update()
		{
			if (_Timer < 0)
			{
				if (!CanShootWeapon()) return;

				if (_FireCallback.phase is not InputActionPhase.Performed)
				{
					_ShotsFired = 0;
					return;
				}

				if (!_FullAuto && _ShotsFired >= 1) return;

				Vector3 barrelPoint = _Barrel.transform.position;
				Vector2 direction = new(0.5F, 0.5F);
				Ray shotRay = _Camera.ViewportPointToRay(direction);
				Ray shot = new(barrelPoint, shotRay.direction);

				if (Physics.Raycast(shot, out RaycastHit hit, Mathf.Infinity, _LayerMask))
				{
					Debug.DrawRay(barrelPoint, shot.direction * 10, Color.magenta, 5f);
					IDamageable damageable = hit.transform.gameObject.GetComponent<IDamageable>();
					damageable?.DamageTarget(_Damage);
				}

				_Timer = _FireRate;
				_ShotsFired++;
				_Magazine.AmmunitionCount--;
			}
			else
			{
				_Timer -= Time.deltaTime;
			}
		}

		private void OnEnable()
		{
			PlayerInputHandler.WeaponFireAction += context => _FireCallback = context;
			PlayerInputHandler.WeaponReloadAction += WeaponReload;
		}

		private void OnDisable()
		{
			PlayerInputHandler.WeaponFireAction -= context => _FireCallback = context;
			PlayerInputHandler.WeaponReloadAction -= WeaponReload;
		}

		private void WeaponReload()
		{
			_Timer = _FireRate;
			_Magazine.Reload();
		}

		private Boolean CanShootWeapon()
		{
			if (_Magazine.CanShoot()) return true;
			WeaponReload();
			return false;
		}
	}
}