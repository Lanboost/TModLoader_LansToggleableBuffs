using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace LansToggleableBuffs
{
	class ActionCommands : ModCommand
	{
		public override bool Autoload(ref string name)
		{
			return true;
		}

		public override string Description => "Resets all unlocked buffs for this character";

		public override string Command => "lanstoggleablebuffs";

		public override CommandType Type => CommandType.Chat;

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			
			for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
			{
				Main.LocalPlayer.GetModPlayer<LPlayer>().boughtbuffsavail[i] = false;
			}

			LansToggleableBuffs.instance.somethingUI.needValidate = true;
			for (int i = 0; i < Main.LocalPlayer.buffTime.Length; i++)
			{
				Main.LocalPlayer.buffTime[i] = 0;
			}
			
			Main.NewText("LansToggleableBuffs: Buffs reseted.");
			Main.NewText($"netmode is:{Main.netMode}");
		}
	}
}
