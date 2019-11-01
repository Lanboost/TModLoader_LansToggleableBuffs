using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LansToggleableBuffs.ui.layout
{
    class LayoutGrid : ILayoutType
    {
		int columns;
		public LayoutGrid(int columns )
		{
			this.columns = columns;
		}

		public void Recalculate(Layout layout)
        {
            
            int cx = layout.PaddingLeft + layout.x;
            int cy = layout.PaddingTop + layout.y;
			int count = 0;
            foreach (var c in layout.children)
            {
				c.Recalculate();
				if (columns == count)
				{
					cx = layout.PaddingLeft + layout.x;
					cy += c.GetHeight() + layout.Spaceing;
					count = 0;
				}
				count++;


				c.SetX(cx);
                c.SetY(cy);
                c.Recalculate();
				cx += c.GetWidth() + layout.Spaceing;

			}
        }
    }
}
