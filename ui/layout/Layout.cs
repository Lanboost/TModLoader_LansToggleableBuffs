using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace AutoBuff.ui.layout
{

    class LayoutElement
    {

        public int x;
        public int y;

        public virtual int GetHeight()
        {
            return 0;
        }
        public virtual int GetWidth()
        {
            return 0;
        }

        public virtual void SetX(int v)
        {
            this.x = v;
        }

        public virtual void SetY(int v)
        {
            this.y = v;
        }

        public virtual void Recalculate()
        {
        }

    }

    class LayoutElementWrapperUIElement: LayoutElement
    {
        UIElement elem;

        public LayoutElementWrapperUIElement(UIElement elem)
        {
            this.elem = elem;

            elem.PaddingBottom = 0;
            elem.PaddingLeft = 0;
            elem.PaddingRight = 0;
            elem.PaddingTop = 0;

            elem.MarginBottom = 0;
            elem.MarginLeft = 0;
            elem.MarginRight = 0;
            elem.MarginTop = 0;

        }

        public override int GetHeight()
        {
            elem.Recalculate();
            return (int) Math.Max(elem.GetDimensions().Height, elem.Height.Pixels);
        }
        public override int GetWidth()
        {
            elem.Recalculate();
            return (int)Math.Max(elem.GetDimensions().Width, elem.Width.Pixels);
        }

        public override void SetX(int v)
        {
            elem.Left.Set(v, 0);
            this.x = v;
        }

        public override void SetY(int v)
        {
            elem.Top.Set(v, 0);
            this.y = v;
        }

        public void Remove()
        {
            elem.Remove();
        }

        public void Add(UIElement e)
        {
            e.Append(this.elem);
        }
    }

    class RemoveValue
    {
        public bool ToRemove = false;

        public RemoveValue(bool toRemove)
        {
            ToRemove = toRemove;
        }
    }

    class LayoutWrapperUIElement: Layout
    {

        public LayoutWrapperUIElement(UIElement elem, int paddingTop, int paddingBottom, int paddingLeft, int paddingRight, int spaceing, ILayoutType layoutType):base(paddingTop, paddingBottom, paddingLeft, paddingRight, spaceing, layoutType)
        {
            this.elem = elem;
            elem.PaddingBottom = 0;
            elem.PaddingLeft = 0;
            elem.PaddingRight = 0;
            elem.PaddingTop = 0;

            elem.MarginBottom = 0;
            elem.MarginLeft = 0;
            elem.MarginRight = 0;
            elem.MarginTop = 0;
        }

        UIElement elem;

        Dictionary<LayoutElementWrapperUIElement, RemoveValue> childrenAdded = new Dictionary<LayoutElementWrapperUIElement, RemoveValue>();

       

        protected void checkchildren(Layout l)
        {
            foreach(var c in l.children)
            {
                if(c.GetType() == typeof(Layout))
                {
                    checkchildren((Layout)c);
                }

                if (c.GetType() == typeof(LayoutElementWrapperUIElement))
                {
                    LayoutElementWrapperUIElement e = (LayoutElementWrapperUIElement)c;
                    if (childrenAdded.ContainsKey(e))
                    {
                        childrenAdded[e].ToRemove = true;
                    }
                    else
                    {
                        childrenAdded.Add(e, new RemoveValue(true));
                        e.Add(elem);
                    }
                }
            }
        }

        public override void Recalculate()
        {
            foreach(var c in childrenAdded.Keys)
            {
                childrenAdded[c].ToRemove = false;
            }
            checkchildren(this);
            List<LayoutElementWrapperUIElement> toremove = new List<LayoutElementWrapperUIElement>();
            foreach (var c in childrenAdded)
            {
                if(!childrenAdded[c.Key].ToRemove)
                {
                    toremove.Add(c.Key);
                }
            }

            foreach(var k in toremove)
            {
                k.Remove();
                childrenAdded.Remove(k);
            }




            base.Recalculate();


            elem.Height.Set(this.height,0);
            elem.Width.Set(this.width, 0);
        }
    }

    interface ILayoutType
    {
        void Recalculate(Layout layout);
    }

    class Layout : LayoutElement
    {
        public int PaddingTop { get; set; }
        public int PaddingBottom { get; set; }
        public int PaddingLeft { get; set; }
        public int PaddingRight { get; set; }

        public int Spaceing { get; set; }
        

        public List<LayoutElement> children = new List<LayoutElement>();

        protected int width;
        protected int height;

        

        public ILayoutType LayoutType {get;set;}

        /*public Layout(UIElement panel)
        {
            this.panel = panel;
        }*/

        public Layout(int paddingTop, int paddingBottom, int paddingLeft, int paddingRight, int spaceing, ILayoutType layoutType)
        {
            PaddingTop = paddingTop;
            PaddingBottom = paddingBottom;
            PaddingLeft = paddingLeft;
            PaddingRight = paddingRight;
            Spaceing = spaceing;
            LayoutType = layoutType;
        }



        protected virtual void RecalculateSize()
        {
            int width = 0;
            foreach (var c in children)
            {
                width = (int)Math.Max(width,
                    c.GetWidth()+c.x
                );
            }
            width += PaddingLeft + PaddingRight-this.x;
            this.width = width;

            int height = 0;
            foreach (var c in children)
            {
                height = Math.Max(height, c.GetHeight()+c.y);
            }
            height += PaddingTop;
            height += PaddingBottom;

            this.height = height - this.y;
            
        }

        public override void Recalculate()
        {
            LayoutType.Recalculate(this);


            
            RecalculateSize();
        }

        public override int GetHeight()
        {
            return height;
        }
        public override int GetWidth()
        {
            return width;
        }


    }
}
