using RaFSM;

namespace OnePunchFighterZ.GameplayScene
{
	public class GameplayFightState : GameplayStateBase, IRaFSMCycler
	{
		/*
		 * Cycles through the Fight States Until a conclusion is drawn
		 * Chooses any state, then when the next state is requested, goes back to the base
		 */

		private RaGOFiniteStateMachine _fsm;

		protected override void OnInit()
		{
			_fsm = new RaGOFiniteStateMachine(this, RaGOFiniteStateMachine.GetGOStates(transform));
		}

		protected override void OnEnter()
		{
			_fsm.SwitchState(0);
		}

		protected override void OnExit(bool isSwitch)
		{
			_fsm.SwitchState(null);
		}

		protected override void OnDeinit()
		{
			if(_fsm != null)
			{
				_fsm.Dispose();
				_fsm = null;
			}
		}

		public void GoToNextState()
		{
			if(IsCurrentState)
			{
				if(_fsm != null)
				{
					// Check Conclusion Condition
					/*
					 * if(condition)
					 * {
					 *		FSM_GoToNextState();
					 * }
					 */
					// Else, Select Next Fight State by Random
					_fsm.SwitchState(UnityEngine.Random.Range(0, _fsm.States.Length - 1));
				}
			}
		}
	}
}