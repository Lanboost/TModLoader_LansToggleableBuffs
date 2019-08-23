﻿using AutoBuff.ui;
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

		public bool[] boughtbuffsavail;
		public bool[] buffsavail;

        public override bool CloneNewInstances => false;


		public AutoBuffPlayer()
		{
			var size = AutoBuff.instance.getBuffLength();
			boughtbuffsavail = new bool[size];
			buffsavail = new bool[size];
		}

		public override void clientClone(ModPlayer clientClone)
        {
            AutoBuffPlayer clone = clientClone as AutoBuffPlayer;

			var size = AutoBuff.instance.getBuffLength();
			for (int i = 0; i < size; i++)
            {
                clone.boughtbuffsavail[i] = boughtbuffsavail[i];
                clone.buffsavail[i] = buffsavail[i];
            }
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)AutoBuffModMessageType.AutoBuffSyncPlayer);
            packet.Write((byte)player.whoAmI);
            for (int i = 0; i < AutoBuff.instance.getBuffLength(); i++)
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

            for (int i = 0; i < AutoBuff.instance.getBuffLength(); i++)
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

            if (send)
            {
                var packet = mod.GetPacket();
                packet.Write((byte)AutoBuffModMessageType.AutoBuffChange);
                packet.Write((byte)player.whoAmI);
                for (int i = 0; i < AutoBuff.instance.getBuffLength(); i++)
                {
                    packet.Write(boughtbuffsavail[i]);
                    packet.Write(buffsavail[i]);
                }
                packet.Send();
            }
        }

        public override TagCompound Save()
        {
			int buffIndex = 0;

			var tagComp = new TagCompound();
			foreach(var v in AutoBuff.instance.modBuffValues)
			{
				byte[] b1 = new byte[v.buffs.Length];
				byte[] b2 = new byte[v.buffs.Length];

				for (int i = 0; i < v.buffs.Length; i++)
				{
					b1[i] = boughtbuffsavail[buffIndex] ? (byte)1 : (byte)0;
					b2[i] = buffsavail[buffIndex] ? (byte)1 : (byte)0;

					buffIndex++;
				}

				tagComp.Add(v.saveTag, new TagCompound {
					{"boughtbuffsavail", b1},
					{"buffsavail", b2},
				});
			}



			return tagComp;
        }

        public override void Load(TagCompound tag)
        {
			int buffIndex = 0;

			var size = AutoBuff.instance.getBuffLength();

			var tagComp = new TagCompound();
			foreach (var v in AutoBuff.instance.modBuffValues)
			{
				try {
					var tempTag = tag.Get<TagCompound>(v.saveTag);
					if (tempTag != null)
					{

						var b1 = tempTag.GetByteArray("boughtbuffsavail");
						var b2 = tempTag.GetByteArray("buffsavail");


						if (b1.Length == v.buffs.Length && b2.Length == v.buffs.Length)
						{

							for (int i = 0; i < v.buffs.Length; i++)
							{
								boughtbuffsavail[buffIndex] = b1[i] == 1 ? true : false;
								buffsavail[buffIndex] = b2[i] == 1 ? true : false;
								buffIndex++;
							}
						}
						else
						{
							for (int i = 0; i < v.buffs.Length; i++)
							{
								boughtbuffsavail[buffIndex] = false;
								buffsavail[buffIndex] = false;
								buffIndex++;
							}
						}

					}
					else
					{
						for (int i = 0; i < v.buffs.Length; i++)
						{
							boughtbuffsavail[buffIndex] = false;
							buffsavail[buffIndex] = false;
							buffIndex++;
						}
					}
				}
				catch
				{
					for (int i = 0; i < v.buffs.Length; i++)
					{
						boughtbuffsavail[buffIndex] = false;
						buffsavail[buffIndex] = false;
						buffIndex++;
					}
				}
			}
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if(Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Panel.visible = false;
            }



            if (((AutoBuff)mod).ShowUI.JustPressed)
            {
                Panel.visible = !Panel.visible;
            }
        }


        public override void PreUpdate()
        {

			var mp = player.GetModPlayer<AutoBuffPlayer>();
		
			for (int i = 0; i < AutoBuff.instance.getBuffLength(); i++)
			{
				var buffId = AutoBuff.instance.getBuff(i).id;
				if (mp.boughtbuffsavail[i])
				{
					if (mp.buffsavail[i])
					{
						if (!player.HasBuff(buffId))
						{
							player.AddBuff(buffId, int.MaxValue);
						}
					}
					else
					{
						if (player.HasBuff(buffId))
						{
							player.ClearBuff(buffId);
						}
					}
				}
			}
        }
    }
}