using UnityEngine;
using UnityEngine.InputSystem;
using RaTweening;

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

			_duelMinigame.IntentResolvedEvent += OnIntentResolvedEvent;
			_duelMinigame.SetData(new DuelMinigame.CoreData()
			{
				Count = 20,
			});

			Dependency.OtherCharacter.DuelResources.MinigameView.SetData(_duelMinigame);
		}

		protected override void OnExit(bool isSwitch)
		{
			_duelMinigame.IntentResolvedEvent -= OnIntentResolvedEvent;
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
			Vector2 input = context.ReadValue<Vector2>();
			switch(input.normalized)
			{
				case Vector2 v when v == Vector2.left:
					_duelMinigame.Submit(InputType.Left);
					break;
				case Vector2 v when v == Vector2.right:
					_duelMinigame.Submit(InputType.Right);
					break;
				case Vector2 v when v == Vector2.up:
					_duelMinigame.Submit(InputType.Up);
					break;
				case Vector2 v when v == Vector2.down:
					_duelMinigame.Submit(InputType.Down);
					break;
			}
		}

		private void OnIntentResolvedEvent(DuelMinigame.Intent intent, bool success)
		{
			switch(intent.InputType)
			{
				case InputType.Right:
					if(success)
					{
						Dependency.MainCharacter.DuelResources.Attack2Animation.Play();
					}
					else
					{
						Dependency.MainCharacter.CoreResources.HitAnimation.Play();
					}

					Dependency.OtherCharacter.DuelResources.Attack2Animation.Play();
					break;
				case InputType.Left:
					if(success)
					{
						Dependency.MainCharacter.DuelResources.Attack1Animation.Play();
					}
					else
					{
						Dependency.MainCharacter.CoreResources.HitAnimation.Play();
					}

					Dependency.OtherCharacter.DuelResources.Attack1Animation.Play();
					break;
				case InputType.Up:
					if(success)
					{
						Dependency.MainCharacter.DuelResources.Attack3Animation.Play();
					}
					else
					{
						Dependency.MainCharacter.CoreResources.HitAnimation.Play();
					}

					Dependency.OtherCharacter.DuelResources.Attack3Animation.Play();
					break;
				case InputType.Down:
					if(success)
					{
						Dependency.MainCharacter.DuelResources.Attack4Animation.Play();
					}
					else
					{
						Dependency.MainCharacter.CoreResources.HitAnimation.Play();
					}

					Dependency.OtherCharacter.DuelResources.Attack4Animation.Play();
					break;
			}

			Camera.main.transform.TweenShakePos(0.05f, 0.2f).OnComplete(() => Camera.main.transform.position = new Vector3(0f, 0, -10f));
		}
	}
}