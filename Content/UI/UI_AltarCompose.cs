using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Tiles;
using MysteriousAlchemy.UI.UIElements;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.UI;

namespace MysteriousAlchemy.UI
{
    public class UI_AltarCompose : BetterUIState
    {
        public static bool visable = false;
        public override int UILayer(List<GameInterfaceLayer> layers) => layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        public override bool Visable => visable;

        public static MysteriousAlterTileEntity MysteriousAlterTileEntity = null;
        public override void OnInitialize()
        {
            base.OnInitialize();

        }
        public UI_AltarCompose() : base()
        {

        }
        public override void OnActivate()
        {
            base.OnActivate();

        }

        public override void Update(GameTime gameTime)
        {
            Recalculate();
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (MysteriousAlterTileEntity != null)
            {
                Recalculate();
            }
            base.Draw(spriteBatch);
        }


    }
}