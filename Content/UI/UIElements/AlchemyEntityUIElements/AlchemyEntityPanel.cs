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
        private CalculatedStyle OpenTargetRect = new CalculatedStyle(Main.screenWidth / 2f - 500, Main.screenHeight / 2f - 330, 1000, 660);
        private CalculatedStyle CloseTargetRect = new CalculatedStyle();

        private AEOpreatePanel opreatePanel;
        private AESearchPanel searchPanel;
        private AEViewNodePanel viewNodePanel;
        private AEAdjacencyNodePanel adjacencyNodePanel;
        private AEAdjacencyLinksPanel adjacencyLinksPanel;


        public AEEditionPanel(CalculatedStyle start)
        {
            AnimationTimer = TimerSystem.RegisterTimer<Timer>(1, 30);
            CloseTargetRect = start;
            Open = true;
            AddSubPanel();
        }

        private void AddSubPanel()
        {
            AddViewNodePanel();
            AddOpreatePanel();
            AddSearchPanel();
            AddAdjacencyNodePanel();
            AddAdjacencyLinksPanel();
        }
        private void AddOpreatePanel()
        {
            float OpWidth = 170;
            opreatePanel = new AEOpreatePanel();
            opreatePanel.Top.Set(0, 0);
            opreatePanel.Left.Set(viewNodePanel.Left.Pixels + viewNodePanel.Width.Pixels, 0);
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
        private void AddAdjacencyNodePanel()
        {
            float LeftPanelWidth = 170;
            adjacencyNodePanel = new AEAdjacencyNodePanel();
            adjacencyNodePanel.Top.Set(0, 0);
            adjacencyNodePanel.Left.Set(opreatePanel.Left.Pixels + opreatePanel.Width.Pixels, 0);
            adjacencyNodePanel.Width.Set(LeftPanelWidth, 0);
            adjacencyNodePanel.Height.Set(0, 0.33f);
            Append(adjacencyNodePanel);
        }
        private void AddAdjacencyLinksPanel()
        {
            float LeftPanelWidth = 170;
            adjacencyLinksPanel = new AEAdjacencyLinksPanel();
            adjacencyLinksPanel.Top.Set(220, 0);
            adjacencyLinksPanel.Left.Set(opreatePanel.Left.Pixels + opreatePanel.Width.Pixels, 0);
            adjacencyLinksPanel.Width.Set(LeftPanelWidth, 0);
            adjacencyLinksPanel.Height.Set(0, 0.66f);
            Append(adjacencyLinksPanel);
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
            });
        }
        public override void Update(GameTime gameTime)
        {
            AnimationUpdate();
            base.Update(gameTime);
        }

    }
}
