using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    /// <summary>
    /// 横向滚动条
    /// </summary>
    public class CrossScrollBar : UIElement
    {
        public bool IsDragging;

        public float ViewPosition = 0.5f;

        public float MinViewPosition;
        public float MaxViewPosition;

        private string backline = "Images/UI/Scrollbar";
        private string Toggle = "Images/UI/ScrollbarInner";


        public Action<float> OnDragging;
        public CrossScrollBar()
        {
            MinHeight.Set(24, 0f);
            MaxHeight.Set(24, 0f);
        }
        public void SetRange(float min, float max)
        {
            MinViewPosition = min;
            MaxViewPosition = max;
        }
        public float GetMappingValue()
        {
            return (MaxViewPosition - MinViewPosition) * ViewPosition;
        }
        public override void Update(GameTime gameTime)
        {
            if (IsDragging)
            {
                float move = UserInterface.ActiveInstance.MousePosition.X - GetDimensions().X;
                if ((move / (GetDimensions().Width)) != ViewPosition)
                {
                    SoundEngine.PlaySound(MASoundID.MenuTick);
                }
                ViewPosition = MathHelper.Clamp(move / (GetDimensions().Width), 0, 1);

            }
            OnDragging?.Invoke(GetMappingValue());
            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
            base.Update(gameTime);
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            ViewPosition = MathHelper.Clamp(ViewPosition, 0, 1);
            Recalculate();
            DrawBackLine(spriteBatch);
            DrawToggle(spriteBatch);
            base.DrawSelf(spriteBatch);
        }

        private void DrawBackLine(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)Main.Assets.Request<Texture2D>(backline);
            Rectangle dimensions = GetDimensions().ToRectangle();
            spriteBatch.Draw(texture, new Rectangle(dimensions.X - 12, dimensions.Y + 4, 6, 16), new Rectangle(0, 0, 6, texture.Height), Color.White);
            spriteBatch.Draw(texture, new Rectangle(dimensions.X - 6, dimensions.Y + 4, dimensions.Width + 12, 16), new Rectangle(6, 0, 8, texture.Height), Color.White);
            spriteBatch.Draw(texture, new Rectangle(dimensions.X + dimensions.Width + 6, dimensions.Y + 4, 6, 16), new Rectangle(texture.Width - 6, 0, 6, texture.Height), Color.White);
        }
        private void DrawToggle(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)Main.Assets.Request<Texture2D>(Toggle);
            Rectangle dimensions = GetDimensions().ToRectangle();
            int X = CalculateToggleX();
            spriteBatch.Draw(texture, new Rectangle(X, dimensions.Y, texture.Width, 4), new Rectangle(0, 0, texture.Width, 4), Color.White);
            spriteBatch.Draw(texture, new Rectangle(X, dimensions.Y + 4, texture.Width, 16), new Rectangle(0, 4, texture.Width, 8), Color.White);
            spriteBatch.Draw(texture, new Rectangle(X, dimensions.Y + 20, texture.Width, 4), new Rectangle(0, texture.Height - 4, texture.Width, 4), Color.White);
        }
        private int CalculateToggleX()
        {
            Texture2D texture = (Texture2D)Main.Assets.Request<Texture2D>(Toggle);
            float pixel = GetDimensions().X + GetDimensions().Width * ViewPosition;
            int target = (int)(pixel - texture.Width / 2f);
            return target;
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            IsDragging = true;
            base.LeftMouseDown(evt);
        }
        public override void LeftMouseUp(UIMouseEvent evt)
        {
            IsDragging = false;
            OnDragging?.Invoke(GetMappingValue());
            base.LeftMouseUp(evt);
        }
        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            float perScroll = 1 / 20f;
            ViewPosition += perScroll * Math.Sign(evt.ScrollWheelValue);
            ViewPosition = MathHelper.Clamp(ViewPosition, 0, 1);
            SoundEngine.PlaySound(MASoundID.MenuTick);
            base.ScrollWheel(evt);
        }
    }
}
