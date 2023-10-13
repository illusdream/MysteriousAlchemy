using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Animators;
using MysteriousAlchemy.Content.Global.ModPlayers;
using MysteriousAlchemy.Content.Projectiles.Chilliness;
using MysteriousAlchemy.Core;
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
            Item.value = Item.buyPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.damage = 84;
            Item.crit = 13;
            Item.mana = 10;
            Item.knockBack = 2;
            Item.DamageType = DamageClass.Magic;
            Item.UseSound = MASoundID.IceMagic_Item28;
            Item.autoReuse = true;
            Item.maxStack = 1;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<FrostEssenceSwordProj>();
            Item.shootSpeed = 15f;
        }
        public override bool MagicPrefix()
        {
            return true;
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
                for (int i = 0; i < Main.rand.Next(1, 4); i++)
                {
                    Vector2 randomShootPos = MathUtils.RandomRingRange(-MathHelper.Pi - MathHelper.PiOver4, MathHelper.PiOver4, 100, 200);
                    projectile = Projectile.NewProjectileDirect(source, position + randomShootPos, velocity, type, damage, knockback);
                    animator.AddIceKingsTreasure(projectile.Center - player.Center);
                }
            }
            if (player.altFunctionUse == 2 && player.GetModPlayer<PLayerChilliness>().EnhanceChilliness)
            {
                if (player.GetModPlayer<PLayerChilliness>().FrostEssenceCrystalCount < player.GetModPlayer<PLayerChilliness>().MaxfrostEssenceCrystalCount)
                {
                    projectile = Projectile.NewProjectileDirect(source, Main.MouseWorld, velocity, ModContent.ProjectileType<FrostEssenceCrystalBeam>(), (int)(damage * 0.65f), knockback);
                    player.GetModPlayer<PLayerChilliness>().FrostEssenceCrystalCount++;
                }
            }
            DebugUtils.NewText(player.GetModPlayer<PLayerChilliness>().FrostEssenceCrystalCount);
            return false;

        }

        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            mult = player.GetModPlayer<PLayerChilliness>().EnhanceChilliness ? 1 : 1.5f;
            base.ModifyManaCost(player, ref reduce, ref mult);
        }
        public override void UpdateInventory(Player player)
        {
            if (player.GetModPlayer<PLayerChilliness>().EnhanceChilliness)
            {
                if ((float)Main.time % 30 == 0 && player.statMana > 10)
                {
                    player.statMana -= 10;
                }
            }
            base.UpdateInventory(player);
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.GetModPlayer<PLayerChilliness>().EnhanceChilliness)
            {
                damage.Flat = 40;
            }
            base.ModifyWeaponDamage(player, ref damage);
        }

    }
}