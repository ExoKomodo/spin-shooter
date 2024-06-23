using Godot;
using SpinShooter.Scripts.Game;

namespace SpinShooter.Scripts.Planet.Guns
{
	public abstract partial class Gun : Node2D
	{
		#region Public

		#region Constructors
		protected Gun(float gunSize)
		{
			GunSize = gunSize;
		}
		#endregion

		#region Properties
		public Vector2 BasePosition { get; set; }
		public float Phi { get; set; }
		#endregion

		#region Fields
		public readonly float GunSize;
		#endregion

		#region Member Methods
		public virtual void Destroy()
		{
			QueueFree();
		}

		public abstract void Shoot(
			GameController game,
			float baseRotation
		);
		#endregion

		#endregion

		#region Protected

		#region Member Methods
		protected abstract new void Draw();
		protected virtual void LoadScenes() {}
		#endregion

		#endregion
		
		#region Godot Hooks
		public override void _Draw()
		{
			base._Draw();

			Draw();
		}

		public override void _Ready() {}
		#endregion
	}
}
