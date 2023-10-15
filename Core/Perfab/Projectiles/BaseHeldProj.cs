using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace MysteriousAlchemy.Core.Perfab.Projectiles
{
    public abstract class BaseHeldProj : ModProjectile
    {
        /// <summary>
        /// 弹幕的拥有者，默认是玩家，如果需要是NPC请自行重写
        /// </summary>
        public virtual Entity ProjOwner => Main.player[Projectile.owner];

        /// <summary>
        /// 弹幕的玩家拥有者
        /// </summary>
        public Player Owner => Main.player[Projectile.owner];

        /// <summary>
        /// 玩家拥有者的朝向
        /// </summary>
        public virtual int OwnerDirection => Math.Sign(Owner.gravDir) * Owner.direction;

        public virtual float WeaponSizeModify => Owner.GetAdjustedItemScale(Owner.HeldItem);

        public virtual float WeaponSpeedModify => Owner.GetAdjustedItemScale(Owner.HeldItem);

        public override void SetDefaults()
        {
            Projectile.ignoreWater = true; // 弹幕是否忽视水
            Projectile.tileCollide = false; // 弹幕撞到物块会创死吗
            Projectile.penetrate = -1; // 弹幕的穿透数，默认1次
            base.SetDefaults();
        }
    }
}
