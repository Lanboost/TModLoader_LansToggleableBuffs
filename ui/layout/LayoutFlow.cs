using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LansToggleableBuffs.ui.components;

namespace LansToggleableBuffs.ui.layout
{
    public enum LayoutFlowType
    {
        Vertical,
        Horizontal
    }
    class LayoutFlow: LayoutGrid
    {

        public LayoutFlow(bool[] fitSizeToChildren, bool[] expandChildren, LayoutFlowType layoutType = LayoutFlowType.Vertical, int paddingLeft = 0, int paddingTop = 0, int paddingRight = 0, int paddingBottom = 0, int spacing = 0):base(1, fitSizeToChildren, expandChildren, layoutType == LayoutFlowType.Vertical ? LayoutGridType.Columns : LayoutGridType.Rows, paddingLeft, paddingTop, paddingRight, paddingBottom, spacing)
        {
        }
    }
}
