using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Projectiles.Chilliness;
using MysteriousAlchemy.Projectiles.WeaponProjectile;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Items.Chilliness
{

    public class IceScythe : ModItem
    {
        public override string Texture => AssetUtils.ItemChilliness + Name;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Melee;
            Item.crit = 6;
            Item.width = 52;
            Item.height = 44;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.shoot = ModContent.ProjectileType<IceScytheProj>();
            Item.shootSpeed = 1;
            Item.knockBack = 4;
            Item.scale = 1;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.maxStack = 1;
        }
        public override bool MeleePrefix()
        {
            return true; // return true to allow weapon to have melee prefixes (e.g. Legendary)
        }
    }
}