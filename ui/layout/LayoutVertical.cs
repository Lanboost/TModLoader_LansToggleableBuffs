using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LansToggleableBuffs.ui.layout
{
    class LayoutVertical : ILayoutType
    {

        public void Recalculate(Layout layout)
        {
            
            int cx = layout.PaddingLeft + layout.x;
            int cy = layout.PaddingTop + layout.y;
            foreach (var c in layout.children)
            {
                c.SetX(cx);
                c.SetY(cy);
                c.Recalculate();
                cy += c.GetHeight()+ layout.Spaceing;

            }
        }
    }
}
