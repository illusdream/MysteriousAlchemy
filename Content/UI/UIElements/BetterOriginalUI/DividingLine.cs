using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    /// <summary>
    /// 分割线
    /// </summary>
    public class DividingLine : UIElement
    {
        public string DividingLinePath = AssetUtils.Texture + "White";
        private float Alpha = 0.66f;
        private Color Color = Color.White;
        public DividingLine()
        {
            MinHeight.Set(1, 0);
            MaxHeight.Set(1, 0);
        }
        public DividingLine(StyleDimension width, float alpha = 0.66f)
        {
            Width = width;
            MinHeight.Set(1, 0);
            MaxHeight.Set(1, 0);
            Alpha = alpha;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Texture2D tex = AssetUtils.GetTexture2D(DividingLinePath);
            spriteBatch.Draw(tex, GetDimensions().ToRectangle(), Color * Alpha);
        }
        public void SetAlpha(float alpha)
        {
            Alpha = alpha;
        }
        /// <summary>
        /// 必须是纯色，不能写透明度在里面
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            Color = color;
        }
    }
}
