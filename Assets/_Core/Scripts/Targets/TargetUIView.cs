using RaTweening;
using UnityEngine;

public class TargetUIView : MonoBehaviour
{
	[SerializeField]
	private Target _target = null;

	protected void Awake()
	{
		_target.PositionChangedEvent += OnPositionChangedEvent;
	}

	protected void Start()
	{
		OnPositionChangedEvent(_target, _target.Position, 0, Target.SpawnPositionMetadata);
	}

	protected void OnDestroy()
	{
		if(_target != null)
		{
			_target.PositionChangedEvent -= OnPositionChangedEvent;
		}
	}

	private void OnPositionChangedEvent(Entity entity, float newPosition, float oldPosition, object metadata)
	{
		RaTween.StopGroup(this);

		if(metadata == Target.SpawnPositionMetadata)
		{
			((RectTransform)transform).anchoredPosition = new Vector2(newPosition, 0);
		}
		else
		{
			((RectTransform)transform).TweenAnchorPosX(newPosition, 0.1f).SetGroup(this);
		}
	}
}