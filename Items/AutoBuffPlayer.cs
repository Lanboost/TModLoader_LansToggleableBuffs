using AutoBuff.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static AutoBuff.AutoBuff;

namespace AutoBuff.Items
{
    class AutoBuffPlayer: ModPlayer
    {
        public bool[] boughtbuffsavail = new bool[AutoBuffBuffs.buffs.Length];
        public bool[] buffsavail = new bool[AutoBuffBuffs.buffs.Length];


        public override void clientClone(ModPlayer clientClone)
        {
            AutoBuffPlayer clone = clientClone as AutoBuffPlayer;
            // Here we would make a backup clone of values that are only correct on the local players Player instance.
            // Some examples would be RPG stats from a GUI, Hotkey states, and Extra Item Slots
            /*clone.boughtbuffsavail = new bool[AutoBuffBuffs.buffs.Length];
            clone.buffsavail = new bool[AutoBuffBuffs.buffs.Length];
            for(int i=0; i< AutoBuffBuffs.buffs.Length; i++)
            {
                clone.boughtbuffsavail[i] = boughtbuffsavail[i];
                clone.buffsavail[i] = buffsavail[i];
            }*/
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)AutoBuffModMessageType.AutoBuffSyncPlayer);
            packet.Write((byte)player.whoAmI);
            for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
            {
                packet.Write(boughtbuffsavail[i]);
                packet.Write(buffsavail[i]);
            }
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            // Here we would sync something like an RPG stat whenever the player changes it.
            AutoBuffPlayer clone = clientPlayer as AutoBuffPlayer;
            bool send = false;

            for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
            {
                if (clone.boughtbuffsavail[i] != boughtbuffsavail[i])
                {
                    send = true;
                    break;
                }
                if (clone.buffsavail[i] != buffsavail[i])
                {
                    send = true;
                    break;
                }
            }

            if(send)
            {
                var packet = mod.GetPacket();
                packet.Write((byte)AutoBuffModMessageType.AutoBuffChange);
                packet.Write((byte)player.whoAmI);
                for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
                {
                    packet.Write(boughtbuffsavail[i]);
                    packet.Write(buffsavail[i]);
                }
                packet.Send();
            }
        }

        public override TagCompound Save()
        {

            byte[] b1 = new byte[AutoBuffBuffs.buffs.Length];
            byte[] b2 = new byte[AutoBuffBuffs.buffs.Length];

            for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
            {
                b1[i] = boughtbuffsavail[i] ? (byte)1 : (byte)0;
                b2[i] = buffsavail[i] ? (byte)1 : (byte)0;
            }

            return new TagCompound {
				// {"somethingelse", somethingelse}, // To save more data, add additional lines
				{"boughtbuffsavail", b1},
                {"buffsavail", b2},
            };
        }

        public override void Load(TagCompound tag)
        {
            var b1 = tag.GetByteArray("boughtbuffsavail");
            var b2 = tag.GetByteArray("buffsavail");
            if (b1.Length == AutoBuffBuffs.buffs.Length)
            {
                for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
                {
                    boughtbuffsavail[i] = b1[i] == 1 ? true : false;
                    buffsavail[i] = b2[i] == 1 ? true : false;
                }
            }
            else
            {
                for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
                {
                    boughtbuffsavail[i] = false;
                    buffsavail[i] = false;
                }
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (((AutoBuff)mod).ShowUI.JustPressed)
            {
                Panel.visible = !Panel.visible;
            }
        }


        public override void PreUpdate()
        {
            if(!this.player.HasBuff(mod.BuffType("CompactBuff")))
            {
                this.player.AddBuff(mod.BuffType("CompactBuff"), int.MaxValue, true);
            }

            var mp = Main.player[Main.myPlayer].GetModPlayer<AutoBuffPlayer>();
            for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
            {
                if (mp.boughtbuffsavail[i] && mp.buffsavail[i])
                {
                    if (AutoBuffBuffs.buffs[i].useMainBuff)
                    {
                        Main.player[Main.myPlayer].AddBuff(AutoBuffBuffs.buffs[i].id, int.MaxValue, true);
                    }
                }
            }
        }
    }
}
