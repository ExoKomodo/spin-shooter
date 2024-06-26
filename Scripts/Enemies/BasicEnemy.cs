using Godot;
using System;

namespace SpinShooter.Enemies
{
	public class BasicEnemy : Enemy
	{
		#region Protected

		#region Member Methods
		protected override void Draw()
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

		protected override void Move(float delta)
		{
			GlobalTranslate(_direction * Speed * delta);
		}
		#endregion

		#endregion

		#region Private

		#region Properties
		private CollisionShape2D _collisionShape { get; set; }
		private RectangleShape2D _rectangleShape => _collisionShape?.Shape as RectangleShape2D;
		#endregion

		#endregion

		#region Godot Hooks
		public override void _Ready()
		{
			base._Ready();

			_collisionShape = GetNode<CollisionShape2D>("Body/CollisionShape2D");
			Speed = 50f;
			Score = 1;
			Id = EnemyId.Basic;
		}

		public override void _Process(float delta)
		{
			base._Process(delta);

			Move(delta);
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
