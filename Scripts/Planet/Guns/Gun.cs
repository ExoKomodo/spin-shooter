using Godot;
using SpinShooter.Game;
using System;

namespace SpinShooter.Planet.Guns
{
	public abstract class Gun : Node2D
	{
		#region Public

		#region Constructors
		protected Gun(float gunSize)
		{
			GunSize = gunSize;
		}
		#endregion

		#region Properties
		public Vector2 BasePosition { get; set; }
		public float Phi { get; set; }
		#endregion

		#region Fields
		public readonly float GunSize;
		#endregion

		#region Member Methods
		public virtual void Destroy()
		{
			QueueFree();
		}

		public abstract void Shoot(
			GameController game,
			float baseRotation
		);
		#endregion

		#endregion

		#region Protected

		#region Member Methods
		protected virtual void LoadScenes() {}
		#endregion

		#endregion
		
		#region Godot Hooks
		public override void _Draw()
		{
		}

		public override void _Ready()
		{
		}
		#endregion
	}
}
