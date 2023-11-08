using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Loader;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Core.Timers;
using MysteriousAlchemy.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    public class AEEditionPanel : UIElement
    {
        private bool Open;
        public bool IsInAnimation;

        public Timer AnimationTimer;
        private CalculatedStyle OpenTargetRect = new CalculatedStyle(Main.screenWidth / 2f - 550, Main.screenHeight / 2f - 330, 1100, 660);
        private CalculatedStyle CloseTargetRect = new CalculatedStyle();

        public AEOpreatePanel opreatePanel;
        public AESearchPanel searchPanel;
        public AEViewNodePanel viewNodePanel;


        public AEEditionPanel(CalculatedStyle start)
        {
            AnimationTimer = TimerSystem.RegisterTimer<Timer>(1, 30, true);
            CloseTargetRect = start;
            Open = true;
            AddSubPanel();
        }

        private void AddSubPanel()
        {

            AddOpreatePanel();
            AddViewNodePanel();
            AddSearchPanel();
            //AddAdjacencyNodePanel();
            //AddAdjacencyLinksPanel();
        }
        private void AddOpreatePanel()
        {
            float OpWidth = 440;
            opreatePanel = new AEOpreatePanel();
            opreatePanel.Top.Set(0, 0);
            opreatePanel.Left.Set(665, 0);
            opreatePanel.Width.Set(OpWidth, 0);
            opreatePanel.Height.Set(0, 1f);
            Append(opreatePanel);
        }
        private void AddSearchPanel()
        {
            float SpWidth = 170;
            searchPanel = new AESearchPanel(170);
            searchPanel.Top.Set(0, 0);
            searchPanel.Left.Set(viewNodePanel.Left.Pixels + 3f, 0);
            searchPanel.Width.Set(SpWidth, 0);
            searchPanel.Height.Set(0, 1f);
            Append(searchPanel);

            viewNodePanel.NodeViewer.OnMircoIconClicked += OnIconClicked;
        }
        private void AddViewNodePanel()
        {
            viewNodePanel = new AEViewNodePanel();
            viewNodePanel.Top.Set(0, 0);
            viewNodePanel.Left.Set(0, 0);

            viewNodePanel.Height.Set(0, 1f);
            viewNodePanel.Width.Set(660, 0);
            Append(viewNodePanel);
        }
        public void RefreshAllPanel(AlchemyUnicode unicode)
        {
            //searchPanel
            //viewpanel
            opreatePanel.SetUnicode(unicode);
        }

        private void OnIconClicked(AlchemyUnicode unicode)
        {
            UIloader.GetUIState<UI_AlchemyEditor>().SetUnicode(unicode);
        }


        private void AnimationUpdate()
        {
            AnimationTimer.pause = !IsInAnimation;
            if (Open && IsInAnimation)
            {
                CalculatedStyle target = MathUtils.Lerp(CloseTargetRect, OpenTargetRect, AnimationTimer.GetEaseInter());

                Left.Set(target.X, 0);
                Top.Set(target.Y, 0);
                Width.Set(target.Width, 0);
                Height.Set(target.Height, 0);
                Recalculate();
            }
            if (!Open && IsInAnimation)
            {
                CalculatedStyle target = MathUtils.Lerp(OpenTargetRect, CloseTargetRect, AnimationTimer.GetEaseInter());

                Left.Set(target.X, 0);
                Top.Set(target.Y, 0);
                Width.Set(target.Width, 0);
                Height.Set(target.Height, 0);
                Recalculate();
            }
            AnimationTimer.ConditionTrigger(IsInAnimation, () =>
            {
                IsInAnimation = false;
                Left.Set(-OpenTargetRect.Width / 2f, 0.5f);
                Top.Set(-OpenTargetRect.Height / 2f, 0.5f);
                Width.Set(OpenTargetRect.Width, 0);
                Height.Set(OpenTargetRect.Height, 0);
                Recalculate();
            });
        }
        public override void Update(GameTime gameTime)
        {
            AnimationUpdate();
            base.Update(gameTime);
        }


    }
}
