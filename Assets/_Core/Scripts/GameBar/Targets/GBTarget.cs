using UnityEngine;

public class GBTarget : GBEntity
{
	public static readonly object SpawnPositionMetadata = new object();

	public delegate void EvaluationHandler(GBTarget target, float deltaTime, GBEntity focusEntity, bool reachedFocusEntity);
	public event EvaluationHandler EvaluatedEvent;

	[SerializeField]
	private float _speed = 50;

	public void Evaluate(float deltaTime, GBEntity focusEntity, GBDirection direction = GBDirection.None)
	{
		if(focusEntity == null)
		{
			return;
		}

		if(direction == GBDirection.None)
		{
			GetDirection(Position, focusEntity.Position);
		}

		float newPoisition = GetEvaluationStep(deltaTime, focusEntity, direction, out float targetPosition);
		SetPosition(newPoisition);
		EvaluatedEvent?.Invoke(this, deltaTime, focusEntity, Mathf.Abs(newPoisition - targetPosition) < 0.01f);
	}

	public float GetEvaluationStep(float deltaTime, GBEntity focusEntity, GBDirection direction, out float targetPosition)
	{
		if(direction == GBDirection.None)
		{
			GetDirection(Position, focusEntity.Position);
		}

		targetPosition = focusEntity.GetSidePosition(this, direction);
		float delta = targetPosition - Position;
		float step = Mathf.Sign(delta) * deltaTime * _speed;

		float newPosition = Position + step;

		if(Mathf.Approximately(delta, 0))
		{
			newPosition = targetPosition;
		}
		else
		{
			switch(direction)
			{
				case GBDirection.Right:
					newPosition = Mathf.Min(targetPosition, newPosition);
					break;
				case GBDirection.Left:
					newPosition = Mathf.Max(targetPosition, newPosition);
					break;
			}
		}

		return newPosition;
	}
}
