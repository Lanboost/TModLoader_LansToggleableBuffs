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

        UIImageButtonLabel[] ownedImages;
        UIHoverImageToggleButton[] toggleButtons;
        Texture2D buttonPlayTexture1;
        Texture2D buttonPlayTexture2;

		bool created = false;
        public override void OnInitialize()
        {

            
        }

		public void create()
		{
			created = true;
			TooltipPanel.Instance = new TooltipPanel();
			TooltipPanel.Instance.Init();


			// if you set this to true, it will show up in game
			//visible = false;

			panel = new DragableUIPanel(); //initialize the panel
										   // ignore these extra 0s
			panel.Left.Set(800, 0); //this makes the distance between the left of the screen and the left of the panel 500 pixels (somewhere by the middle)
			panel.Top.Set(100, 0); //this is the distance between the top of the screen and the top of the panel


			var buffSize = AutoBuff.instance.getBuffLength();



			if (buffSize > 20)
			{
				panel.Width.Set(20 * 40 + 20, 0);
			}
			else
			{
				panel.Width.Set(buffSize * 40 + 20, 0);
			}


			panel.Height.Set((buffSize / 20 + 1) * 130 + 20, 0);

			buttonPlayTexture1 = ModContent.GetTexture("AutoBuff/ui/checkbox");
			buttonPlayTexture2 = ModContent.GetTexture("AutoBuff/ui/checkboxunchecked");
			var unownedTexture = ModContent.GetTexture("AutoBuff/ui/unowned");

			ownedImages = new UIImageButtonLabel[buffSize];
			toggleButtons = new UIHoverImageToggleButton[buffSize];

			int top = 10;
			int left = 10;
			int leftstep = 40;
			for (int i = 0; i < buffSize; i++)
			{
				int lleft = left + leftstep * (i % 20);
				int ttop = top + (10 + 3 * leftstep) * (i / 20);

				//ugly haccck

				var buff = AutoBuff.instance.getBuff(i);

				buff.texture = Main.buffTexture[buff.id];

				int j = i;
				UIImage icon = new UIImage(buff.texture);
				icon.Left.Set(lleft, 0f);
				icon.Top.Set(ttop, 0f);
				icon.Width.Set(32, 0f);
				icon.Height.Set(32, 0f);
				panel.Append(icon);

				ownedImages[i] = new UIImageButtonLabel(unownedTexture, "Buy buff " + buff.name);
				ownedImages[i].Left.Set(lleft, 0f);
				ownedImages[i].Top.Set(ttop + 40, 0f);
				ownedImages[i].Width.Set(32, 0f);
				ownedImages[i].Height.Set(32, 0f);
				panel.Append(ownedImages[i]);
				ownedImages[i].OnClick += delegate (UIMouseEvent evt, UIElement listeningElement)
				{


					var tempBuff = AutoBuff.instance.getBuff(j);

					var mp = Main.player[Main.myPlayer].GetModPlayer<AutoBuffPlayer>();
					if (!mp.boughtbuffsavail[j])
					{
						bool canbuy = true;
						foreach (var v in tempBuff.cost)
						{
							if (!v.CheckBuy())
							{
								canbuy = false;
								break;
							}
						}

						if (canbuy)
						{
							foreach (var v in tempBuff.cost)
							{
								v.Buy();
							}
							mp.boughtbuffsavail[j] = true;
						}
						else
						{
							Main.NewText("You do not have enough items to buy this!", new Color(255, 0, 0));
						}
					}
				};


				toggleButtons[i] = new UIHoverImageToggleButton(buttonPlayTexture1, buttonPlayTexture2, "Disable buff " + buff.name, "Use buff " + buff.name);
				toggleButtons[i].Left.Set(lleft, 0f);
				toggleButtons[i].Top.Set(ttop + 40, 0f);
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
			if(!created)
			{
				create();
			}
            TooltipPanel.Instance.Update(this);

            for (int i = 0; i < AutoBuff.instance.getBuffLength(); i++)
            {
				var buff = AutoBuff.instance.getBuff(i);
                if(toggleButtons[i].IsMouseHovering)
                {
                    TooltipPanel.Instance.SetInfo(buff.id, buff.name, buff.effect, buff.texture);
                }

                if (ownedImages[i].IsMouseHovering)
                {
                    TooltipPanel.Instance.SetInfo(buff.cost, buff.id, buff.name, buff.effect, buff.texture);
                }
            }

            base.Update(gameTime);

            if (visible)
            {
                
                var mp = Main.player[Main.myPlayer].GetModPlayer<AutoBuffPlayer>();

                for (int i = 0; i < AutoBuff.instance.getBuffLength(); i++)
                {
                    if (mp.boughtbuffsavail[i]) {
                        panel.Append(toggleButtons[i]);
                        ownedImages[i].Remove();
                    }
                    else
                    {
                        toggleButtons[i].Remove();
						panel.Append(ownedImages[i]);
					}
                }

                for (int i = 0; i < AutoBuff.instance.getBuffLength(); i++)
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
