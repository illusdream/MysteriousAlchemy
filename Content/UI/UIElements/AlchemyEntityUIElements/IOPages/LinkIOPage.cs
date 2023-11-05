using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Alchemy.Graphs.Links;
using MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.LinkShelfs;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.IOPages
{
    public class LinkIOPage : IOPage
    {
        public LinkIOPage(Pull_outButtom pull_OutButtom, int AnimationTime, Vector2 StaticEdgeRange, Vector2 ActiveEdgeRange, Vector2 PulloutVector, Vector2 bottomOriginOffest) : base(pull_OutButtom, AnimationTime, StaticEdgeRange, ActiveEdgeRange, PulloutVector, bottomOriginOffest)
        {


        }

        public override void OnSetUnicode(AlchemyUnicode unicode)
        {
            ResetLinks(unicode);
            base.OnSetUnicode(unicode);
        }
        public virtual void ResetLinks(AlchemyUnicode unicode)
        {

        }
    }
}
