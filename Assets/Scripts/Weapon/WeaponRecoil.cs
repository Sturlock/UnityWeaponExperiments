using System;
using Owl.Character.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Owl.Weapon
{
	public class WeaponRecoil : MonoBehaviour
	{
		[Header("Reference Point"), SerializeField]
		private Transform _RecoilTransform;

		[Space, Header("Speed Settings"), SerializeField]
		private Single _MovementRecoilSpeed = 8f;

		[SerializeField] private Single _RotationRecoilSpeed = 8f;
		[SerializeField] private Single _MovementReturnSpeed = 18f;
		[SerializeField] private Single _RotationReturnSpeed = 38f;

		[Space, Header("Speed Settings"), SerializeField]
		private Vector3 _RecoilKickBack = new Vector3(0.015f, 0, -0.2f);

		[SerializeField] private Vector3 _RecoilKickBackAiming = new Vector3(0.015f, 0, -0.2f);
		[SerializeField] private Vector3 _RecoilRotation = new Vector3(10, 5, 7);
		[SerializeField] private Vector3 _RecoilRotationAiming = new Vector3(10, 4, 6);

		[Space, Header("State"), SerializeField]
		private Boolean _Aiming;

		private Vector3 _MovementRecoil;
		private Vector3 _Rotation;
		private Vector3 _RotationRecoil;
		private WeaponFireControl _WeaponFireControl;

		private void Awake()
		{
			_WeaponFireControl = GetComponentInChildren<WeaponFireControl>();
		}

		private void FixedUpdate()
		{
			_RotationRecoil = Vector3.Lerp(_RotationRecoil, Vector3.zero, _RotationReturnSpeed * Time.deltaTime);
			_MovementRecoil = Vector3.Lerp(_MovementRecoil, Vector3.zero, _MovementReturnSpeed * Time.deltaTime);

			_RecoilTransform.localPosition = Vector3.Slerp(_RecoilTransform.localPosition, _MovementRecoil, _MovementRecoilSpeed * Time.fixedDeltaTime);
			_Rotation = Vector3.Slerp(_Rotation, _RotationRecoil, _RotationRecoilSpeed * Time.fixedDeltaTime);
			_RecoilTransform.localRotation = Quaternion.Euler(_Rotation);
		}

		private void OnEnable()
		{
			_WeaponFireControl.WeaponFireAction += DoFire;
			PlayerInputHandler.WeaponAimAction += DoAim;
		}

		private void OnDisable()
		{
			_WeaponFireControl.WeaponFireAction -= DoFire;
		}

		private void DoAim(InputAction.CallbackContext callbackContext)
		{
			_Aiming = callbackContext.phase is InputActionPhase.Performed;
		}

		private void DoFire()
		{
			if (_Aiming)
			{
				_RotationRecoil += new Vector3(-_RecoilRotationAiming.x, _RecoilRotationAiming.y.Range(), _RecoilRotationAiming.z.Range());
				_MovementRecoil += new Vector3(-_RecoilKickBackAiming.x.Range(), _RecoilKickBackAiming.y.Range(), _RecoilKickBackAiming.z);
			}
			else
			{
				_RotationRecoil += new Vector3(-_RecoilRotation.x, _RecoilRotation.y.Range(), _RecoilRotation.z.Range());
				_MovementRecoil += new Vector3(-_RecoilKickBack.x.Range(), _RecoilKickBack.y.Range(), _RecoilKickBack.z);
			}
		}
	}
}