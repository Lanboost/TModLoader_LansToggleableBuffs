using AutoBuff.Items;
using ExampleMod.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace AutoBuff.ui
{
    class Panel : UIState
    {
        public static bool visible = false;
        public DragableUIPanel panel;

        UIImageButton[] ownedImages;
        UIHoverImageToggleButton[] toggleButtons;
        Texture2D buttonPlayTexture1;
        Texture2D buttonPlayTexture2;

        public override void OnInitialize()
        {
            // if you set this to true, it will show up in game
            visible = false;

            panel = new DragableUIPanel(); //initialize the panel
            // ignore these extra 0s
            panel.Left.Set(800, 0); //this makes the distance between the left of the screen and the left of the panel 500 pixels (somewhere by the middle)
            panel.Top.Set(100, 0); //this is the distance between the top of the screen and the top of the panel

            if (AutoBuffBuffs.buffs.Length > 20)
            {
                panel.Width.Set(20 * 40 + 20, 0);
            }
            else
            {
                panel.Width.Set(AutoBuffBuffs.buffs.Length * 40 + 20, 0);
            }


            panel.Height.Set((AutoBuffBuffs.buffs.Length / 20+1)*130+20, 0);

            buttonPlayTexture1 = ModContent.GetTexture("AutoBuff/ui/checkbox");
            buttonPlayTexture2 = ModContent.GetTexture("AutoBuff/ui/checkboxunchecked");

            ownedImages = new UIImageButton[AutoBuffBuffs.buffs.Length];
            toggleButtons = new UIHoverImageToggleButton[AutoBuffBuffs.buffs.Length];

            int top = 10;
            int left = 10;
            int leftstep = 40;
            for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
            {
                int lleft = left + leftstep * (i % 20);
                int ttop = top + (10+3 * leftstep) * (i / 20);

                int j = i;
                UIImage icon = new UIImage(Main.buffTexture[AutoBuffBuffs.buffs[i].id]);
                icon.Left.Set(lleft, 0f);
                icon.Top.Set(ttop, 0f);
                icon.Width.Set(32, 0f);
                icon.Height.Set(32, 0f);
                panel.Append(icon);

                ownedImages[i] = new UIImageButton(buttonPlayTexture2);
                ownedImages[i].Left.Set(lleft, 0f);
                ownedImages[i].Top.Set(ttop + 40, 0f);
                ownedImages[i].Width.Set(32, 0f);
                ownedImages[i].Height.Set(32, 0f);
                panel.Append(ownedImages[i]);
                ownedImages[i].OnClick += delegate (UIMouseEvent evt, UIElement listeningElement)
                {
                    var mp = Main.player[Main.myPlayer].GetModPlayer<AutoBuffPlayer>();
                    if (!mp.boughtbuffsavail[j])
                    {
                        int count = 0;
                        for(int k = 0; k< Main.player[Main.myPlayer].inventory.Length; k++)
                        {
                            if(Main.player[Main.myPlayer].inventory[k].netID == AutoBuffBuffs.buffs[j].itemid)
                            {
                                count += Main.player[Main.myPlayer].inventory[k].stack;
                            }
                        }

                        if (AutoBuff.DEBUG || count >= AutoBuffBuffs.buffs[j].count)
                        {
                            if (!AutoBuff.DEBUG)
                            {
                                count = AutoBuffBuffs.buffs[j].count;

                                for (int k = 0; k < Main.player[Main.myPlayer].inventory.Length; k++)
                                {
                                    if (Main.player[Main.myPlayer].inventory[k].netID == AutoBuffBuffs.buffs[j].itemid)
                                    {
                                        if (Main.player[Main.myPlayer].inventory[k].stack <= count)
                                        {
                                            count -= Main.player[Main.myPlayer].inventory[k].stack;
                                            Main.player[Main.myPlayer].inventory[k].TurnToAir();

                                        }
                                        else
                                        {
                                            Main.player[Main.myPlayer].inventory[k].stack -= count;
                                            break;
                                        }
                                    }
                                }
                            }

                            mp.boughtbuffsavail[j] = true;
                        }
                        else
                        {
                            Main.NewText("You do not have enough items to buy this!", new Color(255, 0,0));
                        }
                    }
                };


                toggleButtons[i] = new UIHoverImageToggleButton(buttonPlayTexture1, buttonPlayTexture2, "Use buff " + AutoBuffBuffs.buffs[i].name);
                toggleButtons[i].Left.Set(lleft, 0f);
                toggleButtons[i].Top.Set(ttop+80, 0f);
                toggleButtons[i].Width.Set(32, 0f);
                toggleButtons[i].Height.Set(32, 0f);



                panel.Append(toggleButtons[i]);

                toggleButtons[i].OnChecked += delegate (bool val)
                {
                    Main.player[Main.myPlayer].GetModPlayer<AutoBuffPlayer>().buffsavail[j] = val;
                };
            }

            Append(panel); //appends the panel to the UIState
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (visible)
            {
                var mp = Main.player[Main.myPlayer].GetModPlayer<AutoBuffPlayer>();

                for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
                {
                    if (mp.boughtbuffsavail[i]) {
                        ownedImages[i].SetImage(buttonPlayTexture1);
                    }
                    else
                    {
                        ownedImages[i].SetImage(buttonPlayTexture2);
                    }
                }

                for (int i = 0; i < AutoBuffBuffs.buffs.Length; i++)
                {
                    toggleButtons[i].IsChecked = mp.buffsavail[i];
                    if (mp.buffsavail[i])
                    {
                        toggleButtons[i].SetImage(buttonPlayTexture1);
                    }
                    else
                    {
                        toggleButtons[i].SetImage(buttonPlayTexture2);
                    }
                }
            }
        }
    }
}
