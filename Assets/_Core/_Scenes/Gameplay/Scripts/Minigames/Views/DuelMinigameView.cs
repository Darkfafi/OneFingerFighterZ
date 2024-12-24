using RaDataHolder;
using RaTweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OnePunchFighterZ.GameplayScene
{
	public class DuelMinigameView : RaMonoDataHolderBase<DuelMinigame>
	{
		[SerializeField]
		private Image _intentPrototype = null;

		[SerializeField]
		private RectTransform _executionLine = null;

		private Dictionary<DuelMinigame.Intent, Image> _instanceMap = new Dictionary<DuelMinigame.Intent, Image>();

		private VerticalLayoutGroup _verticalLayoutGroup;

		protected override void OnInitialization()
		{
			base.OnInitialization();
			gameObject.SetActive(false);
			_intentPrototype.gameObject.SetActive(false);
		}

		protected override void OnSetData()
		{
			gameObject.SetActive(true);

			_verticalLayoutGroup = _intentPrototype.transform.parent.GetComponent<VerticalLayoutGroup>();
			_verticalLayoutGroup.spacing = Data.Offset - _intentPrototype.GetComponent<LayoutElement>().minHeight;

			foreach(var intent in Data.Intents)
			{
				var instance = Instantiate(_intentPrototype, _intentPrototype.transform.parent);
				instance.gameObject.SetActive(true);
				intent.InputType.Apply(instance);
				instance.rectTransform.anchoredPosition = new Vector2(0, intent.LocalPosition);
				_instanceMap[intent] = instance;
			}

			_executionLine.anchoredPosition = new Vector2(0, Data.ExecutionPosition);

			Data.PositionChangedEvent += OnPositionChanged;
			Data.IntentResolvedEvent += OnIntentResolvedEvent;
			Data.StoppedPlayingEvent += OnStoppedPlayingEvent;
			OnPositionChanged();
		}

		protected override void OnClearData()
		{
			Data.StoppedPlayingEvent -= OnStoppedPlayingEvent;
			Data.PositionChangedEvent -= OnPositionChanged;
			Data.IntentResolvedEvent -= OnIntentResolvedEvent;

			foreach(var instancePair in _instanceMap)
			{
				if(instancePair.Value != null)
				{
					Destroy(instancePair.Value.gameObject);
				}
			}

			_instanceMap.Clear();

			gameObject.SetActive(false);
		}

		private void OnPositionChanged()
		{
			if(Mathf.Abs(Data.Position - _verticalLayoutGroup.padding.bottom) > Data.Offset)
			{
				RaTweenLambda.TweenFloat
				(
					_verticalLayoutGroup.padding.bottom, Data.Position, 0.1f,
					(x, n) => _verticalLayoutGroup.padding = new RectOffset(left: 0, right: 0, top: 0, bottom: (int)x),
					() => this && _verticalLayoutGroup != null
				).SetGroup(_verticalLayoutGroup);
			}
			else
			{
				RaTween.StopGroup(_verticalLayoutGroup);
				_verticalLayoutGroup.padding = new RectOffset(left: 0, right: 0, top: 0, bottom: (int)Data.Position);
			}
		}

		private void OnIntentResolvedEvent(DuelMinigame.Intent intent, bool success)
		{
			if(_instanceMap.TryGetValue(intent, out var instance))
			{
				instance.GetComponent<LayoutElement>().ignoreLayout = true;

				Color color = success ? Color.green : Color.red;
				color.a = 0.45f;
				instance.TweenColor(color, 0.25f);
			}
		}

		private void OnStoppedPlayingEvent()
		{
			RaTweenLambda.TweenFloat
			(
				_verticalLayoutGroup.padding.bottom, 0, _verticalLayoutGroup.padding.bottom / Data.Speed,
				(x, n) => _verticalLayoutGroup.padding = new RectOffset(left: 0, right: 0, top: 0, bottom: (int)x),
				() => this && _verticalLayoutGroup != null
			).SetGroup(_verticalLayoutGroup);
		}
	}
}