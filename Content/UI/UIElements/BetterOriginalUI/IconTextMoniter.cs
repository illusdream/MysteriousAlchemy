using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{

    public class IconTextMoniter : UIElement
    {
        UIImageText IconText;
        UIText MonitorText;

        public IconTextMoniter(string LocalizationText, float heightPiexl)
        {
            Height.Set(heightPiexl, 0);

            IconText = new UIImageText(LocalizationText, heightPiexl);
            IconText.Height.Set(0, 1f);

            MonitorText = new UIText("");
            MonitorText.TextOriginX = 0;
            MonitorText.TextOriginY = 0;
            MonitorText.VAlign = 0.5f;

            Append(IconText);
            Append(MonitorText);


            MonitorText.Left.Set(IconText.Left.Pixels + IconText.Width.Pixels, 0);
            Width.Set(IconText.Width.Pixels + MonitorText.Width.Pixels, 0);
        }

        public override void Update(GameTime gameTime)
        {
            MonitorText.Left.Set(IconText.Left.Pixels + IconText.Width.Pixels, 0);
            Width.Set(IconText.Width.Pixels + MonitorText.MinWidth.Pixels, 0);
            base.Update(gameTime);
        }
        public void SetText(string text)
        {
            MonitorText.SetText(text);
        }
        public void SetLocalization(string localization)
        {
            IconText.SetLocalization(localization);
        }
    }
}
