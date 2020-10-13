using Godot;
using System;

namespace SpinShooter.Planet
{
	public class PlanetController : Node2D
	{
		#region Public
		
		#region Constants
		public const float MIN_SIZE = 10f;
		public const float GUN_LENGTH = 25f;
		public const float GUN_WIDTH = 10f;
		#endregion

		#region Properties
		[Export]
		public float Size
		{
			get => _size;
			set
			{
				if (value < MIN_SIZE)
				{
					value = MIN_SIZE;
				}
				_size = value;
				UpdateSize();
			}
		}

		[Export]
		public float RotationSpeed { get; set; } = 1f;
		#endregion
		
		#endregion
		
		#region Private
		
		#region Fields
		private float _size = 10f;
		#endregion
		
		#region Properties
		private CollisionShape2D _collisionShape { get; set; }
		private CircleShape2D _circleShape => _collisionShape?.Shape as CircleShape2D;
		#endregion

		#region Member Methods
		private void DrawPlanet()
		{
			// Planet body
			DrawCircle(
				Vector2.Zero,
				_circleShape.Radius,
				Color.Color8(255, 0, 0)
			);
			// Planet gun
			DrawRect(
				new Rect2(
					position: new Vector2(_circleShape.Radius, -GUN_WIDTH / 2f),
					size: new Vector2(GUN_LENGTH, GUN_WIDTH)
				),
				Color.Color8(0, 255, 0),
				filled: true
			);
		}

		private void RotatePlanet(float delta)
		{
			Rotate(RotationSpeed * delta);
		}

		private void UpdateSize()
		{
			if (_circleShape is null)
			{
				return;
			}
			_circleShape.Radius = _size;
		}
		#endregion

		#endregion

		#region Godot Hooks
		public override void _Draw()
		{
			base._Draw();

			DrawPlanet();
		}

		public override void _Ready()
		{
			_collisionShape = GetNode<CollisionShape2D>("Body/CollisionShape2D");
			UpdateSize();
		}

		public override void _Process(float delta)
		{
			RotatePlanet(delta);
		}
		#endregion
	}
}
