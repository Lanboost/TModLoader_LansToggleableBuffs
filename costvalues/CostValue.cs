namespace LansToggleableBuffs
{
	public interface CostValue
    {
        bool CheckBuy();
        void Buy();
        string UIInfo();
    }
}
