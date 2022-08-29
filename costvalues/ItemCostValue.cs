using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace LansToggleableBuffs
{
	public class ItemCostValue: CostValue
    {
        public int itemid;
        public int count;
		public string itemname;
        Func<int> overrideCount;

        public ItemCostValue(int itemid, int count, string itemname = null, Func<int> overrideCount = null)
        {
            this.itemid = itemid;
            this.count = count;
			this.itemname = itemname;
			if (itemname == null)
			{
				this.itemname = "";
			}
            this.overrideCount = overrideCount;
        }

        public int GetCountCost()
        {
            if(overrideCount != null)
            {
                return overrideCount();
            }
            return count;
        }

        public bool CheckBuy()
        {
            int count = 0;
            for (int k = 0; k < Main.LocalPlayer.inventory.Length; k++)
            {
                if (Main.LocalPlayer.inventory[k].netID == itemid)
                {
                    count += Main.LocalPlayer.inventory[k].stack;
                }
            }

            return GetInstance<Config>().Debug || count >= GetCountCost();
        }

        public void Buy()
        {
            if (!GetInstance<Config>().Debug)
            {
                int count = GetCountCost();

                for (int k = 0; k < Main.LocalPlayer.inventory.Length; k++)
                {
                    if (Main.LocalPlayer.inventory[k].netID == itemid)
                    {
                        if (Main.LocalPlayer.inventory[k].stack <= count)
                        {
                            count -= Main.LocalPlayer.inventory[k].stack;
                            Main.LocalPlayer.inventory[k].TurnToAir();

                        }
                        else
                        {
                            Main.LocalPlayer.inventory[k].stack -= count;
                            break;
                        }
                    }
                }
            }
        }

        public string UIInfo()
        {
            return "";
        }

        public static int PotionCostCount()
        {
            return ModContent.GetInstance<Config>().PotionCount;
        }

        public static int ItemCostCount()
        {
            return ModContent.GetInstance<Config>().ItemCount;
        }
    }
}
