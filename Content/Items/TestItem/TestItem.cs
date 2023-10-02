using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Loader;
using MysteriousAlchemy.Core.System;
using MysteriousAlchemy.Utils;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Items.TestItem
{
    public class TestItem : ModItem
    {

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
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.autoReuse = false;
        }

        public override bool? UseItem(Player player)
        {
            TextDiscriptionSystem.instance.AddTextDiscription(Main.MouseWorld, "ËêËê±ä°×ÄãµÄ²âÊÔÎÄ×Öu", 255, Core.Perfab.TextSpreadMode.letter, 10, 120, 120);
            return base.UseItem(player);
        }
        public override void UpdateInventory(Player player)
        {
            base.UpdateInventory(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}