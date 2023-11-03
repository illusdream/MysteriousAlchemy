using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Timers;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    public class AESearchPanel : PulloutPanel
    {
        public InputTextBar inputText;
        public AESearchPanel(float width, float widthpercent = 0) : base(width, widthpercent)
        {
            inputText = new InputTextBar(width - 40);
            inputText.Top.Set(5, 0);
            inputText.Left.Set(-(width - 40) / 2f, 0.5f);
            Append(inputText);
        }
    }
}
