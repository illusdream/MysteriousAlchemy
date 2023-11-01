using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Items.TestItem
{
    public class Link : ModItem
    {
        public AlchemyEntity FristSelect;
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
            Item.maxStack = 1;
            Item.consumable = false;
            Item.useAnimation = 1;
            Item.useTime = 1;
            Item.useStyle = ItemUseStyleID.Swing;
        }
        public override bool? UseItem(Player player)
        {
            if (AlchemySystem.TryGetEtherEntity<AlchemyEntity>(Main.MouseWorld, out var select))
            {
                if (FristSelect != null && FristSelect?.unicode != select.unicode)
                {
                    AlchemySystem.etherGraph.AddLink(FristSelect, select, (o) =>
                    {
                        o.EtherCountPerFrame = 0.1f;

                    });
                    AlchemySystem.subordinateGraph.AddLink(FristSelect, select, (o) =>
                    {


                    });
                    FristSelect = null;
                    return base.UseItem(player);
                }

                FristSelect = select;
            }
            return base.UseItem(player);
        }
        public override void UpdateInventory(Player player)
        {
            if (FristSelect != null)
                DebugUtils.NewText(FristSelect?.unicode.value);
            base.UpdateInventory(player);
        }
    }
}