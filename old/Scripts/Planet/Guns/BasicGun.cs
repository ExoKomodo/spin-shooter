using Godot;
using SpinShooter.Game;
using System;

namespace SpinShooter.Planet.Guns
{
	public class BasicGun : Gun
	{
		#region Public

		#region Constructors
		public BasicGun() : base(5f) {}
		#endregion

		#region Static Methods
		public static PackedScene GetScene()
		{
			var basicGunScene = GD.Load<PackedScene>("res://Scenes/Planet/Guns/BasicGun.tscn");
			if (basicGunScene == null)
			{
				throw new Exception("Basic Gun scene did not load correctly");
			}
			return basicGunScene;
		}
		#endregion

		#region Member Methods
		public void Initialize(Vector2 basePosition, float phi)
		{
			BasePosition = basePosition;
			Phi = phi;
		}

		public override void Shoot(GameController game, float baseRotation)
		{
			var bullet = _bulletScene.Instance() as BulletController;
			game.AddChild(bullet);

			var muzzlePosition = CalculateMuzzlePosition(Phi + baseRotation);
			bullet.Position = muzzlePosition;
			bullet.Direction = muzzlePosition.Normalized();
		}
		#endregion

		#endregion

		#region Protected

		#region Member Methods
		protected override void Draw()
		{
			DrawCircle(
				position: Vector2.Zero,
				radius: GunSize,
				color: Color.Color8(0, 0, 255)
			);
		}

		protected override void LoadScenes()
		{
			base.LoadScenes();

			_bulletScene = GD.Load<PackedScene>("res://Scenes/Planet/Bullet.tscn");
			if (_bulletScene == null)
			{
				throw new Exception("Bullet scene did not load correctly");
			}
		}
		#endregion

		#endregion

		#region Private
		
		#region Properties
		private PackedScene _bulletScene { get; set; }
		#endregion

		#region Member Methods
		private Vector2 CalculateGunPosition(float phi)
		{
			return BasePosition.Rotated(phi);
		}

		private Vector2 CalculateMuzzlePosition(float phi)
		{
			var muzzlePosition = BasePosition;
			muzzlePosition.x += GunSize;
			muzzlePosition.y = 0f;
			return muzzlePosition.Rotated(phi);
		}
		#endregion

		#endregion

		#region Godot Hooks
		public override void _Process(float delta)
		{
			base._Process(delta);

			Position = CalculateGunPosition(Phi);
		}

		public override void _Ready()
		{
			base._Ready();

			LoadScenes();
		}
		#endregion
	}
}
