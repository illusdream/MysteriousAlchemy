using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.LinkShelfs;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Loader;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.IOPages.LinkPages
{
    public class EtherLinkIOPage : LinkIOPage
    {
        ButtomWithImageText AddNewLink;
        public EtherLinkIOPage(Pull_outButtom pull_OutButtom, int AnimationTime, Vector2 StaticEdgeRange, Vector2 ActiveEdgeRange, Vector2 PulloutVector, Vector2 bottomOriginOffest) : base(pull_OutButtom, AnimationTime, StaticEdgeRange, ActiveEdgeRange, PulloutVector, bottomOriginOffest)
        {

            UIList.Height.Set(0, 0.8f);
            UIList.Top.Set(0, 0.2f);

            UIScrollbar.Height.Set(-6, 0.8f);
            UIScrollbar.Top.Set(0, 0.2f);
            AddDividingLine();


            AddNewLink = new ButtomWithImageText(AssetUtils.GetUIImageTextInfoValue("AlchemyUI.Link.CommenUI.AddNewLink"), 30);
            AddNewLink.HAlign = 0.5f;
            AddNewLink.Top.Set(-60, 0.2f);
            AddNewLink.OnLeftClick += AddNewLink_OnLeftClick;
            BaseAppend(AddNewLink);
        }

        private void AddNewLink_OnLeftClick(Terraria.UI.UIMouseEvent evt, Terraria.UI.UIElement listeningElement)
        {
            UIloader.GetUIState<UI_AlchemyEditor>().SetIsLinking(true);
        }

        public void AddDividingLine()
        {
            DividingLine dividingLine_List_Opreator = new DividingLine(new Terraria.UI.StyleDimension(0, 1f));
            dividingLine_List_Opreator.Top.Set(-10, 0.2f);
            BaseAppend(dividingLine_List_Opreator);
        }
        public override void ResetLinks(AlchemyUnicode unicode)
        {
            UIList.Clear();
            if (AlchemySystem.etherGraph.FindLinks(unicode, out var result))
            {
                result.ForEach((o =>
                {
                    LinkShowshelf instancce = new EtherLinkShowShelf(unicode, o.end);
                    instancce.removeSelf += UIList.Remove;
                    Append(instancce);
                }));
            }
            else
            {
                UIList.Clear();
            }
            base.ResetLinks(unicode);
        }
        private void AddLink()
        {

        }
    }
}
