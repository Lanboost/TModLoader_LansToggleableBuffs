using Terraria;
using static Terraria.ModLoader.ModContent;

namespace LansToggleableBuffs
{
	public class MoneyCostValue: CostValue
    {
        public int cost;

        public MoneyCostValue(int cost)
        {
            this.cost = cost;
        }

        public bool CheckBuy()
        {
            Main.LocalPlayer.CanAfford(cost);

            return GetInstance<Config>().Debug || Main.LocalPlayer.CanAfford(cost);
        }

        public void Buy()
        {
            if (!GetInstance<Config>().Debug)
            {
                Main.LocalPlayer.BuyItem(cost);
            }
        }

        public string UIInfo()
        {
            return "";
        }
    }
}
