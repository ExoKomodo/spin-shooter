using Godot;
using SpinShooter.Scripts.Singletons;

namespace SpinShooter.Scripts.Enemies
{
	public abstract partial class Enemy : Node2D
	{
		#region Public
		
		#region Properties
		public EnemyId Id { get; protected set; }
		public ulong Score { get; protected set; }
		public float Speed { get; set; }
		#endregion
		
		#region Member Methods
		public void Target(Node2D target)
		{
			if (target is null)
			{
				return;
			}
			_direction = (target.GlobalPosition - GlobalPosition).Normalized();
		}
		#endregion
		
		#endregion
		
		#region Protected
		
		#region Properties
		protected Vector2 _direction { get; set; }
		#endregion
		
		#region Member Methods
		protected abstract new void Draw();
		
		protected abstract void Move(float delta);
		
		protected void TakeDamage()
		{
			StatTracker.AddKill(this);
			QueueFree();
		}
		#endregion
		
		#endregion
		
		#region Godot Hooks
		public override void _Draw()
		{
			base._Draw();

			Draw();
		}
		#endregion
	}
}
