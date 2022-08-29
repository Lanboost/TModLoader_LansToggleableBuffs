using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
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
		public int ItemCount;

		[Label("Potions required (TODO)")]
		[Tooltip("Number of potions required to buy")]
		[Range(1, 100)]
		[Increment(1)]
		[DrawTicks]
		[DefaultValue(30)]
		public int PotionCount;

		public override void OnChanged()
		{
			base.OnChanged();
			var ui = MainUI.instance?.somethingUI;
			if(ui != null)
			{
				ui.needValidate = true;

			}

			var buffLength = LansToggleableBuffs.instance?.getBuffLength();
			if (buffLength != null)
			{
				for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
				{
					var buff = LansToggleableBuffs.instance.getBuff(i);

					var buffId = buff.id;
					LPlayer mp = null;
                    var player = Main.LocalPlayer?.TryGetModPlayer<LPlayer>(out mp);
					
                
                
					if (mp != null)
					{
						if (buff.isDebuff && !ModContent.GetInstance<Config>().AllowDebuff)
						{
							if (Main.LocalPlayer.HasBuff(buffId))
							{
								Main.LocalPlayer.ClearBuff(buffId);
							}
						}
					}
				}
			}
        }
	}
}
