using UnityEngine;

public class Entity : MonoBehaviour
{
	public delegate void PositionHandler(Entity entity, float newPosition, float oldPosition);
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

	protected void OnDestroy()
	{
		PositionChangedEvent = null;
	}

	public float GetSidePosition(float observerPosition, Direction direction = Direction.None)
	{
		if(direction == Direction.None)
		{
			direction = GetDirection(observerPosition, Position);
		}

		if(direction == Direction.Right)
		{
			return Position - Radius;
		}
		else
		{
			return Position + Radius;
		}
	}

	public float GetSidePosition(Entity observerEntity, Direction direction, float offset = 0)
	{
		if(direction == Direction.None)
		{
			direction = GetDirection(observerEntity.Position, Position);
		}

		if(direction == Direction.Right)
		{
			return (Position - Radius) - observerEntity.Radius - offset;
		}
		else
		{
			return (Position + Radius) + observerEntity.Radius + offset;
		}
	}

	public void SetPosition(float position)
	{
		float oldPisition = Position;
		Position = position;
		PositionChangedEvent?.Invoke(this, Position, oldPisition);
	}

	public static Direction GetDirection(float origin, float target)
	{
		if(origin < target)
		{
			return Direction.Right;
		}
		else if(origin > target)
		{
			return Direction.Left;
		}

		return Direction.None;
	}
}
