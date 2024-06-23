using Godot;
using System;
using SpinShooter.Scripts.Enemies;
using SpinShooter.Scripts.Planet;
using SpinShooter.Scripts.Singletons;

namespace SpinShooter.Scripts.Game
{
	public class GameController : Control
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
		private PackedScene _basicEnemyScene { get; set; }
		private PackedScene _mainMenuScene { get; set; }
		private PlanetController _planetController { get; set; }
		private float _accumulatedTime { get; set; }
		#endregion
		
		#region Member Methods
		private void EndRound()
		{
			StatTracker.EndRound();
		}

		private void LoadScenes()
		{
			_mainMenuScene = GD.Load<PackedScene>("res://Scenes/UI/MainMenu.tscn");
			if (_mainMenuScene == null)
			{
				throw new Exception("Main Menu scene did not load correctly");
			}
			
			_basicEnemyScene = GD.Load<PackedScene>("res://Scenes/Enemies/BasicEnemy.tscn");
			if (_basicEnemyScene == null)
			{
				throw new Exception("Basic Enemy scene did not load correctly");
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
			var enemy = _basicEnemyScene.Instance() as BasicEnemy;
			AddChild(enemy);
			
			var origin = _planetController.GlobalPosition;
			var rotation = (float)_random.Next(0, 360);
			var offset = (Vector2.Right * SPAWN_DISTANCE).Rotated(rotation);
			enemy.GlobalPosition = origin + offset;
			enemy.Target(_planetController);
		}

		private void StartRound()
		{
			StatTracker.StartRound();
		}

		public void ReverseRotation()
		{
			_planetController.RotationSpeed *= -1f;
		}
		#endregion
		
		#endregion

		#region Godot Hooks
		public override void _Ready()
		{
			_random = new Random();
			LoadScenes();
			_planetController = GetNode<PlanetController>("Planet");

			StartRound();
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

		public override void _ExitTree()
		{
			base._ExitTree();

			EndRound();
		}
		#endregion

		#region Godot Signals
		
		#endregion

		#region Godot Signals
		private void _on_ChangeSpinButton_pressed()
		{
			ReverseRotation();
		}

		private void _on_ExitButton_pressed()
		{
			GetTree().ChangeSceneTo(_mainMenuScene);
		}
		#endregion
	}
}
