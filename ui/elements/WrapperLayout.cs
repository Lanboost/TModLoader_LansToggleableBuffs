using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LansToggleableBuffs.ui.components;
using LansToggleableBuffs.ui.layout;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace LansToggleableBuffs.ui.elements
{
    public class WrapperLayout : ILayout
    {
        Asset<Texture2D> asset;
        LComponent component;
        public WrapperLayout(Asset<Texture2D> asset)
        {
            this.asset = asset;
        }

        public void SetComponentOwner(LComponent component)
        {
            this.component = component;
        }

        public void Calculate()
        {
            component.X = 0;
            component.Y = 0;
            if (this.asset.IsLoaded)
            {
                var t = this.asset.Value;
                component.Width = t.Width;
                component.Height = t.Height;
            }
            else
            {
                component.Width = 32 ;
                component.Height = 32;
            }
        }

        public bool ControlsChildren()
        {
            return false;
        }
    }

    public class WrapperLayoutText : ILayout
    {
        LComponent component;

        public void SetComponentOwner(LComponent component)
        {
            this.component = component;
        }

        public void Calculate()
        {
            component.X = 0;
            component.Y = 0;
            var wrapper = component as WrapperComponent;
            component.Width = (int)wrapper.element.MinWidth.Pixels;
            component.Height = (int)wrapper.element.MinHeight.Pixels;
        }

        public bool ControlsChildren()
        {
            return false;
        }
    }
}
