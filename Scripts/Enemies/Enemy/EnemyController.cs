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
		private CollisionShape2D _collisionShape { get; set; }
		private RectangleShape2D _rectangleShape => _collisionShape?.Shape as RectangleShape2D;
		private Vector2 _direction { get; set; }
		#endregion
		
		#region Member Methods
		private void DrawEnemy()
		{
			// Enemy body
			DrawRect(
				new Rect2(
					position: -_rectangleShape.Extents,
					// Extents on shape are half extents
					size: _rectangleShape.Extents * 2f
				),
				Color.Color8(255, 0, 255),
				filled: true
			);
		}
		
		private void MoveEnemy(float delta)
		{
			GlobalTranslate(_direction * Speed * delta);
		}
		
		private void TakeDamage()
		{
			QueueFree();
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
			_collisionShape = GetNode<CollisionShape2D>("Body/CollisionShape2D");
		}

		public override void _Process(float delta)
		{
			MoveEnemy(delta);
		}
		#endregion
		
		#region Godot Signals
		private void _on_Body_area_entered(object area)
		{
			TakeDamage();
		}
		#endregion
	}
}
