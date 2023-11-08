using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Chat.Commands;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    public class AEViewNodePanel : UIPanel
    {
        public InnerNodeViewer NodeViewer;
        public NodeViewOpreationPanel OpreationPanel;
        public UIPanel TipPanel;
        public AEViewNodePanel()
        {


            AddInnerViewer();
            AddTipPanel();
            AddOpreationPanel();
        }

        private void ToDefaultPosition_Size_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            NodeViewer.ViewRange = new Vector2(Main.screenWidth, Main.screenHeight);
            NodeViewer.ViewCenter = Main.LocalPlayer.Center;
        }

        private void ToDefaultSize_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            NodeViewer.ViewRange = new Vector2(Main.screenWidth, Main.screenHeight);
        }

        private void ToDefaultCenter_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            NodeViewer.ViewCenter = Main.LocalPlayer.Center;
        }

        private void AddInnerViewer()
        {
            NodeViewer = new InnerNodeViewer();
            NodeViewer.Width.Set(-44, 1);
            NodeViewer.Height.Set(660 - 44, 0);
            NodeViewer.Top.Set(-660 + 44, 1);
            NodeViewer.Left.Set(+44, 0);
            Append(NodeViewer);
        }
        private void AddOpreationPanel()
        {
            OpreationPanel = new NodeViewOpreationPanel();
            OpreationPanel.Width.Set(44, 0);
            OpreationPanel.Height.Set(0, 1);
            OpreationPanel.Top.Set(0, 0);
            OpreationPanel.Left.Set(0, 0);

            OpreationPanel.ToDefaultCenter.OnLeftClick += ToDefaultCenter_OnLeftClick;
            OpreationPanel.ToDefaultSize.OnLeftClick += ToDefaultSize_OnLeftClick;
            OpreationPanel.ToDefaultPosition_Size.OnLeftClick += ToDefaultPosition_Size_OnLeftClick;
            Append(OpreationPanel);
        }
        private void AddTipPanel()
        {
            TipPanel = new UIPanel();
            TipPanel.Width.Set(-44, 1);
            TipPanel.Height.Set(190, 0);
            TipPanel.Top.Set(0, 0);
            TipPanel.Left.Set(44, 0);
            Append(TipPanel);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.RightMouseDown(evt);
        }
        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.RightMouseUp(evt);
        }
        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
        }
    }

    /// <summary>
    /// 可以拖动内部元素
    /// </summary>
    public class InnerNodeViewer : UIElement
    {
        //初始映射坐标的中心
        public Vector2 ViewCenter = Main.LocalPlayer.Center;
        //初始映射坐标的范围
        public Vector2 ViewRange = new Vector2(Main.screenWidth, Main.screenWidth);
        //是否被拖动
        public bool IsToggle;
        //被拖动的相对距离
        public Vector2 ToggleVec = Vector2.Zero;
        //被拖动的起始坐标
        public Vector2 StartPoint = Vector2.Zero;

        public Action<AlchemyUnicode> OnMircoIconClicked;


        public List<AEMircoIcon> icons = new List<AEMircoIcon>();

        public InnerNodeViewer()
        {
            OverflowHidden = true;

            AddMircoIcon();

        }
        private void AddMircoIcon()
        {
            AlchemySystem.subordinateGraph.InWorld.ForEach((o) =>
            {
                if (AlchemySystem.FindAlchemyEntitySafely(o, out AlchemyEntity result))
                {
                    var instance = new AEMircoIcon(o);
                    instance.OnLeftClick += AEIconClick;
                    icons.Add(instance);
                    Append(instance);
                }
            });
        }

        private void AEIconClick(UIMouseEvent evt, UIElement listeningElement)
        {
            DebugUtils.NewText(((AEMircoIcon)listeningElement).unicode.value);
            OnMircoIconClicked?.Invoke(((AEMircoIcon)listeningElement).unicode);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsToggle)
            {
                //实时计算拖动的距离
                ToggleVec = Main.MouseScreen - StartPoint;
            }
            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            icons.ForEach((o) =>
            {
                o.RecalculateShowPostion(IsToggle ? ViewCenter - ToggleVec : ViewCenter, ViewRange);
            });
            base.Update(gameTime);
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            IsToggle = true;
            StartPoint = Main.MouseScreen;
            base.RightMouseDown(evt);
        }
        public override void LeftMouseUp(UIMouseEvent evt)
        {
            IsToggle = false;
            ViewCenter -= ToggleVec;
            ToggleVec = Vector2.Zero;
            base.RightMouseUp(evt);
        }
        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            ViewRange -= new Vector2(evt.ScrollWheelValue);
            ViewRange = new Vector2(Math.Clamp(ViewRange.X, 0, Main.rightWorld), Math.Clamp(ViewRange.Y, 0, Main.bottomWorld));
            base.ScrollWheel(evt);
        }
    }

    public class NodeViewOpreationPanel : UIPanel
    {
        public ImageButtomWithText ToDefaultCenter;
        public ImageButtomWithText ToDefaultSize;
        public ImageButtomWithText ToDefaultPosition_Size;
        public NodeViewOpreationPanel()
        {
            Width.Set(44, 0);
            ToDefaultCenter = new ImageButtomWithText(AssetUtils.UI + "ToDefaultPosition", "ToDefaultPosition");
            ToDefaultCenter.Width.Set(22, 0);
            ToDefaultCenter.Height.Set(22, 0);
            ToDefaultCenter.Top.Set(11, 0);
            ToDefaultCenter.Left.Set(-11, 0.5f);
            Append(ToDefaultCenter);

            ToDefaultSize = new ImageButtomWithText(AssetUtils.UI + "ToDefaultSize", "ToDefaultSize");
            ToDefaultSize.Width.Set(22, 0);
            ToDefaultSize.Height.Set(22, 0);
            ToDefaultSize.Top.Set(44, 0);
            ToDefaultSize.Left.Set(-11, 0.5f);
            Append(ToDefaultSize);

            ToDefaultPosition_Size = new ImageButtomWithText(AssetUtils.UI + "ToDefaultPosition_Size", "ToDefaultPosition&Size");
            ToDefaultPosition_Size.Width.Set(22, 0);
            ToDefaultPosition_Size.Height.Set(22, 0);
            ToDefaultPosition_Size.Top.Set(77, 0);
            ToDefaultPosition_Size.Left.Set(-11, 0.5f);
            Append(ToDefaultPosition_Size);
        }
        public override void Update(GameTime gameTime)
        {
            if (IsMouseHovering)
                Main.LocalPlayer.mouseInterface = true;
            base.Update(gameTime);
        }
    }
}
