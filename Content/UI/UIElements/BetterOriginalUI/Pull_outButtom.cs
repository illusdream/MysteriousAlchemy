using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Utils;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using MysteriousAlchemy.Core;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    internal class Pull_outButtom : UIElement
    {
        public float PulloutInter;

        public bool IsInAnimation;

        public bool Pullout;

        public Vector2 PulloutVector;

        private Texture2D _texture;

        private float _visibilityActive = 1f;

        private float _visibilityInactive = 0.4f;

        private Texture2D _borderTexture;
        public Pull_outButtom(Vector2 pulloutVector)
        {
            PulloutVector = pulloutVector;
            _texture = AssetUtils.GetTexture2D(AssetUtils.UI + "triangle");
            _borderTexture = AssetUtils.GetTexture2D(AssetUtils.UI + "triangleBackground");
            Width.Set(_borderTexture.Width, 0);
            Height.Set(_borderTexture.Height, 0);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (ContainsPoint(Main.MouseScreen))
                Main.LocalPlayer.mouseInterface = true;
            float DrawRotation = PulloutVector.ToRotation() + MathHelper.Pi + (Pullout ? MathHelper.Pi * PulloutInter : MathHelper.Pi + MathHelper.Pi * PulloutInter);
            CalculatedStyle dimensions = GetDimensions();
            spriteBatch.Draw(_borderTexture, dimensions.Position(), Color.White);
            if (!IsInAnimation)
            {
                spriteBatch.Draw(_texture, dimensions.Position() + _borderTexture.Size() / 2f, null, Color.White * (base.IsMouseHovering ? _visibilityActive : _visibilityInactive), DrawRotation, _texture.Size() / 2f, 1, SpriteEffects.None, 0);
            }
            if (IsInAnimation)
            {
                spriteBatch.Draw(_texture, dimensions.Position() + _borderTexture.Size() / 2f, null, Color.White, DrawRotation, _texture.Size() / 2f, 1, SpriteEffects.None, 0);
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
