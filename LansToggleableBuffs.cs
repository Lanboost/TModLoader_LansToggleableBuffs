using LansToggleableBuffs.ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;

namespace LansToggleableBuffs
{

        class LansToggleableBuffs : Mod
	{


        public static LansToggleableBuffs instance;
        internal ModKeybind ShowUI;
        internal ModKeybind ToggleBuffs;

		public bool renderBuffs = true;

		public List<ModBuffValues> modBuffValues;

        public LansToggleableBuffs()
		{
            instance = this;
			modBuffValues = new List<ModBuffValues>();
			modBuffValues.Add(new ModBuffValues("Vanilla", VanilaBuffs.getVanilla()));
		}

        public override void Load()
        {
			ShowUI = KeybindLoader.RegisterKeybind(this, "Show UI", Keys.L.ToString());
			ToggleBuffs = KeybindLoader.RegisterKeybind(this, "Toggle buff rendering", Keys.P.ToString());

			IL.Terraria.Main.DrawInterface_Resources_Buffs += ModifyRenderBuffs;
		}

		public void ModifyRenderBuffs(ILContext iLContext)
		{
			InjectSkipOnBoolean(iLContext, ModifyRenderBuffsFunc);
		}

		// Stolen from my other project...
		public static void InjectSkipOnBoolean(ILContext il, Func<bool> action)
		{
			var c = new ILCursor(il);
			c.Emit(OpCodes.Call, action.GetMethodInfo());
			var after = c.DefineLabel();
			c.Emit(OpCodes.Brfalse_S, after);
			c.Emit(OpCodes.Ret);
			c.MarkLabel(after);
		}

        public static bool ModifyRenderBuffsFunc()
		{
			if(LansToggleableBuffs.instance.renderBuffs)
			{
				RenderBuffs.DrawInterface_Resources_Buffs();
			}

			return LansToggleableBuffs.instance.renderBuffs;
		}

		public override void PostSetupContent()
		{
			base.PostSetupContent();
			{
				var modBuffs = new Dictionary<string, List<BuffValue>>();

				for (int i = 0; i < ItemLoader.ItemCount; i++)
				{
					var mitem = ItemLoader.GetItem(i);
					if (mitem != null)
					{
						if (mitem.Item.buffType >= 1)
						{
							var buff = BuffLoader.GetBuff(mitem.Item.buffType);
							if (buff != null && !Main.lightPet[mitem.Item.buffType] && !Main.vanityPet[mitem.Item.buffType] && !mitem.Item.CountsAsClass(DamageClass.Summon))
							{
								if (!modBuffs.ContainsKey(mitem.Mod.Name))
								{
									modBuffs.Add(mitem.Mod.Name, new List<BuffValue>());
								}

								var bvalue = new BuffValue(
									false, 
									mitem.Item.buffType, 
									buff.DisplayName.GetDefault(), 
									buff.Description.GetDefault(), 
									mitem.Mod.Name, 
									new CostValue[] { 
										new ItemCostValue(mitem.Item.type, -1, mitem.DisplayName.GetDefault()) 
									}, 
									null, 
									true, 
									Main.debuff[mitem.Item.buffType]
								);

								modBuffs[mitem.Mod.Name].Add(bvalue);
							}
						}
					}
				}

				foreach (var v in modBuffs)
				{
					modBuffValues.Add(new ModBuffValues(v.Key, v.Value.ToArray()));
				}
			}

		}


		public int getBuffLength() {
			int c = 0;
			foreach(var v in modBuffValues)
			{
				c +=v.buffs.Length;
			}
			return c;
		}

		public BuffValue getBuff(int index)
		{
			int c = index;
			foreach (var v in modBuffValues)
			{
				if(c<v.buffs.Length)
				{
					return v.buffs[c];
				}

				c -= v.buffs.Length;
			}
			throw new Exception("Index out of range " + index);
		}


		public override void Unload()
        {
            instance = null;
        }


		// Messages:
		// string:"AddModBuffs" - string:Save/load Tag - BuffValue[]:buffValues
		public override object Call(params object[] args)
		{
			try
			{
				string message = args[0] as string;
				if (message == "AddModBuffs")
				{
					string savetag = args[1] as string;

					BuffValue[] buffs = args[2] as BuffValue[];

					modBuffValues.Add(new ModBuffValues(savetag, buffs));
					return "Success";
				}
				else
				{
					this.Logger.Warn($"LansToggleableBuffs Call Error: Unknown Message: {message}");
				}
			}
			catch (Exception e)
			{
                this.Logger.Warn($"LansToggleableBuffs Call Error: {e.StackTrace} {e.Message}");
			}
			return "Failure";
		}


		public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModMessageType msgType = (ModMessageType)reader.ReadByte();
            switch (msgType)
            {
                // This message sent by the server to initialize the Volcano Tremor on clients
                case ModMessageType.SyncPlayer:
                    {
                        byte playernumber = reader.ReadByte();

						LPlayer examplePlayer = Main.player[playernumber].GetModPlayer<LPlayer>();

						if(examplePlayer.boughtbuffsavail == null)
						{
							examplePlayer.boughtbuffsavail = new bool[LansToggleableBuffs.instance.getBuffLength()];
							examplePlayer.buffsavail = new bool[LansToggleableBuffs.instance.getBuffLength()];
						}

                        for (int i = 0; i < this.getBuffLength(); i++)
                        {
                            examplePlayer.boughtbuffsavail[i] = reader.ReadBoolean();
                            examplePlayer.buffsavail[i] = reader.ReadBoolean();
                        }
                        break;
                    }
                case ModMessageType.Change:
                    {
                        

                        byte playernumber = reader.ReadByte();


                        LPlayer examplePlayer = Main.player[playernumber].GetModPlayer<LPlayer>();


						if (examplePlayer.boughtbuffsavail == null)
						{
							examplePlayer.boughtbuffsavail = new bool[LansToggleableBuffs.instance.getBuffLength()];
							examplePlayer.buffsavail = new bool[LansToggleableBuffs.instance.getBuffLength()];
						}

						for (int i = 0; i < this.getBuffLength(); i++)
                        {
                            examplePlayer.boughtbuffsavail[i] = reader.ReadBoolean();
                            examplePlayer.buffsavail[i] = reader.ReadBoolean();
                        }

                        if (Main.netMode == NetmodeID.Server)
                        {

                            var packet = GetPacket();
                            packet.Write((byte)ModMessageType.Change);
                            packet.Write((byte)playernumber);
                            for (int i = 0; i < this.getBuffLength(); i++)
                            {
                                packet.Write(examplePlayer.boughtbuffsavail[i]);
                                packet.Write(examplePlayer.buffsavail[i]);
                            }
                            packet.Send(-1, playernumber);
                        }

                        break;
                    }
            }
        }



        internal enum ModMessageType : byte
        {
            SyncPlayer,
            Change,
        }
    }
}