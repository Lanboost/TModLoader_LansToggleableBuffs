using Terraria;

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
            Main.LocalPlayer.CanBuyItem(cost);

            return LansToggleableBuffs.DEBUG || Main.LocalPlayer.CanBuyItem(cost);
        }

        public void Buy()
        {
            if (!LansToggleableBuffs.DEBUG)
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
