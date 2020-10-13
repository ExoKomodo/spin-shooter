using Godot;
using System;
using SpinShooter.Planet;

namespace SpinShooter.Game
{
	public class GameController : Node2D
	{
		#region Private

		#region Properties
		private PackedScene _mainMenuScene { get; set; }
		private PlanetController _planetController { get; set; }
		#endregion
		
		#region Member Methods
		private void LoadScenes()
		{
			_mainMenuScene = GD.Load<PackedScene>("res://Scenes/UI/MainMenu.tscn");
			if (_mainMenuScene == null)
			{
				throw new Exception("Main Menu scene did not load correctly");
			}
		}
		#endregion
		
		#endregion

		#region Godot Hooks
		public override void _Ready()
		{
			LoadScenes();
			_planetController = GetNode<PlanetController>("Planet");
		}

		public override void _Process(float delta)
		{
			
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
