using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LansToggleableBuffs
{
	class RenderBuffs
	{

		static int DrawBuffIcon(int drawBuffText, int i, int x, int y)
		{
			return Main.DrawBuffIcon(drawBuffText, i, x, y);
			
		}


		static bool BuffIsToggable(int id)
		{
			var mp = Main.LocalPlayer.GetModPlayer<LPlayer>();
			for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
			{
				var buffId = LansToggleableBuffs.instance.getBuff(i).id;
				if(buffId == id)
				{
					if (mp.boughtbuffsavail[i])
					{
						if (mp.buffsavail[i])
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Decompiled code
		public static void DrawInterface_Resources_Buffs()
		{
			Main.recBigList = false;
			int num = -1;
			int num2 = 11;

			int offsetCount = 0;

			for (int i = 0; i < Player.MaxBuffs; i++)
			{
				var buffType = Main.player[Main.myPlayer].buffType[i];

				
				if (buffType > 0)
				{
					if (!BuffIsToggable(buffType))
					{

						int b = buffType;
						int x = 32 + offsetCount * 38;
						int num3 = 76;
						if (offsetCount >= num2)
						{
							x = 32 + Math.Abs(offsetCount % 11) * 38;
							num3 += 50 * (offsetCount / 11);
						}

						num = DrawBuffIcon(num, i, x, num3);

						offsetCount++;
					}
				}
				else
				{
					Main.buffAlpha[i] = 0.4f;
				}
			}

			if (num < 0)
				return;

			int num4 = Main.player[Main.myPlayer].buffType[num];
			if (num4 > 0)
			{
				Main.buffString = Lang.GetBuffDescription(num4);
				if (num4 == 26 && Main.expertMode)
					Main.buffString = Language.GetTextValue("BuffDescription.WellFed_Expert");

				if (num4 == 147)
					Main.bannerMouseOver = true;

				if (num4 == 94)
				{
					int num5 = (int)(Main.player[Main.myPlayer].manaSickReduction * 100f) + 1;
					Main.buffString = Main.buffString + num5 + "%";
				}

				int rare = Main.meleeBuff[num4] ? -10 : 0;
				BuffLoader.ModifyBuffTip(num4, ref Main.buffString, ref rare);
				Main.instance.MouseTextHackZoom(Lang.GetBuffName(num4), rare);
			}
		}
	}
}
