using Microsoft.Xna.Framework;
using MysteriousAlchemy.Projectiles.WeaponProjectile;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Items.Weapons
{
    public class BloodHarvesting : ModItem
    {
        public override string Texture => AssetUtils.Weapons + Name;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.DamageType = DamageClass.Melee;
            Item.crit = 6;
            Item.width = 80;
            Item.height = 80;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.shoot = ModContent.ProjectileType<BloodHarvestingProj>();
            Item.shootSpeed = 1;
            Item.knockBack = 4;
            Item.scale = 1;
            Item.value = 27 * 100;
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.maxStack = 1;

        }
        int hitSytleCount = 0;
        int HitSytleCount
        {
            get { return hitSytleCount % 3; }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //传入攻击方式
            var proj = Projectile.NewProjectileDirect(source, player.Center, velocity, type, damage, knockback, player.whoAmI, HitSytleCount, Item.useAnimation, AttackSytleTimer);
            //挥动一次改变攻击方式
            hitSytleCount++;
            AttackSytleTimer = 0;
            return false;
        }
        public override bool? UseItem(Player player)
        {
            return base.UseItem(player);
        }
        public override void UpdateInventory(Player player)
        {
            UpdateAttackSytle(player);
            base.UpdateInventory(player);
        }
        public override bool MeleePrefix()
        {
            return true; // return true to allow weapon to have melee prefixes (e.g. Legendary)
        }
        int AttackSytleTimer = 0;
        int AttackSytleTimerMax = 180;
        private void UpdateAttackSytle(Player player)
        {
            //计时器
            if (!player.ItemAnimationActive)
            {
                AttackSytleTimer++;
            }
            //检测是否大于充值时间
            if (AttackSytleTimer > AttackSytleTimerMax)
            {
                //归零计时器和攻击计数器
                AttackSytleTimer = 0;
                hitSytleCount = 0;
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimtaneBar, 12);
            recipe.AddIngredient(ItemID.Vertebrae, 5);
            recipe.AddIngredient(ItemID.TissueSample, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
            base.AddRecipes();
        }
    }
}