using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LansToggleableBuffs.ui.layout;

namespace LansToggleableBuffs.ui.components
{

    public enum AnchorPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        RightCenter,
        BottomRight,
        BottomCenter,
        BottomLeft,
        LeftCenter,
        Center,
        ExpandToParent
    }

    public class LComponent
    {
        public static int MinX = 0;
        public static int MinY = 1;
        public static int MaxX = 2;
        public static int MaxY = 3;
        public static int SIDE_COUNT = 4;

        protected float[] anchor = new float[SIDE_COUNT];
        protected int[] margin = new int[SIDE_COUNT];

        protected List<LComponent> children = new List<LComponent>();

        protected ILayout layout;

        protected LComponent _parent = null;
        public string name;
        public LComponent(string name)
        {
            SetLayout(ILayout.None());
            this.name = name;
        }

        // used internaly to cache position and size
        protected int _x = 0;
        protected int _y = 0;
        protected int _width = 0;
        protected int _height = 0;

        public virtual int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public virtual int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public virtual int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public virtual int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public LComponent Parent
        {
            get { return _parent; }
        }

        public IEnumerable<LComponent> Children
        {
            get { return children.AsEnumerable(); }
        }

        public virtual void Add(LComponent component)
        {
            if (component._parent != null)
            {
                throw new Exception("This component is already owned by someone else!");
            }
            component._parent = this;
            this.children.Add(component);
        }

        public virtual void Remove(LComponent component)
        {
            component._parent = null;
            this.children.Remove(component);
        }

        public virtual void RemoveChildren()
        {
            for(int i= children.Count- 1; i >= 0; i--)
            {
                Remove(children[i]);
            }
        }

        public virtual void SetMargin(int side, int value)
        {
            margin[side] = value;
        }

        public virtual int GetMargin(int side)
        {
            return margin[side];
        }

        public virtual void SetMargins(int minX, int minY, int maxX, int maxY)
        {
            margin[MinX] = minX;
            margin[MinY] = minY;
            margin[MaxX] = maxX;
            margin[MaxY] = maxY;
        }

        public virtual void SetLayout(ILayout layout)
        {
            this.layout = layout;
            layout.SetComponentOwner(this);
        }

        public virtual void SetAnchor(int side, float value)
        {
            if(value < 0 || value > 1)
            {
                throw new Exception("Value range is 0-1");
            }
            anchor[side] = value;
        }

        public virtual void SetAnchors(float minX, float minY, float maxX, float maxY)
        {
            anchor[MinX] = minX;
            anchor[MinY] = minY;
            anchor[MaxX] = maxX;
            anchor[MaxY] = maxY;
        }

        public virtual float GetAnchor(int side)
        {
            return anchor[side];
        }

        public virtual void SetAnchor(AnchorPosition anchorPosition)
        {
            if(anchorPosition == AnchorPosition.TopLeft)
            {
                SetAnchors(0,0,0,0);
            }
            else if (anchorPosition == AnchorPosition.TopCenter)
            {
                SetAnchors(0.5f, 0, 0.5f, 0);
            }
            else if (anchorPosition == AnchorPosition.TopRight)
            {
                SetAnchors(1f, 0, 1f, 0);
            }
            else if (anchorPosition == AnchorPosition.RightCenter)
            {
                SetAnchors(1f, 0.5f, 1f, 0.5f);
            }
            else if (anchorPosition == AnchorPosition.BottomRight)
            {
                SetAnchors(1f, 1f, 1f, 1f);
            }
            else if (anchorPosition == AnchorPosition.BottomCenter)
            {
                SetAnchors(0.5f, 1f, 0.5f, 1f);
            }
            else if (anchorPosition == AnchorPosition.BottomLeft)
            {
                SetAnchors(0f, 1f, 0f, 1f);
            }
            else if (anchorPosition == AnchorPosition.LeftCenter)
            {
                SetAnchors(0f, 0.5f, 0f, 0.5f);
            }
            else if (anchorPosition == AnchorPosition.Center)
            {
                SetAnchors(0.5f, 0.5f, 0.5f, 0.5f);
            }
            else if (anchorPosition == AnchorPosition.ExpandToParent)
            {
                SetAnchors(0, 0, 1, 1);
            }
        }

        public virtual void SetMargin(int minX, int minY, int maxX, int maxY)
        {
            margin[MinX] = minX;
            margin[MinY] = minY;
            margin[MaxX] = maxX;
            margin[MaxY] = maxY;
        }

        public virtual void SetSize(int x, int y, int width, int height)
        {
            if (anchor[MinX] != anchor[MaxX] || anchor[MinY] != anchor[MaxY])
            {
                throw new Exception("Size only works correctly if you have anchored to the same (MinX/MaxX and MinY/MaxY). Set that first!");
            }

            margin[MinX] = x;
            margin[MinY] = y;
            margin[MaxX] = x+width;
            margin[MaxY] = y+height;
        }


        public virtual void Invalidate()
        {
            if(this.Parent != null)
            {
                this.Parent.Invalidate();
            }
            else
            {
                this.Recalculate();
            }
        }

        public virtual void Recalculate()
        {
            
            this.layout.Calculate();
            if (!this.layout.ControlsChildren())
            {
                foreach (var child in children)
                {
                    child.Recalculate();
                }
            }
        }

    }
}
