using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    /// <summary>
    /// 暂时没jb用
    /// </summary>
    public class CrossScrollBarWithText : UIElement
    {
        private CrossScrollBar CrossScrollBar;
        private UIText UIText;

        public string TextTile;

        public CrossScrollBarWithText(string textTile, float min, float max)
        {
            Height.Set(50, 0);
            MinHeight.Set(50, 0);
            MaxHeight.Set(50, 0);

            this.TextTile = textTile;
            CrossScrollBar = new CrossScrollBar();
            CrossScrollBar.Width.Set(0, 1f);
            CrossScrollBar.Top.Set(20, 0);
            CrossScrollBar.SetRange(min, max);
            Append(CrossScrollBar);

            UIText = new UIText(TextTile, 0.75f);
            UIText.Width.Set(0, 1f);
            UIText.Left.Set(0, 0);
            UIText.TextOriginX = 0f;
            UIText.Height.Set(20, 0);
            Append(UIText);

        }
        public override void Update(GameTime gameTime)
        {

            UIText.SetText(TextTile + " : " + CrossScrollBar.GetMappingValue());
            base.Update(gameTime);
        }
    }
}
