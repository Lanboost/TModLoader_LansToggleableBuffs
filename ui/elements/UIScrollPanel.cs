using LansToggleableBuffs.ui.components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace LansToggleableBuffs.ui.elements
{
    class UIScrollPanel : UIElement
    {
        protected FixedUIScrollbar Scrollbar;

        LComponent contentPanel;
        public UIScrollPanel(LComponent contentPanel, UserInterface userInterface)
        {
            this.contentPanel = contentPanel;
            Scrollbar = new FixedUIScrollbar(userInterface);
        }



        int lastOffset = 0;

        private static int CORNER_SIZE = 12;
        private static int BAR_SIZE = 4;
        private static int SCROLLBAR_SIZE = 15;
        private static int SCROLLBAR_OFFSET = 10;

        private static Asset<Texture2D> _borderTexture;
        private static Asset<Texture2D> _backgroundTexture;
        public Color BorderColor = Color.Black;
        public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;


        private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point = new Point((int)dimensions.X, (int)dimensions.Y);
            Point point2 = new Point(point.X + (int)dimensions.Width - CORNER_SIZE, point.Y + (int)dimensions.Height - CORNER_SIZE);
            int width = point2.X - point.X - CORNER_SIZE;
            int height = point2.Y - point.Y - CORNER_SIZE;
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(0, 0, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, 0, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(0, CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point.Y, width, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE, 0, BAR_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point2.Y, width, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE, CORNER_SIZE + BAR_SIZE, BAR_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + CORNER_SIZE, CORNER_SIZE, height), new Rectangle?(new Rectangle(0, CORNER_SIZE, CORNER_SIZE, BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + CORNER_SIZE, CORNER_SIZE, height), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE, BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point.Y + CORNER_SIZE, width, height), new Rectangle?(new Rectangle(CORNER_SIZE, CORNER_SIZE, BAR_SIZE, BAR_SIZE)), color);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            UpdateScrollbar();
            // moved from constructor to avoid texture loading on JIT thread
            if (_borderTexture == null)
            {
                _borderTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PanelBorder");
            }
            if (_backgroundTexture == null)
            {
                _backgroundTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/PanelBackground");
            }


            var panelSpriteBatch = new SpriteBatch(Main.graphics.GraphicsDevice);

            panelSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

            if (_backgroundTexture.Value != null)
            {
                DrawPanel(panelSpriteBatch, _backgroundTexture.Value, BackgroundColor);
            }
            if (_borderTexture.Value != null)
            {
                DrawPanel(panelSpriteBatch, _borderTexture.Value, BorderColor);
            }

            CalculatedStyle space = GetInnerDimensions();
            float position = 0f;
            if (Scrollbar != null)
            {
                position = -Scrollbar.GetValue();
            }

            Scrollbar.Top.Pixels = Top.Pixels+ SCROLLBAR_OFFSET;
            Scrollbar.Left.Pixels = Left.Pixels + Width.Pixels - SCROLLBAR_SIZE- SCROLLBAR_OFFSET;
            Scrollbar.Height.Pixels = Height.Pixels- SCROLLBAR_OFFSET- SCROLLBAR_OFFSET;
            Scrollbar.Recalculate();
            Scrollbar.Draw(panelSpriteBatch);

            panelSpriteBatch.End();

            var innerSpriteBatch = new SpriteBatch(Main.graphics.GraphicsDevice);

            RasterizerState r = new RasterizerState();
            r.ScissorTestEnable = true;

            innerSpriteBatch.GraphicsDevice.RasterizerState = r;
            /*try
			{*/
            CalculatedStyle dimensions = GetDimensions();
            int left = (int)Math.Max(0, dimensions.X);
            int top = (int)Math.Max(0, dimensions.Y);
            int width = Math.Min((int)dimensions.Width- SCROLLBAR_SIZE, innerSpriteBatch.GraphicsDevice.Viewport.Width - left- SCROLLBAR_SIZE);
            int height = Math.Min((int)dimensions.Height, innerSpriteBatch.GraphicsDevice.Viewport.Height - top);
            var start = Vector2.Transform(new Vector2(left, top), Main.UIScaleMatrix);
            var end = Vector2.Transform(new Vector2(width, height), Main.UIScaleMatrix);
            var rect = new Rectangle((int)start.X, (int)start.Y, (int)end.X, (int)end.Y);
            innerSpriteBatch.GraphicsDevice.ScissorRectangle = rect;

            //innerSpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, r, null, Matrix.CreateTranslation(0, -Scrollbar.ViewPosition, 0));
            innerSpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, r, null, Main.UIScaleMatrix);

            

            foreach (var child in Elements)
            {
                

                child.Draw(innerSpriteBatch);
            }

            innerSpriteBatch.End();


            bool hoveringOverReforgeButton = Left.Pixels <= Main.mouseX && Main.mouseX <= Left.Pixels + Width.Pixels &&
                Top.Pixels <= Main.mouseY && Main.mouseY <= Top.Pixels + Height.Pixels && !PlayerInput.IgnoreMouseInterface;
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

            int diff = (int)-Scrollbar.ViewPosition;
            lastOffset += diff;


            foreach (var child in Elements)
            {
                
                    child.Top.Pixels = diff;
                
            }
            base.Recalculate();
        }

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
            if (Scrollbar != null)
            {
                Scrollbar.ViewPosition -= evt.ScrollWheelValue;
            }
        }

        public void SetScrollbar(FixedUIScrollbar scrollbar)
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
            Scrollbar?.SetView(Height.Pixels, contentPanel.Height);
        }

    }

}
