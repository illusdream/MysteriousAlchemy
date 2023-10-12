using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Global.ModPlayers;
using MysteriousAlchemy.Utils;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Projectiles.Chilliness
{
    public class FrostEssenceCrystalBeam : ModProjectile
    {
        int timer;
        int frameCount;
        public override string Texture => AssetUtils.Proj_Chilliness + Name;
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
            Projectile.width = 16; // ��Ļ����ײ�����
            Projectile.height = 16; // ��Ļ����ײ��߶�

            Projectile.scale = 1f; // ��Ļ���ű��ʣ���Ӱ����ײ���С��Ĭ��1f
            Projectile.ignoreWater = true; // ��Ļ�Ƿ����ˮ
            Projectile.tileCollide = false; // ��Ļײ�����ᴴ����
            Projectile.penetrate = 1; // ��Ļ�Ĵ�͸����Ĭ��1��
            Projectile.timeLeft = 180; // ��Ļ�Ĵ��ʱ��
            //Projectile.alpha = 0; // ��Ļ��͸���ȣ�0 ~ 255��0����ȫ��͸����int��
            Projectile.Opacity = 1; // ��Ļ�Ĳ�͸���ȣ�0 ~ 1��0����ȫ͸����1����ȫ��͸��(float)�����ĸ������Լ����������ǻ���Ӱ���
            Projectile.friendly = true; // ��Ļ�Ƿ񹥻��з���Ĭ��false
            Projectile.hostile = false; // ��Ļ�Ƿ񹥻��ѷ��ͳ���NPC��Ĭ��false
            Projectile.DamageType = DamageClass.MagicSummonHybrid; // ��Ļ���˺����ͣ�Ĭ��default��npc��ĵ�Ļ��

            // Projectile.aiStyle// ��Ļʹ��ԭ�����ֵ�ĻAI����
            // AIType  // ��Ļģ��ԭ�����ֵ�Ļ����Ϊ
            // ������������һ����ĳ����Ϊ���ͣ����Բ�Դ�뿴�����ڶ���Ҫ�е�һ������Ч�������������Ļ��ִ�ж�Ӧ��Ļ�������ж���Ϊ
            Projectile.aiStyle = -1; // ����ԭ��ľ�д�����Ҳ���Բ�д

            // Projectile.extraUpdates = 0; // ��Ļÿ֡�Ķ�����´�����Ĭ��0
        }
        //���ɺ���
        public override void OnSpawn(IEntitySource source)
        {
            base.OnSpawn(source);
        }
        public override void OnKill(int timeLeft)
        {
            Main.player[Projectile.owner].GetModPlayer<PLayerChilliness>().FrostEssenceCrystalCount--;
            base.OnKill(timeLeft);
        }
        public override bool PreAI()
        {
            return base.PreAI();
        }
        public override void AI()
        {
            frameCount++;
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
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(AssetUtils.GetTexture2D(AssetUtils.Proj_Chilliness + Name), Projectile.Center - Main.screenPosition, new Rectangle(0, ((frameCount / 5) % 8) * 46, 28, 46), Color.White, 0, new Vector2(28, 46) / 2f, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            return false;
        }
        //�Ƿ����λ��
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}