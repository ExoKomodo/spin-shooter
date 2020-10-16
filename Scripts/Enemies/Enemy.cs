using Godot;
using System;

namespace SpinShooter.Enemies
{
	public abstract class Enemy : Node2D
	{
		#region Public
		
		#region Properties
		public float Speed { get; set; }
		#endregion
		
		#region Member Methods
		public void Target(Node2D target)
		{
			if (target is null)
			{
				return;
			}
			_direction = (target.GlobalPosition - GlobalPosition).Normalized();
		}
		#endregion
		
		#endregion
		
		#region Protected
		
		#region Properties
		protected Vector2 _direction { get; set; }
		#endregion
		
		#region Member Methods
		protected abstract void Draw();
		
		protected abstract void Move(float delta);
		
		protected virtual void TakeDamage()
		{
			QueueFree();
		}
		#endregion
		
		#endregion
		
		#region Godot Hooks
		public override void _Draw()
		{
			base._Draw();

			Draw();
		}

		public override void _Ready() {}

		public override void _Process(float delta) {}
		#endregion
	}
}
