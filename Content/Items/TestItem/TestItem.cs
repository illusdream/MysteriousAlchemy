using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Utils;
using MysteriousAlchemy.Content.Animators;
using MysteriousAlchemy.Content.Dusts;
using MysteriousAlchemy.Content.Particles;
using MysteriousAlchemy.Content.Projectiles.Chilliness;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Loader;
using MysteriousAlchemy.Core.Systems;
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
            Item.shoot = ModContent.ProjectileType<FrostEssenceSwordProj>();
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.autoReuse = false;
        }

        public override bool? UseItem(Player player)
        {
            for (int i = 0; i < 20; i++)
            {
                Particle.NewParticle<Particles.Fog>(Main.MouseWorld, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.One).RotatedBy(Main.rand.NextFloat(0, 1)) * 5 * Main.rand.NextFloat(0.7f, 1), new Color(131, 189, 238), 0.1f);
            }
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
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}