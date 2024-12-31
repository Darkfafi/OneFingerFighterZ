using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GBGameplayManager : MonoBehaviour
{
	public delegate void TargetHandler(Target target);
	public event TargetHandler TargetHitEvent;
	public event TargetHandler TargetDestroyedEvent;

	[field: SerializeField]
	public GBInputManager UserInputManager
	{
		get; private set;
	}

	[field: SerializeField]
	public GBTargetsManager TargetsManager
	{
		get; private set;
	}

	public Entity CenterEntity => TargetsManager != null ? TargetsManager.CenterEntity : null;

	private HashSet<Entity> _levelEntities = new HashSet<Entity>();
	private IEnumerator _levelRoutine = null;

	public bool IsLevelActive => _levelRoutine != null;

	protected void Awake()
	{
		UserInputManager.InputEvent += OnInputEvent;
	}

	protected void Update()
	{
		if(IsLevelActive)
		{
			TargetsManager.Evaluate(Time.deltaTime);
		}
	}

	protected void OnDestroy()
	{
		EndLevel();

		if(UserInputManager != null)
		{
			UserInputManager.InputEvent -= OnInputEvent;
		}
	}

	public void StartLevel(int targetsCount)
	{
		EndLevel();
		StartCoroutine(_levelRoutine = LevelRoutine(targetsCount));
	}

	public void EndLevel()
	{
		if(_levelRoutine != null)
		{
			if(TargetsManager != null)
			{
				TargetsManager.TargetRemovedEvent -= OnTargetRemovedEvent;

				var entities = _levelEntities.ToArray();

				for(int i = entities.Length; i >= 0; i--)
				{
					TargetsManager.Remove((Target)entities[i]);
				}
			}

			StopCoroutine(_levelRoutine);
			_levelRoutine = null;

			_levelEntities.Clear();
		}
	}

	private void OnInputEvent(Direction direction)
	{
		if(!IsLevelActive)
		{
			return;
		}

		if(TargetsManager.GetTargetsQueue(direction).TryGetFront(out Target frontTarget))
		{
			float impactPosition = frontTarget.GetSidePosition(CenterEntity, direction);

			// Teleport Center Entity
			CenterEntity.SetPosition(impactPosition);

			// Push Back Target
			frontTarget.SetPosition(CenterEntity.GetSidePosition(frontTarget, Direction.None, 50));

			// Hit (Remove when no components left)
			TargetsManager.Remove(frontTarget);
		}
	}

	private IEnumerator LevelRoutine(int enemiesCount)
	{
		yield return new WaitForSeconds(1f);

		TargetsManager.TargetRemovedEvent += OnTargetRemovedEvent;

		float minRange = 200;

		Target lastLeftTarget = null;
		Target lastRightTarget = null;

		for(int i = 0; i < enemiesCount; i++)
		{
			Target lastTarget = null;
			Direction direction = (Direction)Random.Range(1, 3);

			switch(direction)
			{
				case Direction.Left:
					lastTarget = lastLeftTarget;
					break;
				case Direction.Right:
					lastTarget = lastRightTarget;
					break;
			}

			float spawnPosition = lastTarget != null ? lastTarget.Position + lastTarget.Radius : minRange;
			spawnPosition += Random.Range(10f, 100f);

			lastTarget = TargetsManager.CreateBack(direction, spawnPosition);

			switch(direction)
			{
				case Direction.Left:
					lastLeftTarget = lastTarget;
					break;
				case Direction.Right:
					lastRightTarget = lastTarget;
					break;
			}

			_levelEntities.Add(lastTarget);
		}

		yield return new WaitUntil(() => _levelEntities.Count == 0);
		yield return new WaitForSeconds(1f);

		EndLevel();
	}

	private void OnTargetRemovedEvent(Entity entity)
	{
		_levelEntities.Remove(entity);
	}
}

public class TestSceneRoot : MonoBehaviour
{
	[SerializeField]
	private GBGameplayManager _targetsManager = null;


	// Game Rules
	/*
	 * Targets
	 * - Targets are sorted by side
	 * - A target always tries to reach the facing entity
	 * - An entity has multiple direction components
	 * - Once it reaches the center entity, each evaluation process will progress its effect
	 *	- Attack: This behaviour evaluates an attack every x time to be executed on the center entity
	 *	
	 *	Rules
	 *	- Targets Evaluation Rule
	 *		- Target Movement Behaviour
	 *		- Target Attack Behaviour
	 *	- User Input
	 *		- Submit Input Rule
	 *			- Input -> Target Attack Behaviour
	 *			- Input -> Target Miss Behaviour
	 */

	protected void Start()
	{
		_targetsManager.StartLevel(10);
	}

	protected void OnDestroy()
	{
		if(_targetsManager != null)
		{
			_targetsManager.EndLevel();
		}
	}
}
