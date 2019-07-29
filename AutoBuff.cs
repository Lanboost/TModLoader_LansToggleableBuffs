using AutoBuff.Items;
using AutoBuff.ui;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace AutoBuff
{
	class AutoBuff : Mod
	{

        public static bool DEBUG = true;


        public static AutoBuff instance;
        internal ModHotKey ShowUI;

        public AutoBuff()
		{
            instance = this;

        }

        internal Panel somethingUI;
        public UserInterface somethingInterface;

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

            ShowUI = RegisterHotKey("Show UI", "L");

            
            

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
            layers.Add(new LegacyGameInterfaceLayer("AutoBuff", DrawSomethingUI, InterfaceScaleType.UI));
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


        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            AutoBuffModMessageType msgType = (AutoBuffModMessageType)reader.ReadByte();
            switch (msgType)
            {
                // This message sent by the server to initialize the Volcano Tremor on clients
                case AutoBuffModMessageType.AutoBuffSyncPlayer:
                    {
                        byte playernumber = reader.ReadByte();
                        AutoBuffPlayer examplePlayer = Main.player[playernumber].GetModPlayer<AutoBuffPlayer>();

                        for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
                        {
                            examplePlayer.boughtbuffsavail[i] = reader.ReadBoolean();
                            examplePlayer.buffsavail[i] = reader.ReadBoolean();
                        }
                        break;
                    }
                case AutoBuffModMessageType.AutoBuffChange:
                    {
                        byte playernumber = reader.ReadByte();
                        AutoBuffPlayer examplePlayer = Main.player[playernumber].GetModPlayer<AutoBuffPlayer>();

                        for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
                        {
                            examplePlayer.boughtbuffsavail[i] = reader.ReadBoolean();
                            examplePlayer.buffsavail[i] = reader.ReadBoolean();
                        }

                        if (Main.netMode == NetmodeID.Server)
                        {
                            var packet = GetPacket();
                            packet.Write((byte)AutoBuffModMessageType.AutoBuffChange);
                            packet.Write((byte)playernumber);
                            for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
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



        internal enum AutoBuffModMessageType : byte
        {
            AutoBuffSyncPlayer,
            AutoBuffChange,
        }
    }
}