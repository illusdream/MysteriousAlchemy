using Microsoft.Xna.Framework;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Items
{
    public class CavityCrystal : ModItem
    {
        public override string Texture => AssetUtils.Items + Name;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            Recipe CommenRecipe = CreateRecipe();
            CommenRecipe.AddIngredient(ItemID.Diamond, 4);
            CommenRecipe.AddIngredient(5349, 1);
            CommenRecipe.AddTile(TileID.WorkBenches);
            CommenRecipe.DisableDecraft();
            CommenRecipe.Register();

            Recipe EvolutionRecipe = CreateRecipe();
            EvolutionRecipe.AddIngredient(ModContent.ItemType<TransmutationCrystal>());
            EvolutionRecipe.AddCondition(Condition.ZenithWorld);
            EvolutionRecipe.Register();
            base.AddRecipes();
        }
    }
}