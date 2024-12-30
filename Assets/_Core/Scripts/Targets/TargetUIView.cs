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
		OnPositionChangedEvent(_target, _target.Position, 0);
	}

	protected void OnDestroy()
	{
		if(_target != null)
		{
			_target.PositionChangedEvent -= OnPositionChangedEvent;
		}
	}

	private void OnPositionChangedEvent(Entity entity, float newPosition, float oldPosition)
	{
		((RectTransform)transform).anchoredPosition = new Vector2(newPosition, 0);
	}
}