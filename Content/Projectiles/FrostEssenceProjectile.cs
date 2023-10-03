using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Global.Element;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Projectiles
{
    public class FrostEssenceProjectile : ModProjectile
    {
        public override string Texture => AssetUtils.Projectiles + Name;
        int extraUpdate = 5;

        float TrackScale = 0.4f;
        float TrackRadium = 500;
        float TrackVelocity = 15;
        float TrackRealScale { get { return TrackScale / (1 + extraUpdate); } }
        float TrackRealVelocity { get { return TrackVelocity / (1 + extraUpdate); } }
        private Vector2[] oldPosi = new Vector2[180];
        private int frametime = 0;
        public override void SetStaticDefaults()
        {
            // Main.projFrames[Type] = 1;
            // �����Ļ��֡ͼ��������д����1

            // ���������������Ȩ����ʶһ�°�

            // ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;
            // ��Ļ�������ƾ��룬��д���ǳ���Ļ�Ͳ����ƣ���λ����

            // ProjectileID.Sets.TrailCacheLength[Type] = 10;
            // ��Ļ�ľ����ݼ�¼���鳤�ȣ�10Ϊ��¼10�����ݣ��µ����ݻḲ�Ǹ�������ݣ�Ĭ��Ϊ10

            // ProjectileID.Sets.TrailingMode[Type] = -1;
            // ��Ļ�ľ����ݼ�¼���ͣ�Ĭ��Ϊ-1��������¼
            // 0����¼λ��(Projectile.oldPos)������ע�⣬����һ���������Projectile.oldPosition�����ǵ�Ļ��һ֡��λ�ã���oldPos��һ������
            // 1���벻Ҫʹ�����
            // 2����¼λ�á��Ƕ�(Projectile.oldRot)�����Ʒ���(Projecitle.oldSpriteDirection)
            // 3����2��ͬ�������Բ�ֵ��ƽ���������
            // 4����2��ͬ��������������Ը��浯Ļ������
        }
        public override void SetDefaults()
        {
            Projectile.width = 16; // ��Ļ����ײ����
            Projectile.height = 8; // ��Ļ����ײ��߶�

            Projectile.scale = 1f; // ��Ļ���ű��ʣ���Ӱ����ײ���С��Ĭ��1f
            Projectile.ignoreWater = true; // ��Ļ�Ƿ����ˮ
            Projectile.tileCollide = false; // ��Ļײ�����ᴴ����
            Projectile.penetrate = 5; // ��Ļ�Ĵ�͸����Ĭ��1��
            Projectile.timeLeft = 180 * (1 + extraUpdate); // ��Ļ�Ĵ��ʱ��
            //Projectile.alpha = 0; // ��Ļ��͸���ȣ�0 ~ 255��0����ȫ��͸����int��
            Projectile.Opacity = 1; // ��Ļ�Ĳ�͸���ȣ�0 ~ 1��0����ȫ͸����1����ȫ��͸��(float)�����ĸ������Լ����������ǻ���Ӱ���
            Projectile.friendly = true; // ��Ļ�Ƿ񹥻��з���Ĭ��false
            Projectile.hostile = false; // ��Ļ�Ƿ񹥻��ѷ��ͳ���NPC��Ĭ��false
            Projectile.DamageType = DamageClass.Default; // ��Ļ���˺����ͣ�Ĭ��default��npc��ĵ�Ļ��

            // Projectile.aiStyle// ��Ļʹ��ԭ�����ֵ�ĻAI����
            // AIType  // ��Ļģ��ԭ�����ֵ�Ļ����Ϊ
            // ������������һ����ĳ����Ϊ���ͣ����Բ�Դ�뿴�����ڶ���Ҫ�е�һ������Ч�������������Ļ��ִ�ж�Ӧ��Ļ�������ж���Ϊ
            Projectile.aiStyle = -1; // ����ԭ��ľ�д�����Ҳ���Բ�д

            Projectile.extraUpdates = extraUpdate; // ��Ļÿ֡�Ķ�����´�����Ĭ��0
            Projectile.GetGlobalProjectile<ProjectileElement>().freezeElement.Stack = 10;
            Projectile.GetGlobalProjectile<ProjectileElement>().freezeElement.ElementAcculationModifier *= 2;
            Projectile.GetGlobalProjectile<ProjectileElement>().radiantElement.Stack = 10;
            Projectile.GetGlobalProjectile<ProjectileElement>().necroticElement.Stack = 10;
        }
        //���ɺ���
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
            //�����β����
            if (frametime % 1 == 0)
            {
                for (int i = oldPosi.Length - 1; i > 0; i--)
                {
                    oldPosi[i] = oldPosi[i - 1];
                }
                oldPosi[0] = Projectile.Center;
            }
            frametime++;
            //׷��
            #region
            if (NPCUtils.GetNPCCanTrack(Projectile.Center, TrackRadium) != null)
            {
                Vector2 TargetPos = NPCUtils.GetNPCCanTrack(Projectile.Center, TrackRadium).Center;
                Projectile.velocity = ((1 - TrackRealScale) * Projectile.velocity + (TargetPos - Projectile.Center).SafeNormalize(Vector2.Zero) * TrackRealScale).SafeNormalize(Vector2.Zero) * TrackRealVelocity;
            }

            #endregion
            //�ı���ת����
            #region
            Projectile.rotation = Projectile.velocity.ToRotation();
            #endregion
            //��������
            #region
            if (frametime % (1 + extraUpdate) == 0)
            {
                Dust dust;
                // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
                dust = Main.dust[Terraria.Dust.NewDust(Projectile.Center - Projectile.Hitbox.Size() / 2f, Projectile.width, 8, DustID.Frost, -Projectile.velocity.X, -Projectile.velocity.Y, 0, new Color(255, 255, 255), 1.0465117f)];
                dust.noGravity = true;
                dust.fadeIn = 0.69767445f;
            }
            #endregion

            base.AI();
        }
        public override void PostAI()
        {
            base.PostAI();
        }
        //�Ƿ��������˺�
        public override bool? CanDamage()
        {
            // ����Ļʣ����ʱ��С��0.5sʱ������˺�
            if (Projectile.timeLeft < 30) return false;
            // ����nullʱ��ִ��Ĭ�ϵ�ԭ���ж�
            return null;
        }
        //�Ƿ����λ��
        public override bool ShouldUpdatePosition()
        {
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D ProjectileTex = ModContent.Request<Texture2D>(Texture).Value;
            Main.EntitySpriteDraw(ProjectileTex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, ProjectileTex.Size() / 2f, Projectile.Hitbox.Size() / ProjectileTex.Size(), SpriteEffects.None, 0);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D Mainshape = AssetUtils.GetTexture2D(AssetUtils.Extra + "Extra_1");
            Texture2D MainMask = AssetUtils.GetMask("Mask2");
            Texture2D MainColor = AssetUtils.GetColorBar("Frost");
            DrawUtil.DrawProjectileTrail(oldPosi, Mainshape, MainMask, MainColor, 25, ((float)Main.time / 600f) % 1f);
            base.PostDraw(lightColor);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}