using ExampleMod.UI;
using LansToggleableBuffs.ui.layout;
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

namespace LansToggleableBuffs.ui
{
    class Panel : UIState
    {
        public static bool visible = false;
        public DragableUIPanel panel;

		LayoutWrapperUIElement panelwrapper;

		Texture2D buttonPlayTexture1;
        Texture2D buttonPlayTexture2;

		bool created = false;
        public override void OnInitialize()
        {

            
        }

		bool needValidate = false;

		public void create()
		{
			created = true;
			TooltipPanel.Instance = new TooltipPanel();
			TooltipPanel.Instance.Init();


			// if you set this to true, it will show up in game
			//visible = false;

			buttonPlayTexture1 = ModContent.GetTexture("LansToggleableBuffs/ui/checkbox");
			buttonPlayTexture2 = ModContent.GetTexture("LansToggleableBuffs/ui/checkboxunchecked");

			panel = new DragableUIPanel(); //initialize the panel
										   // ignore these extra 0s
			panel.Left.Set(800, 0); //this makes the distance between the left of the screen and the left of the panel 500 pixels (somewhere by the middle)
			panel.Top.Set(100, 0); //this is the distance between the top of the screen and the top of the panel
			this.Append(panel);

			panelwrapper = new LayoutWrapperUIElement(panel, 10, 10, 10, 10, 10, new LayoutVertical());

			Revalidate();
		}

		public void Revalidate() {
			needValidate = false;
			var buffSize = LansToggleableBuffs.instance.getBuffLength();
			
			var unownedTexture = ModContent.GetTexture("LansToggleableBuffs/ui/unowned");
			var mp = Main.player[Main.myPlayer].GetModPlayer<LPlayer>();
			panelwrapper.children.Clear();

			int buffIndex = 0;
			foreach (var modBuffValues in LansToggleableBuffs.instance.modBuffValues)
			{
				var modbuffpanel = new Layout(0, 0, 0, 0, 10, new LayoutVertical());

				var modlabel = new UIText(""+modBuffValues.saveTag);
				modlabel.TextColor = new Color(232, 181, 16);
				modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

				var modbuffgridpanel = new Layout(0, 0, 0, 0, 10, new LayoutGrid(24));
				modbuffpanel.children.Add(modbuffgridpanel);

				//populate modbuffgridpanel

				foreach (var buffValue in modBuffValues.buffs)
				{
					var currentBuffIndex = buffIndex;
					buffIndex += 1;

					var buffpanel = new Layout(0, 0, 0, 0, 10, new LayoutVertical());

					var buff = LansToggleableBuffs.instance.getBuff(currentBuffIndex);

					buff.texture = Main.buffTexture[buff.id];

					{
						UIImage icon = new UIImage(buff.texture);
						buffpanel.children.Add(new LayoutElementWrapperUIElement(icon));
						
						if (!mp.boughtbuffsavail[currentBuffIndex])
						{
							var ownedImages = new UIImageButtonLabel(unownedTexture, "Buy buff " + buff.name);

							buffpanel.children.Add(new LayoutElementWrapperUIElement(ownedImages));

							ownedImages.OnClick += delegate (UIMouseEvent evt, UIElement listeningElement)
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
							};

							ownedImages.OnMouseOver += delegate (UIMouseEvent evt, UIElement listeningElement)
							{
								TooltipPanel.Instance.SetInfo(buff.cost, buff.id, buff.name, buff.effect, buff.texture);
							};
						}
						else
						{
							var toggleButtons = new UIHoverImageToggleButton(buttonPlayTexture1, buttonPlayTexture2, "Disable buff " + buff.name, "Use buff " + buff.name);

							toggleButtons.IsChecked = mp.buffsavail[currentBuffIndex];
							if (mp.buffsavail[currentBuffIndex])
							{
								toggleButtons.SetImage(buttonPlayTexture1);
							}
							else
							{
								toggleButtons.SetImage(buttonPlayTexture2);
							}

							toggleButtons.OnChecked += delegate (bool val)
							{
								Main.player[Main.myPlayer].GetModPlayer<LPlayer>().buffsavail[currentBuffIndex] = val;
								needValidate = true;
							};

							toggleButtons.OnMouseOver += delegate (UIMouseEvent evt, UIElement listeningElement)
							{
								TooltipPanel.Instance.SetInfo(buff.id, buff.name, buff.effect, buff.texture);
							};

							buffpanel.children.Add(new LayoutElementWrapperUIElement(toggleButtons));
						}
					}

					modbuffgridpanel.children.Add(buffpanel);
				}

				panelwrapper.children.Add(modbuffpanel);
			}

			panelwrapper.Recalculate();

			
		}


        public override void Update(GameTime gameTime)
        {
			if(!created)
			{
				create();
			}
			base.Update(gameTime);
			TooltipPanel.Instance.Update(this);

			if(needValidate)
			{
				Revalidate();
			}

			/*
			if (visible)
			{

				var mp = Main.player[Main.myPlayer].GetModPlayer<LPlayer>();

				for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
				{
					if (mp.boughtbuffsavail[i])
					{
						panel.Append(toggleButtons[i]);
						ownedImages[i].Remove();
					}
					else
					{
						toggleButtons[i].Remove();
						panel.Append(ownedImages[i]);
					}
				}

				for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
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


			
            for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
            {
				var buff = LansToggleableBuffs.instance.getBuff(i);
                if(toggleButtons[i].IsMouseHovering)
                {
                    TooltipPanel.Instance.SetInfo(buff.id, buff.name, buff.effect, buff.texture);
                }

                if (ownedImages[i].IsMouseHovering)
                {
                    TooltipPanel.Instance.SetInfo(buff.cost, buff.id, buff.name, buff.effect, buff.texture);
                }
            }
			
            if (visible)
            {
                
                var mp = Main.player[Main.myPlayer].GetModPlayer<LPlayer>();

                for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
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

                for (int i = 0; i < LansToggleableBuffs.instance.getBuffLength(); i++)
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
			*/
		}
	}
}
