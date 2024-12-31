using RaTweening;
using UnityEngine;
using UnityEngine.UI;

public class GBTargetUIView : MonoBehaviour
{
	[SerializeField]
	private GBTarget _target = null;

	private Image _image = null;

	protected void Awake()
	{
		_image = GetComponent<Image>();

		_target.PositionChangedEvent += OnPositionChangedEvent;
		_target.EvaluatedEvent += OnEvaluatedEvent;
	}

	protected void Start()
	{
		OnPositionChangedEvent(_target, _target.Position, 0, GBTarget.SpawnPositionMetadata);
	}

	protected void OnDestroy()
	{
		if(_target != null)
		{
			_target.EvaluatedEvent -= OnEvaluatedEvent;
			_target.PositionChangedEvent -= OnPositionChangedEvent;
		}
	}

	private void OnPositionChangedEvent(GBEntity entity, float newPosition, float oldPosition, object metadata)
	{
		RaTween.StopGroup(this);

		if(metadata == GBTarget.SpawnPositionMetadata)
		{
			((RectTransform)transform).anchoredPosition = new Vector2(newPosition, 0);
		}
		else
		{
			((RectTransform)transform).TweenAnchorPosX(newPosition, 0.1f).SetGroup(this);
		}
	}

	private void OnEvaluatedEvent(GBTarget target, float deltaTime, GBEntity focusEntity, bool reachedFocusEntity)
	{
		_image.color = reachedFocusEntity ? Color.green : Color.blue;
	}
}