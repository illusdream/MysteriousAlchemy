using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    //纯纯偷懒，
    public class ButtomWithImageText : UIPanel
    {
        public UIImageText imageText;
        public ButtomWithImageText(string LocalizationText, float Height)
        {
            imageText = new UIImageText(LocalizationText, Height);
            imageText.VAlign = 0.5f;
            imageText.HAlign = 0.5f;
            Append(imageText);
            this.Height.Set(Height + 10, 0);
            BackgroundColor = new Color(63, 82, 151);
            this.Width.Set(imageText.Width.Pixels + 20, 0);
            SetPadding(5);
        }
        public void SetLocalizationText(string text)
        {
            imageText.SetLocalization(text);
        }
    }
}
