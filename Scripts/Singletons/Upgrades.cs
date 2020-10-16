using Godot;
using System;
using SpinShooter.Enemies;
using System.Collections.Generic;
using System.Linq;

namespace SpinShooter.Singletons
{
	public class Upgrades : Node
	{
		#region Public

		#region Static Properties
		public static Upgrade TwoGuns { get; private set; }
		public static Upgrade ThreeGuns { get; private set; }
		public static Upgrade FourGuns { get; private set; }
		#endregion

		#region Static Methods
		public static void DisableUpgrade(UpgradeId id)
		{
			var upgrade = Upgrade.GetById(id);
			if (!upgrade.IsPurchased)
			{
				return;
			}
			upgrade.IsEnabled = false;
		}

		public static void EnableUpgrade(UpgradeId id)
		{
			var upgrade = Upgrade.GetById(id);
			if (!upgrade.IsPurchased)
			{
				return;
			}
			upgrade.IsEnabled = true;
		}

		public static void Initialize()
		{
			// TODO: Load from Google Play
			TwoGuns = Upgrade.GetById(UpgradeId.TWO_GUNS);
			LoadUpgradeState(TwoGuns);
			
			ThreeGuns = Upgrade.GetById(UpgradeId.THREE_GUNS);
			LoadUpgradeState(ThreeGuns);
			
			FourGuns = Upgrade.GetById(UpgradeId.FOUR_GUNS);
			LoadUpgradeState(FourGuns);
		}

		public static void Purchase(UpgradeId id)
		{
			var upgrade = Upgrade.GetById(id);
			if (upgrade.IsPurchased)
			{
				return;
			}
			if (StatTracker.Buy(upgrade.Cost))
			{
				upgrade.IsPurchased = true;
				upgrade.IsEnabled = true;
				// TODO: GooglePlay.Persist();
			}
		}
		#endregion

		#endregion

		#region Private

		#region Static Methods
		private static void LoadUpgradeState(Upgrade upgrade)
		{
			// TODO: Loads whether or not upgrade is purchased and enabled from the Play Store
		}
		#endregion

		#endregion

		#region Godot Hooks
		public override void _Ready()
		{
			base._Ready();

			Initialize();
		}
		#endregion

		#region Classes
		public class Upgrade
		{
			#region Public
			
			#region Fields
			public readonly UInt64 Cost;
			public readonly UpgradeId Id;
			public readonly string Name;
			public bool IsEnabled { get; set; }
			public bool IsPurchased { get; set; }
			#endregion

			#region Static Methods
			public static Upgrade GetById(UpgradeId id)
			{
				return _upgradeMap[id];
			}
			#endregion

			#endregion

			#region Private

			#region Constructors
			private Upgrade(UpgradeId id, string name, UInt64 cost)
			{
				Cost = cost;
				Name = name;
				Id = id;
			}
			#endregion

			#region Constants
			private static readonly IReadOnlyDictionary<UpgradeId, Upgrade> _upgradeMap = new Dictionary<UpgradeId, Upgrade>
			{
				[UpgradeId.TWO_GUNS] = new Upgrade(
					id: UpgradeId.TWO_GUNS,
					name: "Two Guns",
					cost: 5
				),
				[UpgradeId.THREE_GUNS] = new Upgrade(
					id: UpgradeId.THREE_GUNS,
					name: "Three Guns",
					cost: 10
				),
				[UpgradeId.FOUR_GUNS] = new Upgrade(
					id: UpgradeId.FOUR_GUNS,
					name: "Four Guns",
					cost: 20
				),
			};
			#endregion

			#endregion
		}
		#endregion

		#region Enums
		public enum UpgradeId
		{
			TWO_GUNS,
			THREE_GUNS,
			FOUR_GUNS,
		}
		#endregion
	}
}
