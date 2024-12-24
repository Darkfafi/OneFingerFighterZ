using System;
using UnityEngine;
using UnityEngine.UI;

namespace OnePunchFighterZ.GameplayScene
{
	[Flags]
	public enum InputType
	{
		None = 0,
		Right = 1,
		Left = 2,
		Up = 4,
		Down = 8,
	}

	public static class InputTypeHelpers
	{
		public static Sprite GetIcon(this InputType inputType)
		{
			switch(inputType)
			{
				case InputType.None:
					return null;
				default:
					return Resources.Load<Sprite>("arrow");
			}
		}

		public static float GetRotation(this InputType inputType)
		{
			switch(inputType)
			{
				case InputType.Right:
					return 0f;
				case InputType.Down:
					return -90f;
				case InputType.Left:
					return 180f;
				case InputType.Up:
					return 90f;
			}

			return 0f;
		}

		public static void Apply(this InputType inputType, Image target)
		{
			target.sprite = inputType.GetIcon();
			target.transform.rotation = Quaternion.Euler(0, 0, inputType.GetRotation());
		}
	}
}