using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Owl
{
	public class CameraRecoil : MonoBehaviour
	{
		[Header("Recoil Settings"), SerializeField]
		private Single _RotationSpeed = 6;

		[SerializeField] private Single _ReturnSpeed = 25;

		[Space, Header("Hip-fire"), SerializeField]
		private Vector3 _RecoilRotation = new Vector3(2, 2, 2);

		[Space, Header("Aim Down Sight"), SerializeField]
		private Vector3 _RecoilRotationAiming = new Vector3(0.5f, 0.5f, 0.5f);

		[Space, Header("State"), SerializeField]
		private Boolean _Aiming;

		private Vector3 _CurrentRotation;
		private Vector3 _Rotation;
		private WeaponFireControl _WeaponFireControl;

		private void Awake()
		{
			_WeaponFireControl = GetComponentInChildren<WeaponFireControl>();
		}

		private void FixedUpdate()
		{
			_CurrentRotation = Vector3.Lerp(_CurrentRotation, Vector3.zero, _ReturnSpeed * Time.deltaTime);
			_Rotation = Vector3.Slerp(_Rotation, _CurrentRotation, _RotationSpeed * Time.fixedDeltaTime);
			transform.localRotation = Quaternion.Euler(_Rotation);
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
				_CurrentRotation += new Vector3(-_RecoilRotationAiming.x, _RecoilRotationAiming.y.Range(), _RecoilRotationAiming.z.Range());
			}
			else
			{
				_CurrentRotation += new Vector3(-_RecoilRotation.x, _RecoilRotation.y.Range(), _RecoilRotation.z.Range());
			}
		}
	}
}