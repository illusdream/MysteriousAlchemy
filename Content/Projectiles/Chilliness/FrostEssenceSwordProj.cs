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
            // �����Ļ��֡ͼ��������д����1

            // ���������������Ȩ����ʶһ�°�

            // ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
            // ��Ļ�������ƾ��룬��д���ǳ���Ļ�Ͳ����ƣ���λ����

            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            // ��Ļ�ľ����ݼ�¼���鳤�ȣ�10Ϊ��¼10�����ݣ��µ����ݻḲ�Ǹ�������ݣ�Ĭ��Ϊ10

            ProjectileID.Sets.TrailingMode[Type] = 0;
            // ��Ļ�ľ����ݼ�¼���ͣ�Ĭ��Ϊ-1��������¼
            // 0����¼λ��(Projectile.oldPos)������ע�⣬����һ���������Projectile.oldPosition�����ǵ�Ļ��һ֡��λ�ã���oldPos��һ������
            // 1���벻Ҫʹ�����
            // 2����¼λ�á��Ƕ�(Projectile.oldRot)�����Ʒ���(Projecitle.oldSpriteDirection)
            // 3����2��ͬ�������Բ�ֵ��ƽ���������
            // 4����2��ͬ��������������Ը��浯Ļ������
        }
        public override void SetDefaults()
        {
            Projectile.width = 38; // ��Ļ����ײ����
            Projectile.height = 38; // ��Ļ����ײ��߶�

            Projectile.scale = 1f; // ��Ļ���ű��ʣ���Ӱ����ײ���С��Ĭ��1f
            Projectile.ignoreWater = true; // ��Ļ�Ƿ����ˮ
            Projectile.tileCollide = false; // ��Ļײ�����ᴴ����
            Projectile.penetrate = -1; // ��Ļ�Ĵ�͸����Ĭ��1��
            Projectile.timeLeft = 300; // ��Ļ�Ĵ��ʱ��
            //Projectile.alpha = 0; // ��Ļ��͸���ȣ�0 ~ 255��0����ȫ��͸����int��
            Projectile.Opacity = 1; // ��Ļ�Ĳ�͸���ȣ�0 ~ 1��0����ȫ͸����1����ȫ��͸��(float)�����ĸ������Լ����������ǻ���Ӱ���
            Projectile.friendly = true; // ��Ļ�Ƿ񹥻��з���Ĭ��false
            Projectile.hostile = false; // ��Ļ�Ƿ񹥻��ѷ��ͳ���NPC��Ĭ��false
            Projectile.DamageType = DamageClass.Magic; // ��Ļ���˺����ͣ�Ĭ��default��npc��ĵ�Ļ��

            // Projectile.aiStyle// ��Ļʹ��ԭ�����ֵ�ĻAI����
            // AIType  // ��Ļģ��ԭ�����ֵ�Ļ����Ϊ
            // ������������һ����ĳ����Ϊ���ͣ����Բ�Դ�뿴�����ڶ���Ҫ�е�һ������Ч�������������Ļ��ִ�ж�Ӧ��Ļ�������ж���Ϊ
            Projectile.aiStyle = -1; // ����ԭ��ľ�д�����Ҳ���Բ�д
            // Projectile.extraUpdates = 0; // ��Ļÿ֡�Ķ�����´�����Ĭ��0
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        //���ɺ���
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
        //�Ƿ��������˺�
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
        //�Ƿ����λ��
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