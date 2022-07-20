using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LansToggleableBuffs.ui.components;
using Terraria.UI;

namespace LansToggleableBuffs.ui.layout
{

    public interface ILayout
    {
        public static ILayout None()
        {
            return new LayoutDefault();
        }

        bool ControlsChildren();

        void Calculate();

        void SetComponentOwner(LComponent component);

        public static void CalculateDefaultSize(LComponent component)
        {
            var x = component.Parent.X + component.Parent.Width * component.GetAnchor(LComponent.MinX) + component.GetMargin(LComponent.MinX);
            var y = component.Parent.Y + component.Parent.Height * component.GetAnchor(LComponent.MinY) + component.GetMargin(LComponent.MinY);
            var maxX = component.Parent.X + component.Parent.Width * component.GetAnchor(LComponent.MaxX) - component.GetMargin(LComponent.MaxX);
            var maxY = component.Parent.Y + component.Parent.Height * component.GetAnchor(LComponent.MaxY) - component.GetMargin(LComponent.MaxY);

            var width = Math.Max(0, maxX - x);
            var height = Math.Max(0, maxY - y);

            component.X = (int)x;
            component.Y = (int)y;
            component.Width = (int)width;
            component.Height = (int)height;
        }
    }

    public class LayoutDefault: ILayout
    {
        LComponent component;
        public void SetComponentOwner(LComponent component)
        {
            this.component = component;
        }

        public void Calculate()
        {
            ILayout.CalculateDefaultSize(component);
        }

        public bool ControlsChildren()
        {
            return false;
        }
    }

}
