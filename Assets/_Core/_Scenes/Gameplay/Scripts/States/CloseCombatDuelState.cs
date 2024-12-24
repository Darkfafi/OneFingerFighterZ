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

		[SerializeField]
		private DuelMinigame _duelMinigame = null;

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

			_duelMinigame.SetData(new DuelMinigame.CoreData()
			{
				Count = 20,
			});

			Dependency.OtherCharacter.DuelResources.MinigameView.SetData(_duelMinigame);
		}

		protected override void OnExit(bool isSwitch)
		{
			_duelMinigame.ClearData();
			Dependency.OtherCharacter.DuelResources.MinigameView.ClearData();
			Dependency.OtherCharacter.ResetState();

			_moveInputAction.Disable();
		}

		protected override void OnDeinit()
		{
			_moveInputAction.performed -= OnMovePerformed;
		}

		private void OnMovePerformed(InputAction.CallbackContext context)
		{
			bool isCorrect = false;
			Vector2 input = context.ReadValue<Vector2>();
			switch(input.normalized)
			{
				case Vector2 v when v == Vector2.left:
					_duelMinigame.Submit(InputType.Left);
					Dependency.MainCharacter.DuelResources.Attack1Animation.Play();
					break;
				case Vector2 v when v == Vector2.right:
					_duelMinigame.Submit(InputType.Right);
					Dependency.MainCharacter.DuelResources.Attack2Animation.Play();
					break;
				case Vector2 v when v == Vector2.up:
					_duelMinigame.Submit(InputType.Up);
					Dependency.MainCharacter.DuelResources.Attack3Animation.Play();
					break;
				case Vector2 v when v == Vector2.down:
					_duelMinigame.Submit(InputType.Down);
					Dependency.MainCharacter.DuelResources.Attack4Animation.Play();
					break;
			}

			Debug.Log("IsCorrect: " + isCorrect);
		}
	}
}