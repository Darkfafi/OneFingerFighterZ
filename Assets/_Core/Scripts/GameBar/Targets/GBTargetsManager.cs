using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GBTargetsManager : MonoBehaviour
{
	public delegate void TargetHandler(GBTarget target);
	public event TargetHandler TargetCreatedEvent;
	public event TargetHandler TargetRemovedEvent;
	public event GBTarget.EvaluationHandler TargetEvaluatedEvent;

	[SerializeField]
	private GBTarget _prefab = null;

	[SerializeField]
	private Transform _container = null;

	[SerializeField]
	private GBEntity _centerEntity = null;

	private GBTargetsQueue _leftQueue = null;
	private GBTargetsQueue _rightQueue = null;

	private HashSet<GBTarget> _instances = new HashSet<GBTarget>();

	public GBEntity CenterEntity => _centerEntity;

	protected void Awake()
	{
		_leftQueue = new GBTargetsQueue(_centerEntity, GBDirection.Right);
		_rightQueue = new GBTargetsQueue(_centerEntity, GBDirection.Left);
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

	public GBTarget Create(GBDirection direction, int index = 0, float position = 0, bool isRelativePosition = true)
	{
		GBTarget target = Create(direction, position, isRelativePosition, out GBTargetsQueue queue);
		queue.Insert(target, index);
		queue.Evaluate(0f);
		TargetCreatedEvent?.Invoke(target);
		return target;
	}

	public GBTarget CreateFront(GBDirection direction, float position = 0, bool isRelativePosition = true)
	{
		GBTarget target = Create(direction, position, isRelativePosition, out GBTargetsQueue queue);
		
		queue.AddToFront(target);
		queue.Evaluate(0f);

		TargetCreatedEvent?.Invoke(target);
		return target;
	}

	public GBTarget CreateBack(GBDirection direction, float position = 0, bool isRelativePosition = true)
	{
		GBTarget target = Create(direction, position, isRelativePosition, out GBTargetsQueue queue);

		queue.AddToBack(target);
		queue.Evaluate(0f);

		TargetCreatedEvent?.Invoke(target);
		return target;
	}

	private GBTarget Create(GBDirection direction, float position, bool isRelativePosition, out GBTargetsQueue targetsQueue)
	{
		GBTarget target = Instantiate(_prefab, _container);

		target.transform.localPosition = Vector3.zero;
		target.EvaluatedEvent += OnTargetEvaluatedEvent;
		target.DestroyedEvent += OnTargetDestroyedEvent;

		_instances.Add(target);
		targetsQueue = GetTargetsQueue(direction);

		if(isRelativePosition)
		{
			position = _centerEntity.GetSidePosition(target, direction, position);
		}

		target.SetPosition(position, GBTarget.SpawnPositionMetadata);
		return target;
	}

	public void Evaluate(float deltaTime)
	{
		_leftQueue.Evaluate(deltaTime);
		_rightQueue.Evaluate(deltaTime);
	}

	public void Remove(GBTarget target, bool destroy = true)
	{
		if(target == null)
		{
			return;
		}

		if(_instances.Remove(target))
		{
			target.EvaluatedEvent -= OnTargetEvaluatedEvent;
			target.DestroyedEvent -= OnTargetDestroyedEvent;

			_leftQueue?.Remove(target);
			_rightQueue?.Remove(target);

			if(destroy)
			{
				Destroy(target.gameObject);
			}

			TargetRemovedEvent?.Invoke(target);
		}
	}

	public void Clear()
	{
		var instances = _instances.ToArray();
		for(int i = instances.Length - 1; i >= 0; i--)
		{
			Remove(instances[i]);
		}
	}

	public GBTargetsQueue GetTargetsQueue(GBDirection direction)
	{
		switch(direction)
		{
			default:
			case GBDirection.Right:
				return _rightQueue;
			case GBDirection.Left:
				return _leftQueue;
		}
	}

	private void OnTargetEvaluatedEvent(GBTarget target, float deltaTime, GBEntity focusEntity, bool reachedFocusEntity)
	{
		TargetEvaluatedEvent?.Invoke(target, deltaTime, focusEntity, reachedFocusEntity);
	}

	private void OnTargetDestroyedEvent(GBEntity entity)
	{
		Remove((GBTarget)entity, destroy: false);
	}
}