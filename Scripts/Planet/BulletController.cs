using Godot;
using System;

namespace SpinShooter.Planet
{
	public class BulletController : Node2D
	{
		#region Public

		#region Properties
		public Vector2 Direction { get; set; } = Vector2.Right;
		#endregion

		#endregion

		#region Private

		#region Properties
		private CollisionShape2D _collisionShape { get; set; }
		private CircleShape2D _circleShape => _collisionShape?.Shape as CircleShape2D;
		private float _velocity { get; set; } = 100f;
		#endregion

		#region Member Methods
		private void DrawBullet()
		{
			// Bullet body
			DrawCircle(
				Vector2.Zero,
				_circleShape.Radius,
				Color.Color8(255, 0, 0)
			);
		}

		private void MoveBullet(float delta)
		{
			Translate(Direction * _velocity * delta);
		}
		#endregion

		#endregion

		#region Godot Hooks
		public override void _Draw()
		{
			base._Draw();

			DrawBullet();
		}

		public override void _Ready()
		{
			_collisionShape = GetNode<CollisionShape2D>("Body/CollisionShape2D");    
		}

		public override void _Process(float delta)
		{
			MoveBullet(delta);
		}
		#endregion
	}
}
