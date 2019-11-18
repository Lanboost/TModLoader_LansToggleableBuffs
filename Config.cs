using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace LansToggleableBuffs
{
	class Config : ModConfig
	{
		// You MUST specify a ConfigScope.
		public override ConfigScope Mode => ConfigScope.ServerSide;


		[Label("God Mode (debug mode)")]
		[Tooltip("All buffs are free!!")]
		[DefaultValue(false)]
		public bool Debug;

		[Label("Allow debuffs")]
		[Tooltip("Why not give yourself some debuffs?!")]
		[DefaultValue(true)]
		public bool AllowDebuff;

		[Label("Items required (TODO)")]
		[Tooltip("Ammo box, summoning table etc")]
		[Range(1, 100)]
		[Increment(1)]
		[DrawTicks]
		[DefaultValue(1)]
		public bool ItemCount;

		[Label("Potions required (TODO)")]
		[Tooltip("Number of potions required to buy")]
		[Range(1, 100)]
		[Increment(1)]
		[DrawTicks]
		[DefaultValue(30)]
		public int PotionCount;
	}
}
