using System;
using System.Collections.Generic;

public class TargetsQueue : IDisposable
{
	private Direction _sortingDirection = Direction.Left;
	private Entity _centerEntity = null;
	private List<Target> _targets = new List<Target>();

	public IReadOnlyList<Target> Targets => _targets;

	public TargetsQueue(Entity centerEntity, Direction sortingDirection)
	{
		_centerEntity = centerEntity;
		_sortingDirection = sortingDirection;
	}

	public bool TryGetFront(out Target target)
	{
		if(_targets.Count > 0)
		{
			target = _targets[0];
			return true;
		}

		target = default;
		return false;
	}

	public bool TryGetBack(out Target target)
	{
		if(_targets.Count > 0)
		{
			target = _targets[_targets.Count - 1];
			return true;
		}

		target = default;
		return false;
	}

	public void AddToFront(Target target)
	{
		_targets.Insert(0, target);
	}

	public void AddToBack(Target target)
	{
		_targets.Add(target);
	}

	public void Insert(Target target, int index)
	{
		_targets.Insert(index, target);
	}

	public void Remove(Target target)
	{
		_targets.Remove(target);
	}

	public void Evaluate()
	{
		Entity focus = _centerEntity;
		for(int i = 0; i < _targets.Count; i++)
		{
			var target = _targets[i];
			target.Evaluate(focus, _sortingDirection);
			focus = target;
		}
	}

	public void Dispose()
	{
		_sortingDirection = Direction.None;
	}
}
