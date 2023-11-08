using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Enum;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.LinkShelfs
{
    public class LinkShowshelf : UIElement
    {
        public AlchemyUnicode start;
        public AlchemyUnicode end;

        public delegate bool UIListremoveSelf(UIElement element);
        public UIListremoveSelf removeSelf;
        public LinkShowshelf(AlchemyUnicode start, AlchemyUnicode end)
        {

            this.start = start;
            this.end = end;
            Width.Set(0, 1);

        }
    }
}