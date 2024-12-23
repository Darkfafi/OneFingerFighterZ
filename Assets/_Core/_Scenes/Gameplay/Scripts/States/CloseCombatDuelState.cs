using UnityEngine;
using UnityEngine.InputSystem;

namespace OnePunchFighterZ.GameplayScene
{
	public class CloseCombatDuelState : GameplayStateBase
	{
		[SerializeField]
		private Transform _mainPlayerCloseCombatPosition = null;

		[SerializeField]
		private Transform _otherPlayerCloseCombatPosition = null;

		private InputAction _moveInputAction = null;

		protected override void OnInit()
		{
			InputSystem_Actions input = new InputSystem_Actions();
			_moveInputAction = input.Player.Move;

			_moveInputAction.performed += OnMovePerformed;
		}

		protected override void OnEnter()
		{
			Dependency.MainCharacter.transform.position = _mainPlayerCloseCombatPosition.transform.position;
			Dependency.OtherCharacter.transform.position = _otherPlayerCloseCombatPosition.transform.position;

			_moveInputAction.Enable();
		}

		protected override void OnExit(bool isSwitch)
		{
			_moveInputAction.Disable();
		}

		protected override void OnDeinit()
		{
			_moveInputAction.performed -= OnMovePerformed;
		}

		private void OnMovePerformed(InputAction.CallbackContext context)
		{
			Vector2 input = context.ReadValue<Vector2>();
			switch(input.normalized)
			{
				case Vector2 v when v == Vector2.left:
					Dependency.MainCharacter.DuelAnimations.Attack1Animation.Play();
					break;
				case Vector2 v when v == Vector2.right:
					Dependency.MainCharacter.DuelAnimations.Attack2Animation.Play();
					break;
				case Vector2 v when v == Vector2.up:
					Dependency.MainCharacter.DuelAnimations.Attack3Animation.Play();
					break;
				case Vector2 v when v == Vector2.down:
					Dependency.MainCharacter.DuelAnimations.Attack4Animation.Play();
					break;
			}
		}
	}
}