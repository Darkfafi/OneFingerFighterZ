using RaFSM;
using UnityEngine;

namespace OnePunchFighterZ.GameplayScene
{
	public class GameplaySceneRoot : SceneRootBase
	{
		[field: SerializeField]
		public Character MainCharacter
		{
			get; private set;
		}

		[field: SerializeField]
		public Character OtherCharacter
		{
			get; private set;
		}

		[field: SerializeField]
		public Transform MainCharacterSpawn
		{
			get; private set;
		}

		[field: SerializeField]
		public Transform OtherCharacterSpawn
		{
			get; private set;
		}

		private RaGOFiniteStateMachine _fsm = null;

		protected void Awake()
		{
			_fsm = new RaGOFiniteStateMachine(this, RaGOFiniteStateMachine.GetGOStates(transform));
		}

		protected void Start()
		{
			SetCharactersToSpawn();

			_fsm.SwitchState(0);
		}

		public void SetCharactersToSpawn()
		{
			MainCharacter.transform.position = MainCharacterSpawn.transform.position;
			OtherCharacter.transform.position = OtherCharacterSpawn.transform.position;
		}

		protected void OnDestroy()
		{
			if(_fsm != null)
			{
				_fsm.Dispose();
				_fsm = null;
			}
		}
	}
}