using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Utils;
using MysteriousAlchemy.Content.Animators;
using MysteriousAlchemy.Content.Dusts;
using MysteriousAlchemy.Content.Particles;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Loader;
using MysteriousAlchemy.Core.System;
using MysteriousAlchemy.Utils;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

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
            Particle.NewParticle<Particles.Fog>(Main.MouseWorld, Vector2.Zero, Color.White, 0.5f);
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