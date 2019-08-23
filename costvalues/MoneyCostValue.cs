using Terraria;

namespace AutoBuff
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

            return AutoBuff.DEBUG || Main.LocalPlayer.CanBuyItem(cost);
        }

        public void Buy()
        {
            if (!AutoBuff.DEBUG)
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
