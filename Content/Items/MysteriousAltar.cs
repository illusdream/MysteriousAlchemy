using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Tiles;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Items
{
    public class MysteriousAltar : ModItem
    {
        public override string Texture => AssetUtils.Items + Name;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 80;
            Item.height = 48;
            Item.value = 10000;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.maxStack = 99;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<MysteriousAltarTile>();
            Item.placeStyle = 0;
        }

    }
}