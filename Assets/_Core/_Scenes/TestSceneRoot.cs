using UnityEngine;

public class TestSceneRoot : MonoBehaviour
{
	[SerializeField]
	private GBGameplayManager _targetsManager = null;

	protected void Start()
	{
		_targetsManager.StartLevel(20);
	}

	protected void OnDestroy()
	{
		if(_targetsManager != null)
		{
			_targetsManager.EndLevel();
		}
	}
}
