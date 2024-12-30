using RaTweening;
using UnityEngine;

public class TargetsConainerUIView : MonoBehaviour
{
	[SerializeField]
	private TargetsManager _targetsManager = null;

	protected void Awake()
	{
		_targetsManager.CenterEntity.PositionChangedEvent += OnCenterPositionChangedEvent;
	}

	protected void OnDestroy()
	{
		if(_targetsManager != null)
		{
			if(_targetsManager.CenterEntity != null)
			{
				_targetsManager.CenterEntity.PositionChangedEvent -= OnCenterPositionChangedEvent;
			}
		}
	}

	private void OnCenterPositionChangedEvent(Entity entity, float newPosition, float oldPosition, object metadata)
	{
		RaTween.StopGroup(this);
		((RectTransform)transform).TweenAnchorPosX(-newPosition, 0.1f).SetGroup(this);
	}
}
