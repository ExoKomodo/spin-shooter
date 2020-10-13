using Godot;
using System;
using SpinShooter.Enemies.Enemy;
using SpinShooter.Planet;

namespace SpinShooter.Game
{
	public class GameController : Node2D
	{
		#region Public

		#region Constants
		public const string GAME_SHOOT = "game_shoot";
		public const float SPAWN_DISTANCE = 500f;
		#endregion
		
		#region Properties
		[Export]
		public int NumberToSpawn { get; set; } = 1;
		[Export]
		public float SpawnTime { get; set; } = 5f;
		#endregion

		#endregion

		#region Private

		#region Properties
		private Random _random { get; set; }
		private PackedScene _enemyScene { get; set; }
		private PackedScene _mainMenuScene { get; set; }
		private PlanetController _planetController { get; set; }
		private float _accumulatedTime { get; set; }
		#endregion
		
		#region Member Methods
		private void LoadScenes()
		{
			_mainMenuScene = GD.Load<PackedScene>("res://Scenes/UI/MainMenu.tscn");
			if (_mainMenuScene == null)
			{
				throw new Exception("Main Menu scene did not load correctly");
			}
			
			_enemyScene = GD.Load<PackedScene>("res://Scenes/Enemies/Enemy.tscn");
			if (_enemyScene == null)
			{
				throw new Exception("Enemy scene did not load correctly");
			}
		}
		
		private void SpawnEnemies()
		{
			if (_accumulatedTime >= SpawnTime)
			{
				_accumulatedTime = 0f;
				for (int i = 0; i < NumberToSpawn; i++)
				{
					SpawnEnemy();
				}
			}
		}
		
		private void SpawnEnemy()
		{
			var enemy = _enemyScene.Instance() as EnemyController;
			AddChild(enemy);
			
			var origin = _planetController.GlobalPosition;
			var rotation = (float)_random.Next(0, 360);
			var offset = (Vector2.Right * SPAWN_DISTANCE).Rotated(rotation);
			enemy.GlobalPosition = origin + offset;
			enemy.Target(_planetController);
		}
		#endregion
		
		#endregion

		#region Godot Hooks
		public override void _Ready()
		{
			_random = new Random();
			LoadScenes();
			_planetController = GetNode<PlanetController>("Planet");
			SpawnEnemies();
		}

		public override void _Input(InputEvent @event)
		{
			base._UnhandledInput(@event);

			if (@event is InputEventScreenTouch touch && touch.Pressed)
			{
				_planetController?.Shoot(this);
			}
		}
		
		public override void _Process(float delta)
		{
			_accumulatedTime += delta;
			
			SpawnEnemies();
		}
		#endregion

		#region Godot Signals
		private void _on_ChangeSpinButton_pressed()
		{
			_planetController.RotationSpeed *= -1f;
		}

		private void _on_ExitButton_pressed()
		{
			GetTree().ChangeSceneTo(_mainMenuScene);
		}
		#endregion
	}
}
