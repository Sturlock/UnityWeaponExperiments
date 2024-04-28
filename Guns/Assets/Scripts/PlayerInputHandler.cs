using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Owl
{
	/// <summary>
	///    A Singleton-based class that handles player inputs. Currently used for weapon control handling exclusively.
	/// </summary>
	public class PlayerInputHandler : MonoBehaviour
	{
		/// <summary>
		///    The current instance of <see cref="PlayerInputHandler" />.
		/// </summary>
		public static PlayerInputHandler Instance;

		/// <summary>
		///    The callback action for firing a weapon.
		/// </summary>
		public static Action<InputAction.CallbackContext> WeaponFireAction;

		/// <summary>
		///    The callback action for aiming a weapon.
		/// </summary>
		public static Action WeaponAimAction;

		/// <summary>
		///    The callback action for reloading a weapon.
		/// </summary>
		public static Action WeaponReloadAction;

		/// <summary>
		///    The current instance of <see cref="GameControls" />.
		/// </summary>
		public static GameControls Controls;

		/// <summary>
		///    The callback action for reloading the level.
		/// </summary>
		public static Action ReloadLevelAction;

		/// <summary>
		///    The callback action for the Tab key menu.
		/// </summary>
		public static Action TabMenuAction;

		/// <summary>
		///    The callback action for the displaying the FPSCounter.
		/// </summary>
		public static Action FPSCounterAction;

		public static Vector2 PlayerLook { get; private set; }

		public static Vector2 PlayerMovement { get; private set; }

		public static Action PlayerJumpAction { get; set; }

		private void Awake()
		{
			Controls = new GameControls();
		}

		private void Start()
		{
			if (Instance != null) Destroy(Instance);
			Instance = this;

			RegisterGameplayControls();

			RegisterUIControls();

			RegisterDebugControls();
		}

		private void Update()
		{
			PlayerMovement = Controls.Gameplay.Move.ReadValue<Vector2>();
			PlayerLook = Controls.Gameplay.Look.ReadValue<Vector2>();
		}

		private void OnEnable()
		{
			Controls.Enable();
		}

		private void OnDisable()
		{
			Controls.Disable();
		}

		private static void RegisterDebugControls()
		{
			Controls.Debug.FPSCounter.performed += _ => FPSCounterAction?.Invoke();
			Controls.Debug.ReloadLevel.performed += _ => ReloadLevelAction?.Invoke();
		}

		private static void RegisterUIControls()
		{
			Controls.UI.TabMenu.performed += _ => TabMenuAction?.Invoke();
		}

		private static void RegisterGameplayControls()
		{
			Controls.Gameplay.Fire.performed += context => WeaponFireAction?.Invoke(context);
			Controls.Gameplay.Fire.canceled += context => WeaponFireAction?.Invoke(context);
			Controls.Gameplay.Zoom.performed += _ => WeaponAimAction?.Invoke();
			Controls.Gameplay.Reload.performed += _ => WeaponReloadAction?.Invoke();

			Controls.Gameplay.Jump.performed += _ => PlayerJumpAction?.Invoke();
		}
	}
}