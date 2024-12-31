using RaCollection;
using System;

public class GBTargetsQueue : IDisposable
{
	private GBDirection _sortingDirection = GBDirection.Left;
	private GBEntity _centerEntity = null;
	private RaElementCollection<GBTarget> _targets = new RaElementCollection<GBTarget>();

	public IReadOnlyRaElementCollection<GBTarget> Targets => _targets;
	public GBDirection SortingDirection => _sortingDirection;

	public GBTargetsQueue(GBEntity centerEntity, GBDirection sortingDirection)
	{
		_centerEntity = centerEntity;
		_sortingDirection = sortingDirection;
	}

	public bool TryGetFront(out GBTarget target)
	{
		if(_targets.Count > 0)
		{
			target = _targets[0];
			return true;
		}

		target = default;
		return false;
	}

	public bool TryGetBack(out GBTarget target)
	{
		if(_targets.Count > 0)
		{
			target = _targets[_targets.Count - 1];
			return true;
		}

		target = default;
		return false;
	}

	public void AddToFront(GBTarget target)
	{
		_targets.Insert(0, target);
	}

	public void AddToBack(GBTarget target)
	{
		_targets.Add(target);
	}

	public void Insert(GBTarget target, int index)
	{
		_targets.Insert(index, target);
	}

	public void Remove(GBTarget target)
	{
		_targets.Remove(target.Id);
	}

	public void Evaluate(float deltaTime)
	{
		GBEntity focus = _centerEntity;
		for(int i = 0; i < _targets.Count; i++)
		{
			var target = _targets[i];
			target.Evaluate(deltaTime, focus, _sortingDirection);
			focus = target;
		}
	}

	public void Dispose()
	{
		_sortingDirection = GBDirection.None;
	}
}
