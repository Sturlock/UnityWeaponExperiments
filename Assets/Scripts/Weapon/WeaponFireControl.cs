using System;
using System.Collections;
using System.Collections.Generic;
using Owl.Character.Player;
using Owl.Raycast;
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

		[SerializeField] private GameObject _MuzzlePoint;
		[SerializeField] private LayerMask _BulletMask;
		[SerializeField] private Boolean _FullAuto;

		[Header("Weapon Graphics")] [SerializeField]
		private GameObject _MuzzleFlash;

		[SerializeField] private GameObject _ImpactGraphic;
		[SerializeField] private GameObject _BulletPrefab;

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

		private void ShootWeapon()
		{
			Vector3 barrelPoint = _MuzzlePoint.transform.position;

			WeaponFireAction?.Invoke();

			for (Int32 index = 0; index < _ShotsPerRound; index++)
			{
				//Spread
				Single x = _Spread.Range();
				Single y = _Spread.Range();
				Vector3 direction = _MuzzlePoint.transform.forward;

				(Vector3[] shotPath, RaycastHit? raycastHit) = CurvedRaycast.CalculateParabolicPath(barrelPoint, direction, 975, new Vector3(x, y, x), 10000, _BulletMask);

				for (Int32 i = 0; i < shotPath.Length - 1; i++)
				{
					Debug.DrawLine(shotPath[i], shotPath[i + 1], Color.magenta, 5f);
				}

				Single travelTime = CalculateTravelTime(shotPath, 975);

				if (raycastHit is not null)
				{
					RaycastHit hit = (RaycastHit) raycastHit;

					StartCoroutine(LerpAlongPath(travelTime, shotPath, hit));
				}
				else
				{
					StartCoroutine(LerpAlongPath(travelTime, shotPath));
				}
			}

			if (_MuzzleFlash)
			{
				MuzzleFlash(barrelPoint);
			}

			_Timer = _FireRate;
			_ShotsFired++;
			_Magazine.AmmunitionCount--;
		}

		private static Single CalculateTravelTime(Vector3[] path, Single speed)
		{
			Single totalDistance = 0f;

			for (Int32 i = 0; i < path.Length - 1; i++)
			{
				totalDistance += Vector3.Distance(path[i], path[i + 1]);
			}

			return totalDistance / speed;
		}

		private IEnumerator LerpAlongPath(Single travelTime, Vector3[] path, RaycastHit hit = default)
		{
			if (!_BulletPrefab) yield break;

			GameObject bullet = Instantiate(_BulletPrefab, _MuzzlePoint.transform.position, Quaternion.identity);
			Single segmentDuration = travelTime / (path.Length - 1);

			for (Int32 i = 0; i < path.Length - 1; i++)
			{
				Vector3 startPoint = path[i];
				Vector3 endPoint = path[i + 1];
				Single timeElapsed = 0f;

				Boolean targetHit = false;
				while (timeElapsed < segmentDuration)
				{
					bullet.transform.position = Vector3.Lerp(startPoint, endPoint, timeElapsed / segmentDuration);
					if (Vector3.Distance(bullet.transform.position, hit.point) <= 3)
					{
						targetHit = true;
						bullet.transform.position = hit.point;

						DamageTarget(hit);

						if (!_ImpactGraphic) break;

						CreateImpactPoint(hit);
						break;
					}
					timeElapsed += Time.deltaTime;
					yield return null;
				}

				if (targetHit) break;

				bullet.transform.position = endPoint;
			}

			Destroy(bullet);
		}

		private void DamageTarget(RaycastHit hit)
		{
			IDamageable damageable = hit.transform.gameObject.GetComponent<IDamageable>();
			damageable?.DamageTarget(_Damage);
		}

		private void CreateImpactPoint(RaycastHit hit)
		{
			Quaternion rotation = Quaternion.LookRotation(hit.normal);
			Instantiate(_ImpactGraphic, hit.point, rotation);
		}

		private void MuzzleFlash(Vector3 barrelPoint)
		{
			Quaternion rotation = Quaternion.LookRotation(_MuzzlePoint.transform.forward);
			Instantiate(_MuzzleFlash, barrelPoint, rotation, _MuzzlePoint.transform);
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