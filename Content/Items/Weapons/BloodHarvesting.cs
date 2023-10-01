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
            //���빥����ʽ
            var proj = Projectile.NewProjectileDirect(source, player.Center, velocity, type, damage, knockback, player.whoAmI, HitSytleCount, Item.useAnimation, AttackSytleTimer);
            //�Ӷ�һ�θı乥����ʽ
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
            //��ʱ��
            if (!player.ItemAnimationActive)
            {
                AttackSytleTimer++;
            }
            //����Ƿ���ڳ�ֵʱ��
            if (AttackSytleTimer > AttackSytleTimerMax)
            {
                //�����ʱ���͹���������
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