using Godot;
using SpinShooter.Scripts.Singletons;
using System;

namespace SpinShooter.Scripts.UI.UpgradeMenu
{
	public class UpgradeMenuController : Control
	{
		#region Private

		#region Properties
		private PackedScene _mainMenuScene { get; set; }
		private Label _scoreLabel { get; set; }
		// Two guns
		private Button _twoGunsButton { get; set; }
		private CheckBox _twoGunsCheckBox { get; set; }
		private Label _twoGunsLabel { get; set; }
		// Three guns
		private Button _threeGunsButton { get; set; }
		private CheckBox _threeGunsCheckBox { get; set; }
		private Label _threeGunsLabel { get; set; }
		// Four guns
		private Button _fourGunsButton { get; set; }
		private CheckBox _fourGunsCheckBox { get; set; }
		private Label _fourGunsLabel { get; set; }
		#endregion

		#region Member Methods
		private bool IsPurchaseable(Upgrades.Upgrade upgrade)
		{
			return !upgrade.IsPurchased && StatTracker.TryBuy(upgrade.Cost);
		}

		private void LoadScenes()
		{
			_mainMenuScene = GD.Load<PackedScene>("res://Scenes/UI/MainMenu.tscn");
			if (_mainMenuScene == null)
			{
				throw new Exception("Main Menu scene did not load correctly");
			}
		}
		
		private void LoadUIReferences()
		{
			_scoreLabel = GetNode<Label>("ScoreLabel");
			
			// Two guns
			_twoGunsButton = GetNode<Button>("GridContainer/TwoGunsButton");
			_twoGunsCheckBox = GetNode<CheckBox>("GridContainer/TwoGunsCheckBox");
			_twoGunsLabel = GetNode<Label>("GridContainer/TwoGunsLabel");
			// Three guns
			_threeGunsButton = GetNode<Button>("GridContainer/ThreeGunsButton");
			_threeGunsCheckBox = GetNode<CheckBox>("GridContainer/ThreeGunsCheckBox");
			_threeGunsLabel = GetNode<Label>("GridContainer/ThreeGunsLabel");
			// Four guns
			_fourGunsButton = GetNode<Button>("GridContainer/FourGunsButton");
			_fourGunsCheckBox = GetNode<CheckBox>("GridContainer/FourGunsCheckBox");
			_fourGunsLabel = GetNode<Label>("GridContainer/FourGunsLabel");
		}

		private void Purchase(Upgrades.Upgrade upgrade)
		{
			Upgrades.Purchase(upgrade.Id);
		}

		private void ToggleEnable(Upgrades.Upgrade upgrade)
		{
			upgrade.IsEnabled = !upgrade.IsEnabled;
		}
		
		private void UpdateUI()
		{
			UpdateScoreLabel();
			UpdateList();
		}
		
		private void UpdateList()
		{
			UpdateListEntry(Upgrades.TwoGuns, _twoGunsLabel, _twoGunsButton, _twoGunsCheckBox);
			UpdateListEntry(Upgrades.ThreeGuns, _threeGunsLabel, _threeGunsButton, _threeGunsCheckBox);
			UpdateListEntry(Upgrades.FourGuns, _fourGunsLabel, _fourGunsButton, _fourGunsCheckBox);
		}

		private void UpdateListEntry(Upgrades.Upgrade upgrade, Label label, Button button, CheckBox checkBox)
		{
			label.Text = $"{upgrade.Name} costs {upgrade.Cost}";
			
			button.Disabled = !IsPurchaseable(upgrade);
			
			checkBox.Disabled = !upgrade.IsPurchased;
			checkBox.Pressed = upgrade.IsEnabled;
		}

		private void UpdateScoreLabel()
		{
			_scoreLabel.Text = $"Score: {StatTracker.AvailableScore}";
		}
		#endregion

		#endregion

		#region Godot Hooks
		public override void _Ready()
		{
			LoadScenes();
			LoadUIReferences();
		}
		
		public override void _Process(float delta)
		{
			UpdateUI();
		}
		#endregion

		#region Godot Signals
		private void _on_ExitButton_pressed()
		{
			GetTree().ChangeSceneTo(_mainMenuScene);
		}

		// Two guns
		private void _on_TwoGunsButton_pressed()
		{
			Purchase(Upgrades.TwoGuns);
		}

		private void _on_TwoGunsCheckBox_pressed()
		{
			ToggleEnable(Upgrades.TwoGuns);
		}

		// Three guns
		private void _on_ThreeGunsButton_pressed()
		{
			Purchase(Upgrades.ThreeGuns);
		}

		private void _on_ThreeGunsCheckBox_pressed()
		{
			ToggleEnable(Upgrades.ThreeGuns);
		}

		// Four guns
		private void _on_FourGunsButton_pressed()
		{
			Purchase(Upgrades.FourGuns);
		}

		private void _on_FourGunsCheckBox_pressed()
		{
			ToggleEnable(Upgrades.FourGuns);
		}
		#endregion
	}
}
