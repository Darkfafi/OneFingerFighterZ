using UnityEngine;

public class Target : Entity
{
	[SerializeField]
	private float _speed = 50;

	public void Evaluate(Entity focusEntity, Direction direction = Direction.None)
	{
		if(focusEntity == null)
		{
			return;
		}

		if(direction == Direction.None)
		{
			GetDirection(Position, focusEntity.Position);
		}

		SetPosition(GetEvaluationStep(focusEntity, direction));
	}

	public float GetEvaluationStep(Entity target, Direction direction = Direction.None)
	{
		if(direction == Direction.None)
		{
			GetDirection(Position, target.Position);
		}

		float targetPosition = target.GetSidePosition(this, direction);
		float delta = targetPosition - Position;
		float step = Mathf.Sign(delta) * Time.deltaTime * _speed;

		float newPosition = Position + step;

		if(Mathf.Approximately(delta, 0))
		{
			newPosition = targetPosition;
		}
		else
		{
			switch(direction)
			{
				case Direction.Right:
					newPosition = Mathf.Min(targetPosition, newPosition);
					break;
				case Direction.Left:
					newPosition = Mathf.Max(targetPosition, newPosition);
					break;
			}
		}

		return newPosition;
	}
}
