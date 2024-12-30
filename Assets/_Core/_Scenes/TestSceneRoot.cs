using UnityEngine;

public class TestSceneRoot : MonoBehaviour
{
	[SerializeField]
	private TargetsManager _targetsManager = null;
	
	protected void Awake()
	{
		
	}

	protected void Start()
	{

	}

	protected void Update()
	{
		_targetsManager.Evaluate();

		if(Input.GetKeyDown(KeyCode.E))
		{
			_targetsManager.Create(Direction.Right);
		}

		if(Input.GetKeyDown(KeyCode.Q))
		{
			_targetsManager.Create(Direction.Left);
		}

		if(Input.GetKeyDown(KeyCode.A))
		{
			Submit(Direction.Left);
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			Submit(Direction.Right);
		}
	}

	public void Submit(Direction direction)
	{
		if(_targetsManager.GetTargetsQueue(direction).TryGetFront(out Target frontTarget))
		{
			float impactPosition = frontTarget.GetSidePosition(_targetsManager.CenterEntity, direction);
			
			// Teleport Center Entity
			_targetsManager.CenterEntity.SetPosition(impactPosition);

			// Push Back Target
			frontTarget.SetPosition(_targetsManager.CenterEntity.GetSidePosition(frontTarget, Direction.None, 50));
		}
	}

	protected void OnDestroy()
	{

	}
}
