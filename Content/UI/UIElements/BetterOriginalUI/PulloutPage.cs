using log4net.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Timers;
using MysteriousAlchemy.Utils;
using System.Drawing;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    /// <summary>
    /// 可以向四个方向拉出的Panel
    /// </summary>
    public class PulloutPage : UIElement
    {
        public Pull_outButtom OpenButtom;
        public PulloutInnerPanel innerPanel;

        private bool _open;
        public bool Open { get { return _open; } }

        private bool _InAnimation;
        public bool InAnimation { get { return _InAnimation; } }

        //计时器，用来控制动画
        private Timer AnimationTimer;
        private int AnimationTime;

        //不该动的边
        private Vector2 StaticEdgeRange;
        //要动的边
        private Vector2 ActiveEdgeRange;
        //超哪边动
        public Vector2 PulloutVector = Vector2.UnitX;
        //按钮的坐标偏移
        public Vector2 BottomOriginOffest;
        //右下角坐标
        public Vector2 RightButtom;
        private Vector2 widthRange { get { return (PulloutVector == Vector2.UnitX || PulloutVector == -Vector2.UnitX) ? ActiveEdgeRange : StaticEdgeRange; } }
        private Vector2 heightRange { get { return (PulloutVector == Vector2.UnitY || PulloutVector == -Vector2.UnitY) ? ActiveEdgeRange : StaticEdgeRange; } }

        public PulloutPage(Pull_outButtom pull_OutButtom, int AnimationTime, Vector2 StaticEdgeRange, Vector2 ActiveEdgeRange, Vector2 PulloutVector, Vector2 bottomOriginOffest)
        {
            OpenButtom = pull_OutButtom;
            OpenButtom.OnLeftClick += ClickOpenButtom;
            BottomOriginOffest = bottomOriginOffest;
            this.AnimationTime = AnimationTime;
            this.PulloutVector = PulloutVector;
            if (!CheckPulloutVectorSafe())
                throw new System.Exception("PulloutPage.PulloutVector输入值不是单位向量");
            this.StaticEdgeRange = StaticEdgeRange;
            this.ActiveEdgeRange = ActiveEdgeRange;
            innerPanel = new PulloutInnerPanel();
            innerPanel.Top.Set(0, 0);
            innerPanel.Left.Set(0, 0);
            innerPanel.Width.Set(0, 1);
            innerPanel.Height.Set(0, 1);
            base.Append(innerPanel);

            AnimationTimer = TimerSystem.RegisterTimer<Timer>(1, AnimationTime, IsUI_Timer: true);
            AnimationTimer.pause = true;

            float targetWidth = Open ? widthRange.Y : widthRange.X;
            float targetHeight = Open ? heightRange.Y : heightRange.X;
            Width.Set(targetWidth, 0);
            Height.Set(targetHeight, 0);

        }

        public new void Append(UIElement element)
        {
            innerPanel.Append(element);
        }
        private bool CheckPulloutVectorSafe()
        {
            if (PulloutVector == Vector2.UnitX || PulloutVector == -Vector2.UnitX || PulloutVector == Vector2.UnitY || PulloutVector == -Vector2.UnitY)
                return true;
            return false;
        }
        public void AnimationUpdate()
        {
            AnimationTimer.pause = !InAnimation;
            OpenButtom.IsInAnimation = InAnimation;
            innerPanel.Open = Open;
            float targetWidth = Open ? widthRange.Y : widthRange.X;
            float targetHeight = Open ? heightRange.Y : heightRange.X;
            //计算面板的位置和大小
            if (InAnimation)
            {
                OpenButtom.PulloutInter = AnimationTimer.GetEaseInter();
                float _nowWidth = MathHelper.Lerp(!Open ? widthRange.Y : widthRange.X, targetWidth, AnimationTimer.GetEaseInter());
                float _nowHeight = MathHelper.Lerp(!Open ? heightRange.Y : heightRange.X, targetHeight, AnimationTimer.GetEaseInter());
                Width.Set(_nowWidth, 0);
                Height.Set(_nowHeight, 0);
                //top，和left需要改变
                if (PulloutVector == -Vector2.UnitX || PulloutVector == -Vector2.UnitY)
                {
                    Left.Set(RightButtom.X - _nowWidth, 0);
                    Top.Set(RightButtom.Y - _nowHeight, 0);
                }
                Recalculate();
            }
            //计算按钮位置
            RectangleF UIElemntD = new RectangleF(Left.GetValue(Parent.Width.Pixels), Top.GetValue(Parent.Height.Pixels), Width.GetValue(Parent.Width.Pixels), Height.GetValue(Parent.Width.Pixels));

            float topPixel = (PulloutVector == Vector2.UnitX || PulloutVector == -Vector2.UnitX) ? UIElemntD.Y + UIElemntD.Height / 2f : (PulloutVector == Vector2.UnitY ? UIElemntD.Y + UIElemntD.Height : UIElemntD.Y);
            float leftPixel = (PulloutVector == Vector2.UnitY || PulloutVector == -Vector2.UnitY) ? UIElemntD.X + UIElemntD.Width / 2f : (PulloutVector == Vector2.UnitX ? UIElemntD.X + UIElemntD.Width : UIElemntD.X);

            OpenButtom.Top.Set(-OpenButtom.Height.Pixels / 2f + topPixel + BottomOriginOffest.Y, 0);
            OpenButtom.Left.Set(-OpenButtom.Width.Pixels + leftPixel + BottomOriginOffest.X, 0);
            OpenButtom.Recalculate();

            //动画结束后该干的事
            AnimationTimer.ConditionTrigger(InAnimation, () =>
            {
                _InAnimation = false;
                if (PulloutVector == -Vector2.UnitX || PulloutVector == -Vector2.UnitY)
                {
                    Left.Set(RightButtom.X - targetWidth, 0);
                    Top.Set(RightButtom.Y - targetHeight, 0);
                }
                Width.Set(targetWidth, 0);
                Height.Set(targetHeight, 0);

                Recalculate();

                AnimationTimer.pause = true;
                AnimationTimer.ResetTimer();
            });
        }
        private void ClickOpenButtom(UIMouseEvent evt, UIElement listeningElement)
        {
            _open = !Open;
            OpenButtom.Pullout = Open;
            _InAnimation = true;

            AnimationTimer.pause = false;
            AnimationTimer.ResetTimer();

            var d = GetDimensions();
            if (Open)
                RightButtom = new Vector2(d.X + d.Width, d.Y + d.Height);
        }

        public void SetOpen(bool value)
        {
            if (Open == value)
                return;
            _open = value;
            OpenButtom.Pullout = Open;
            _InAnimation = true;

            AnimationTimer.pause = false;
            AnimationTimer.ResetTimer();

            var d = GetDimensions();
            if (Open)
                RightButtom = new Vector2(d.X + d.Width, d.Y + d.Height);
        }
        public void SetOpenNoAnimation(bool value)
        {
            if (Open == value)
                return;
            _open = value;
            OpenButtom.Pullout = Open;
            OpenButtom.PulloutInter = 1;
            float targetWidth = Open ? widthRange.Y : widthRange.X;
            float targetHeight = Open ? heightRange.Y : heightRange.X;
            if (PulloutVector == -Vector2.UnitX || PulloutVector == -Vector2.UnitY)
            {
                Left.Set(RightButtom.X - targetWidth, 0);
                Top.Set(RightButtom.Y - targetHeight, 0);
            }
            Width.Set(targetWidth, 0);
            Height.Set(targetHeight, 0);

            Recalculate();

            var d = GetDimensions();
            if (Open)
                RightButtom = new Vector2(d.X + d.Width, d.Y + d.Height);
        }





        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            IgnoresMouseInteraction = InAnimation || !Open;
            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
            AnimationUpdate();
            base.Update(gameTime);
        }
    }
}