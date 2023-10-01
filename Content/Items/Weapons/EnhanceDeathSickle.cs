using Microsoft.Xna.Framework;
using MysteriousAlchemy.Projectiles.WeaponProjectile;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Items.Weapons
{
    public class EnhanceDeathSickle : ModItem
    {
        public override string Texture => AssetUtils.Weapons + Name;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.damage = 87;
            Item.DamageType = DamageClass.Melee;
            Item.crit = 15;
            Item.width = 70;
            Item.height = 64;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.shoot = ModContent.ProjectileType<SyctheProj>();
            Item.shootSpeed = 1;
            Item.knockBack = 4;
            Item.scale = 1;
            Item.value = 10000;
            Item.rare = ItemRarityID.Red;
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
    }
}