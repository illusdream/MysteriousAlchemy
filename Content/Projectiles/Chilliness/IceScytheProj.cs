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
            Projectile.height = 16; // ��Ļ����ײ��߶�

            Projectile.scale = 1f; // ��Ļ���ű��ʣ���Ӱ����ײ���С��Ĭ��1f
            Projectile.ignoreWater = true; // ��Ļ�Ƿ����ˮ
            Projectile.tileCollide = false; // ��Ļײ�����ᴴ����
            Projectile.penetrate = -1; // ��Ļ�Ĵ�͸����Ĭ��1��
            Projectile.timeLeft = 180; // ��Ļ�Ĵ��ʱ��
            //Projectile.alpha = 0; // ��Ļ��͸���ȣ�0 ~ 255��0����ȫ��͸����int��
            Projectile.Opacity = 1; // ��Ļ�Ĳ�͸���ȣ�0 ~ 1��0����ȫ͸����1����ȫ��͸��(float)�����ĸ������Լ����������ǻ���Ӱ���
            Projectile.friendly = true; // ��Ļ�Ƿ񹥻��з���Ĭ��false
            Projectile.hostile = false; // ��Ļ�Ƿ񹥻��ѷ��ͳ���NPC��Ĭ��false
            Projectile.DamageType = DamageClass.Melee; // ��Ļ���˺����ͣ�Ĭ��default��npc��ĵ�Ļ��

            // Projectile.aiStyle// ��Ļʹ��ԭ�����ֵ�ĻAI����
            // AIType  // ��Ļģ��ԭ�����ֵ�Ļ����Ϊ
            // ������������һ����ĳ����Ϊ���ͣ����Բ�Դ�뿴�����ڶ���Ҫ�е�һ������Ч�������������Ļ��ִ�ж�Ӧ��Ļ�������ж���Ϊ
            Projectile.aiStyle = -1; // ����ԭ��ľ�д�����Ҳ���Բ�д
            AngleV = MathHelper.PiOver2;
            WeaponStaticSize = 4;
            // Projectile.extraUpdates = 0; // ��Ļÿ֡�Ķ�����´�����Ĭ��0
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
            // ����nullʱ��ִ��Ĭ�ϵ�ԭ���ж�
            return null;
        }
        //�Ƿ����λ��
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