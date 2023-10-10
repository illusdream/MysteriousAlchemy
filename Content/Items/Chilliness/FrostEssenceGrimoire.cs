using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Animators;
using MysteriousAlchemy.Content.Projectiles.Chilliness;
using MysteriousAlchemy.Core.System;
using MysteriousAlchemy.Projectiles;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Items.Chilliness
{
    public class FrostEssenceGrimoire : ModItem
    {
        ChillinessGrimoireAnimator animator;
        public override string Texture => AssetUtils.Weapons + Name;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 15;
            Item.value = 100;
            Item.rare = ItemRarityID.Master;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.damage = 50;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.useStyle = ItemUseStyleID.RaiseLamp;
            Item.shoot = ModContent.ProjectileType<FrostEssenceSwordProj>();
            Item.shootSpeed = 10f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            animator ??= AnimatorManager.Instance.Register<ChillinessGrimoireAnimator>();
            animator.Player = player;
            animator.Position = player.Center;
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile projectile;
            if (player.altFunctionUse == 0)
            {

                projectile = Projectile.NewProjectileDirect(source, position + MathUtils.RandomRing(new Vector2(1, 1), 100, 150), velocity, type, damage, knockback);
                animator.AddMagicCircle(projectile);

            }
            if (player.altFunctionUse == 2)
            {

            }
            return false;

        }

    }
}