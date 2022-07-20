using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExampleMod.UI;
using LansToggleableBuffs.ui.components;
using LansToggleableBuffs.ui.elements;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace LansToggleableBuffs.ui
{

    public class UIFactory
    {
        public static WrapperComponent CreatePanel(string name)
        {
            var element = new UIElement();
            return new WrapperComponent(name, element);
        }

        public static WrapperComponent CreateUIPanel(string name)
        {
            var element = new UIPanel();

            return new WrapperComponent(name, element);
        }

        public static WrapperComponent CreateScrollPanel(string name, LComponent contentPanel)
        {
            var element = new UIScrollPanel(contentPanel);
            return new WrapperComponent(name, element);
        }

        public static WrapperComponent CreateImage(string name, Asset<Texture2D> texture)
        {
            var element = new UIImage(texture);
            var loaded = texture.IsLoaded;
            WrapperComponent wrapper = null;
            Func<bool> del = null;
            if(!loaded)
            {
                del = delegate ()
                {
                    if(texture.IsLoaded)
                    {
                        wrapper.updateDelegate = null;
                        return true;
                    }
                    return false;
                };
            }
            wrapper = new WrapperComponent(name, element, del);
            var layout = new WrapperLayout(texture);
            wrapper.SetLayout(layout);
            return wrapper;
        }

        public static WrapperComponent CreateButton(Asset<Texture2D> texture)
        {
            var element = new UIImageButton(texture);
            var loaded = texture.IsLoaded;
            WrapperComponent wrapper = null;
            Func<bool> del = null;
            if (!loaded)
            {
                del = delegate ()
                {
                    if (texture.IsLoaded)
                    {
                        wrapper.updateDelegate = null;
                        return true;
                    }
                    return false;
                };
            }
            wrapper = new WrapperComponent("", element, del);
            return wrapper;
        }

        public static WrapperComponent CreateText(string name, string text)
        {
            var modlabel = new UIText(text);
            var wrapper = new WrapperComponent(name,modlabel);
            wrapper.SetLayout(new WrapperLayoutText());
            return wrapper;
        }

        public static WrapperComponent CreateImageButtonLabel(string name, Asset<Texture2D> texture, string hoverText,
            UIElement.MouseEvent mouseClick, UIElement.MouseEvent mouseOver)
        {
            var element = new UIImageButtonLabel(texture, hoverText);
            element.OnClick += mouseClick;
            element.OnMouseOver += mouseOver;
            element.OnMouseOver += delegate (UIMouseEvent evt, UIElement listeningElement)
            {
                var asd = 0;
            };
            var wrapper = new WrapperComponent(name, element);
            wrapper.SetLayout(new WrapperLayout(texture));
            return wrapper;
        }

        public static WrapperComponent CreateHoverImageToggleButton(string name, Asset<Texture2D> texture_checked, Asset<Texture2D> texture_unchecked, string hoverTextchecked, string hoverTextunchecked,
            UIHoverImageToggleButton.CheckEvent onChecked, UIElement.MouseEvent mouseOver)
        {
            var element = new UIHoverImageToggleButton(texture_checked, texture_unchecked, hoverTextchecked, hoverTextunchecked);
            element.OnChecked += onChecked;
            element.OnMouseOver += mouseOver;
            var wrapper = new WrapperComponent(name, element);
            wrapper.SetLayout(new WrapperLayout(texture_checked));
            return wrapper;
        }




    }
}
