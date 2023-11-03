using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Alchemy.AlchemyEntities;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Items.TestItem
{
    public class TestEtherProducerItem : ModItem
    {
        public override string Texture => AssetUtils.Items + Name;
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
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = ItemUseStyleID.Swing;
        }
        public override bool? UseItem(Player player)
        {

            AlchemySystem.RegisterAlchemyEntity<TestEtherProducer>(Main.MouseWorld);
            return base.UseItem(player);
        }
    }
}