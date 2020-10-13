using Godot;
using System;

namespace SpinShooter.Enemies.Enemy
{
	public class EnemyController : Node2D
	{
		#region Public
		
		#region Properties
		[Export]
		public float Speed { get; set; } = 50f;
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
		
		#region Private
		
		#region Properties
		private Vector2 _direction { get; set; }
		#endregion
		
		#region Member Methods
		private void DrawEnemy()
		{
			// Enemy body
			DrawRect(
				new Rect2(
					position: Vector2.Zero,
					size: new Vector2(10f, 10f)
				),
				Color.Color8(255, 0, 255),
				filled: true
			);
		}
		
		private void MoveEnemy(float delta)
		{
			GlobalTranslate(_direction * Speed * delta);
		}
		#endregion
		
		#endregion
		
		#region Godot Hooks
		public override void _Draw()
		{
			base._Draw();

			DrawEnemy();
		}

		public override void _Ready()
		{
		}

		public override void _Process(float delta)
		{
			MoveEnemy(delta);
		}
		#endregion
	}
}
