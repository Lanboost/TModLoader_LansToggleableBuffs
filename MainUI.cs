using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LansToggleableBuffs.ui;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace LansToggleableBuffs
{
    public class MainUI : ModSystem
    {
        internal Panel somethingUI;
        internal TooltipPanel tooltipPanel;
        public UserInterface somethingInterface;
        public UserInterface tooltipInterface;

        public static MainUI instance;

        public override void Load()
        {
            MainUI.instance = this;
            // this makes sure that the UI doesn't get opened on the server
            // the server can't see UI, can it? it's just a command prompt
            if (!Main.dedServ)
            {
                somethingInterface = new UserInterface();
                somethingUI = new Panel(somethingInterface);
                somethingUI.Initialize();
                somethingInterface.SetState(somethingUI);
            }
        }
        public override void UpdateUI(GameTime gameTime)
        {
            // it will only draw if the player is not on the main menu
            if (!Main.gameMenu
                && Panel.visible)
            {
                somethingInterface?.Update(gameTime);
            }
            else
            {
                somethingUI.needValidate = true;
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
    }
}
