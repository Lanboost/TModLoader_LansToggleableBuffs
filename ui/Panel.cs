using ExampleMod.UI;
using LansToggleableBuffs.ui.elements;
using LansToggleableBuffs.ui.layout;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace LansToggleableBuffs.ui
{
	class Panel : UIState
    {
        public static bool visible = false;
		public WrapperScreen Screen;
        public WrapperComponent panel;
        public WrapperComponent contentPanel;



        Asset<Texture2D> buttonPlayTexture1;
        Asset<Texture2D> buttonPlayTexture2;

		bool created = false;
        public override void OnInitialize()
        {

            
        }

		public bool needValidate = false;

		public void create()
		{
			created = true;
			TooltipPanel.Instance = new TooltipPanel();
			TooltipPanel.Instance.Init();

			Screen = new WrapperScreen(this);

            // if you set this to true, it will show up in game
            //visible = false;

            buttonPlayTexture1 = ModContent.Request<Texture2D>("LansToggleableBuffs/ui/checkbox");
			buttonPlayTexture2 = ModContent.Request<Texture2D>("LansToggleableBuffs/ui/checkboxunchecked");


            contentPanel = UIFactory.CreatePanel("Content panel");
            contentPanel.SetAnchor(components.AnchorPosition.ExpandToParent);
            contentPanel.SetMargins(0, 0, 0, 0);

            var contentPanelLayout = new LayoutFlow(new bool[] { true, true }, new bool[] { false, false }, LayoutFlowType.Vertical, 10, 10, 10, 10, 10);

            contentPanel.SetLayout(contentPanelLayout);

            

            panel = UIFactory.CreateScrollPanel("Main Scroll", contentPanel);
			panel.SetAnchor(components.AnchorPosition.Center);
			panel.SetMargins(-250, -150, -250, -150);

            panel.Add(contentPanel);

            Screen.Add(panel);


            Main.OnResolutionChanged += delegate (Vector2 newSize)
			{
				needValidate = true;

            };
		}


		public void Revalidate() {
			needValidate = false;
			var buffSize = LansToggleableBuffs.instance.getBuffLength();
			
			var unownedTexture = ModContent.Request<Texture2D>("LansToggleableBuffs/ui/unowned");
			var mp = Main.player[Main.myPlayer].GetModPlayer<LPlayer>();
            contentPanel.RemoveChildren();



            int buffIndex = 0;
			foreach (var modBuffValues in LansToggleableBuffs.instance.modBuffValues)
			{
				var modlabel = UIFactory.CreateText("Mod label", $"{modBuffValues.saveTag}");
				((UIText)modlabel.element).TextColor = new Color(232, 181, 16);
                contentPanel.Add(modlabel);

                var modbuffgridpanel = UIFactory.CreatePanel("Mod buff grid");
                var modbuffgridpanelLayout = new LayoutGrid(10, new bool[] { true, true }, new bool[] { false, false }, LayoutGridType.Columns, 0,0,0,0,10);
                modbuffgridpanel.SetLayout(modbuffgridpanelLayout);

				var atleastOneBuffAdded = false;

                //populate modbuffgridpanel

                foreach (var buffValue in modBuffValues.buffs)
				{
					var currentBuffIndex = buffIndex;
					buffIndex += 1;

					if(buffValue.isDebuff && !ModContent.GetInstance<Config>().AllowDebuff)
					{
						continue;
					}
					atleastOneBuffAdded = true;


                    var buffpanel = UIFactory.CreatePanel("A buff panel");
                    var buffpanelLayout = new LayoutFlow(new bool[] { true, true }, new bool[] { false, false }, LayoutFlowType.Vertical, 0, 0, 0, 0, 10);
					buffpanel.SetLayout(buffpanelLayout);
					modbuffgridpanel.Add(buffpanel);

                    var buff = LansToggleableBuffs.instance.getBuff(currentBuffIndex);

                    buff.texture = TextureAssets.Buff[buff.id];

					{
						var icon = UIFactory.CreateImage("Buff image", buff.texture);
						buffpanel.Add(icon);
						
						if (!mp.boughtbuffsavail[currentBuffIndex])
						{
							var ownedImages = UIFactory.CreateImageButtonLabel(
								"Buy buff button", 
								unownedTexture, 
								$"Buy buff {buff.name}", 
								delegate (UIMouseEvent evt, UIElement listeningElement)
								{
									var tempBuff = LansToggleableBuffs.instance.getBuff(currentBuffIndex);

									if (!mp.boughtbuffsavail[currentBuffIndex])
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
											mp.boughtbuffsavail[currentBuffIndex] = true;
											needValidate = true;
										}
										else
										{
											Main.NewText("You do not have enough items to buy this!", new Color(255, 0, 0));
										}
									}
								}, 
								delegate (UIMouseEvent evt, UIElement listeningElement)
                                {
                                    TooltipPanel.Instance.SetInfo(buff.cost, buff.id, buff.name, buff.effect, buff.texture);
                                });


							buffpanel.Add(ownedImages);

						}
						else
						{
							var toggleButtons = UIFactory.CreateHoverImageToggleButton(
								"Toggle buff button",
								buttonPlayTexture1, 
								buttonPlayTexture2, 
								$"Disable buff {buff.name}", 
								$"Use buff {buff.name}",
                                delegate (bool val)
                                {
                                    Main.player[Main.myPlayer].GetModPlayer<LPlayer>().buffsavail[currentBuffIndex] = val;
                                    needValidate = true;
                                },
                                delegate (UIMouseEvent evt, UIElement listeningElement)
                                {
                                    TooltipPanel.Instance.SetInfo(buff.id, buff.name, buff.effect, buff.texture);
                                });

                            (toggleButtons.element as UIHoverImageToggleButton).IsChecked = mp.buffsavail[currentBuffIndex];
							/*if (mp.buffsavail[currentBuffIndex])
							{
								(toggleButtons.element as UIHoverImageToggleButton).SetImage(buttonPlayTexture1);
							}
							else
							{
                                (toggleButtons.element as UIHoverImageToggleButton).SetImage(buttonPlayTexture2);
							}*/

							buffpanel.Add(toggleButtons);
						}
                    }

				}
				if(atleastOneBuffAdded)
				{
                    contentPanel.Add(modbuffgridpanel);
                }

            }

			panel.Invalidate();

        }
		

		public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!created)
            {
                create();
            }
            TooltipPanel.Instance.Update(this);

			if(needValidate)
			{
                var dim = UserInterface.ActiveInstance.GetDimensions();
                Screen.ScreenSizeChanged((int)dim.Width, (int)dim.Height);
                Revalidate();
                
            }
			Screen.Update();

        }
	}
}
