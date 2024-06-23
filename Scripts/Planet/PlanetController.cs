using Godot;
using System;
using SpinShooter.Scripts.Game;
using SpinShooter.Scripts.Planet.Guns;
using System.Collections.Generic;
using SpinShooter.Scripts.Singletons;

namespace SpinShooter.Scripts.Planet
{
	public partial class PlanetController : Node2D
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
		private readonly IList<Gun> _guns = new List<Gun>();
		private float _elapsedShotTime = 0f;
		private float _fireDelay = MIN_FIRE_DELAY;
		private int _numberOfGuns = 1;
		private float _size = 10f;
		#endregion

		#region Properties
		private Vector2 _baseGunPosition => new Vector2(_circleShape.Radius, 0f);
		private CollisionShape2D _collisionShape { get; set; }
		private CircleShape2D _circleShape => _collisionShape?.Shape as CircleShape2D;
		private PackedScene _mainMenuScene { get; set; }
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

			var gunStep = NumberOfGuns == 0 ? Mathf.Tau : Mathf.Tau / NumberOfGuns;
			var basicGunScene = BasicGun.GetScene();
			for (float phi = 0; phi < Mathf.Tau; phi += gunStep)
			{
				var gun = basicGunScene.Instantiate() as BasicGun;
				gun.Initialize(_baseGunPosition, phi);
				_guns.Add(gun);
				AddChild(gun);
			}
		}

		private bool IsUpgradeActive(Upgrades.Upgrade upgrade)
		{
			return upgrade.IsPurchased && upgrade.IsEnabled;
		}

		private void LoadNumberOfGuns()
		{
			NumberOfGuns = MIN_NUMBER_OF_GUNS;
			// NOTE: This is wildly ugly. Consider a map of attributes that get set programmatically
			if (IsUpgradeActive(Upgrades.TwoGuns))
			{
				NumberOfGuns = 2;
			}
			if (IsUpgradeActive(Upgrades.ThreeGuns))
			{
				NumberOfGuns = 3;
			}
			if (IsUpgradeActive(Upgrades.FourGuns))
			{
				NumberOfGuns = 4;
			}
		}

		private void LoadPlanet()
		{
			LoadNumberOfGuns();
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
			GetTree().ChangeSceneToPacked(_mainMenuScene);
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
			LoadPlanet();
			GenerateGuns();
		}

		public override void _Process(double delta)
		{
			float dt = System.Convert.ToSingle(delta);
			_elapsedShotTime += dt;
			RotatePlanet(dt);
			UpdateGuns();
		}
		#endregion
		
		#region Godot Signals
		private void _on_Body_area_entered(object area)
		{
			GD.Print("taking damage");
			TakeDamage();
		}
		#endregion
	}
}
