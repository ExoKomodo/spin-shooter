using Godot;
using System;
using SpinShooter.Enemies;
using System.Collections.Generic;
using System.Linq;

namespace SpinShooter.Singletons
{
	public class StatTracker : Node
	{
		#region Public

		#region Static Properties
		public static UInt64 AvailableScore { get; private set; }
		public static UInt64 RoundScore { get; private set; }
		public static UInt64 TotalScore { get; private set; }
		public static IDictionary<EnemyId, UInt64> EnemyKills { get; private set; }
		#endregion

		#region Static Methods
		public static void AddKill(Enemy enemy)
		{
			RoundScore += enemy.Score;
			EnemyKills[enemy.Id]++;
		}

		public static bool Buy(UInt64 cost)
		{
			if (
				!TryBuy(cost)
				|| cost > AvailableScore
			)
			{
				return false;
			}
			AvailableScore -= cost;
			return true;
		}

		public static void Credit(UInt64 credit)
		{
			AvailableScore += credit;
		}
		
		public static void EndRound()
		{
			CollateScores();
			// TODO: GooglePlay.Persist();
			ResetRound();

			GD.Print(TotalScore);
			GD.Print(EnemyKills.First().Value);
		}

		public static void Initialize()
		{
			// TODO: Load from Google Play
		}

		public static void StartRound()
		{
			ResetRound();
		}

		public static bool TryBuy(UInt64 cost)
		{
			return cost <= AvailableScore;
		}
		#endregion

		#endregion

		#region Private

		#region Static Methods
		private static void CollateScores()
		{
			TotalScore += RoundScore;
			AvailableScore += RoundScore;
		}

		private static void ResetRound()
		{
			RoundScore = 0;
		}
		#endregion

		#endregion

		#region Godot Hooks
		public override void _Ready()
		{
			base._Ready();

			EnemyKills = new Dictionary<EnemyId, UInt64>();
			var enemyIds = Enum.GetValues(typeof(EnemyId)).Cast<EnemyId>();
			foreach (var enemyId in enemyIds)
			{
				EnemyKills[enemyId] = 0;
			}
		}
		#endregion
	}
}
