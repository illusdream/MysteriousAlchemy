using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.IOPages;
using MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.IOPages.LinkPages;
using MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.IOPages.NodePages;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using rail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using static System.Net.Mime.MediaTypeNames;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    /// <summary>
    /// 显示被选中的<see cref="alch"/>
    /// </summary>
    public class AEOpreatePanel : UIPanel
    {


        public AlchemyUnicode unicode;
        public AEGraphCategory OpreateGraphType;


        public Pull_outButtom NodeOpreationPageButtom;
        public IOPage NodeOpreationPage;
        private bool IsOpen_Node;

        public Pull_outButtom LinkOpreationPageButtom;
        public IOPage LinkOpreationPage;
        private bool IsOpen_Link;

        public Pull_outButtom AdjacencyNodeOpreationPageButtom;
        public IOPage AdjacencyNodeOpreationPage;
        private bool IsOpen_AdjNode;

        CrossScrollElements crossScrollElements;

        public float PagesWidth = 400;
        public Vector2 PagesHeightRange = new Vector2(21, 460);
        public AEOpreatePanel()
        {
            AddCrossScrollElements();

        }

        private void AddCrossScrollElements()
        {
            crossScrollElements = new CrossScrollElements();
            crossScrollElements.Top.Set(32, 0);
            crossScrollElements.Left.Set(-PagesWidth / 2f, 0.5f);
            crossScrollElements.Width.Set(PagesWidth, 0);
            crossScrollElements.Height.Set(96, 0);
            crossScrollElements.ShowElementSize = new Vector2(64, 96);
            Append(crossScrollElements);
        }
        #region //防止出现两个page同时打开
        private void AdjacencyNodeOpreationPageButtom_OnLeftClick(Terraria.UI.UIMouseEvent evt, Terraria.UI.UIElement listeningElement)
        {
            NodeOpreationPage.SetOpen(false);
            LinkOpreationPage.SetOpen(false);
        }

        private void LinkOpreationPageButtom_OnLeftClick(Terraria.UI.UIMouseEvent evt, Terraria.UI.UIElement listeningElement)
        {
            NodeOpreationPage.SetOpen(false);
            AdjacencyNodeOpreationPage.SetOpen(false);
        }

        private void NodeOpreationPageButtom_OnLeftClick(Terraria.UI.UIMouseEvent evt, Terraria.UI.UIElement listeningElement)
        {
            AdjacencyNodeOpreationPage.SetOpen(false);
            LinkOpreationPage.SetOpen(false);
        }
        #endregion

        public void SetUnicode(AlchemyUnicode unicode)
        {
            this.unicode = unicode;
            OnSetUnicode_ResetCSE(unicode);
            NodeOpreationPage.SetUnicode(unicode);
            LinkOpreationPage.SetUnicode(unicode);
            AdjacencyNodeOpreationPage.SetUnicode(unicode);
        }
        private void OnSetUnicode_ResetCSE(AlchemyUnicode unicode)
        {
            crossScrollElements.Clear();
            OpreateGraphType = AEGraphCategory.Ether;
            //先获得实例
            if (AlchemySystem.FindAlchemyEntitySafely<AlchemyEntity>(unicode, out var result))
            {
                //反射获取应该加入哪些操作面板
                if (result.GetType().IsSubclassOf(typeof(AlchemyEntity)))
                {
                    crossScrollElements.AddIcon(new CSEIcon(AssetUtils.UI_Alchemy + "OpreationType_Ether", AEGraphCategory.Ether), OnSwitchOpreateGraphType);

                    crossScrollElements.AddIcon(new CSEIcon(AssetUtils.UI_Alchemy + "AEmciroIcon_0", AEGraphCategory.Subordinate), OnSwitchOpreateGraphType);
                }
            }
            OnSwitchOpreateGraphType(AEGraphCategory.Ether);

        }
        //加入操作面板的逻辑
        private void OnSwitchOpreateGraphType(object graphCategory)
        {
            IsOpen_Node = NodeOpreationPage == null ? false : NodeOpreationPage.Open;
            IsOpen_Link = LinkOpreationPage == null ? false : LinkOpreationPage.Open;
            IsOpen_AdjNode = AdjacencyNodeOpreationPage == null ? false : AdjacencyNodeOpreationPage.Open;
            RemoveAllPulloutPage();
            OpreateGraphType = (AEGraphCategory)graphCategory;
            switch (OpreateGraphType)
            {
                case AEGraphCategory.Ether:
                    ResetIOPanel_Ether();
                    break;
                case AEGraphCategory.Subordinate:
                    break;
                default:
                    break;
            }
        }

        private void ResetIOPanel_Ether()
        {
            NodeOpreationPageButtom = new Pull_outButtom(Vector2.UnitY);
            NodeOpreationPageButtom.OnLeftClick += NodeOpreationPageButtom_OnLeftClick;
            LinkOpreationPageButtom = new Pull_outButtom(Vector2.UnitY);
            LinkOpreationPageButtom.OnLeftClick += LinkOpreationPageButtom_OnLeftClick;
            AdjacencyNodeOpreationPageButtom = new Pull_outButtom(Vector2.UnitY);
            AdjacencyNodeOpreationPageButtom.OnLeftClick += AdjacencyNodeOpreationPageButtom_OnLeftClick;

            NodeOpreationPage = new EtherNodeIOPage(NodeOpreationPageButtom, 20, new Vector2(PagesWidth, PagesWidth), PagesHeightRange, Vector2.UnitY, Vector2.Zero);
            NodeOpreationPage.Top.Set(160, 0);
            NodeOpreationPage.Left.Set(-PagesWidth / 2f, 0.5f);

            LinkOpreationPage = new EtherLinkIOPage(LinkOpreationPageButtom, 20, new Vector2(PagesWidth, PagesWidth), PagesHeightRange, Vector2.UnitY, new Vector2(-22, 0));
            LinkOpreationPage.Top.Set(160, 0);
            LinkOpreationPage.Left.Set(-PagesWidth / 2f, 0.5f);

            AdjacencyNodeOpreationPage = new IOPage(AdjacencyNodeOpreationPageButtom, 20, new Vector2(PagesWidth, PagesWidth), PagesHeightRange, Vector2.UnitY, new Vector2(22, 0));
            AdjacencyNodeOpreationPage.Top.Set(160, 0);
            AdjacencyNodeOpreationPage.Left.Set(-PagesWidth / 2f, 0.5f);

            Append(NodeOpreationPage);
            Append(LinkOpreationPage);
            Append(AdjacencyNodeOpreationPage);

            Append(NodeOpreationPageButtom);
            Append(LinkOpreationPageButtom);
            Append(AdjacencyNodeOpreationPageButtom);

            ResetIOPanel_Open();
        }
        private void ResetIOPanel_Subordinate()
        {

        }
        //切换AE时防止面板一直开启动画
        private void ResetIOPanel_Open()
        {
            NodeOpreationPage.SetOpenNoAnimation(IsOpen_Node);
            LinkOpreationPage.SetOpenNoAnimation(IsOpen_Link);
            AdjacencyNodeOpreationPage.SetOpenNoAnimation(IsOpen_AdjNode);
        }
        //移除面板相关内容
        private void RemoveAllPulloutPage()
        {
            if (NodeOpreationPage != null)
                RemoveChild(NodeOpreationPage);
            if (LinkOpreationPage != null)
                RemoveChild(LinkOpreationPage);
            if (AdjacencyNodeOpreationPage != null)
                RemoveChild(AdjacencyNodeOpreationPage);
            if (NodeOpreationPageButtom != null)
                RemoveChild(NodeOpreationPageButtom);
            if (LinkOpreationPageButtom != null)
                RemoveChild(LinkOpreationPageButtom);
            if (AdjacencyNodeOpreationPageButtom != null)
                RemoveChild(AdjacencyNodeOpreationPageButtom);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }

}
