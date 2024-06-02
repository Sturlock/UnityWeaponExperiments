using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Owl
{
	public class GunSystem : MonoBehaviour
	{
		//Gun stats
		public Int32 damage;
		public Single timeBetweenShooting;
		public Single spread;
		public Single range;
		public Single reloadTime;
		public Single timeBetweenShots;
		public Int32 magazineSize;
		public Int32 bulletsPerTap;
		public Boolean allowButtonHold;

		//Reference
		public Camera fpsCam;
		public Transform attackPoint;
		public LayerMask whatIsEnemy;

		//Graphics
		public GameObject muzzleFlash;
		public GameObject bulletHoleGraphic;
		public Single camShakeMagnitude;
		public Single camShakeDuration;
		public TextMeshProUGUI text;
		private Int32 _BulletsLeft;
		private Int32 _BulletsShot;

		//Booleans
		private Boolean _Shooting;
		private Boolean _ReadyToShoot;
		private Boolean _Reloading;
		public CamShake camShake;
		public RaycastHit RayHit;

		private void Awake()
		{
			_BulletsLeft = magazineSize;
			_ReadyToShoot = true;
		}

		private void Update()
		{
			MyInput();

			//SetText
			text.SetText(_BulletsLeft + " / " + magazineSize);
		}

		private void MyInput()
		{
			_Shooting = allowButtonHold ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);

			if (Input.GetKeyDown(KeyCode.R) && _BulletsLeft < magazineSize && !_Reloading) Reload();

			//Shoot
			if (_ReadyToShoot && _Shooting && !_Reloading && _BulletsLeft > 0)
			{
				_BulletsShot = bulletsPerTap;
				Shoot();
			}
		}

		private void Shoot()
		{
			_ReadyToShoot = false;

			//Spread
			Single x = Random.Range(-spread, spread);
			Single y = Random.Range(-spread, spread);

			//Calculate Direction with Spread
			Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

			//RayCast
			if (Physics.Raycast(fpsCam.transform.position, direction, out RayHit, range, whatIsEnemy))
			{
				Debug.Log(RayHit.collider.name);

				if (RayHit.collider.CompareTag("Enemy"))
				{
					//RayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
				}
			}

			//ShakeCamera
			camShake.Shake(camShakeDuration, camShakeMagnitude);

			//Graphics
			Instantiate(bulletHoleGraphic, RayHit.point, Quaternion.Euler(0, 180, 0));
			Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

			_BulletsLeft--;
			_BulletsShot--;

			Invoke("ResetShot", timeBetweenShooting);

			if (_BulletsShot > 0 && _BulletsLeft > 0)
				Invoke("Shoot", timeBetweenShots);
		}

		private void ResetShot()
		{
			_ReadyToShoot = true;
		}

		private void Reload()
		{
			_Reloading = true;
			Invoke("ReloadFinished", reloadTime);
		}

		private void ReloadFinished()
		{
			_BulletsLeft = magazineSize;
			_Reloading = false;
		}
	}
}