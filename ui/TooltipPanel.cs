using LansToggleableBuffs.ui.layout;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    class TooltipPanel
    {

        public object caller = null;

        public bool visible = false;

        protected bool isadded = false;

        public static TooltipPanel Instance {get;set;}


        public int X {
            set
            {
                panel.Left.Set(value, 0f);
            }
        }
        public int Y {
            set
            {
                panel.Top.Set(value, 0f);
            }
        }

        UIPanel panel;

        public UIText buffName;
        public UIImage buffIcon;
        public UIText buffEffect;

        List<UIElement> children = new List<UIElement>();
        LayoutWrapperUIElement lv;

        Layout costPanel = new Layout(0, 0, 0, 0, 10, new LayoutVertical());

        public void Init()
        {
            panel = new UIPanel();

            lv = new LayoutWrapperUIElement(panel, 10, 10, 10, 10, 10, new LayoutVertical());
            lv.children.Add(costPanel);

            { 
                var topLabel = new UIText("Buff");
                topLabel.TextColor = new Color(137, 233, 9);
                lv.children.Add(new LayoutElementWrapperUIElement(topLabel));

                var ll = new Layout(0, 0, 0, 0, 10, new LayoutHorizontal());

                buffIcon = new UIImage(ModContent.Request<Texture2D>("LansToggleableBuffs/ui/unowned"));
                ll.children.Add(new LayoutElementWrapperUIElement(buffIcon));

                buffName = new UIText("TestName");
                ll.children.Add(new LayoutElementWrapperUIElement(buffName));

                lv.children.Add(ll);

                buffEffect = new UIText("Reduces the chance of consuming any ammunition by 20%. ");
                lv.children.Add(new LayoutElementWrapperUIElement(buffEffect));
            }

            lv.Recalculate();
        }

        public void Update(UIState state)
        {
            //UpdateLayout();
            if (visible && !isadded)
            {
                state.Append(panel);
                isadded = true;
            }

            if (!visible && isadded)
            {
                panel.Remove();
                isadded = false;
            }
        }


        public void Show(object c)
        {
            this.visible = true;
            caller = c;
        }

        public void Hide(object c)
        {
            if(caller == c)
            {
                this.visible = false;
            }
        }

        internal void SetInfo(int buffid, string name, string effect, Texture2D texture)
        {
            costPanel.children.Clear();
            this.buffName.SetText(name);
            this.buffIcon.SetImage(texture);


			var buffdesc = Lang.GetBuffDescription(buffid);

			int itemRarity = 0;
			if (Main.meleeBuff[buffid])
			{
				itemRarity = -10;
			}

			BuffLoader.ModifyBuffTip(buffid, ref buffdesc, ref itemRarity);
			this.buffEffect.SetText(buffdesc);
			lv.Recalculate();
		}

        internal void SetInfo(CostValue[] cost, int buffid, string name, string effect, Texture2D texture)
        {
            costPanel.children.Clear();
            var costtopLabel = new UIText("Cost");
            costtopLabel.TextColor = new Color(232, 181, 16);
            costPanel.children.Add(new LayoutElementWrapperUIElement(costtopLabel));

            foreach (var v in cost)
            {
                if(v.GetType() == typeof(ItemCostValue))
                {
                    var ll = new Layout(0, 0, 0, 0, 10, new LayoutHorizontal());
                    var costIcon = new UIImage(TextureAssets.Item[((ItemCostValue) v).itemid].Value);
                    costIcon.Height.Set(20, 0);
                    ll.children.Add(new LayoutElementWrapperUIElement(costIcon));

                    var costcountLabel = new UIText("x"+ ((ItemCostValue)v).count);
                    costcountLabel.TextColor = new Color(232, 181, 16);
                    ll.children.Add(new LayoutElementWrapperUIElement(costcountLabel));
					
					var costnamelabel = new UIText(((ItemCostValue)v).itemname);
					costnamelabel.TextColor = new Color(232, 181, 16);
					ll.children.Add(new LayoutElementWrapperUIElement(costnamelabel));

					costPanel.children.Add(ll);
                }
                else if (v.GetType() == typeof(MoneyCostValue))
                {
                    var ll = new Layout(0, 0, 0, 0, 10, new LayoutHorizontal());
                    var money = ((MoneyCostValue)v).cost;
                    for (int i = 4; i >= 1; i--)
                    {
                        int mcount = (int) (money / Math.Pow(100, i - 1));
                        money = (money % ((int)Math.Pow(100, i - 1)));
                        if (mcount > 0)
                        {
                            var costIcon = new UIImage(TextureAssets.Item[70 + i].Value);
                            costIcon.Height.Set(20, 0);
                            ll.children.Add(new LayoutElementWrapperUIElement(costIcon));

                            var costcountLabel = new UIText("x" + mcount);
                            costcountLabel.TextColor = new Color(232, 181, 16);
                            ll.children.Add(new LayoutElementWrapperUIElement(costcountLabel));
                        }
                    }
                    costPanel.children.Add(ll);
                }
            }



            this.buffName.SetText(name);
            this.buffIcon.SetImage(texture);
			var buffdesc = Lang.GetBuffDescription(buffid);

			int itemRarity = 0;
			if (Main.meleeBuff[buffid])
			{
				itemRarity = -10;
			}

			BuffLoader.ModifyBuffTip(buffid, ref buffdesc, ref itemRarity);
			this.buffEffect.SetText(buffdesc);
            lv.Recalculate();
        }
    }
}
