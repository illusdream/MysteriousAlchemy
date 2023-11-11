using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.Alchemy.Graphs.Links;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Core.Loader;
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
            BackgroundColor = new Color(63, 82, 151);

            AddInnerViewer();
            AddTipPanel();
            AddOpreationPanel();
        }

        private void ToDefaultPosition_Size_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            NodeViewer.ViewRange = new Vector2(Main.screenHeight, Main.screenHeight);
            NodeViewer.ViewCenter = Main.LocalPlayer.Center;
        }

        private void ToDefaultSize_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            NodeViewer.ViewRange = new Vector2(Main.screenHeight, Main.screenHeight);
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

        public void ResetViewPanel(AlchemyUnicode? unicode = null)
        {
            AlchemyUnicode _unicode = UIloader.GetUIState<UI_AlchemyEditor>().Selectunicode;
            if (unicode != null)
                _unicode = (AlchemyUnicode)unicode;
            NodeViewer.ResetInnerViewer(_unicode);
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
        public Vector2 ViewRange = new Vector2(Main.screenHeight, Main.screenHeight);
        //是否被拖动
        public bool IsToggle;
        //被拖动的相对距离
        public Vector2 ToggleVec = Vector2.Zero;
        //被拖动的起始坐标
        public Vector2 StartPoint = Vector2.Zero;

        public Action<AlchemyUnicode> OnMircoIconClicked;


        public Dictionary<AlchemyUnicode, AEMicroIcon> icons = new Dictionary<AlchemyUnicode, AEMicroIcon>();
        //应该只需要list，一般不查找
        public List<AE_MicroLink> microLinks = new List<AE_MicroLink>();
        public AEGraphCategory AEGraphType;

        private IconTextMoniter CenterMonitor;
        private IconTextMoniter SizeMonitor;

        public InnerNodeViewer()
        {
            OverflowHidden = true;
            AEGraphType = AEGraphCategory.Ether;
            InitializeInnerViewer();


            AddMonitor();

        }
        private void AddMonitor()
        {
            //添加提示器
            CenterMonitor = new IconTextMoniter(AssetUtils.GetUIImageTextInfoValue("AlchemyUI.Other.Monitor_ViewCenter"), 30);
            CenterMonitor.Top.Set(10, 0);
            CenterMonitor.Left.Set(10, 0);
            Append(CenterMonitor);

            SizeMonitor = new IconTextMoniter(AssetUtils.GetUIImageTextInfoValue("AlchemyUI.Other.Monitor_ViewSize"), 30);
            SizeMonitor.Top.Set(50, 0);
            SizeMonitor.Left.Set(10, 0);
            Append(SizeMonitor);
        }
        private void InitializeInnerViewer()
        {
            AlchemySystem.subordinateGraph.InWorld.ForEach((unicode) =>
            {
                if (AlchemySystem.FindAlchemyEntitySafely(unicode, out AlchemyEntity Noderesult))
                {
                    var instance = new AEMicroIcon(unicode);
                    instance.OnLeftClick += AEIconClick;
                    instance.OnHover += AEIconHovering;
                    icons.Add(unicode, instance);
                    Append(instance);

                    //初始化就是到这个，所以我就不管了
                    if (AlchemySystem.etherGraph.FindLinks(unicode, out List<EtherLink> Linksresult))
                    {
                        Linksresult.ForEach(link =>
                        {
                            var AEMicroLink = new AE_MicroLink(link);
                            Append(AEMicroLink);
                            microLinks.Add(AEMicroLink);
                        });
                    }
                }
            });
        }
        public void ResetInnerViewer(AlchemyUnicode Selectunicode)
        {
            RemoveAllChildren();
            icons.Clear();
            microLinks.Clear();
            UIloader.GetUIState<UI_AlchemyEditor>().SelectListOfAE.ForEach((unicode) =>
            {
                if (AlchemySystem.FindAlchemyEntitySafely(unicode, out AlchemyEntity Noderesult))
                {
                    DebugUtils.NewText(unicode.value);
                    var instance = new AEMicroIcon(unicode);
                    instance.OnLeftClick += AEIconClick;
                    instance.OnHover += AEIconHovering;
                    icons.Add(unicode, instance);
                    Append(instance);
                    if (unicode == Selectunicode)
                    {
                        instance.isSelect = true;
                        instance.selectTip = true;
                    }

                    AddMicroLink(UIloader.GetUIState<UI_AlchemyEditor>().AEGraphType, unicode, Selectunicode);
                }
            });

            AddMonitor();
        }


        #region AddMicroLink
        private void AddMicroLink(AEGraphCategory graphType, AlchemyUnicode unicode, AlchemyUnicode SelectUnicode)
        {
            switch (graphType)
            {
                case AEGraphCategory.Ether:
                    AddMicroLink_Ether(unicode, SelectUnicode);
                    break;
                case AEGraphCategory.Subordinate:
                    break;
                default:
                    break;
            }
        }
        private void AddMicroLink_Ether(AlchemyUnicode unicode, AlchemyUnicode SelectUnicode)
        {
            if (AlchemySystem.etherGraph.FindLinks(unicode, out List<EtherLink> Linksresult))
            {
                Linksresult.ForEach(link =>
                {
                    var AEMicroLink = new AE_MicroLink(link);
                    if (link.start == SelectUnicode)
                    {
                        AEMicroLink.IsSelect = true;
                    }
                    Append(AEMicroLink);
                    microLinks.Add(AEMicroLink);
                });
            }
        }
        #endregion
        private void AEIconClick(UIMouseEvent evt, UIElement listeningElement)
        {
            DebugUtils.NewText(((AEMicroIcon)listeningElement).unicode.value);
            OnMircoIconClicked?.Invoke(((AEMicroIcon)listeningElement).unicode);
        }
        private void AEIconHovering(AlchemyUnicode unicode)
        {
            UIloader.GetUIState<UI_AlchemyEditor>().HoverUnicode = unicode;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        private void DrawLink(SpriteBatch spriteBatch)
        {

        }
        public override void Update(GameTime gameTime)
        {
            if (IsToggle)
            {
                //实时计算拖动的距离
                ToggleVec = Main.MouseScreen - StartPoint;
                ToggleVec *= ViewRange / Main.ScreenSize.ToVector2();
            }
            CenterMonitor.SetText((IsToggle ? ViewCenter - ToggleVec : ViewCenter).Round(0).ToString());
            SizeMonitor.SetText((ViewRange / new Vector2(Main.screenHeight, Main.screenHeight)).Round().ToString());
            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            foreach (var AEMicro in icons.Values)
            {
                AEMicro.RecalculateShowPostion(IsToggle ? ViewCenter - ToggleVec : ViewCenter, ViewRange);
            }
            microLinks.ForEach(microLink =>
            {
                microLink.RecalculateShowPostion(icons[microLink.Link.start].GetDimensions().Center(), icons[microLink.Link.end].GetDimensions().Center());
            });
            base.Update(gameTime);
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            bool notInChildren = false;
            foreach (var child in Children)
            {
                notInChildren |= child.ContainsPoint(evt.MousePosition);
            }
            if (!notInChildren)
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
            ViewRange -= ViewRange * Math.Sign(evt.ScrollWheelValue) * 0.1f;
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
