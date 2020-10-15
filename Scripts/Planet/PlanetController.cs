using Godot;
using System;
using SpinShooter.Game;

namespace SpinShooter.Planet
{
	public class PlanetController : Node2D
	{
		#region Public

		#region Constants
		public const float MIN_FIRE_DELAY = 0.1f;
		public const int MIN_NUMBER_OF_GUNS = 1;
		public const float MIN_SIZE = 10f;
		#endregion

		#region Properties
		public bool CanShoot => _elapsedShotTime >= FireDelay;
		[Export]
		public float FireDelay
		{
			get => _fireDelay;
			set
			{
				_fireDelay = Mathf.Max(MIN_FIRE_DELAY, value);
			}
		}
		[Export]
		public float GunSize = 5f;
		[Export]
		public int NumberOfGuns
		{
			get => _numberOfGuns;
			set
			{
				_numberOfGuns = Math.Max(MIN_NUMBER_OF_GUNS, value);
			}
		}
		[Export]
		public float Size
		{
			get => _size;
			set
			{
				_size = Mathf.Max(MIN_SIZE, value);
				UpdateSize();
			}
		}

		[Export]
		public float RotationSpeed { get; set; } = 1f;
		#endregion

		#region Member Methods
		public void Shoot(GameController game)
		{
			if (!CanShoot)
			{
				return;
			}

			_elapsedShotTime = 0f;
			for (float phi = 0f; phi < Mathf.Tau; phi += _gunStep)
			{
				ShootGun(game, phi);
			}
		}
		#endregion

		#endregion

		#region Private

		#region Fields
		private float _elapsedShotTime = 0f;
		private float _fireDelay = MIN_FIRE_DELAY;
		private int _numberOfGuns = 1;
		private float _size = 10f;
		#endregion

		#region Properties
		private Vector2 _baseGunPosition => new Vector2(_circleShape.Radius, 0f);
		private PackedScene _bulletScene { get; set; }
		private CollisionShape2D _collisionShape { get; set; }
		private CircleShape2D _circleShape => _collisionShape?.Shape as CircleShape2D;
		public float _gunStep => NumberOfGuns == 0 ? Mathf.Tau : Mathf.Tau / NumberOfGuns;
		private PackedScene _mainMenuScene { get; set; }
		private Vector2 _muzzlePosition
		{
			get
			{
				var muzzlePosition = _baseGunPosition;
				muzzlePosition.x += GunSize;
				muzzlePosition.y = 0f;
				return muzzlePosition.Rotated(Rotation);
			}
		}
		#endregion

		#region Member Methods
		private Vector2 CalculateGunPosition(float phi)
		{
			return _baseGunPosition.Rotated(phi);
		}

		private Vector2 CalculateMuzzlePosition(float phi)
		{
			var muzzlePosition = _baseGunPosition;
			muzzlePosition.x += GunSize;
			muzzlePosition.y = 0f;
			return muzzlePosition.Rotated(phi);
		}

		private void DrawBody()
		{
			DrawCircle(
				position: Vector2.Zero,
				radius: _circleShape.Radius,
				color: Color.Color8(0, 255, 0)
			);
		}

		private void DrawGun(float phi)
		{
			DrawCircle(
				position: CalculateGunPosition(phi),
				radius: GunSize,
				color: Color.Color8(0, 0, 255)
			);
		}

		private void DrawGuns()
		{
			for (float phi = 0; phi < Mathf.Tau; phi += _gunStep)
			{
				DrawGun(phi);
			}
		}
		
		private void DrawPlanet()
		{
			DrawBody();
			DrawGuns();
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

		private void ShootGun(GameController game, float phi)
		{
			var bullet = _bulletScene.Instance() as BulletController;
			game.AddChild(bullet);

			var muzzlePosition = CalculateMuzzlePosition(phi + Rotation);
			// Translate local position to global
			bullet.GlobalPosition = muzzlePosition + GlobalPosition;
			bullet.Direction = muzzlePosition.Normalized();
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
			_elapsedShotTime = FireDelay;
			LoadScenes();
			_collisionShape = GetNode<CollisionShape2D>("Body/CollisionShape2D");
			UpdateSize();
		}

		public override void _Process(float delta)
		{
			_elapsedShotTime += delta;
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
