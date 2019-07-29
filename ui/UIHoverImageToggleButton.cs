using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ExampleMod.UI
{
    // This UIHoverImageButton class inherits from UIImageButton. 
    // Inheriting is a great tool for UI design. 
    // By inheriting, we get the Image drawing, MouseOver sound, and fading for free from UIImageButton
    // We've added some code to allow the Button to show a text tooltip while hovered. 
    internal class UIHoverImageToggleButton : UIImageButton
    {
        internal string HoverText;

        public bool IsChecked = false;

        public delegate void CheckEvent(bool val);

        public event CheckEvent OnChecked;

        Texture2D texture_checked;
        Texture2D texture_unchecked;
        public UIHoverImageToggleButton(Texture2D texture_checked, Texture2D texture_unchecked, string hoverText) : base(texture_unchecked)
        {
            HoverText = hoverText;
            this.OnClick += new MouseEvent(PlayButtonClicked);
            this.texture_checked = texture_checked;
            this.texture_unchecked = texture_unchecked;


        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            if (IsMouseHovering)
            {
                Main.hoverItemName = HoverText;
            }
        }

        private void PlayButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            this.IsChecked = !IsChecked;

            if (IsChecked)
            {
                base.SetImage(texture_checked);
            }
            else
            {
                base.SetImage(texture_unchecked);
            }

            OnChecked?.Invoke(this.IsChecked);
        }
    }
}