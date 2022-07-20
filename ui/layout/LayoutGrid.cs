using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LansToggleableBuffs.ui.components;

namespace LansToggleableBuffs.ui.layout
{
    public enum LayoutGridType
	{
		Columns,
		Rows
	}
    class LayoutGrid : ILayout
    {
		

		LComponent component = null;
		LayoutGridType layoutType;
        int columnsOrRows;
		int[] padding = new int[4];
		int spacing = 0;

		bool[] fitSizeToChildren;
		bool[] expandChildren;

        public LayoutGrid(int columnsOrRows, bool[] fitSizeToChildren, bool[] expandChildren, LayoutGridType layoutType = LayoutGridType.Columns, int paddingLeft = 0, int paddingTop = 0, int paddingRight = 0, int paddingBottom = 0, int spacing = 0)
		{
			this.layoutType = layoutType;

            this.columnsOrRows = columnsOrRows;
			padding[0] = paddingLeft;
			padding[1] = paddingTop;
			padding[2] = paddingRight;
			padding[3] = paddingBottom;
			this.spacing = spacing;
			this.fitSizeToChildren = fitSizeToChildren;
			this.expandChildren = expandChildren;

        }

		public void SetComponentOwner(LComponent component)
		{
			this.component = component;
		}

		public void SetColumnsOrRowsCount(int count)
		{

			this.columnsOrRows = count;

        }

		public void SetPadding(int paddingLeft = 0, int paddingTop = 0, int paddingRight = 0, int paddingBottom = 0, int spacing = 0)
		{
            padding[0] = paddingLeft;
            padding[1] = paddingTop;
            padding[2] = paddingRight;
            padding[3] = paddingBottom;
            this.spacing = spacing;
        }

		public void Calculate()
        {
			ILayout.CalculateDefaultSize(this.component);

			var childCount = component.Children.Count();

            int columns = columnsOrRows;
			int rows = (childCount+ columnsOrRows-1) / columnsOrRows;
			if(layoutType == LayoutGridType.Rows)
			{
                var temp = columns;
				columns = rows;
				rows = temp;
            }


            int[] columnWidth = new int[columns];
			int[] rowHeight = new int[rows];

			int index = 0;
			foreach (var c in component.Children)
			{
                c.Recalculate();
				var cColumn = index % columns;
				var cRow = index / columns;

				columnWidth[cColumn] = Math.Max(columnWidth[cColumn], c.Width);
				rowHeight[cRow] = Math.Max(rowHeight[cRow], c.Height);

				index++;
            }


            int cx = padding[0] + this.component.X;
            int cy = padding[1] + this.component.Y;

            index = 0;
            foreach (var c in component.Children)
            {
                var cColumn = index % columns;
                var cRow = index / columns;

                if (expandChildren[0])
                {
					c.Width = columnWidth[cColumn];
                }
				if(expandChildren[1])
				{
                    c.Height = rowHeight[cRow];
                }

				c.X = cx;
                c.Y = cy;

				if (cColumn == columns - 1)
				{
					cx = padding[0] + this.component.X;
					cy += rowHeight[cRow] + spacing;
				}
				else
				{
					cx += columnWidth[cColumn] + spacing;
				}

                index++;
            }

			if (fitSizeToChildren[0])
			{
				component.Width = columnWidth.Sum() + (columnWidth.Count() - 1) * spacing + padding[0] + padding[2];
            }
			if(fitSizeToChildren[1])
			{
                component.Height = rowHeight.Sum() + (rowHeight.Count() - 1) * spacing + padding[1] + padding[3];
            }
        }

		public bool ControlsChildren()
		{
			return true;
		}
    }
}
