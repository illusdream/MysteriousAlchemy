using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.TileEntitys;
using MysteriousAlchemy.Content.Tiles;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Items
{
    public class ActiveEther : ModItem
    {
        public override string Texture => AssetUtils.Ether + Name;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 1);
            Item.rare = ItemRarityID.Blue;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;
            Item.maxStack = 9999;
        }
        public override bool CanUseItem(Player player)
        {
            if (Main.tile[Main.MouseWorld.ToTileCoordinates()].TileType == ModContent.TileType<MysteriousAltarTile>())
            {
                MysteriousAltarTileEntity entity;
                TileUtils.TryGetTileEntityAs<MysteriousAltarTileEntity>(Main.MouseWorld.ToTileCoordinates().X, Main.MouseWorld.ToTileCoordinates().Y, out entity);
                entity?.altarAnimator?.AddEtherCrystal();
                Item.stack--;
            }
            return base.CanUseItem(player);
        }
    }
}