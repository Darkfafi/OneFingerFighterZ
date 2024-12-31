using UnityEngine;
using UnityEngine.InputSystem;

public class GBInputManager : MonoBehaviour
{
	public delegate void DirectionInputHandler(Direction direction);
	public event DirectionInputHandler InputEvent;

	private InputAction _moveInputAction = null;

	protected void Awake()
	{
		InputSystem_Actions input = new InputSystem_Actions();
		_moveInputAction = input.Player.Move;

		_moveInputAction.performed += OnMovePerformed;
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
		_moveInputAction.performed -= OnMovePerformed;
	}

	private void OnMovePerformed(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();
		switch(input.normalized)
		{
			case Vector2 v when v == Vector2.left:
				InputEvent?.Invoke(Direction.Left);
				break;
			case Vector2 v when v == Vector2.right:
				InputEvent?.Invoke(Direction.Right);
				break;
		}
	}
}
