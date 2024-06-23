using Godot;
using System;

namespace SpinShooter.Scripts.UI.MainMenu
{
	public class MainMenuController : Control
	{
		#region Private

		#region Properties
		private PackedScene _gameScene { get; set; }
		private PackedScene _upgradeMenuScene { get; set; }
		#endregion

		#region Member Methods
		private void LoadScenes()
		{
			_gameScene = GD.Load<PackedScene>("res://Scenes/Game.tscn");
			if (_gameScene == null)
			{
				throw new Exception("Game scene did not load correctly");
			}

			_upgradeMenuScene = GD.Load<PackedScene>("res://Scenes/UI/UpgradeMenu.tscn");
			if (_upgradeMenuScene == null)
			{
				throw new Exception("Upgrade Menu scene did not load correctly");
			}
		}
		#endregion

		#endregion

		#region Godot Hooks
		public override void _Ready()
		{
			LoadScenes();	
		}
		#endregion

		#region Godot Signals
		private void _on_QuitButton_pressed()
		{
			GetTree().Quit();
		}

		private void _on_StartButton_pressed()
		{
			GetTree().ChangeSceneTo(_gameScene);
		}

		private void _on_UpgradeButton_pressed()
		{
			GetTree().ChangeSceneTo(_upgradeMenuScene);
		}
		#endregion
	}
}
