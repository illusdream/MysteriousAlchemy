using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.LinkShelfs;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.IOPages.LinkPages
{
    public class EtherLinkIOPage : LinkIOPage
    {
        public EtherLinkIOPage(Pull_outButtom pull_OutButtom, int AnimationTime, Vector2 StaticEdgeRange, Vector2 ActiveEdgeRange, Vector2 PulloutVector, Vector2 bottomOriginOffest) : base(pull_OutButtom, AnimationTime, StaticEdgeRange, ActiveEdgeRange, PulloutVector, bottomOriginOffest)
        {

            UIList.Height.Set(0, 0.7f);
            UIList.Top.Set(0, 0.3f);

            UIScrollbar.Height.Set(-6, 0.7f);
            UIScrollbar.Top.Set(0, 0.3f);
            AddDividingLine();
        }
        public void AddDividingLine()
        {
            DividingLine dividingLine_List_Opreator = new DividingLine(new Terraria.UI.StyleDimension(0, 1f));
            dividingLine_List_Opreator.Top.Set(-10, 0.3f);
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
                    Append(instancce);
                }));
            }
            else
            {
                UIList.Clear();
            }
            base.ResetLinks(unicode);
        }
    }
}
