using System;
using UnityEngine;

namespace Owl
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private CharacterController _CharacterController;
		[SerializeField] private Single _PlayerSpeedMultiplier = 12f;
		[SerializeField] private Single _PlayerJumpHeight = 3f;

		[Header("Ground Checking")] [SerializeField]
		private Single _Gravity = -9.81f;

		[SerializeField] private Transform _GroundCheck;
		[SerializeField] private Single _GroundDistance = 0.4f;
		[SerializeField] private LayerMask _GroundMask;
		private Boolean _IsGrounded;

		private Vector3 _Velocity;

		// Start is called before the first frame update
		private void Start()
		{
			_CharacterController = GetComponent<CharacterController>();
			PlayerInputHandler.PlayerJumpAction += OnJump;
		}

		// Update is called once per frame
		private void Update()
		{
			_IsGrounded = Physics.CheckSphere(_GroundCheck.position, _GroundDistance, _GroundMask);

			if (_IsGrounded && _Velocity.y < 0)
			{
				const Single DOWN_FORCE = -2;
				_Velocity.y = DOWN_FORCE;
			}

			Vector2 input = PlayerInputHandler.PlayerMovement;
			Vector3 movement = transform.right * input.x + transform.forward * input.y;


			_CharacterController.Move(movement * (_PlayerSpeedMultiplier * Time.deltaTime));

			_Velocity.y += _Gravity * Time.deltaTime;

			_CharacterController.Move(_Velocity * Time.deltaTime);
		}

		private void OnJump()
		{
			if (_IsGrounded)
			{
				_Velocity.y = Mathf.Sqrt(_PlayerJumpHeight * -2 * _Gravity);
			}
		}
	}
}