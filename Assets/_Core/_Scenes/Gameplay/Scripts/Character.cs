using UnityEngine;
using RaTweening;
using System;

namespace OnePunchFighterZ.GameplayScene
{
	public class Character : MonoBehaviour
	{
		[field: SerializeField]
		public SpriteRenderer MainRenderer
		{
			get; private set;
		}

		[Header("Animations")]
		[field: SerializeField]
		public CoreAnimations CoreAnimations
		{
			get; private set;
		}

		[field: SerializeField]
		public DuelAnimations DuelAnimations
		{
			get; private set;
		}

		protected void Awake()
		{
			CoreAnimations.IdleAnimation.Play();
		}

		protected void OnDestroy()
		{
			
		}
	}

	[Serializable]
	public struct CoreAnimations
	{
		[field: SerializeField]
		public RaTweenerComponent IdleAnimation
		{
			get; private set;
		}
	}

	[Serializable]
	public struct DuelAnimations
	{
		[field: SerializeField]
		public RaTweenerComponent Attack1Animation
		{
			get; private set;
		}

		[field: SerializeField]
		public RaTweenerComponent Attack2Animation
		{
			get; private set;
		}

		[field: SerializeField]
		public RaTweenerComponent Attack3Animation
		{
			get; private set;
		}

		[field: SerializeField]
		public RaTweenerComponent Attack4Animation
		{
			get; private set;
		}
	}
}