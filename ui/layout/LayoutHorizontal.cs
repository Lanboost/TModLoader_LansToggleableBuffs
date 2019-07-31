using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBuff.ui.layout
{
    class LayoutHorizontal: ILayoutType
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
                cx += c.GetWidth()+layout.Spaceing;

            }
        }
    }
}
