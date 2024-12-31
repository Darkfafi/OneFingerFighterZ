using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GBGameplayManager : MonoBehaviour
{
	public delegate void TargetHandler(GBTarget target);
	// public event TargetHandler TargetHitEvent;
	// public event TargetHandler TargetDestroyedEvent;

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

	public GBEntity CenterEntity => TargetsManager != null ? TargetsManager.CenterEntity : null;

	private HashSet<GBEntity> _levelEntities = new HashSet<GBEntity>();
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

				for(int i = entities.Length - 1; i >= 0; i--)
				{
					TargetsManager.Remove((GBTarget)entities[i]);
				}
			}

			StopCoroutine(_levelRoutine);
			_levelRoutine = null;

			_levelEntities.Clear();
		}
	}

	private void OnInputEvent(GBDirection direction)
	{
		if(!IsLevelActive)
		{
			return;
		}

		if(TargetsManager.GetTargetsQueue(direction).TryGetFront(out GBTarget frontTarget))
		{
			float impactPosition = frontTarget.GetSidePosition(CenterEntity, direction);

			// Teleport Center Entity
			CenterEntity.SetPosition(impactPosition);

			// Push Back Target
			frontTarget.SetPosition(CenterEntity.GetSidePosition(frontTarget, GBDirection.None, 50));

			// Hit (Remove when no components left)
			TargetsManager.Remove(frontTarget);
		}
	}

	private IEnumerator LevelRoutine(int enemiesCount)
	{
		yield return new WaitForSeconds(1f);

		TargetsManager.TargetRemovedEvent += OnTargetRemovedEvent;

		float minRange = 200;

		GBTarget lastLeftTarget = null;
		GBTarget lastRightTarget = null;

		for(int i = 0; i < enemiesCount; i++)
		{
			GBTarget lastTarget = null;
			GBDirection direction = (GBDirection)Random.Range(1, 3);
			GBDirection centerDirection = GBDirection.None;

			switch(direction)
			{
				case GBDirection.Left:
					lastTarget = lastLeftTarget;
					centerDirection = GBDirection.Right;
					break;
				case GBDirection.Right:
					lastTarget = lastRightTarget;
					centerDirection = GBDirection.Left;
					break;
			}

			float spawnPosition = lastTarget != null ? lastTarget.GetSidePosition(0, centerDirection, Random.Range(10f, 100f)) : CenterEntity.GetSidePosition(0, centerDirection, minRange);

			lastTarget = TargetsManager.CreateBack(direction, spawnPosition, isRelativePosition: false);

			switch(direction)
			{
				case GBDirection.Left:
					lastLeftTarget = lastTarget;
					break;
				case GBDirection.Right:
					lastRightTarget = lastTarget;
					break;
			}

			_levelEntities.Add(lastTarget);
		}

		yield return new WaitUntil(() => _levelEntities.Count == 0);
		yield return new WaitForSeconds(1f);

		EndLevel();
	}

	private void OnTargetRemovedEvent(GBEntity entity)
	{
		_levelEntities.Remove(entity);
	}
}
