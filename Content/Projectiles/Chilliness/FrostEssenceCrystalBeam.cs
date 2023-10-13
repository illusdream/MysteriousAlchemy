using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.Global.ModPlayers;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Mapping;
using MysteriousAlchemy.Core.Perfab.Projectiles;
using MysteriousAlchemy.Core.System;
using MysteriousAlchemy.Core.Trails;
using MysteriousAlchemy.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Projectiles.Chilliness
{
    public class FrostEssenceCrystalBeam : ProjStateMachine, IDrawAddtive
    {
        int timer;
        int frameCount;
        int MaxBeamLength = 50;
        int BeamLength = 30;
        Vector2[] BeamPosition;
        Trail trail;
        Trail bloomTrail;

        public float StartAngle;
        public float EndAngle { get { return StartAngle + TotalAngle * direction; } }
        public float TotalAngle = MathHelper.PiOver2 * 0.75f;
        public int direction;
        public override string Texture => AssetUtils.Proj_Chilliness + Name;

        public SpriteSortMode Sort => SpriteSortMode.Deferred;

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
            Projectile.timeLeft = 1200; // ��Ļ�Ĵ��ʱ��
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
            BeamPosition = new Vector2[MaxBeamLength];
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
            for (int i = 0; i < MaxBeamLength; i++)
            {
                BeamPosition[i] = Projectile.velocity.SafeNormalize(Vector2.One) * BeamLength * i + Projectile.Center;
            }
            trail = new Trail(Main.graphics.GraphicsDevice, MaxBeamLength, new NoTip(), factor =>
            {
                if (factor > 0.7f)
                    return MathF.Sqrt((1 - factor) / 0.3f) * 8;
                else
                    return 8;
            }, null);
            bloomTrail = new Trail(Main.graphics.GraphicsDevice, MaxBeamLength, new NoTip(), factor =>
            {
                if (factor > 0.7f)
                    return MathF.Sqrt((1 - factor) / 0.3f) * 16;
                else
                    return 18;
            }, null);
            trail.Positions = BeamPosition;
            bloomTrail.Positions = BeamPosition;
            base.AI();
        }
        public override void PostAI()
        {
            base.PostAI();
        }
        //�Ƿ��������˺�
        public override bool? CanDamage()
        {
            return null;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            for (int i = 0; i < 2; i++)
            {
                trail.Render(Rendered);
                trail.Render(Rendered);
            }



            //DrawUtils.DrawProjectileTrail(BeamPosition, AssetUtils.GetTexture2D(AssetUtils.Extra + "Extra_194"), AssetUtils.GetTexture2D(AssetUtils.Extra + "Extra_194"), AssetUtils.GetColorBar("Frost"));
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.BloomAreaDraw, () =>
            {
                bloomTrail.Render(render2);
            });
            Main.spriteBatch.Draw(AssetUtils.GetTexture2D(AssetUtils.Proj_Chilliness + Name), Projectile.Center - Main.screenPosition, new Rectangle(0, ((frameCount / 5) % 8) * 46, 28, 46), Color.White, 0, new Vector2(28, 46) / 2f, 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            bool result = CurrectState is Attack_NoEnhanceChilliness ?
                Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.One) * BeamLength * 50 + Projectile.Center, 8, ref point)
                : false;
            return result;
        }
        //�Ƿ����λ��
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public void Rendered()
        {
            Effect effect = AssetUtils.GetEffect("DefaultBeam");
            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["shapeTexure"].SetValue(AssetUtils.GetTexture2D(AssetUtils.Extra + "Extra_5"));
            effect.Parameters["colorTexture"].SetValue(AssetUtils.GetColorBar("Frost"));
            effect.Parameters["repeat"].SetValue(10);
            effect.Parameters["rowSpeed"].SetValue(5);
            effect.Parameters["noiseScale"].SetValue(15);
            effect.Parameters["noiseSpeed"].SetValue(-6);
            effect.Parameters["noisePower"].SetValue(2);
            effect.Parameters["noiseLerpAmount"].SetValue(0.1f);
            effect.Parameters["AlphaLerpAmount"].SetValue(0.2f);
            effect.Parameters["time"].SetValue(MathUtils.GetTime(-0.03f));
            effect.CurrentTechnique.Passes[0].Apply();
        }
        private void render2()
        {
            Effect effect = AssetUtils.GetEffect("DefaultBeam");
            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["shapeTexure"].SetValue(AssetUtils.GetTexture2D(AssetUtils.Extra + "Extra_5"));
            effect.Parameters["colorTexture"].SetValue(AssetUtils.GetColorBar("White"));
            effect.Parameters["repeat"].SetValue(20);
            effect.Parameters["rowSpeed"].SetValue(5);
            effect.Parameters["noiseScale"].SetValue(15);
            effect.Parameters["noiseSpeed"].SetValue(-3);
            effect.Parameters["noisePower"].SetValue(2);
            effect.Parameters["noiseLerpAmount"].SetValue(0.1f);
            effect.Parameters["AlphaLerpAmount"].SetValue(0.2f);
            effect.Parameters["time"].SetValue(MathUtils.GetTime(-0.03f));
            // effect.CurrentTechnique.Passes[0].Apply();
        }

        public void DrawAddtive(SpriteBatch spriteBatch)
        {

        }
        public override void Initialize()
        {
            RegisterState<standby>(new standby(this));
            RegisterState<Attack_NoEnhanceChilliness>(new Attack_NoEnhanceChilliness(this));
            SetState<standby>();
            base.Initialize();
        }

        #region
        private class standby : ProjState<FrostEssenceCrystalBeam>
        {
            int Timer;
            public standby(ProjStateMachine projStateMachine) : base(projStateMachine)
            {
            }
            public override void OnState(IStateMachine stateMachine)
            {

                if (Timer % 5 == 0)
                    ParticleUtils.SpwonFog(Projectile.Projectile.Center, MathUtils.RandomRing(new Vector2(1, 1), 2, 7), ColorMap.Chiliiness, 0.2f, MathHelper.TwoPi, 0.05f);

                if (NPCUtils.GetNPCCanTrack(Projectile.Projectile.Center, 1000) is not null)
                {
                    Projectile.SwitchState<Attack_NoEnhanceChilliness>();
                    Projectile.direction = Main.rand.NextBool() ? 1 : -1;
                    Projectile.StartAngle = NPCUtils.GetVector2ToCanTrackNPC(Projectile.Projectile.Center, 1000).ToRotation() - (Projectile.TotalAngle / 2f * Projectile.direction);
                }
                base.OnState(stateMachine);
            }
            public override void ExitState(IStateMachine stateMachine)
            {
                Timer = 0;
                base.ExitState(stateMachine);
            }
        }
        private class Attack_NoEnhanceChilliness : ProjState<FrostEssenceCrystalBeam>
        {
            int Timer;
            int AttackTime = 50;
            public Attack_NoEnhanceChilliness(ProjStateMachine projStateMachine) : base(projStateMachine)
            {
            }
            public override void OnState(IStateMachine stateMachine)
            {
                Timer++;
                Projectile.Projectile.velocity = (Vector2.UnitX).RotatedBy(MathHelper.Lerp(Projectile.StartAngle, Projectile.EndAngle, ((float)Timer / AttackTime)));

                if (Timer % 5 == 0)
                    ParticleUtils.SpwonFog(Projectile.Projectile.Center, MathUtils.RandomRing(new Vector2(1, 1), 2, 7), ColorMap.Chiliiness, 0.2f, MathHelper.TwoPi, 0.05f);

                if (Timer > AttackTime)
                    Projectile.SwitchState<standby>();
                base.OnState(stateMachine);
            }
            public override void ExitState(IStateMachine stateMachine)
            {
                Timer = 0;
                base.ExitState(stateMachine);
            }

        }
        #endregion
    }
}