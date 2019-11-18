using LansToggleableBuffs.ui;
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
using static LansToggleableBuffs.LansToggleableBuffs;

namespace LansToggleableBuffs
{
    class LPlayer: ModPlayer
    {

		public bool[] boughtbuffsavail;
		public bool[] buffsavail;

        public override bool CloneNewInstances => false;


		public LPlayer()
		{
			var size = LansToggleableBuffs.instance.getBuffLength();
			boughtbuffsavail = new bool[size];
			buffsavail = new bool[size];
		}

		public override void clientClone(ModPlayer clientClone)
        {
            LPlayer clone = clientClone as LPlayer;

			var size = LansToggleableBuffs.instance.getBuffLength();
			for (int i = 0; i < size; i++)
            {
                clone.boughtbuffsavail[i] = boughtbuffsavail[i];
                clone.buffsavail[i] = buffsavail[i];
            }
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)ModMessageType.SyncPlayer);
            packet.Write((byte)player.whoAmI);
            for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
            {
                packet.Write(boughtbuffsavail[i]);
                packet.Write(buffsavail[i]);
            }
            packet.Send(toWho, fromWho);
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            // Here we would sync something like an RPG stat whenever the player changes it.
            LPlayer clone = clientPlayer as LPlayer;
            bool send = false;

            for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
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
                packet.Write((byte)ModMessageType.Change);
                packet.Write((byte)player.whoAmI);
                for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
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
			tagComp.Add("version", 1);

			List<TagCompound> modTags = new List<TagCompound>();

			foreach (var v in LansToggleableBuffs.instance.modBuffValues)
			{
				var modTag = new TagCompound();
				modTag.Add("modName", v.saveTag);


				List<TagCompound> buffTags = new List<TagCompound>();

				for (int i = 0; i < v.buffs.Length; i++)
				{

					var buffTag = new TagCompound();
					buffTag.Add("name", v.buffs[i].name);
					buffTag.Add("bought", boughtbuffsavail[buffIndex]);
					buffTag.Add("using", buffsavail[buffIndex]);
					buffTags.Add(buffTag);


					buffIndex++;
				}

				modTag.Add("buffs", buffTags);
				modTags.Add(modTag);
			}

			tagComp.Add("mods", modTags);



			return tagComp;
        }

        public override void Load(TagCompound tag)
        {
			int buffIndex = 0;

			var size = LansToggleableBuffs.instance.getBuffLength();

			int version = 0;

			for (int i = 0; i < boughtbuffsavail.Length; i++)
			{
				boughtbuffsavail[i] = false;
				buffsavail[i] = false;
			}

			try
			{
				version = tag.GetAsInt("version");
			}
			catch
			{

			}
			try
			{
				if (version == 0)
				{

					foreach (var v in LansToggleableBuffs.instance.oldSaveModBuffValues)
					{

						var tempTag = tag.Get<TagCompound>(v.saveTag);
						if (tempTag != null)
						{

							var b1 = tempTag.GetByteArray("boughtbuffsavail");
							var b2 = tempTag.GetByteArray("buffsavail");


							if (b1.Length == v.buffs.Length && b2.Length == v.buffs.Length)
							{


								for (int i = 0; i < v.buffs.Length; i++)
								{
									bool found = false;
									int buffSaveIndex = 0;
									foreach (var newModBuffs in LansToggleableBuffs.instance.modBuffValues)
									{

										if (newModBuffs.saveTag == v.saveTag)
										{
											for (int j = 0; j < newModBuffs.buffs.Length; j++)
											{
												if (newModBuffs.buffs[j].name == v.buffs[i].name)
												{
													boughtbuffsavail[buffSaveIndex] = b1[i] == 1?true:false;
													buffsavail[buffSaveIndex] = b2[i] == 1?true:false;;

													found = true;
												}
												else
												{
													buffSaveIndex++;
												}
											}
										}
										else
										{
											buffSaveIndex += newModBuffs.buffs.Length;
										}

									}

									if (!found)
									{
										throw new Exception("Mod buff was not found");
									}

								}

							}
							else
							{
								throw new Exception("Mod length was not the same");
							}

						}
						else
						{
							throw new Exception("Mod without buff save");
						}

					}

				}
				else
				{

					var mods = tag.GetList<TagCompound>("mods");

					foreach (var value in mods)
					{
						buffIndex = 0;

						foreach (var v in LansToggleableBuffs.instance.modBuffValues)
						{
							if (value.GetString("modName") == v.saveTag)
							{
								foreach (var buffValue in value.GetList<TagCompound>("buffs"))
								{
									int buffIndexOffset = buffIndex;
									foreach (var buff in v.buffs)
									{
										if (buff.name == buffValue.GetString("name"))
										{
											boughtbuffsavail[buffIndexOffset] = buffValue.GetBool("bought");
											buffsavail[buffIndexOffset] = buffValue.GetBool("using");

										}
										else
										{
											buffIndexOffset++;
										}
									}
								}
							}
							else
							{
								buffIndex += v.buffs.Length;
							}
						}
					}
				}
			}
			catch
			{
			}
		}

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if(Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Panel.visible = false;
            }



            if (((LansToggleableBuffs)mod).ShowUI.JustPressed)
            {
                Panel.visible = !Panel.visible;
            }
        }


        public override void PreUpdate()
        {

			var mp = player.GetModPlayer<LPlayer>();
		
			for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
			{
				var buffId = LansToggleableBuffs.instance.getBuff(i).id;
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
