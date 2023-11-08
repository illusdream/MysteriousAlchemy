using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.IOPages
{
    public class IOPage : PulloutPage
    {
        public AlchemyUnicode unicode;

        public UIList UIList;
        public UIScrollbar UIScrollbar;

        public Action NeedUpdate;
        public IOPage(Pull_outButtom pull_OutButtom, int AnimationTime, Vector2 StaticEdgeRange, Vector2 ActiveEdgeRange, Vector2 PulloutVector, Vector2 bottomOriginOffest) : base(pull_OutButtom, AnimationTime, StaticEdgeRange, ActiveEdgeRange, PulloutVector, bottomOriginOffest)
        {

            UIList = new UIList();
            UIScrollbar = new UIScrollbar();

            UIList.Width.Set(-25f, 1f);
            UIList.Height.Set(0, 0.95f);

            UIList.Top.Set(0, 0.05f);
            UIList.Left.Set(0, 0f);

            UIList.SetPadding(2f);
            UIList.PaddingBottom = 4f;
            UIList.PaddingTop = 4f;
            base.Append(UIList);
            UIList.ListPadding = 4f;


            UIScrollbar.SetView(100f, 1000f);
            UIScrollbar.Height.Set(-6, 0.95f);
            UIScrollbar.Top.Set(0, 0.05f);
            UIScrollbar.Left.Set(-25, 1f);

            UIList.SetScrollbar(UIScrollbar);
            base.Append(UIScrollbar);

        }
        public override void Update(GameTime gameTime)
        {
            NeedUpdate?.Invoke();
            base.Update(gameTime);
        }
        public new void Append(UIElement element)
        {
            UIList.Add(element);
        }
        protected void BaseAppend(UIElement element)
        {
            base.Append(element);
        }
        public void ListRemove(UIElement element)
        {
            UIList.Remove(element);
        }
        public void SetUnicode(AlchemyUnicode unicode)
        {
            this.unicode = unicode;
            OnSetUnicode(unicode);
        }
        public virtual void OnSetUnicode(AlchemyUnicode unicode)
        {

        }
        public virtual void OnSetOpreateGraphType(AEGraphCategory graphType)
        {

        }
    }
}
