using Godot;
using System;
using SpinShooter.Game;
using SpinShooter.Planet.Guns;
using System.Collections.Generic;

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
			foreach (var gun in _guns)
			{
				gun.Shoot(game, Rotation);
			}
		}
		#endregion

		#endregion

		#region Private

		#region Fields
		private IList<Gun> _guns = new List<Gun>();
		private float _elapsedShotTime = 0f;
		private float _fireDelay = MIN_FIRE_DELAY;
		private int _numberOfGuns = 1;
		private float _size = 10f;
		#endregion

		#region Properties
		private Vector2 _baseGunPosition => new Vector2(_circleShape.Radius, 0f);
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
		private void DrawBody()
		{
			DrawCircle(
				position: Vector2.Zero,
				radius: _circleShape.Radius,
				color: Color.Color8(0, 255, 0)
			);
		}
		
		private void DrawPlanet()
		{
			DrawBody();
		}

		private void GenerateGuns()
		{
			foreach (var gun in _guns)
			{
				gun.Destroy();
			}
			_guns.Clear();

			var basicGunScene = BasicGun.GetScene();
			for (float phi = 0; phi < Mathf.Tau; phi += _gunStep)
			{
				var gun = basicGunScene.Instance() as BasicGun;
				gun.Initialize(_baseGunPosition, phi);
				_guns.Add(gun);
				AddChild(gun);
			}
		}

		private void LoadScenes()
		{
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

		private void UpdateGuns()
		{
			foreach (var gun in _guns)
			{
				gun.BasePosition = _baseGunPosition;
			}
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
			GenerateGuns();
		}

		public override void _Process(float delta)
		{
			_elapsedShotTime += delta;
			RotatePlanet(delta);
			UpdateGuns();
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
