using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    public class UILinkList : UIPanel
    {
        public UIList UIList;
        public UIScrollbar UIScrollbar;

        public UILinkList()
        {
            UIList = new UIList();
            UIScrollbar = new UIScrollbar();

            UIList.Width.Set(0, 1f);
            UIList.Height.Set(0, 1f);

            UIList.Top.Set(0, 0f);
            UIList.Left.Set(0, 0f);

            UIList.SetPadding(2f);
            UIList.PaddingBottom = 4f;
            UIList.PaddingTop = 4f;
            Append(UIList);
            UIList.ListPadding = 4f;


            UIScrollbar.SetView(100f, 1000f);
            UIScrollbar.Height.Set(-20f, 1f);
            UIScrollbar.HAlign = 1f;
            UIScrollbar.VAlign = 0.5f;
            UIScrollbar.Left.Set(-6f, 0f);

            UIList.SetScrollbar(UIScrollbar);
            Append(UIScrollbar);


            for (int i = 0; i < 20; i++)
            {
                var instance = new AESubPanel();
                instance.Width.Set(100, 0f);
                instance.Height.Set(66, 0f);
                UIList.Add(instance);
            }
        }
    }
}
