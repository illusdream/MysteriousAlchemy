using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Core.Platforms;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    public class UISampleIcon : UIElement
    {
        private string texturePath;
        private bool StaticEdgeH;
        public UISampleIcon(string texturePath)
        {
            this.texturePath = texturePath;
        }
        public UISampleIcon(string texturePath, Tuple<float, float> height, bool StaticEdgeH = true, Tuple<float, float> width = null)
        {
            this.texturePath = texturePath;
            this.StaticEdgeH = StaticEdgeH;
            SetDimensons(null, null, width, height);
        }
        public void SetTexture(string path)
        {
            texturePath = path;
        }
        public void SetDimensons(Tuple<float, float> left, Tuple<float, float> top, Tuple<float, float> width, Tuple<float, float> height)
        {
            if (left != null)
            {
                Left.Set(left.Item1, left.Item2);
            }
            if (top != null)
            {
                Top.Set(top.Item1, top.Item2);
            }
            if (width != null)
            {
                Width.Set(width.Item1, width.Item2);
            }
            if (height != null)
            {
                Height.Set(height.Item1, height.Item2);
            }
        }
        public void CalculateCurrectSize()
        {
            Vector2 TextureSize = AssetUtils.GetTexture2D(texturePath).Size();
            //高不变
            if (StaticEdgeH)
            {
                float height = Height.GetValue(Parent.GetDimensions().Height);
                float CurrectWidth = height * (TextureSize.X / TextureSize.Y);
                Width.Set(CurrectWidth, 0);
            }
            //宽不变
            else
            {
                float width = Width.GetValue(Parent.GetDimensions().Width);
                float CurrectHeight = width * (TextureSize.Y / TextureSize.X);
                Height.Set(CurrectHeight, 0);
            }
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculateCurrectSize();
            Recalculate();
            spriteBatch.Draw(AssetUtils.GetTexture2D(texturePath), GetDimensions().ToRectangle(), Color.White);
        }
    }
}
