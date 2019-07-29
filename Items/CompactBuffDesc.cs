using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AutoBuff.Items
{
    class CompactBuffDesc: GlobalBuff
    {

        public override void ModifyBuffTip(int type, ref string tip, ref int rare)
        {

            rare = -10;

            if (type == mod.GetBuff<CompactBuff>().Type)
            {

                string desc = "";

                var mp = Main.player[Main.myPlayer].GetModPlayer<AutoBuffPlayer>();
                for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
                {
                    if (mp.boughtbuffsavail[i] && mp.buffsavail[i])
                    {
                        if (!AutoBuffBuffs.buffs[i].useMainBuff)
                        {
                            desc += AutoBuffBuffs.buffs[i].name + "\r\n";
                        }
                        else
                        {
                            desc += "***"+AutoBuffBuffs.buffs[i].name + "***\r\n";
                        }
                    }
                }
                tip = desc;
            }
        }
    }
}
