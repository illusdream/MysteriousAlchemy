using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.Alchemy.Graphs.Links;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Core.Timers;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    public class AEMicroIcon : UIElement
    {
        public Vector2 PositionInWorld;
        public AlchemyUnicode unicode;
        public string mircoIcon = AssetUtils.UI_Alchemy + "AEmciroIcon_0";
        public Rectangle IconRectangle;

        private Timer AnimationTimer;
        public bool selectTip;
        public bool isSelect;

        public Action<AlchemyUnicode> OnHover;
        public AEMicroIcon(AlchemyUnicode unicode)
        {
            this.unicode = unicode;
            AnimationTimer = TimerSystem.RegisterTimer<Timer>(1, 5, IsUI_Timer: true);
        }
        public void RecalculateShowPostion(Vector2 Center, Vector2 Range)
        {
            if (AlchemySystem.FindAlchemyEntitySafely<AlchemyEntity>(unicode, out AlchemyEntity result))
            {
                PositionInWorld = result.TopLeft;
                //获取对应的Icon ：还没写 ，预计内容量较大，单独写个函数吧
                mircoIcon = result.Icon;
                //转换坐标
                Vector2 PanelTopLeft = Center - Range / 2f;

                Vector2 RelativeCoord = PositionInWorld - PanelTopLeft;

                Left = new StyleDimension(0, RelativeCoord.X / Range.X);
                Top = new StyleDimension(0, RelativeCoord.Y / Range.Y);
                if (Parent.GetDimensions().Width * result.Size.X / Range.X > result.Size.X)
                {
                    Width = new StyleDimension(0, result.Size.X / Range.X);
                }
                else
                {
                    Width = new StyleDimension(result.Size.X, 0);
                }
                if (Parent.GetDimensions().Height * result.Size.Y / Range.Y > result.Size.Y)
                {
                    Height = new StyleDimension(0, result.Size.Y / Range.Y);
                }
                else
                {
                    Height = new StyleDimension(result.Size.Y, 0);
                }
                Recalculate();
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimationTimer.ConditionTrigger(isSelect, () =>
            {
                selectTip = !selectTip;
            });
            CalculatedStyle Dimension = GetDimensions();
            Rectangle target = Dimension.ToRectangle();
            spriteBatch.Draw(AssetUtils.GetTexture2D(mircoIcon), target, Color.White);
            if (isSelect)
            {
                //描边
                List<CustomVertexInfo> LineList = new List<CustomVertexInfo>();
                LineList.Add(new CustomVertexInfo(Dimension.ToRectangle().TopLeft(), Color.Yellow, new Vector3(0, 0, 0)));
                LineList.Add(new CustomVertexInfo(Dimension.ToRectangle().TopRight(), Color.Yellow, new Vector3(0, 0, 0)));
                LineList.Add(new CustomVertexInfo(Dimension.ToRectangle().TopRight(), Color.Yellow, new Vector3(0, 0, 0)));
                LineList.Add(new CustomVertexInfo(Dimension.ToRectangle().BottomRight(), Color.Yellow, new Vector3(0, 0, 0)));
                LineList.Add(new CustomVertexInfo(Dimension.ToRectangle().BottomRight(), Color.Yellow, new Vector3(0, 0, 0)));
                LineList.Add(new CustomVertexInfo(Dimension.ToRectangle().BottomLeft(), Color.Yellow, new Vector3(0, 0, 0)));
                LineList.Add(new CustomVertexInfo(Dimension.ToRectangle().BottomLeft(), Color.Yellow, new Vector3(0, 0, 0)));
                LineList.Add(new CustomVertexInfo(Dimension.ToRectangle().TopLeft(), Color.Yellow, new Vector3(0, 0, 0)));
                Main.graphics.GraphicsDevice.Textures[0] = AssetUtils.GetTexture2D(AssetUtils.Texture + "White");
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, LineList.ToArray(), 0, 4);
            }

            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            if (IsMouseHovering)
                OnHover?.Invoke(unicode);
            base.Update(gameTime);
        }
        public override void LeftClick(UIMouseEvent evt)
        {
            selectTip = true;
            isSelect = true;
            base.LeftClick(evt);
        }
    }


    public class AE_MicroLink : UIElement
    {
        public Link Link;
        private Vector2 startPoint;
        private Vector2 endPoint;
        public bool IsSelect;
        public AE_MicroLink(Link link)
        {
            Link = link;
        }
        public void RecalculateShowPostion(Vector2 startPoint, Vector2 endPoint)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DrawUtils.DrawVertexLine(spriteBatch, startPoint, endPoint, Color.White * (IsSelect ? 1 : 0.5f));
            base.DrawSelf(spriteBatch);
        }
    }
}
