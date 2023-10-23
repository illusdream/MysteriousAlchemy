using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Mapping;
using MysteriousAlchemy.Core.Perfab.Projectiles;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Projectiles.Chilliness
{
    public class IceScytheProj : BaseQuickScythe
    {

        public override string Texture => AssetUtils.Proj_Chilliness + Name;

        public override float HeldPosOffest => 0.75f;
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
            Projectile.penetrate = -1; // 弹幕的穿透数，默认1次
            Projectile.timeLeft = 180; // 弹幕的存活时间
            //Projectile.alpha = 0; // 弹幕的透明度，0 ~ 255，0是完全不透明（int）
            Projectile.Opacity = 1; // 弹幕的不透明度，0 ~ 1，0是完全透明，1是完全不透明(float)，用哪个你们自己挑，这两是互相影响的
            Projectile.friendly = true; // 弹幕是否攻击敌方，默认false
            Projectile.hostile = false; // 弹幕是否攻击友方和城镇NPC，默认false
            Projectile.DamageType = DamageClass.Melee; // 弹幕的伤害类型，默认default，npc射的弹幕用

            // Projectile.aiStyle// 弹幕使用原版哪种弹幕AI类型
            // AIType  // 弹幕模仿原版哪种弹幕的行为
            // 上面两条，第一条是某种行为类型，可以查源码看看，第二条要有第一条才有效果，是让这个弹幕能执行对应弹幕的特殊判定行为
            Projectile.aiStyle = -1; // 不用原版的就写这个，也可以不写
            AngleV = MathHelper.PiOver2;
            WeaponStaticSize = 4;
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
            // 返回null时则执行默认的原版判定
            return null;
        }
        //是否更新位置
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnCanSweep()
        {
            SoundEngine.PlaySound(MASoundID.Ding_Item4);
            for (int i = 0; i < 20; i++)
            {
                var dust = Dust.NewDustPerfect(Projectile.Center + MathUtils.GetVector2InCircle(MathHelper.TwoPi * i / 20f, 40),
                    264,
                    -MathUtils.GetVector2InCircle(MathHelper.TwoPi * i / 20f, 3));
                dust.noGravity = true;
                dust.color = ColorMap.Chiliiness;

            }
            base.OnCanSweep();
        }
        public override void OnKill(int timeLeft)
        {
            DelaySlash.NewDelaySlash<DelaySlash>
            (
                HandPos, SlashTop,
                SlashBottom,
                Color.White * 0.5f, 17,
                AssetUtils.Extra + "Slash_1",
                AssetUtils.ColorBar + "Frost2",
                AssetUtils.Flow + "ShatteredNoise",
                AssetUtils.Extra + "Jingge_001",
                1.5f,
                0.45f,
                0,
                -0.02f,
                0.5f,
                null,
                0.05f
            );
            DelaySlash.NewDelaySlash<DelaySlash>
                (
                    HandPos, SlashTop,
                    SlashBottom,
                    Color.White, 13,
                    AssetUtils.Extra + "Slash_1",
                    AssetUtils.ColorBar + "Frost3",
                    AssetUtils.Extra + "Jingge_001",
                    AssetUtils.Extra + "Jingge_001",
                    1f,
                    0.25f,
                    0.9f,
                    -0.05f,
                    0.75f,
                    () =>
                    {
                        DrawUtils.DrawDefaultSlash
                            (
                                SlashTop,
                                SlashBottom,
                                HandPos, Color.White,
                                AssetUtils.GetTexture2D(AssetUtils.Extra + "Slash_1"),
                                AssetUtils.GetColorBar("Frost3"),
                                AssetUtils.GetTexture2D(AssetUtils.Extra + "Jingge_001"),
                                AssetUtils.GetTexture2D(AssetUtils.Extra + "Jingge_001"),
                                1f,
                                0.35f,
                                0.9f,
                                -0.05f,
                                0.4f,
                                0.05f
                            );
                    },
                    0.05f
                );
            base.OnKill(timeLeft);
        }
        public override void DrawSlash(Color color, Texture2D shape, Texture2D colorBar)
        {
            DrawUtils.DrawDefaultSlash
                (
                    MathUtils.TransVector2Array(SlashTop, 0, MathHelper.PiOver2, 1.1f),
                    MathUtils.TransVector2Array(SlashBottom, 0, MathHelper.PiOver2, 1f),
                    HandPos, Color.White * 0.5f,
                    AssetUtils.GetTexture2D(AssetUtils.Extra + "Slash_1"),
                    AssetUtils.GetColorBar("Frost2"),
                    AssetUtils.GetTexture2D(AssetUtils.Flow + "ShatteredNoise"),
                    AssetUtils.GetTexture2D(AssetUtils.Extra + "Jingge_001"),
                    1.5f,
                    0.45f,
                    0,
                    -0.02f,
                    0.5f,
                    0.4f
                );
            DrawUtils.DrawDefaultSlash
                (
                    SlashTop, SlashBottom, HandPos, Color.White,
                    AssetUtils.GetTexture2D(AssetUtils.Extra + "Slash_1"),
                    AssetUtils.GetColorBar("Frost3"),
                    AssetUtils.GetTexture2D(AssetUtils.Extra + "Jingge_001"),
                    AssetUtils.GetTexture2D(AssetUtils.Extra + "Jingge_001"),
                    1f,
                    0.35f,
                    1f,
                    -0.05f,
                    0.4f,
                    -0.5f
                );
            base.DrawSlash(color, shape, colorBar);
        }
        public override void DrawBloom()
        {
            DrawUtils.DrawDefaultSlash
            (
                MathUtils.TransVector2Array(SlashTop, MathHelper.PiOver4 / 2f,
                MathHelper.PiOver2, 1.1f), SlashBottom, HandPos, Color.White * 0.5f,
                AssetUtils.GetTexture2D(AssetUtils.Extra + "Slash_1"),
                AssetUtils.GetColorBar("Frost3"),
                AssetUtils.GetTexture2D(AssetUtils.Extra + "Jingge_001"),
                AssetUtils.GetTexture2D(AssetUtils.Extra + "Jingge_001"),
                1f,
                0.35f,
                0.9f,
                -0.05f,
                0.4f,
                -0.5f
            );
            base.DrawBloom();
        }
    }
}