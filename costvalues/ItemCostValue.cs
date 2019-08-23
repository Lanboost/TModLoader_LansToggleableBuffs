using Terraria;

namespace AutoBuff
{
	public class ItemCostValue: CostValue
    {
        public int itemid;
        public int count;
		public string itemname;

        public ItemCostValue(int itemid, int count, string itemname = null)
        {
            this.itemid = itemid;
            this.count = count;
			this.itemname = itemname;
			if (itemname == null)
			{
				this.itemname = "";
			}
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

            return AutoBuff.DEBUG || count >= this.count;
        }

        public void Buy()
        {
            if (!AutoBuff.DEBUG)
            {
                int count = this.count;

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
    }
}
