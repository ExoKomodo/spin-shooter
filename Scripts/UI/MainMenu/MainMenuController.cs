using Godot;
using System;

namespace SpinShooter.UI.MainMenu
{
	public class MainMenuController : Control
	{
		public override void _Ready()
		{
			
		}
		
		private void _on_StartButton_pressed()
		{
			GD.Print("Start");
		}

		private void _on_QuitButton_pressed()
		{
			GetTree().Quit();
		}
	}
}
