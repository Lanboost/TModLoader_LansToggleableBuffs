using LansToggleableBuffs.ui.components;
using LansToggleableBuffs.ui.elements;
using LansToggleableBuffs.ui.layout;
using log4net.Layout;
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
    class TooltipPanel
    {

        public object caller = null;

        public bool visible = false;

        protected bool isadded = false;

        public static TooltipPanel Instance {get;set;}


        public int X {
            set
            {
                var width = panel.GetMargin(LComponent.MaxX) - panel.GetMargin(LComponent.MinX);
                panel.SetMargin(LComponent.MinX, value);
                panel.SetMargin(LComponent.MaxX, value+ width);
            }
        }
        public int Y {
            set
            {
                var height = panel.GetMargin(LComponent.MaxY) - panel.GetMargin(LComponent.MinY);
                panel.SetMargin(LComponent.MinY, value);
                panel.SetMargin(LComponent.MaxY, value + height);
            }
        }

        WrapperComponent panel;

        //LayoutWrapperUIElement lv;

        //Layout costPanel = new Layout(0, 0, 0, 0, 10, new LayoutVertical());

        public void Init()
        {
            panel = UIFactory.CreateUIPanel("Tooltip Panel");
            panel.element.IgnoresMouseInteraction = true;
            panel.SetAnchor(AnchorPosition.TopLeft);
            panel.SetSize(100, 100, 200, 200);
            var panelLayout = new LayoutFlow(new bool[] { true, true }, new bool[] { false, false }, LayoutFlowType.Vertical, 0, 0, 24, 24, 0);
            panel.SetLayout(panelLayout);
        }

        public void Update(Panel state)
        {
            //UpdateLayout();
            if (visible && !isadded)
            {

                state.Screen.Add(panel);
                isadded = true;
                panel.Invalidate();
            }

            if (!visible && isadded)
            {
                state.Screen.Remove(panel);
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

        internal void SetInfo(int buffid, string name, string effect, Asset<Texture2D> texture)
        {
            panel.RemoveChildren();

            var inner = CreateBuffInfo(buffid, name, effect, texture);
            panel.Add(inner);
            if (panel.Parent != null)
            {
                panel.Invalidate();
            }

        }

        WrapperComponent CreateBuffInfo(int buffid, string name, string effect, Asset<Texture2D> texture)
        {
            var buffdesc = Lang.GetBuffDescription(buffid);

            int itemRarity = 0;
            if (Main.meleeBuff[buffid])
            {
                itemRarity = -10;
            }

            BuffLoader.ModifyBuffTip(buffid, ref buffdesc, ref itemRarity);


            var buffName = UIFactory.CreateText("Buff Name", name);
            buffName.element.IgnoresMouseInteraction = true;
            var buffTexture = UIFactory.CreateImage("Buff Icon", texture);
            buffTexture.element.IgnoresMouseInteraction = true;
            var buffEffect = UIFactory.CreateText("Buff Effect", buffdesc);
            buffEffect.element.IgnoresMouseInteraction = true;

            var inner = UIFactory.CreatePanel("Tooltip Panel inner");
            var innerLayout = new LayoutFlow(new bool[] { true, true }, new bool[] { false, false }, LayoutFlowType.Vertical, 0, 0, 0, 0, 10);
            inner.SetLayout(innerLayout);
            inner.element.IgnoresMouseInteraction = true;

            inner.Add(buffName);
            inner.Add(buffTexture);
            inner.Add(buffEffect);
            return inner;
        }

        internal void SetInfo(CostValue[] cost, int buffid, string name, string effect, Asset<Texture2D> texture)
        {
            panel.RemoveChildren();

            var inner = CreateBuffInfo(buffid, name, effect, texture);

            var costText = UIFactory.CreateText("Cost Label", "Cost");
            (costText.element as UIText).TextColor = new Color(232, 181, 16);

            inner.Add(costText);

            foreach (var v in cost)
            {
                if (v.GetType() == typeof(ItemCostValue))
                {
                    var p = UIFactory.CreatePanel("Cost Panel");
                    var pLayout = new LayoutFlow(new bool[] { true, true }, new bool[] { false, false }, LayoutFlowType.Horizontal, 0, 0, 0, 0, 10);
                    p.SetLayout(pLayout);
                    Main.instance.LoadItem(((ItemCostValue)v).itemid);
                    var costIcon = UIFactory.CreateImage("Cost Image", TextureAssets.Item[((ItemCostValue)v).itemid]);

                    var costcountLabel = UIFactory.CreateText("Cost Count",$"x{((ItemCostValue)v).count}");
                    (costcountLabel.element as UIText).TextColor = new Color(232, 181, 16);

                    var costnamelabel = UIFactory.CreateText("Cost Label", $"{((ItemCostValue)v).itemname}");
                    (costnamelabel.element as UIText).TextColor = new Color(232, 181, 16);

                    p.Add(costIcon);
                    p.Add(costcountLabel);
                    p.Add(costnamelabel);
                    inner.Add(p);
                }
                else if (v.GetType() == typeof(MoneyCostValue))
                {

                    var p = UIFactory.CreatePanel("Cost Panel");
                    var pLayout = new LayoutFlow(new bool[] { true, true }, new bool[] { false, false }, LayoutFlowType.Horizontal, 0, 0, 0, 0, 10);
                    p.SetLayout(pLayout);

                    var money = ((MoneyCostValue)v).cost;
                    for (int i = 4; i >= 1; i--)
                    {
                        int mcount = (int)(money / Math.Pow(100, i - 1));
                        money = (money % ((int)Math.Pow(100, i - 1)));
                        if (mcount > 0)
                        {
                            var itemTexture = ModContent.Request<Texture2D>($"Terraria/Images/Item_{70 + i}");
                            var costIcon = UIFactory.CreateImage("Cost Image", itemTexture);
                            p.Add(costIcon);

                            var costcountLabel = UIFactory.CreateText("Cost Count", $"x{mcount}");
                            (costcountLabel.element as UIText).TextColor = new Color(232, 181, 16);
                            p.Add(costcountLabel);
                        }
                    }
                    inner.Add(p);
                }
            }


            panel.Add(inner);
            if (panel.Parent != null)
            {
                panel.Invalidate();
            }

        }
    }
}
