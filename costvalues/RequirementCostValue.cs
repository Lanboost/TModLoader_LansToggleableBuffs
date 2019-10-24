using System;

namespace LansToggleableBuffs
{
	public class RequirementCostValue : CostValue
    {

        string helpstring;
        internal Func<bool> checkFunc;

        public RequirementCostValue(string helpstring, Func<bool> checkFunc)
        {
            this.helpstring = helpstring;
            this.checkFunc = checkFunc;
        }

        public void Buy()
        {
            
        }

        public bool CheckBuy()
        {
            return checkFunc();
        }

        public string UIInfo()
        {
            return helpstring;
        }
    }
}
