using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LansToggleableBuffs
{
	class ModBuffValues
	{
		public string saveTag;
		public BuffValue[] buffs;

		public ModBuffValues(string saveTag, BuffValue[] buffs)
		{
			this.saveTag = saveTag;
			this.buffs = buffs;
		}
	}
}
