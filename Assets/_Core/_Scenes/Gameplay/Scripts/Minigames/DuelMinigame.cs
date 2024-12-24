using RaDataHolder;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OnePunchFighterZ.GameplayScene
{
	public class DuelMinigame : RaMonoDataHolderBase<DuelMinigame.CoreData>
	{
		public event Action StoppedPlayingEvent;
		public event Action PositionChangedEvent;
		public event Action<Intent, bool> IntentResolvedEvent;

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
		public float Speed => _speed;

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
			StoppedPlayingEvent?.Invoke();
		}

		protected void Update()
		{
			if(!IsPlaying)
			{
				return;
			}

			SetPosition(_position - Time.deltaTime * _speed);

			if(_position <= 0)
			{
				ResolveIntent(false);
			}
		}

		public bool Submit(InputType inputType)
		{
			if(!IsPlaying)
			{
				return false;
			}

			if(_intents.TryPeek(out Intent nextIntent))
			{
				if(nextIntent.InputType == inputType)
				{
					ResolveIntent(true);
				}
				else
				{
					ResolveIntent(false);
				}

				return true;
			}

			return false;
		}

		private void ResolveIntent(bool success)
		{
			if(_intents.TryDequeue(out var intent))
			{
				IntentResolvedEvent?.Invoke(intent, success);
			}

			float origin = Mathf.Min(_executionPoint, _position);

			if(_intents.TryPeek(out _))
			{
				SetPosition(origin + Offset);
			}
			else
			{
				SetPosition(origin);
				IsPlaying = false;
				StoppedPlayingEvent?.Invoke();
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