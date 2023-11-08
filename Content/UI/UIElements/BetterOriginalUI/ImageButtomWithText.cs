using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using MysteriousAlchemy.Utils;
using MysteriousAlchemy.Core;
using Terraria;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    public class ImageButtomWithText : UIElement
    {
        public string _texture;

        private float _visibilityActive = 1f;

        private float _visibilityInactive = 0.4f;

        public string HoverString;

        public Vector2 HoverTextDirection = new Vector2(-1, 1);
        public UIText HoverTextBox;
        public ImageButtomWithText(string TexturePath, string HoverString)
        {
            _texture = TexturePath;
            this.HoverString = HoverString;
            HoverTextBox = new UIText(HoverString);
            HoverTextBox.TextOriginX = 0;
            HoverTextBox.Width.Set(22, 0);
            HoverTextBox.Height.Set(66, 0);
            Append(HoverTextBox);
        }


        public void SetImage(string TexturePath)
        {
            _texture = TexturePath;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            if (_texture != null)
                spriteBatch.Draw(AssetUtils.GetTexture2D(_texture), dimensions.ToRectangle(), Color.White * (base.IsMouseHovering ? _visibilityActive : _visibilityInactive));
        }
        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            HoverTextBox.Top.Set(HoverTextDirection.Y > 0 ? 0 : 0, HoverTextDirection.Y > 0 ? 1 : 0);
            HoverTextBox.Left.Set(HoverTextDirection.X > 0 ? -HoverTextBox.Width.Pixels : 0, HoverTextDirection.X > 0 ? 1 : 0);
            Recalculate();
            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
                base.DrawChildren(spriteBatch);
            }

        }
        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            SoundEngine.PlaySound(MASoundID.MenuTick);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            base.MouseOut(evt);
        }

        public void SetVisibility(float whenActive, float whenInactive)
        {
            _visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
            _visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
        }
    }
}
