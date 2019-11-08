using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.UI;

namespace LansToggleableBuffs.ui.layout
{
	class UIScrollPanel : UIElement
	{
		protected UIScrollbar Scrollbar = new UIScrollbar();

		public UIScrollPanel()
		{
		}

		public int panelWidth = 1000;
		public int panelHeight = 600;


		int lastOffset = 0;

		private static int CORNER_SIZE = 12;
		private static int BAR_SIZE = 4;

		private static Texture2D _borderTexture;
		private static Texture2D _backgroundTexture;
		public Color BorderColor = Color.Black;
		public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;


		private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Point point = new Point((int)this.Left.Pixels-10, (int)this.Top.Pixels-10);
			Point point2 = new Point(point.X + (int)panelWidth+40 - UIScrollPanel.CORNER_SIZE, point.Y + (int)panelHeight + 20 - UIScrollPanel.CORNER_SIZE);
			int width = point2.X - point.X - UIScrollPanel.CORNER_SIZE;
			int height = point2.Y - point.Y - UIScrollPanel.CORNER_SIZE;
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE), new Rectangle?(new Rectangle(0, 0, UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE), new Rectangle?(new Rectangle(UIScrollPanel.CORNER_SIZE + UIScrollPanel.BAR_SIZE, 0, UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE), new Rectangle?(new Rectangle(0, UIScrollPanel.CORNER_SIZE + UIScrollPanel.BAR_SIZE, UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE), new Rectangle?(new Rectangle(UIScrollPanel.CORNER_SIZE + UIScrollPanel.BAR_SIZE, UIScrollPanel.CORNER_SIZE + UIScrollPanel.BAR_SIZE, UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + UIScrollPanel.CORNER_SIZE, point.Y, width, UIScrollPanel.CORNER_SIZE), new Rectangle?(new Rectangle(UIScrollPanel.CORNER_SIZE, 0, UIScrollPanel.BAR_SIZE, UIScrollPanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + UIScrollPanel.CORNER_SIZE, point2.Y, width, UIScrollPanel.CORNER_SIZE), new Rectangle?(new Rectangle(UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE + UIScrollPanel.BAR_SIZE, UIScrollPanel.BAR_SIZE, UIScrollPanel.CORNER_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE, height), new Rectangle?(new Rectangle(0, UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE, UIScrollPanel.BAR_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE, height), new Rectangle?(new Rectangle(UIScrollPanel.CORNER_SIZE + UIScrollPanel.BAR_SIZE, UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE, UIScrollPanel.BAR_SIZE)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + UIScrollPanel.CORNER_SIZE, point.Y + UIScrollPanel.CORNER_SIZE, width, height), new Rectangle?(new Rectangle(UIScrollPanel.CORNER_SIZE, UIScrollPanel.CORNER_SIZE, UIScrollPanel.BAR_SIZE, UIScrollPanel.BAR_SIZE)), color);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			// moved from constructor to avoid texture loading on JIT thread
			if (_borderTexture == null)
			{
				_borderTexture = TextureManager.Load("Images/UI/PanelBorder");
			}
			if (_backgroundTexture == null)
			{
				_backgroundTexture = TextureManager.Load("Images/UI/PanelBackground");
			}


			var panelSpriteBatch = new SpriteBatch(Main.graphics.GraphicsDevice);

			panelSpriteBatch.Begin();

			this.DrawPanel(panelSpriteBatch, _backgroundTexture, BackgroundColor);
			this.DrawPanel(panelSpriteBatch, _borderTexture, BorderColor);

			CalculatedStyle space = GetInnerDimensions();
			float position = 0f;
			if (Scrollbar != null)
			{
				position = -Scrollbar.GetValue();
			}

			Scrollbar.Top.Pixels = this.Top.Pixels;
			Scrollbar.Left.Pixels = this.Left.Pixels + panelWidth + 10;
			Scrollbar.Height.Pixels = panelHeight;
			Scrollbar.Recalculate();
			Scrollbar.Draw(panelSpriteBatch);

			panelSpriteBatch.End();

			var innerSpriteBatch = new SpriteBatch(Main.graphics.GraphicsDevice);

			RasterizerState r = new RasterizerState();
			r.ScissorTestEnable = true;

			innerSpriteBatch.GraphicsDevice.RasterizerState = r;
			innerSpriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)this.Left.Pixels, (int)this.Top.Pixels, (int)panelWidth, (int)panelHeight);
			//innerSpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, r, null, Matrix.CreateTranslation(0, -Scrollbar.ViewPosition, 0));
			innerSpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, r, null);

			int diff = ((int) -Scrollbar.ViewPosition)- lastOffset;
			lastOffset += diff;


			foreach (var child in this.Elements)
			{
				if (diff != 0)
				{
					child.Top.Pixels += diff;
					child.Recalculate();
				}

				child.Draw(innerSpriteBatch);
			}

			innerSpriteBatch.End();


			bool hoveringOverReforgeButton = this.Left.Pixels <= Main.mouseX && Main.mouseX <= this.Left.Pixels + this.panelWidth+20 &&
				this.Top.Pixels <= Main.mouseY && Main.mouseY <= this.Top.Pixels + this.panelHeight+20 && !PlayerInput.IgnoreMouseInterface;
			if (hoveringOverReforgeButton)
			{
				Main.LocalPlayer.mouseInterface = true;
			}



		}

		public int getOffset()
		{
			return (int)-Scrollbar.ViewPosition;
		}

		public override void Recalculate()
		{
			base.Recalculate();
			UpdateScrollbar();
		}

		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			base.ScrollWheel(evt);
			if (Scrollbar != null)
			{
				Scrollbar.ViewPosition -= evt.ScrollWheelValue;
			}
		}

		public void SetScrollbar(UIScrollbar scrollbar)
		{
			Scrollbar = scrollbar;
			UpdateScrollbar();
		}

		private void ResetScrollbar()
		{
			if (Scrollbar != null)
			{
				Scrollbar.ViewPosition = 0;
			}
		}

		private void UpdateScrollbar()
		{
			Scrollbar?.SetView(this.panelHeight, this.Height.Pixels);
		}

	}
	
}
