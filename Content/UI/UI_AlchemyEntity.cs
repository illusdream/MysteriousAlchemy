using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI
{
    public class UI_AlchemyEntity : BetterUIState
    {
        public static bool visable = false;
        public override bool Visable => visable;

        public static Vector2 AEPanelSize = new Vector2(200, 300);
        public override int UILayer(List<GameInterfaceLayer> layers) => layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        public List<AEIconInWorld> _mircoIcons = new List<AEIconInWorld>();

        public void SetVisable(bool value)
        {
            visable = value;
            if (value)
            {
                CreateIconList();
            }
            else
            {
                RemoveAllIconList();
                RemoveAllChildren();
            }
        }
        public void ReSetListOfAEPanels()
        {
            #region 太麻烦了，直接重新生成不就好了 
            //AlchemySystem.subordinateGraph.InWorld.ForEach((o) =>
            //{
            //    bool hasThisUnicode = false;
            //    alchemyEntityPanels.ForEach((p) =>
            //    {
            //        hasThisUnicode = (p.unicode == o) || hasThisUnicode;
            //    });
            //    if (!hasThisUnicode)
            //    {
            //        AlchemyEntityPanel panel = new AlchemyEntityPanel(o);
            //        Append(panel);
            //        alchemyEntityPanels.Add(panel);
            //    }
            //});
            //alchemyEntityPanels.ForEach((o) =>
            //{
            //    bool hasThisUnicode = false;
            //    AlchemySystem.subordinateGraph.InWorld.ForEach((p) =>
            //    {
            //        hasThisUnicode = (p == o.unicode) || hasThisUnicode;
            //    });
            //    if (!hasThisUnicode)
            //    {
            //        RemoveChild(o);
            //        alchemyEntityPanels.Remove(o);
            //    }
            //});
            #endregion

            //更改后
            RemoveAllIconList();
            CreateIconList();
        }
        private void CreateIconList()
        {
            AlchemySystem.subordinateGraph.InWorld.ForEach((unicode) =>
            {
                AEIconInWorld panel = new AEIconInWorld(unicode);
                AddElement(panel, 0, 0, (int)AEPanelSize.X, (int)AEPanelSize.Y);
                panel.OnLeftClick += Click_OpenEdition;
                _mircoIcons.Add(panel);
            });
        }

        private void Click_OpenEdition(UIMouseEvent evt, UIElement listeningElement)
        {
            CreateEditPanel(listeningElement.GetDimensions());
        }

        private void RemoveAllIconList()
        {
            _mircoIcons.ForEach((o) =>
            {
                RemoveChild(o);
            });
            _mircoIcons.Clear();
        }
        private void CreateEditPanel(CalculatedStyle start)
        {
            RemoveAllIconList();
            AEEditionPanel EditPanel = new AEEditionPanel(start);
            EditPanel.IsInAnimation = true;
            Append(EditPanel);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
