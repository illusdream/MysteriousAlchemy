using Microsoft.Xna.Framework;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Projectiles.TestProj
{
    public class BaseProjectile1 : ModProjectile
    {
        public override string Texture => AssetUtils.Projectiles + "TestProj/" + Name;
        public override void SetStaticDefaults()
        {

            // Main.projFrames[Type] = 1;
            // 这个弹幕的帧图数量，不写就是1

            // 下面三个玩意就先权当认识一下吧

            // ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
            // 弹幕的最大绘制距离，不写则是出屏幕就不绘制，单位像素

            // ProjectileID.Sets.TrailCacheLength[Type] = 10;
            // 弹幕的旧数据记录数组长度，10为记录10个数据，新的数据会覆盖更早的数据，默认为10

            // ProjectileID.Sets.TrailingMode[Type] = -1;
            // 弹幕的旧数据记录类型，默认为-1，即不记录
            // 0，记录位置(Projectile.oldPos)，这里注意，还有一个玩意儿叫Projectile.oldPosition，这是弹幕上一帧的位置，而oldPos是一个数组
            // 1，请不要使用这个
            // 2，记录位置、角度(Projectile.oldRot)、绘制方向(Projecitle.oldSpriteDirection)
            // 3，与2相同，但会以插值来平滑这个数据
            // 4，与2相同，但会调整数据以跟随弹幕的主人
        }
        public override void SetDefaults()
        {

            Projectile.width = 16; // 弹幕的碰撞箱宽度
            Projectile.height = 16; // 弹幕的碰撞箱高度

            Projectile.scale = 1f; // 弹幕缩放倍率，会影响碰撞箱大小，默认1f
            Projectile.ignoreWater = true; // 弹幕是否忽视水
            Projectile.tileCollide = false; // 弹幕撞到物块会创死吗
            Projectile.penetrate = 1; // 弹幕的穿透数，默认1次
            Projectile.timeLeft = 180; // 弹幕的存活时间
            //Projectile.alpha = 0; // 弹幕的透明度，0 ~ 255，0是完全不透明（int）
            Projectile.Opacity = 1; // 弹幕的不透明度，0 ~ 1，0是完全透明，1是完全不透明(float)，用哪个你们自己挑，这两是互相影响的
            Projectile.friendly = true; // 弹幕是否攻击敌方，默认false
            Projectile.hostile = false; // 弹幕是否攻击友方和城镇NPC，默认false
            Projectile.DamageType = DamageClass.Default; // 弹幕的伤害类型，默认default，npc射的弹幕用

            // Projectile.aiStyle// 弹幕使用原版哪种弹幕AI类型
            // AIType  // 弹幕模仿原版哪种弹幕的行为
            // 上面两条，第一条是某种行为类型，可以查源码看看，第二条要有第一条才有效果，是让这个弹幕能执行对应弹幕的特殊判定行为
            Projectile.aiStyle = -1; // 不用原版的就写这个，也可以不写

            // Projectile.extraUpdates = 0; // 弹幕每帧的额外更新次数，默认0
        }
        //生成函数
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }

        public override bool PreAI()
        {
            return base.PreAI();
        }
        public override void AI()
        {
            base.AI();
        }
        public override void PostAI()
        {
            base.PostAI();
        }
        //是否可以造成伤害
        public override bool? CanDamage()
        {
            // 当弹幕剩余存活时间小于0.5s时不造成伤害
            if (Projectile.timeLeft < 30) return false;
            // 返回null时则执行默认的原版判定
            return null;
        }
        //是否更新位置
        public override bool ShouldUpdatePosition()
        {
            return true;
        }

    }
}