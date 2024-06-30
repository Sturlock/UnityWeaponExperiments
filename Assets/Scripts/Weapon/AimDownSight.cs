using System;
using Owl.Character.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Owl.Weapon
{
	/// <summary>
	///    A <see cref="MonoBehaviour" /> that moves a weapon from its hip-fire position to its aim down sights position.
	/// </summary>
	public class AimDownSight : MonoBehaviour
	{
		[SerializeField] private Vector3 _BasePosition = new Vector3(0f, -0.3585f, 0.3f);
		[SerializeField] private Vector3 _AdsPosition = new Vector3(0.46f, -0.552f, 0.565f);
		[SerializeField] private Single _AdsSpeed = 10f;

		private Transform _AdsTransform;
		private Boolean _Aiming;
		private Boolean _Initialized;

		private void Awake()
		{
			_AdsTransform = gameObject.transform;
		}

		private void FixedUpdate()
		{
			if (!_Initialized) return;

			_AdsTransform.localPosition = Vector3.Slerp(_AdsTransform.localPosition, WeaponPosition(), _AdsSpeed * Time.fixedDeltaTime);
		}

		private void OnEnable()
		{
			PlayerInputHandler.WeaponAimAction += DoAim;
		}

		private void OnDisable()
		{
			PlayerInputHandler.WeaponAimAction -= DoAim;
		}

		private Vector3 WeaponPosition()
		{
			return !_Aiming ? _AdsPosition : _BasePosition;
		}

		private void DoAim(InputAction.CallbackContext callbackContext)
		{
			_Aiming = callbackContext.phase is InputActionPhase.Performed;
		}

		/// <summary>
		///    Initializes <see cref="AimDownSight" />.
		/// </summary>
		/// <param name="hipFirePosition">
		///    The weapons position when fired from the "hip".
		/// </param>
		/// <param name="adsPosition">
		///    The weapons position when aimed down sights.
		/// </param>
		/// <param name="adsSpeed">
		/// The speed we should move between the <see cref="hipFirePosition"/> and <see cref="adsPosition"/>.
		/// </param>
		public void Initialize(Vector3 hipFirePosition, Vector3 adsPosition, Single adsSpeed)
		{
			_BasePosition = hipFirePosition;
			_AdsPosition = adsPosition;
			_AdsSpeed = adsSpeed;

			_Initialized = true;
		}

		/// <summary>
		///    Initializes <see cref="AimDownSight" /> for when the weapon
		///    should use the default hip-fire and ADS positions
		/// </summary>
		public void Initialize()
		{
			_Initialized = true;
		}
	}
}