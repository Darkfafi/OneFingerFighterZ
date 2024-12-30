using System.Collections.Generic;
using UnityEngine;

public class TargetsManager : MonoBehaviour
{
	public delegate void TargetHandler(Target target);
	public event TargetHandler TargetCreatedEvent;
	public event TargetHandler TargetRemovedEvent;

	[SerializeField]
	private Target _prefab = null;

	[SerializeField]
	private Transform _container = null;

	[SerializeField]
	private Entity _centerEntity = null;

	private TargetsQueue _leftQueue = null;
	private TargetsQueue _rightQueue = null;

	private HashSet<Target> _instances = new HashSet<Target>();

	public Entity CenterEntity => _centerEntity;

	protected void Awake()
	{
		_leftQueue = new TargetsQueue(_centerEntity, Direction.Right);
		_rightQueue = new TargetsQueue(_centerEntity, Direction.Left);
	}

	protected void OnDestroy()
	{
		if(_leftQueue != null)
		{
			_leftQueue.Dispose();
			_leftQueue = null;
		}

		if(_rightQueue != null)
		{
			_rightQueue.Dispose();
			_rightQueue = null;
		}
	}

	public Target Create(Direction direction, int index = 0, float position = 0, bool isRelativePosition = true)
	{
		Target target = Instantiate(_prefab, _container);
		target.transform.localPosition = Vector3.zero;

		_instances.Add(target);
		
		TargetsQueue queue = GetTargetsQueue(direction);
		queue.Insert(target, index);

		if(isRelativePosition)
		{
			position = _centerEntity.GetSidePosition(target, direction, position);
		}

		target.SetPosition(position, Target.SpawnPositionMetadata);

		queue.Evaluate();

		TargetCreatedEvent?.Invoke(target);
		return target;
	}

	public void Evaluate()
	{
		_leftQueue.Evaluate();
		_rightQueue.Evaluate();
	}

	public void Remove(Target target, bool destroy = true)
	{
		if(_instances.Remove(target))
		{
			_leftQueue.Remove(target);
			_rightQueue.Remove(target);

			if(destroy)
			{
				Destroy(target.gameObject);
			}

			TargetRemovedEvent?.Invoke(target);
		}
	}

	public TargetsQueue GetTargetsQueue(Direction direction)
	{
		switch(direction)
		{
			default:
			case Direction.Right:
				return _rightQueue;
			case Direction.Left:
				return _leftQueue;
		}
	}
}