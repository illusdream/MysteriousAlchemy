using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements
{
    public class AlchemyEntityPanel : UIPanel
    {
        public bool Open = false;

        public AlchemyUnicode unicode;
        public AlchemyEntityPanel(AlchemyUnicode unicode)
        {
            this.unicode = unicode;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Open)
            {
                base.Draw(spriteBatch);

            }
            else
            {
                CalculatedStyle Dimension = GetDimensions();
                Rectangle target = Dimension.ToRectangle();
                if (AlchemySystem.FindAlchemyEntitySafely(unicode, out AlchemyEntity entity))
                {
                    target.Width = (int)(entity.Size.X * Main.UIScale);
                    target.Height = (int)(entity.Size.Y * Main.UIScale);
                }
                spriteBatch.Draw(AssetUtils.GetTexture2D(AssetUtils.UI_Alchemy + "AEmciroIcon_0"), target, Color.White);
            }

        }
        public override void Update(GameTime gameTime)
        {

            if (AlchemySystem.FindAlchemyEntitySafely(unicode, out AlchemyEntity entity))
            {
                Left.Set((entity.TopLeft.ToScreenPosition()).X, 0);
                Top.Set((entity.TopLeft.ToScreenPosition()).Y, 0);
                Width.Set(entity.Size.X, 0);
                Height.Set(entity.Size.Y, 0);
                Recalculate();
            }

            base.Update(gameTime);
        }
        public override void RightClick(UIMouseEvent evt)
        {
            Open = !Open;
            base.RightClick(evt);
        }

    }
}
