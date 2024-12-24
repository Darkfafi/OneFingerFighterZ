using RaTweening;
using System;
using UnityEngine;

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
		public CoreResources CoreResources
		{
			get; private set;
		}

		[field: SerializeField]
		public DuelResources DuelResources
		{
			get; private set;
		}

		protected void Awake()
		{
			ResetState();
		}

		public void ResetState()
		{
			StopAnimations();
			CoreResources.IdleAnimation.Play();
		}

		public void StopAnimations()
		{
			CoreResources.StopAnimations();
			DuelResources.StopAnimations();
		}

		protected void OnDestroy()
		{

		}
	}

	[Serializable]
	public struct CoreResources
	{
		[field: SerializeField]
		public RaTweenerComponent IdleAnimation
		{
			get; private set;
		}

		[field: SerializeField]
		public RaTweenerComponent HitAnimation
		{
			get; private set;
		}

		public void StopAnimations()
		{
			IdleAnimation.Stop();
			HitAnimation.Stop();
		}
	}

	[Serializable]
	public struct DuelResources
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

		[field: SerializeField]
		public DuelMinigameView MinigameView
		{
			get; private set;
		}

		public void StopAnimations()
		{
			Attack1Animation.Stop();
			Attack2Animation.Stop();
			Attack3Animation.Stop();
			Attack4Animation.Stop();
		}
	}
}