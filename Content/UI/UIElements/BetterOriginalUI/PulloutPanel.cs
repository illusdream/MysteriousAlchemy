using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System.Xml.Linq;
using Terraria;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    /// <summary>
    /// <see cref="Append(UIElement)"/>为向内部的Panel添加UI元素
    /// </summary>
    public class PulloutPanel : UIElement
    {
        public bool Open = false;
        public bool InAnimation = false;
        private Timer AnimationTimer;

        private PulloutInnerPanel MainPanel;
        private Pull_outButtom OpenButtom;

        private float _OpenWidth;
        private float OpenWidth
        {
            get { return _OpenWidth - 11; }
        }
        public Vector2 PulloutVector = Vector2.UnitX;
        public PulloutPanel(float width, float widthpercent = 0)
        {
            AnimationTimer = TimerSystem.RegisterTimer<Timer>(1, 15, true);
            AnimationTimer.pause = true;

            _OpenWidth = width;
            //额外按钮的宽度
            Width.Set(width + 11, widthpercent);

            MainPanel = new PulloutInnerPanel();
            MainPanel.Top.Set(0, 0);
            MainPanel.Left.Set(0, 0);
            MainPanel.Height.Set(0, 1f);
            MainPanel.Width.Set(0, 0);
            base.Append(MainPanel);

            OpenButtom = new Pull_outButtom(PulloutVector);
            OpenButtom.Top.Set(-OpenButtom.Height.Pixels / 2f, 0.5f);
            OpenButtom.Left.Set(11, 0);
            OpenButtom.OnLeftClick += PulloutSearchPanel;


            base.Append(OpenButtom);
        }
        public void SetPulloutVector(Vector2 vector2)
        {
            PulloutVector = vector2;
            OpenButtom.PulloutVector = vector2;
        }
        private void PulloutSearchPanel(Terraria.UI.UIMouseEvent evt, Terraria.UI.UIElement listeningElement)
        {
            Open = !Open;
            InAnimation = true;
            AnimationTimer.ResetTimer();
            AnimationTimer.pause = false;
            Pull_outButtom instance = (Pull_outButtom)listeningElement;
            instance.PulloutInter = AnimationTimer.GetEaseInter();
            instance.Pullout = !instance.Pullout;
        }

        public override void Update(GameTime gameTime)
        {
            MainPanel.Open = Open;
            AnimationUpdate();

            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
            base.Update(gameTime);
        }
        public void AnimationUpdate()
        {
            OpenButtom.IsInAnimation = InAnimation;
            MainPanel.InAnimation = InAnimation;
            IgnoresMouseInteraction = InAnimation;

            if (InAnimation)
            {

                float targetWidth = Open ? OpenWidth : 22;
                float _nowWidth = MathHelper.Lerp(!Open ? OpenWidth : 22, targetWidth, AnimationTimer.GetEaseInter());
                OpenButtom.PulloutInter = AnimationTimer.GetEaseInter();
                OpenButtom.Left.Set(_nowWidth - OpenButtom.Width.Pixels / 2f, 0);

                MainPanel.Width.Set(_nowWidth, 0);
                Width.Set(_nowWidth + OpenButtom.Width.Pixels / 2f, 0);
                Recalculate();
            }
            if (!Open && !InAnimation && MainPanel.Width.Pixels != 11)
            {
                MainPanel.Width.Set(22, 0);
                Width.Set(22 + OpenButtom.Width.Pixels / 2f, 0);
                Recalculate();
            }
            if (Open && !InAnimation && MainPanel.Width.Pixels != OpenWidth)
            {
                MainPanel.Width.Set(OpenWidth, 0);
                Width.Set(OpenWidth + OpenButtom.Width.Pixels / 2f, 0);
                Recalculate();
            }

            AnimationTimer.ConditionTrigger(InAnimation, () =>
            {
                AnimationTimer.pause = true;
                AnimationTimer.ResetTimer();
                InAnimation = false;
            });
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        /// <summary>
        /// 向内部的Panel添加UI元素
        /// </summary>
        /// <param name="element"></param>
        public new void Append(UIElement element)
        {
            MainPanel.Append(element);
        }
        public override void MouseOver(UIMouseEvent evt)
        {
            Main.LocalPlayer.mouseInterface = true;
            base.MouseOver(evt);
        }
    }

    public class PulloutInnerPanel : UIPanel
    {
        public bool Open = false;
        public bool InAnimation = false;
        public PulloutInnerPanel()
        {
            BackgroundColor = new Color(63, 82, 151);
            OverflowHidden = true;
        }
        public override void Update(GameTime gameTime)
        {
            IgnoresMouseInteraction = !Open;
            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
            if (Open && !InAnimation)
                base.Update(gameTime);
        }
        protected override void DrawChildren(SpriteBatch spriteBatch)
        {
            if (Open && !InAnimation)
                base.DrawChildren(spriteBatch);
        }
    }
}
