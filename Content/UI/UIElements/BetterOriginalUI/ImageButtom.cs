using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Utils;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    public class ImageButtom : UIElement
    {
        private string _texture;

        private float _visibilityActive = 1f;

        private float _visibilityInactive = 0.4f;

        private string _borderTexture;

        public ImageButtom(string texture)
        {
            _texture = texture;
        }

        public void SetHoverImage(string texture)
        {
            _borderTexture = texture;
        }

        public void SetImage(string texture)
        {
            _texture = texture;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            spriteBatch.Draw(AssetUtils.GetTexture2D(_texture), dimensions.ToRectangle(), Color.White * (base.IsMouseHovering ? _visibilityActive : _visibilityInactive));
            if (_borderTexture != null && base.IsMouseHovering)
            {
                spriteBatch.Draw(AssetUtils.GetTexture2D(_borderTexture), dimensions.Position(), Color.White);
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