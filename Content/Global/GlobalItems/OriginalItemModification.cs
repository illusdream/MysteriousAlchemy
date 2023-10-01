using Microsoft.Xna.Framework;
using MysteriousAlchemy.Items;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Global.Item
{
    public class OriginalItemModification : GlobalItem
    {
        public override void AddRecipes()
        {
            //魔法书合成
            Recipe MagicBook = Recipe.Create(531);
            MagicBook.AddIngredient(ModContent.ItemType<CavityCrystal>());
            MagicBook.AddIngredient(ItemID.Book);
            MagicBook.AddIngredient(ItemID.ManaCrystal, 3);
            MagicBook.AddTile(TileID.Bookcases);
            MagicBook.Register();

            //水矢合成
            Recipe WaterBolt = Recipe.Create(ItemID.WaterBolt);
            WaterBolt.AddIngredient(ItemID.SpellTome, 1);
            WaterBolt.AddIngredient(ItemID.BottledWater, 10);
            WaterBolt.AddIngredient(ItemID.FallenStar, 5);
            WaterBolt.AddTile(TileID.Bookcases);
            WaterBolt.Register();
            base.AddRecipes();
        }
    }
}