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
            Projectile.timeLeft = 1200; // 弹幕的存活时间
            //Projectile.alpha = 0; // 弹幕的透明度，0 ~ 255，0是完全不透明（int）
            Projectile.Opacity = 1; // 弹幕的不透明度，0 ~ 1，0是完全透明，1是完全不透明(float)，用哪个你们自己挑，这两是互相影响的
            Projectile.friendly = true; // 弹幕是否攻击敌方，默认false
            Projectile.hostile = false; // 弹幕是否攻击友方和城镇NPC，默认false
            Projectile.DamageType = DamageClass.MagicSummonHybrid; // 弹幕的伤害类型，默认default，npc射的弹幕用

            // Projectile.aiStyle// 弹幕使用原版哪种弹幕AI类型
            // AIType  // 弹幕模仿原版哪种弹幕的行为
            // 上面两条，第一条是某种行为类型，可以查源码看看，第二条要有第一条才有效果，是让这个弹幕能执行对应弹幕的特殊判定行为
            Projectile.aiStyle = -1; // 不用原版的就写这个，也可以不写

            // Projectile.extraUpdates = 0; // 弹幕每帧的额外更新次数，默认0
        }
        //生成函数
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
        //是否可以造成伤害
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
        //是否更新位置
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