using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LansToggleableBuffs.ui.components;
using Terraria.UI;

namespace LansToggleableBuffs.ui.elements
{
    public class WrapperComponent:LComponent 
    {
        public List<Func<bool>> allDelegates = new List<Func<bool>>();
        public Func<bool> updateDelegate;
        public UIElement element;
        public WrapperComponent(string name, UIElement element, Func<bool> updateDelegate = null):base(name)
        {
            this.element = element;
            this.updateDelegate = updateDelegate;
            if(this.updateDelegate != null)
            {
                allDelegates.Add(this.updateDelegate);
            }
        }
        public override int X
        {
            set { 
                base.X = value;
                if (element != null)
                {
                    var v = value - this.Parent.X;
                    element.Left.Set(v, 0);
                }
            }
        }

        public override int Y
        {
            set
            {
                base.Y = value;
                if (element != null)
                {
                    var v = value - this.Parent.Y;
                    element.Top.Set(v, 0);
                }
            }
        }

        public override int Width
        {
            set
            {
                base.Width = value;
                if (element != null)
                {
                    element.MinWidth.Set(value, 0);
                    element.MaxWidth.Set(value, 0);
                    element.Width.Set(value, 0);
                }
            }
        }

        public override int Height
        {
            set
            {
                base.Height = value;
                if (element != null)
                {
                    element.MinHeight.Set(value, 0);
                    element.MaxHeight.Set(value, 0);
                    element.Height.Set(value, 0);
                }
            }
        }

        public override void Add(LComponent component)
        {
            var wrapperComponent = component as WrapperComponent;
            base.Add(component);
            element.Append(wrapperComponent.element);

            allDelegates.AddRange(wrapperComponent.allDelegates);
        }

        public override void Remove(LComponent component)
        {
            var wrapperComponent = component as WrapperComponent;
            element.RemoveChild(wrapperComponent.element);
            base.Remove(component);
            
        }

        public override void Recalculate()
        {
            base.Recalculate();
            allDelegates.Clear();

            if (this.updateDelegate != null)
            {
                allDelegates.Add(this.updateDelegate);
            }

            foreach (var child in this.Children)
            {
                var wrapperComponentChild = child as WrapperComponent;
                allDelegates.AddRange(wrapperComponentChild.allDelegates);
            }
        }
    }
}
