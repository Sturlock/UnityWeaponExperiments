using System;
using Owl.Character.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Owl.Weapon
{
	/// <summary>
	///    A class for handing how a weapon fires
	/// </summary>
	public class WeaponFireControl : MonoBehaviour
	{
		[SerializeField] private Single _Rpm = 120;
		[SerializeField, Range(1, 100)] private Int32 _ShotsPerRound;
		[SerializeField] private Single _Damage;
		[SerializeField] private Single _Spread;

		[SerializeField] private GameObject _Barrel;
		[SerializeField] private LayerMask _LayerMask;
		[SerializeField] private Boolean _FullAuto;

		[Header("Weapon Graphics")] [SerializeField]
		private GameObject _MuzzleFlash;

		[SerializeField] private GameObject _ImpactGraphic;

		private InputAction.CallbackContext _FireCallback;
		private Single _FireRate;
		private Magazine _Magazine;
		private Int32 _ShotsFired;
		private Single _Timer;

		/// <summary>
		///    A <see cref="Action" /> that is invoked when the weapon is successfully fired.
		/// </summary>
		public Action WeaponFireAction;

		private void Start()
		{
			//This is safe to do as there should only be one Magazine per weapon.
			_Magazine = GetComponentInChildren<Magazine>();

			_FireRate = RpmToIntervalTime();
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

				ShootWeapon();
			}
			else
			{
				_Timer -= Time.deltaTime;
			}
		}

		private void ShootWeapon()
		{
			Vector3 barrelPoint = _Barrel.transform.position;

			WeaponFireAction?.Invoke();

			for (Int32 index = 0; index < _ShotsPerRound; index++)
			{
				//Spread
				Single x = _Spread.Range();
				Single y = _Spread.Range();

				Vector3 direction = _Barrel.transform.forward + new Vector3(x, y, x);

				Ray shot = new(barrelPoint, direction);

				if (!Physics.Raycast(shot, out RaycastHit hit, Mathf.Infinity, _LayerMask)) continue;

				Debug.DrawRay(barrelPoint, shot.direction * 10, Color.magenta, 5f);
				IDamageable damageable = hit.transform.gameObject.GetComponent<IDamageable>();
				damageable?.DamageTarget(_Damage);

				if (!_ImpactGraphic) continue;

				CreateImpactPoint(hit);
			}
			if (_MuzzleFlash)
			{
				MuzzleFlash(barrelPoint);
			}
			_Timer = _FireRate;
			_ShotsFired++;
			_Magazine.AmmunitionCount--;
		}

		private void CreateImpactPoint(RaycastHit hit)
		{
			Quaternion rotation = Quaternion.LookRotation(hit.normal);
			Instantiate(_ImpactGraphic, hit.point, rotation);
		}

		private void MuzzleFlash(Vector3 barrelPoint)
		{
			Quaternion rotation = Quaternion.LookRotation(_Barrel.transform.forward);
			Instantiate(_MuzzleFlash, barrelPoint, rotation, _Barrel.transform);
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

		private Single RpmToIntervalTime()
		{
			const Single MINUTE_IN_SECONDS = 60;
			Single rps = _Rpm / MINUTE_IN_SECONDS;
			return 1 / rps;
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