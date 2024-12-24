using RaDataHolder;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OnePunchFighterZ.GameplayScene
{
	public class DuelMinigame : RaMonoDataHolderBase<DuelMinigame.CoreData>
	{
		public event Action PositionChangedEvent;
		public event Action<Intent> IntentResolvedEvent;

		[SerializeField]
		private float _startPosition = 200;

		[SerializeField]
		private float _executionPoint = 30;

		[SerializeField]
		private float _speed = 1f;

		[SerializeField]
		private float _offset = 10f;

		private Queue<Intent> _intents = new Queue<Intent>();
		private float _position = 0;

		public IReadOnlyCollection<Intent> Intents => _intents;

		public float ExecutionPosition => _executionPoint;
		public float Position => _position;
		public float Offset => _offset;

		public bool IsPlaying
		{
			get; private set;
		}

		protected override void OnSetData()
		{
			ResetMinigame();

			List<InputType> options = new List<InputType>();
			options.Add(InputType.Right);
			options.Add(InputType.Left);
			options.Add(InputType.Up);
			options.Add(InputType.Down);

			for(int i = 0; i < Data.Count; i++)
			{
				Intent intent = new Intent(options[UnityEngine.Random.Range(0, options.Count)], i * Offset);
				_intents.Enqueue(intent);
				Debug.Log(intent.InputType);
			}

			IsPlaying = true;
		}

		protected override void OnClearData()
		{
			ResetMinigame();
			IsPlaying = false;
		}

		protected void Update()
		{
			if(!IsPlaying)
			{
				return;
			}
		}

		public bool Submit(InputType inputType, out bool isCorrect)
		{
			if(!IsPlaying)
			{
				isCorrect = default;
				return false;
			}

			if(_intents.TryPeek(out Intent nextIntent))
			{
				if(nextIntent.InputType == inputType)
				{
					ResolveIntent();
					isCorrect = true;
				}
				else
				{
					ResolveIntent();
					isCorrect = false;
				}

				return true;
			}

			isCorrect = default;
			return false;
		}

		private void ResolveIntent()
		{
			if(_intents.TryDequeue(out var intent))
			{
				IntentResolvedEvent?.Invoke(intent);
			}

			if(_intents.TryPeek(out _))
			{
				SetPosition(_executionPoint + Offset);
			}
			else
			{
				SetPosition(_executionPoint);
				IsPlaying = false;
			}
		}

		private void ResetMinigame()
		{
			_position = _startPosition;
			_intents.Clear();
		}

		private void SetPosition(float position)
		{
			_position = position;
			PositionChangedEvent?.Invoke();
		}

		public class Intent
		{
			public InputType InputType
			{
				get; private set;
			}

			public float LocalPosition
			{
				get; private set;
			}

			public Intent(InputType inputType, float localPosition)
			{
				InputType = inputType;
				LocalPosition = localPosition;
			}
		}

		[System.Serializable]
		public struct CoreData
		{
			public int Count;
		}
	}
}