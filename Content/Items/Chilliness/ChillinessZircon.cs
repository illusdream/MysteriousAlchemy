using Microsoft.Xna.Framework;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Items.Chilliness
{
    public class ChillinessZircon : ModItem
    {
        public override string Texture => AssetUtils.ItemChilliness + Name;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 15;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.maxStack = 99;
        }
    }
}