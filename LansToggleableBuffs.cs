using LansToggleableBuffs.ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace LansToggleableBuffs
{
	class LansToggleableBuffs : Mod
	{

        public static bool DEBUG = false;


        public static LansToggleableBuffs instance;
        internal ModHotKey ShowUI;


		public List<ModBuffValues> modBuffValues;

        public LansToggleableBuffs()
		{
            instance = this;

			modBuffValues = new List<ModBuffValues>();
			modBuffValues.Add(new ModBuffValues("Vanilla", VanilaBuffs.getVanilla()));
		}

        internal Panel somethingUI;
        internal TooltipPanel tooltipPanel;
        public UserInterface somethingInterface;
        public UserInterface tooltipInterface;

        public override void Load()
        {
			// this makes sure that the UI doesn't get opened on the server
			// the server can't see UI, can it? it's just a command prompt
			if (!Main.dedServ)
			{
				somethingUI = new Panel();
				somethingUI.Initialize();
				somethingInterface = new UserInterface();
				somethingInterface.SetState(somethingUI);
			}


			ShowUI = RegisterHotKey("Show UI", Keys.L.ToString());

			
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

        public override void UpdateUI(GameTime gameTime)
        {
            // it will only draw if the player is not on the main menu
            if (!Main.gameMenu
                && Panel.visible)
            {
                somethingInterface?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("LansToggleableBuffs", DrawSomethingUI, InterfaceScaleType.UI));
            }
        }

        private bool DrawSomethingUI()
        {
            // it will only draw if the player is not on the main menu
            if (!Main.gameMenu
                && Panel.visible)
            {
                somethingInterface.Draw(Main.spriteBatch, new GameTime());
            }
            return true;
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
					ErrorLogger.Log("LansToggleableBuffs Call Error: Unknown Message: " + message);
				}
			}
			catch (Exception e)
			{
				ErrorLogger.Log("LansToggleableBuffs Call Error: " + e.StackTrace + e.Message);
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