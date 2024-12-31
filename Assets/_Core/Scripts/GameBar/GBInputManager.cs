using UnityEngine;
using UnityEngine.InputSystem;

public class GBInputManager : MonoBehaviour
{
	public delegate void DirectionInputHandler(GBDirection direction);
	public event DirectionInputHandler InputEvent;

	private InputSystem_Actions _input = null;
	private InputAction _moveInputAction = null;

	protected void Awake()
	{
		_input = new InputSystem_Actions();
		_moveInputAction = _input.Player.Move;

		_moveInputAction.performed += OnMovePerformed;

		_moveInputAction.Disable();
	}

	protected void OnEnable()
	{
		_moveInputAction.Enable();
	}

	protected void OnDisable()
	{
		_moveInputAction.Disable();
	}

	protected void OnDestroy()
	{
		if(_moveInputAction != null)
		{
			_moveInputAction.performed -= OnMovePerformed;
			_moveInputAction.Disable();
			_moveInputAction = null;
		}

		if(_input != null)
		{
			_input.Disable();
			_input = null;
		}
	}

	private void OnMovePerformed(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();
		switch(input.normalized)
		{
			case Vector2 v when v == Vector2.left:
				InputEvent?.Invoke(GBDirection.Left);
				break;
			case Vector2 v when v == Vector2.right:
				InputEvent?.Invoke(GBDirection.Right);
				break;
		}
	}
}
