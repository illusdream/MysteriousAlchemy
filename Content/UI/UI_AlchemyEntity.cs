using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.UI.UIElements;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI
{
    public class UI_AlchemyEntity : BetterUIState
    {
        public static bool visable = false;
        public override bool Visable => visable;
        public static UI_AlchemyEntity instance;

        public UI_AlchemyEntity()
        {
            instance ??= this;
        }
        public override int UILayer(List<GameInterfaceLayer> layers) => layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        public List<AlchemyEntityPanel> alchemyEntityPanels = new List<AlchemyEntityPanel>();

        public void SetVisable(bool value)
        {
            visable = value;
            if (value)
            {
                AlchemySystem.subordinateGraph.InWorld.ForEach((unicode) =>
                {
                    AlchemyEntityPanel panel = new AlchemyEntityPanel(unicode);
                    Append(panel);
                    alchemyEntityPanels.Add(panel);
                });
            }
            else
            {
                alchemyEntityPanels.ForEach((o) =>
                {
                    RemoveChild(o);
                });
                alchemyEntityPanels.Clear();
            }
        }
        public void ReSetListOfAEPanels()
        {
            AlchemySystem.subordinateGraph.InWorld.ForEach((o) =>
            {
                bool hasThisUnicode = false;
                alchemyEntityPanels.ForEach((p) =>
                {
                    hasThisUnicode = (p.unicode == o) || hasThisUnicode;
                });
                if (!hasThisUnicode)
                {
                    AlchemyEntityPanel panel = new AlchemyEntityPanel(o);
                    Append(panel);
                    alchemyEntityPanels.Add(panel);
                }
            });
            alchemyEntityPanels.ForEach((o) =>
            {
                bool hasThisUnicode = false;
                AlchemySystem.subordinateGraph.InWorld.ForEach((p) =>
                {
                    hasThisUnicode = (p == o.unicode) || hasThisUnicode;
                });
                if (!hasThisUnicode)
                {
                    RemoveChild(o);
                    alchemyEntityPanels.Remove(o);
                }
            });
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

    }
}
