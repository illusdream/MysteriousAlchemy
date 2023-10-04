using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Global.GlobalNPCs;
using MysteriousAlchemy.Global.ModPlayers;
using MysteriousAlchemy.Utils;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using static Terraria.ModLoader.PlayerDrawLayer;
using MysteriousAlchemy.Core.System;

namespace MysteriousAlchemy.Projectiles.WeaponProjectile
{
    public class BloodHarvestingProj : ModProjectile
    {
        public override string Texture => AssetUtils.WeaponProjectile + Name;
        Player Player => Main.player[Projectile.owner];
        //攻击类型
        AttackType attackType
        {
            get => (AttackType)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }
        AttackState attackState
        {
            get => (AttackState)Projectile.localAI[0];
            set => Projectile.localAI[0] = (float)value;
        }
        //倾角1
        public float AngleV;
        //倾角2
        public float AngleH;

        public float Progress;
        public float StaticAttackFlameCount;
        //基础计时器
        public int Timer;
        public float prepareTime;
        public float attackTime;
        public float unwingdTime;
        //攻击需要的帧数
        public float CurretAttackFlame
        {
            get { return StaticAttackFlameCount; }
        }
        //Progress每帧增加的数值
        public float ProgressAdd_VarFlame
        {
            get { return 1 / CurretAttackFlame; }
        }

        public float Clockwise
        {
            get { return _clockwise > 0 ? 1 : -1; }
            set { _clockwise = value; }
        }
        public float _clockwise;
        //刀光
        public float[] WeaponRotation = new float[180];
        private Vector2[] Slash = new Vector2[180];
        private bool CanGetTrail = false;
        float DefaultWeaponSize = 1.5f;
        float StaticWeaponSize = 1.5f;

        //向下偏移武器大小一半 * value
        public float HeldPosOffest = 0.75f;
        float WeaponCurretSize;
        Vector2 WeaponTopPos = Vector2.Zero;
        Vector2 WeaponCenterFix = Vector2.Zero;
        int DoubleHitPrizeCount = 60;
        bool DoubleHitPrize
        {
            get { return Projectile.ai[2] < 45 ? true : false; }
        }

        float WeaponSizeModify => Player.GetAdjustedItemScale(Player.HeldItem);
        float WeaponSpeedModify => Player.GetAttackSpeed(Projectile.DamageType);

        Vector2 HandPos = Vector2.Zero;
        //起始角度
        float StartAngle = 0;
        //总共需要转动的角度
        float TargetAngle = 0;
        //目前的角度
        float CurretAngle = 0;
        //预设
        #region
        public float SwingAngleV = MathHelper.PiOver4 * 0.3f;
        public float SwingAngleH = MathHelper.PiOver2 * 0.8f;
        public float SwingStartAngle = -MathHelper.PiOver4 * 3.5f;
        public float SwingTargetAngle = MathHelper.Pi * 4f;
        public float SwingPrepareTime = 36f;
        public float SwingAttackTime = 45f;
        public float SwingUnwingTime = 15f;

        public float HarvestAngleV = MathHelper.PiOver4 * 1.3f;
        public float HarvestAngleH = MathHelper.PiOver2;
        public float HarvestStartAngle = -MathHelper.PiOver4 * 3.5f;
        public float HarvestTargetAngle = MathHelper.Pi * 2f;
        public float HarvestPrepareTime = 33f;
        public float HarvestAttackTime = 15f;
        public float HarvestUnwingTime = 24f;

        public float SpinAngleV = 0;
        public float SpinAngleH = MathHelper.PiOver2;
        public float SpinStartAngle = MathHelper.PiOver4 * 3;
        public float SpinTargetAngle = MathHelper.Pi * 1.5f;
        public float SpinPrepareTime = 27f;
        public float SpinAttackTime = 18f;
        public float SpinUnwingTime = 18f;

        public Texture2D FlowGraphTex = AssetUtils.GetTexture2D(AssetUtils.Flow + "Flow_1");

        public Texture2D SlashMainShape;
        public Texture2D SlashMainColor;
        public Texture2D SlashMask;
        public float SlashEnhance = 2.5f;
        #endregion
        private bool CanHitNpc = false;
        public override void SetStaticDefaults()
        {
            // Main.projFrames[Type] = 1;

            // ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2000;

            //ProjectileID.Sets.TrailCacheLength[Type] = 15;
            // 弹幕的旧数据记录数组长度，10为记录10个数据，新的数据会覆盖更早的数据，默认为10
            //ProjectileID.Sets.TrailingMode[Type] = 2;
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
            Projectile.timeLeft = 2; // 弹幕的存活时间
            //Projectile.alpha = 0; // 弹幕的透明度，0 ~ 255，0是完全不透明（int）
            Projectile.Opacity = 1; // 弹幕的不透明度，0 ~ 1，0是完全透明，1是完全不透明(float)，用哪个你们自己挑，这两是互相影响的
            Projectile.friendly = true; // 弹幕是否攻击敌方，默认false
            Projectile.hostile = false; // 弹幕是否攻击友方和城镇NPC，默认false
            Projectile.DamageType = DamageClass.Melee;

            // Projectile.aiStyle// 弹幕使用原版哪种弹幕AI类型
            // AIType  // 弹幕模仿原版哪种弹幕的行为
            // 上面两条，第一条是某种行为类型，可以查源码看看，第二条要有第一条才有效果，是让这个弹幕能执行对应弹幕的特殊判定行为
            Projectile.aiStyle = -1; // 不用原版的就写这个，也可以不写

            Projectile.extraUpdates = 8; // 弹幕每帧的额外更新次数，默认0
            SlashMask = AssetUtils.GetMask("Mask1");
            SlashMainColor = AssetUtils.GetColorBar("BloodHavest");
            SlashMainShape = AssetUtils.GetFlow("Flow_1");
            SlashEnhance = 3f;
        }
        //生成函数
        public override void OnSpawn(IEntitySource source)
        {
            SetAttackInfo();
            base.OnSpawn(source);
        }

        public override bool PreAI()
        {
            return base.PreAI();
        }
        public override void AI()
        {
            //运动部分
            switch (attackState)
            {
                case AttackState.prepare:
                    PrepareState();
                    break;
                case AttackState.attack:
                    Attack_State();
                    break;
                case AttackState.unwind:
                    UnwindState();
                    break;
                default:
                    break;
            }

            Projectile.timeLeft = 2;
            //保持物品使用
            Player.itemTime = Player.itemAnimation = 2;

            //控制手臂旋转
            Player.itemRotation = CurretAngle;
            //记录手臂位置
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, CurretAngle - MathHelper.ToRadians(90f)); // set arm position (90 degree offset since arm starts lowered)
            HandPos = Player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, CurretAngle - (float)Math.PI / 2); // get position of hand
            HandPos.Y += Player.gfxOffY;
            //计算武器实际大小
            WeaponCurretSize = StaticWeaponSize * TextureAssets.Projectile[Projectile.type].Size().Length() * WeaponSizeModify;
            //武器顶点位置
            WeaponTopPos = (CurretAngle).ToRotationVector2() * (WeaponCurretSize / 2f * (1 + HeldPosOffest));
            WeaponTopPos = DrawUtils.MartixTrans(WeaponTopPos, AngleV, AngleH);
            WeaponCenterFix = (CurretAngle).ToRotationVector2() * (WeaponCurretSize / 2f * HeldPosOffest);
            WeaponCenterFix = DrawUtils.MartixTrans(WeaponCenterFix, AngleV, AngleH); ;
            //记录顶点绘制所需的向量
            if (CanGetTrail)
            {
                for (int i = Slash.Length - 1; i > 0; i--)
                {
                    Slash[i] = Slash[i - 1];
                }
                Slash[0] = WeaponTopPos;
                //记录所需的旋转
                for (int i = WeaponRotation.Length - 1; i > 0; i--)
                {
                    WeaponRotation[i] = WeaponRotation[i - 1];
                }
                WeaponRotation[0] = CurretAngle;
            }

            base.AI();
        }
        public override void PostAI()
        {
            base.PostAI();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f; ;
            if (CanHitNpc)
            {
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), HandPos, HandPos + WeaponTopPos * 6 / 7f, 10f * StaticWeaponSize * WeaponSizeModify, ref point);
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            float DamageAttackTypeModify = 1;
            switch (attackType)
            {
                case AttackType.Swing:
                    DamageAttackTypeModify = 0.75f;
                    break;
                case AttackType.UpToDown:
                    DamageAttackTypeModify = 1.4f;
                    break;
                case AttackType.Spin:
                    DamageAttackTypeModify = 2f;
                    break;
                default:
                    break;
            }
            // Make knockback go away from player
            modifiers.HitDirectionOverride = target.position.X > Player.MountedCenter.X ? 1 : -1;
            modifiers.SourceDamage *= DamageAttackTypeModify;
            // If the NPC is hit by the spin attack, increase knockback slightly
            if (attackType == AttackType.UpToDown)
                modifiers.Knockback += 1;
            NPCExtraProperty extraProperty = target.GetGlobalNPC<NPCExtraProperty>();
            if (extraProperty.GetScytheHitCount() > 0)
            {
                float NPCLifeLoseModify = Math.Clamp((1 - target.GetLifePercent()) * 1, 1, 10);
                float ScytheSeparateScale = Player.GetModPlayer<PlayerExtraProperty>()._ScytheSeparateScale;
                float SeparateDamege = extraProperty.GetScytheHitPercent() * ScytheSeparateScale * Projectile.damage * DamageAttackTypeModify * NPCLifeLoseModify;
                NPC.HitInfo SeparateHitInfo = new NPC.HitInfo();
                SeparateHitInfo.Damage = (int)SeparateDamege;
                target.StrikeNPC(SeparateHitInfo);
            }
            extraProperty.AddScytheHitCount(1);

            if (Main.rand.NextFloat(0, 1) > 0.75f)
            {
                float LifeSteal = Projectile.damage * 0.075f * DamageAttackTypeModify;
                if ((int)LifeSteal != 0 && !(Main.player[Main.myPlayer].lifeSteal <= 0f))
                {
                    Main.player[Main.myPlayer].lifeSteal -= LifeSteal;
                    Projectile.NewProjectile(new EntitySource_ItemUse_OnHurt(Player, Player.HeldItem, target), HandPos + WeaponTopPos * Main.rand.NextFloat(0.6f, 1f), Vector2.Zero, 305, (int)(Projectile.damage * 0.3f), 0, Projectile.owner, Projectile.owner, LifeSteal);
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {

            DrawAnimation();
            return false;
        }
        public virtual void DrawAnimation()
        {
            switch (attackState)
            {
                case AttackState.prepare:
                    WeaponDraw();
                    break;
                case AttackState.attack:
                    WeaponDraw();
                    VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.DistortedGraphDraw, GhostDraw);
                    VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.FlowGraphDraw, FlowGraphDraw);
                    VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.DistortedGraphDraw, SlashDraw);

                    break;
                case AttackState.unwind:
                    WeaponDraw();
                    VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.DistortedGraphDraw, GhostDraw);
                    VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.FlowGraphDraw, FlowGraphDraw);
                    VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.DistortedGraphDraw, SlashDraw);
                    break;
                default:
                    break;
            }
        }
        public virtual void WeaponDraw()
        {
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            //贴图旋转修正
            float TexRotationFix = MathHelper.PiOver4 * Clockwise;
            //武器贴图
            Texture2D weapon = TextureAssets.Projectile[Type].Value;
            //长轴修正



            if (Clockwise == 1)
            {
                DrawUtils.DrawEntityInWorld(sb, weapon, HandPos - Main.screenPosition + WeaponCenterFix, Color.White, null, CurretAngle + TexRotationFix * Clockwise, WeaponCurretSize / weapon.Size().Length(), AngleV, AngleH, null, BlendState.AlphaBlend, 0);
            }
            else
            {
                DrawUtils.DrawEntityInWorld(sb, weapon, HandPos - Main.screenPosition + WeaponCenterFix, Color.White, null, CurretAngle + TexRotationFix * Clockwise, WeaponCurretSize / weapon.Size().Length(), AngleV, AngleH, null, BlendState.AlphaBlend, 1);

            }
        }
        public virtual void GhostDraw()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            //贴图旋转修正
            float TexRotationFix = MathHelper.PiOver4 * Clockwise;
            //武器贴图
            Texture2D weapon = TextureAssets.Projectile[Type].Value;
            SpriteBatch sb = Main.spriteBatch;
            //长轴修正
            for (int i = 0; i < WeaponRotation.Length; i++)
            {
                if (i % 30 == 0)
                {
                    if (WeaponRotation[i] == 0)
                    {
                        continue;
                    }
                    Vector2 centerFix = (WeaponRotation[i]).ToRotationVector2() * (WeaponCurretSize / 2f * HeldPosOffest);
                    centerFix = DrawUtils.MartixTrans(centerFix, AngleV, AngleH);
                    //长轴修正
                    if (Clockwise == 1)
                    {
                        DrawUtils.DrawEntityInWorld(sb, weapon, HandPos - Main.screenPosition + centerFix, Color.White * ((WeaponRotation.Length - i) / (float)WeaponRotation.Length), null, WeaponRotation[i] + TexRotationFix * Clockwise, WeaponCurretSize / weapon.Size().Length(), AngleV, AngleH, null, BlendState.AlphaBlend, 0);
                    }
                    else
                    {
                        DrawUtils.DrawEntityInWorld(sb, weapon, HandPos - Main.screenPosition + centerFix, Color.White * ((WeaponRotation.Length - i) / (float)WeaponRotation.Length), null, WeaponRotation[i] + TexRotationFix * Clockwise, WeaponCurretSize / weapon.Size().Length(), AngleV, AngleH, null, BlendState.AlphaBlend, 1);
                    }
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
        }
        public virtual void FlowGraphDraw()
        {
            //武器贴图
            Texture2D weapon = TextureAssets.Projectile[Type].Value;
            List<CustomVertexInfo> bars = new List<CustomVertexInfo>();
            // 把所有的点都生成出来，按照顺序
            for (int i = 0; i < Slash.Length - 1; i++)
            {
                if (Slash[i].SafeNormalize(Vector2.Zero) == Vector2.UnitX) continue;
                //Main.spriteBatch.Draw(Main.magicPixel, oldPosi[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                var factor = i / (float)ProjectileID.Sets.TrailCacheLength[Type];

                var w = MathHelper.Lerp(1f, 0.05f, factor);
                float r = w * 255;
                var color = new Color(255, 255, 0);
                float LengthFix = i < Slash.Length / 2f ? (1 + MathF.Abs(i) / Slash.Length * 0.3f) : (1 + MathF.Abs(Slash.Length - i) / Slash.Length * 0.3f);
                bars.Add(new CustomVertexInfo(HandPos - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 1, 1)));
                bars.Add(new CustomVertexInfo(HandPos + Slash[i] * 6 / 7f * LengthFix - Main.screenPosition, color, new Vector3((float)Math.Sqrt(factor), 0, 1)));
            }

            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形

                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);

                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
                Effect Default = AssetUtils.GetEffect("VertexDefault");
                Default.Parameters["UTransform"].SetValue(model * Main.Transform * projection);
                Main.graphics.GraphicsDevice.Textures[0] = FlowGraphTex;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Default.CurrentTechnique.Passes[0].Apply();

                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin();
            }
        }

        public virtual void SlashDraw()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            //贴图旋转修正
            //武器贴图

            Texture2D weapon = TextureAssets.Projectile[Type].Value;
            Vector2[] ButtomPos = new Vector2[180];
            Vector2[] TopPos = new Vector2[180];
            for (int i = Slash.Length - 1; i >= 0; i--)
            {
                if (Slash[i] == Vector2.Zero)
                {
                    continue;
                }
                float LengthFix = i < Slash.Length / 2f ? (1 + MathF.Abs(i) / Slash.Length * 0.3f) : (1 + MathF.Abs(Slash.Length - i) / Slash.Length * 0.3f);
                //长轴修正
                ButtomPos[i] = Slash[i] * 0.6f + HandPos;
                TopPos[i] = Slash[i] * 6 / 7f * LengthFix + HandPos;
            }
            DrawUtils.DrawTrail(TopPos, ButtomPos, SlashMainShape, SlashMask, SlashMainColor, SlashEnhance);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
        }

        public virtual void SpownDust()
        {
            if (Main.rand.NextFloat() * 3f < Projectile.Opacity)
            {
                // Original Excalibur color: Color.Gold, Color.White
                Color dustColor = Color.Lerp(Color.DarkRed, Color.Red, Main.rand.NextFloat() * 0.3f);
                Dust coloredDust = Dust.NewDustPerfect(HandPos + WeaponTopPos * 6 / 7f * Main.rand.NextFloat(0.5f, 1), DustID.FireworksRGB, CurretAngle.ToRotationVector2().RotatedBy(MathHelper.PiOver2 * Clockwise) * Main.rand.NextFloat(0.5f, 2), 188, dustColor, 0.8f);
                coloredDust.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
                coloredDust.noGravity = false;
                if (Main.rand.NextFloat() * 1.5f < Projectile.Opacity)
                {
                    Dust.NewDustPerfect(HandPos + WeaponTopPos * 6 / 7f * Main.rand.NextFloat(0.5f, 1), 27, CurretAngle.ToRotationVector2().RotatedBy(MathHelper.PiOver2 * Clockwise) * Main.rand.NextFloat(1, 4), 100, Color.Red * Projectile.Opacity, 1.2f * Projectile.Opacity).fadeIn = 0.4f;
                }
            }
        }
        //是否可以造成伤害
        public override bool? CanDamage()
        {

            return null;
        }
        //是否更新位置
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public float GetExtraUpdateModify()
        {
            return 1f / (1 + Projectile.extraUpdates);
        }

        public virtual void PrepareState()
        {

            Progress += ProgressAdd_VarFlame;
            Timer++;
            float SpownAngle = Clockwise > 0 ? 0 : MathHelper.Pi;
            switch (attackType)
            {
                case AttackType.Swing:
                    CanGetTrail = false;
                    CanHitNpc = false;
                    CurretAngle = MathHelper.SmoothStep(-MathHelper.PiOver4 * 1.5f * Clockwise + SpownAngle, StartAngle, Timer / prepareTime);
                    StaticWeaponSize = MathHelper.SmoothStep(0, DefaultWeaponSize, Timer / prepareTime);
                    break;
                case AttackType.UpToDown:
                    CanGetTrail = false;
                    CanHitNpc = false;
                    CurretAngle = StartAngle;
                    StaticWeaponSize = MathHelper.SmoothStep(DefaultWeaponSize * 0.5f, DefaultWeaponSize, Timer / prepareTime);
                    break;
                case AttackType.Spin:
                    CanGetTrail = false;
                    CanHitNpc = false;
                    CurretAngle = StartAngle;
                    StaticWeaponSize = MathHelper.SmoothStep(0, DefaultWeaponSize, Timer / prepareTime);
                    break;
                default:
                    break;
            }
            SoundEngine.PlaySound(SoundID.Item71);
            if (Timer > prepareTime)
            {
                attackState = AttackState.attack;
                Timer = 0;
            }
        }
        public virtual void Attack_State()
        {
            Progress += ProgressAdd_VarFlame;
            Timer++;
            SpownDust();
            switch (attackType)
            {
                case AttackType.Swing:
                    CanGetTrail = true;
                    CanHitNpc = true;
                    float factor = Timer / attackTime;
                    CurretAngle = MathHelper.SmoothStep(StartAngle, StartAngle + TargetAngle * Clockwise, factor);
                    AngleV = MathHelper.SmoothStep(MathHelper.PiOver4 * 0.7f, -MathHelper.PiOver4 * 0.7f, factor);
                    break;
                case AttackType.UpToDown:
                    CanGetTrail = true;
                    CanHitNpc = true;
                    float Sizefactor = (Timer > attackTime / 2f ? Timer / attackTime : (attackTime - Timer) / attackTime) * 2;
                    CurretAngle = MathHelper.SmoothStep(StartAngle, StartAngle + TargetAngle * Clockwise, Timer / attackTime);
                    StaticWeaponSize = MathHelper.SmoothStep(DefaultWeaponSize, DefaultWeaponSize * 1.5f, Sizefactor);
                    break;
                case AttackType.Spin:
                    CanGetTrail = true;
                    CanHitNpc = true;
                    CurretAngle = MathHelper.SmoothStep(StartAngle, StartAngle + TargetAngle * Clockwise, Timer / attackTime);
                    float _Sizefactor = (Timer > attackTime / 2f ? Timer / attackTime : (attackTime - Timer) / attackTime) * 2;
                    StaticWeaponSize = MathHelper.SmoothStep(DefaultWeaponSize, DefaultWeaponSize * 1.5f, _Sizefactor);
                    break;
                default:
                    break;
            }
            if (Timer > attackTime)
            {
                attackState = AttackState.unwind;
                Timer = 0;
            }
        }
        public virtual void UnwindState()
        {
            Timer++;
            float SpownAngle = Clockwise > 0 ? 0 : MathHelper.Pi;
            CanHitNpc = false;
            switch (attackType)
            {
                case AttackType.Swing:
                    StaticWeaponSize = MathHelper.SmoothStep(DefaultWeaponSize, 1, Timer / unwingdTime);
                    CurretAngle = MathHelper.SmoothStep(StartAngle, MathHelper.PiOver4, Timer / unwingdTime);
                    break;
                case AttackType.UpToDown:
                    StaticWeaponSize = MathHelper.SmoothStep(DefaultWeaponSize * 1.5f, DefaultWeaponSize, Timer / unwingdTime);
                    CurretAngle = MathHelper.SmoothStep(StartAngle, MathHelper.PiOver4, Timer / unwingdTime);
                    break;
                case AttackType.Spin:
                    StaticWeaponSize = MathHelper.SmoothStep(DefaultWeaponSize * 1.5f, DefaultWeaponSize, Timer / unwingdTime);
                    break;
                default:
                    break;
            }
            if (Timer > unwingdTime)
            {
                //状态归位
                attackState = AttackState.prepare;
                for (int i = 0; i < Slash.Length - 1; i++)
                {
                    Slash[i] = Vector2.Zero;
                }
                for (int i = 0; i < WeaponRotation.Length - 1; i++)
                {
                    WeaponRotation[i] = 0;

                }
                Timer = 0;
                Projectile.Kill();
                return;

            }
        }
        public virtual void SetAttackInfo()
        {
            //获取方向
            Clockwise = Projectile.velocity.X;
            float SpownAngle = Clockwise > 0 ? 0 : MathHelper.Pi;
            //加载攻击方式的数据
            StaticWeaponSize = DefaultWeaponSize;
            float SpeedPrize = DoubleHitPrize ? 0.8f : 1f;
            switch (attackType)
            {
                case AttackType.Swing:
                    AngleH = SwingAngleH;
                    AngleV = SwingAngleV;
                    TargetAngle = SwingTargetAngle;
                    StartAngle = SpownAngle + SwingStartAngle * Clockwise;
                    prepareTime = SwingPrepareTime * GetFlameModify() * SpeedPrize;
                    attackTime = SwingAttackTime * GetFlameModify();
                    unwingdTime = SwingUnwingTime * GetFlameModify();
                    StaticAttackFlameCount = prepareTime + attackTime + unwingdTime;
                    break;
                case AttackType.UpToDown:
                    AngleH = HarvestAngleH;
                    AngleV = HarvestAngleV;
                    TargetAngle = HarvestTargetAngle;
                    StartAngle = SpownAngle + HarvestStartAngle * Clockwise;
                    prepareTime = HarvestPrepareTime * GetFlameModify() * SpeedPrize;
                    attackTime = HarvestAttackTime * GetFlameModify();
                    unwingdTime = HarvestUnwingTime * GetFlameModify();
                    StaticAttackFlameCount = prepareTime + attackTime + unwingdTime;
                    break;
                case AttackType.Spin:
                    AngleH = SpinAngleH;
                    AngleV = SpinAngleV;
                    TargetAngle = SpinTargetAngle;
                    StartAngle = SpownAngle + SpinStartAngle * Clockwise;
                    prepareTime = SpinPrepareTime * GetFlameModify() * SpeedPrize;
                    attackTime = SpinAttackTime * GetFlameModify();
                    unwingdTime = SpinUnwingTime * GetFlameModify();
                    StaticAttackFlameCount = prepareTime + attackTime + unwingdTime;
                    break;
                default:
                    break;
            }

        }
        public enum AttackType
        {
            Swing, UpToDown, Spin
        }
        private enum AttackState
        {
            prepare, attack, unwind
        }
        private float GetFlameModify()
        {
            return (1 + Projectile.extraUpdates) / WeaponSizeModify;
        }
    }
}