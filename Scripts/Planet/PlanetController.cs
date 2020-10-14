using Godot;
using System;
using SpinShooter.Game;

namespace SpinShooter.Planet
{
	public class PlanetController : Node2D
	{
		#region Public

		#region Constants
		public const float MIN_SIZE = 10f;
		#endregion

		#region Properties
		[Export]
		public float GunLength = 15f;
		[Export]
		public float GunWidth = 5f;
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

		#region Member Methods
		public void Shoot(GameController game)
		{
			var bullet = _bulletScene.Instance() as BulletController;
			game.AddChild(bullet);

			var bulletPosition = _muzzlePosition;
			// Translate local position to global
			bullet.GlobalPosition = bulletPosition + GlobalPosition;
			bullet.Direction = _muzzlePosition.Normalized();
		}
		#endregion

		#endregion

		#region Private

		#region Fields
		private float _size = 10f;
		#endregion

		#region Properties
		private PackedScene _bulletScene { get; set; }
		private CollisionShape2D _collisionShape { get; set; }
		private CircleShape2D _circleShape => _collisionShape?.Shape as CircleShape2D;
		private Vector2 _gunPosition => new Vector2(_circleShape.Radius, -GunWidth / 2f);
		private Vector2 _gunSize => new Vector2(GunLength, GunWidth);
		private PackedScene _mainMenuScene { get; set; }
		private Vector2 _muzzlePosition
		{
			get
			{
				var muzzlePosition = _gunPosition;
				muzzlePosition.x += GunLength;
				muzzlePosition.y = 0f;
				return muzzlePosition.Rotated(Rotation);
			}
		}
		#endregion

		#region Member Methods
		private void DrawPlanet()
		{
			// Planet body
			DrawCircle(
				Vector2.Zero,
				_circleShape.Radius,
				Color.Color8(0, 255, 0)
			);
			// Planet gun
			DrawRect(
				new Rect2(
					position: _gunPosition,
					size: _gunSize
				),
				Color.Color8(0, 0, 255),
				filled: true
			);
		}

		private void LoadScenes()
		{
			_bulletScene = GD.Load<PackedScene>("res://Scenes/Planet/Bullet.tscn");
			if (_bulletScene == null)
			{
				throw new Exception("Bullet scene did not load correctly");
			}

			_mainMenuScene = GD.Load<PackedScene>("res://Scenes/UI/MainMenu.tscn");
			if (_mainMenuScene == null)
			{
				throw new Exception("Main Menu scene did not load correctly");
			}
		}

		private void RotatePlanet(float delta)
		{
			Rotate(RotationSpeed * delta);
		}

		private void TakeDamage()
		{
			GetTree().ChangeSceneTo(_mainMenuScene);
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
			LoadScenes();
			_collisionShape = GetNode<CollisionShape2D>("Body/CollisionShape2D");
			UpdateSize();
		}

		public override void _Process(float delta)
		{
			RotatePlanet(delta);
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
