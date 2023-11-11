using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    public class TipPanel : UIPanel
    {
        private AETipEnum tipEnum;
        private AlchemyUnicode SelectUnicode;
        private AlchemyUnicode Link1Unicode;
        private AlchemyUnicode Link2Unicode;
        public TipPanel()
        {
            BackgroundColor = new Color(63, 82, 151);
        }
        public void SetUnicode(AETipEnum tipEnum, AlchemyUnicode? Select, AlchemyUnicode? Link1, AlchemyUnicode? Link2)
        {
            SetTipEnum(tipEnum);
            if (Select != null)
                SelectUnicode = (AlchemyUnicode)Select;
            if (Link1 != null)
                Link1Unicode = (AlchemyUnicode)Link1;
            if (Link2 != null)
                Link2Unicode = (AlchemyUnicode)Link2;
        }
        public void SetTipEnum(AETipEnum tipEnum)
        {
            this.tipEnum = tipEnum;
        }
        private void AddUI()
        {
            switch (tipEnum)
            {
                case AETipEnum.Select:
                    AddUI_Select();
                    break;
                case AETipEnum.Link:
                    AddUI_Link();
                    break;
                case AETipEnum.Delete:
                    AddUI_Delete();
                    break;

                default:
                    break;
            }
        }

        private void AddUI_Select()
        {

        }
        private void AddUI_Link()
        {

        }
        private void AddUI_Delete()
        {

        }




        public enum AETipEnum
        {
            Select, Link, Delete
        }
    }
}
