using RaCollection;
using UnityEngine;

public class GBEntity : MonoBehaviour, IRaCollectionElement
{
	public delegate void EntityHandler(GBEntity entity);
	public event EntityHandler DestroyedEvent;

	public delegate void PositionHandler(GBEntity entity, float newPosition, float oldPosition, object metadata);
	public event PositionHandler PositionChangedEvent;

	[field: SerializeField]
	public float Radius
	{
		get; private set;
	}

	[field: SerializeField]
	public float Position
	{
		get; private set;
	}

	public string Id => GetInstanceID().ToString();

	protected void OnDestroy()
	{
		DestroyedEvent?.Invoke(this);

		PositionChangedEvent = null;
		DestroyedEvent = null;
	}

	public float GetSidePosition(float observerPosition, GBDirection direction = GBDirection.None, float offset = 0f)
	{
		if(direction == GBDirection.None)
		{
			direction = GetDirection(observerPosition, Position);
		}

		if(direction == GBDirection.Right)
		{
			return Position - Radius - offset;
		}
		else
		{
			return Position + Radius + offset;
		}
	}

	public float GetSidePosition(GBEntity observerEntity, GBDirection direction, float offset = 0)
	{
		if(direction == GBDirection.None)
		{
			direction = GetDirection(observerEntity.Position, Position);
		}

		if(direction == GBDirection.Right)
		{
			return (Position - Radius) - observerEntity.Radius - offset;
		}
		else
		{
			return (Position + Radius) + observerEntity.Radius + offset;
		}
	}

	public void SetPosition(float position, object metadata = null)
	{
		float oldPisition = Position;
		Position = position;
		PositionChangedEvent?.Invoke(this, Position, oldPisition, metadata);
	}

	public static GBDirection GetDirection(float origin, float target)
	{
		if(origin < target)
		{
			return GBDirection.Right;
		}
		else if(origin > target)
		{
			return GBDirection.Left;
		}

		return GBDirection.None;
	}
}
