using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LansToggleableBuffs.ui.components;
using LansToggleableBuffs.ui.layout;
using Terraria.UI;

namespace LansToggleableBuffs.ui.elements
{

    public interface Screen
    {
        public void Add(LComponent component);
        public void Remove(LComponent component);

        public void ScreenSizeChanged(int width, int heigth);
        public void Update();
    }

    public class BaseComponent : LComponent, ILayout
    {
        public BaseComponent():base("Screen")
        {
            this.layout = this;
            this.X = 0;
            this.Y = 0;
            this.Width = 0;
            this.Height = 0;
        }

        public void Calculate()
        {
        }

        public void SetComponentOwner(LComponent component)
        {
        }

        public bool ControlsChildren()
        {
            return false;
        }

        public void SetScreenSize(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Invalidate();
        }

        public override void Invalidate()
        {
            base.Invalidate();
            foreach(var child in this.Children)
            {
                var wc = child as WrapperComponent;
                wc.element.Recalculate();
            }
        }
    }

    public class WrapperScreen : Screen
    {
        BaseComponent baseComponent = new BaseComponent();
        UIState uistate;

        public WrapperScreen(UIState uistate)
        {
            this.uistate = uistate;
        }

        public void Add(LComponent component)
        {
            baseComponent.Add(component);
            var wcomponent = component as WrapperComponent;
            uistate.Append(wcomponent.element);
        }

        public void Remove(LComponent component)
        {
            baseComponent.Remove(component);
            var wcomponent = component as WrapperComponent;
            uistate.RemoveChild(wcomponent.element);
        }

        public void ScreenSizeChanged(int width, int heigth)
        {
            baseComponent.SetScreenSize(width, heigth);
        }

        public void Update()
        {
            foreach(var component in baseComponent.Children)
            {
                var wcomponent = component as WrapperComponent;
                var invalidate = false;
                foreach(var f in wcomponent.allDelegates)
                {
                    if(f())
                    {
                        invalidate = true;
                    }
                }
                if(invalidate)
                {
                    baseComponent.Invalidate();
                }
            }
        }
    }
}
