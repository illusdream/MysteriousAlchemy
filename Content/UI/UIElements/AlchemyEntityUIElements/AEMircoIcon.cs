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
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    public class AEMircoIcon : UIElement
    {
        public Vector2 PositionInWorld;
        public AlchemyUnicode unicode;
        public string mircoIcon = AssetUtils.UI_Alchemy + "AEmciroIcon_0";
        public Rectangle IconRectangle;

        public AEMircoIcon(AlchemyUnicode unicode)
        {
            this.unicode = unicode;
        }
        public void RecalculateShowPostion(Vector2 Center, Vector2 Range)
        {
            if (AlchemySystem.FindAlchemyEntitySafely<AlchemyEntity>(unicode, out AlchemyEntity result))
            {
                PositionInWorld = result.TopLeft;
                //获取对应的Icon ：还没写

                //转换坐标
                Vector2 PanelTopLeft = Center - Range / 2f;

                Vector2 RelativeCoord = PositionInWorld - PanelTopLeft;

                Left = new StyleDimension(0, RelativeCoord.X / Range.X);
                Top = new StyleDimension(0, RelativeCoord.Y / Range.Y);
                Width = new StyleDimension(result.Size.X, 0);
                Height = new StyleDimension(result.Size.Y, 0);
                Recalculate();
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            CalculatedStyle Dimension = GetDimensions();
            Rectangle target = Dimension.ToRectangle();
            spriteBatch.Draw(AssetUtils.GetTexture2D(AssetUtils.UI_Alchemy + "AEmciroIcon_0"), target, Color.White);
        }
    }
}
