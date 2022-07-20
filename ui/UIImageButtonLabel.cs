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

namespace LansToggleableBuffs.ui
{
    class UIImageButtonLabel : UIImageButton
    {

        //internal string HoverLabel;

        public UIImageButtonLabel(Asset<Texture2D> texture_checked, string HoverLabel) : base(texture_checked)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsMouseHovering)
            {

                TooltipPanel.Instance.Show(this);
                TooltipPanel.Instance.X = Main.mouseX;
                TooltipPanel.Instance.Y = Main.mouseY;
            }
            else
            {
                TooltipPanel.Instance.Hide(this);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
        }
    }
}
