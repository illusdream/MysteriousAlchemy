using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.Particles;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Trails;
using MysteriousAlchemy.Utils;
using System.Numerics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MysteriousAlchemy.Content.Projectiles.Chilliness
{
    public class FrostEssenceSwordProj : ModProjectile, IDrawAddtive
    {
        int frameCount;
        Vector2 orginPosition;
        Player player;
        int standbyTime = 45;
        int Timer;
        public override string Texture => AssetUtils.Proj_Chilliness + Name;

        public SpriteSortMode Sort => SpriteSortMode.Deferred;

        public override void SetStaticDefaults()
        {
            // Main.projFrames[Type] = 1;
            // 这个弹幕的帧图数量，不写就是1

            // 下面三个玩意就先权当认识一下吧

            // ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
            // 弹幕的最大绘制距离，不写则是出屏幕就不绘制，单位像素

            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            // 弹幕的旧数据记录数组长度，10为记录10个数据，新的数据会覆盖更早的数据，默认为10

            ProjectileID.Sets.TrailingMode[Type] = 0;
            // 弹幕的旧数据记录类型，默认为-1，即不记录
            // 0，记录位置(Projectile.oldPos)，这里注意，还有一个玩意儿叫Projectile.oldPosition，这是弹幕上一帧的位置，而oldPos是一个数组
            // 1，请不要使用这个
            // 2，记录位置、角度(Projectile.oldRot)、绘制方向(Projecitle.oldSpriteDirection)
            // 3，与2相同，但会以插值来平滑这个数据
            // 4，与2相同，但会调整数据以跟随弹幕的主人
        }
        public override void SetDefaults()
        {
            Projectile.width = 38; // 弹幕的碰撞箱宽度
            Projectile.height = 38; // 弹幕的碰撞箱高度

            Projectile.scale = 1f; // 弹幕缩放倍率，会影响碰撞箱大小，默认1f
            Projectile.ignoreWater = true; // 弹幕是否忽视水
            Projectile.tileCollide = false; // 弹幕撞到物块会创死吗
            Projectile.penetrate = -1; // 弹幕的穿透数，默认1次
            Projectile.timeLeft = 300; // 弹幕的存活时间
            //Projectile.alpha = 0; // 弹幕的透明度，0 ~ 255，0是完全不透明（int）
            Projectile.Opacity = 1; // 弹幕的不透明度，0 ~ 1，0是完全透明，1是完全不透明(float)，用哪个你们自己挑，这两是互相影响的
            Projectile.friendly = true; // 弹幕是否攻击敌方，默认false
            Projectile.hostile = false; // 弹幕是否攻击友方和城镇NPC，默认false
            Projectile.DamageType = DamageClass.Magic; // 弹幕的伤害类型，默认default，npc射的弹幕用

            // Projectile.aiStyle// 弹幕使用原版哪种弹幕AI类型
            // AIType  // 弹幕模仿原版哪种弹幕的行为
            // 上面两条，第一条是某种行为类型，可以查源码看看，第二条要有第一条才有效果，是让这个弹幕能执行对应弹幕的特殊判定行为
            Projectile.aiStyle = -1; // 不用原版的就写这个，也可以不写
            // Projectile.extraUpdates = 0; // 弹幕每帧的额外更新次数，默认0
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        //生成函数
        public override void OnSpawn(IEntitySource source)
        {
            player = Main.player[Projectile.owner];
            frameCount = Main.rand.Next(14);
            orginPosition = Projectile.position - player.Center;
            base.OnSpawn(source);
        }

        public override bool PreAI()
        {
            return base.PreAI();
        }
        public override void AI()
        {
            Timer++;


            if (Timer > standbyTime)
            {
                for (int i = 0; i < 1; i++)
                {
                    ParticleUtils.SpwonFog(Projectile.Center, -Projectile.velocity * Main.rand.NextFloat(0.3f, 0.5f), new Color(131, 189, 238), 0.2f, MathHelper.PiOver2, 0.03f);

                }
                var dust = Dust.NewDustDirect(Projectile.Center, 20, 20, DustID.IceTorch, -Projectile.velocity.X * Main.rand.NextFloat(0.3f, 0.5f), -Projectile.velocity.Y * Main.rand.NextFloat(0.3f, 0.5f));
                var dust1 = Dust.NewDustDirect(Projectile.Center, 20, 20, DustID.DungeonWater, -Projectile.velocity.X * Main.rand.NextFloat(0.3f, 0.5f), -Projectile.velocity.Y * Main.rand.NextFloat(0.3f, 0.5f));
                dust1.noGravity = true;
            }
            else
            {
                Projectile.Opacity = ((Timer / (float)standbyTime));
                Projectile.position = player.Center + orginPosition;
                Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.One) * 15;
                float angleH = -(Main.MouseWorld - player.Center).SafeNormalize(Vector2.One).ToRotation();
                float angleV = MathHelper.PiOver4 * (((Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.One).X) < 0 ? -1 : 1);
                if (Timer == 44)
                {

                    for (int i = 0; i < 40; i++)
                    {
                        var dust = Dust.NewDustPerfect(Projectile.Center + DrawUtils.MartixTrans(MathUtils.GetVector2InCircle(MathHelper.TwoPi * i / 20f, 20), angleH, angleV),
                            264,
                            DrawUtils.MartixTrans(MathUtils.GetVector2InCircle(MathHelper.TwoPi * i / 20f, 2), angleH, angleV));
                        dust.noGravity = true;
                        dust.color = new Color(37, 102, 221);
                    }
                    SoundEngine.PlaySound(MASoundID.MaxMana, Projectile.position);

                    Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.One).RotatedByRandom(MathHelper.PiOver4) * 15;
                }
            }
            Lighting.AddLight(Projectile.Center, new Microsoft.Xna.Framework.Vector3(37, 102, 221) * 0.01f);
            base.AI();
        }
        public override void PostAI()
        {
            base.PostAI();
        }
        //是否可以造成伤害
        public override bool? CanDamage()
        {
            if (Timer > standbyTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //是否更新位置
        public override bool ShouldUpdatePosition()
        {
            if (Timer > standbyTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(MASoundID.CrushedIce_Item27, target.position);
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {

            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.Draw(AssetUtils.GetTexture2D(Texture), DrawUtils.ToScreenPosition(Projectile.Center), new Rectangle(frameCount * 38, 0, 38, 38), new Color(37, 102, 221) * Projectile.Opacity, Projectile.velocity.ToRotation() + MathHelper.PiOver4, new Vector2(38, 38) / 2f, 1, SpriteEffects.None, 0);
            base.PostDraw(lightColor);
        }
        public void DrawAddtive(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 5; i++)
            {
                spriteBatch.Draw(AssetUtils.GetTexture2D(Texture), DrawUtils.ToScreenPosition(Projectile.Center), new Rectangle(frameCount * 38, 0, 38, 38), new Color(37, 102, 221) * Projectile.Opacity, Projectile.velocity.ToRotation() + MathHelper.PiOver4, new Vector2(38, 38) / 2f, 1.1f, SpriteEffects.None, 0);
            }
        }
    }
}