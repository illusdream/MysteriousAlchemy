using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Mapping;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Perfab.Projectiles
{
    //快速挥舞的镰刀,攻击方式为大风车，如果蓄力达到一定程度，松开右键时会触发连段，额外进行两次攻击：向鼠标方向挥砍一次，再向人物正前方挥砍有一次
    //在镰刃区造成正常伤害，长柄部分只造成25%伤害，以及高额击退
    public abstract class BaseQuickScythe : BaseHeldProj, IDrawPrimitives
    {
        //触发连段所需最低动量
        public bool CanSweep { get; set; }
        //长柄指向的角度
        public float AttackAngle { get; set; }

        public float SwingAccTime => 300;
        public float MaxSwingSpeed => MathHelper.TwoPi / 25f;

        public virtual float AngleH { get; set; }
        public virtual float AngleV { get; set; }
        public virtual float WeaponStaticSize { get; set; }

        public Vector2 WeaponCurretSize => WeaponStaticSize * TextureAssets.Projectile[Projectile.type].Size() * WeaponSizeModify;

        public float WeaponCurretLength => WeaponCurretSize.Length();

        public virtual float HeldPosOffest { get; set; }

        public Vector2 HandPos;

        public Vector2 WeaponHeldPostion => DrawUtils.MartixTrans((AttackAngle).ToRotationVector2() * (WeaponCurretSize / 2f * HeldPosOffest), AngleH, AngleV);

        public Vector2 WeaponTopPosition => DrawUtils.MartixTrans((AttackAngle).ToRotationVector2() * (WeaponCurretSize / 2f * (1 + HeldPosOffest)), AngleH, AngleV);

        public int SoundPlayInterval => 60;

        public Vector2[] SlashTop;
        public Vector2[] SlashBottom;
        public int SlashLength => 17;

        public int Timer;

        public int OnHitFreezeTimer;

        public int FreezeTimer;

        public int MaxFreezeTimer => 30;
        //极短的卡肉时间
        public int HitFreezeTime => 3;

        public bool active = true;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void Initialize()
        {
            RegisterState<SwingState>(new SwingState(this));
            RegisterState<SweepState>(new SweepState(this));
            SetState<SwingState>();
            base.Initialize();
        }
        public override void AI()
        {

            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            WeaponDraw(Main.spriteBatch);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {

            base.PostDraw(lightColor);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopRight(), targetHitbox.Size(), Projectile.Center, Projectile.Center + AttackAngle.ToRotationVector2() * WeaponTopPosition.Length() * 0.75f, 3, ref point);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (OnHitFreezeTimer == 0 && FreezeTimer < 0)
            {
                OnHitFreezeTimer = 1;
                FreezeTimer = MaxFreezeTimer;
            }

            VisualEffectSystem.AddScreenShake_OnHit(HandPos, AttackAngle.ToRotationVector2().SafeNormalize(Vector2.One), 0.5f);
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnKill(int timeLeft)
        {
            //DelaySlash.NewDelaySlash<DelaySlash>(Projectile.Center, SlashTop, SlashBottom, 60, AssetUtils.Extra + "Slash_1", AssetUtils.ColorBar + "Frost3");

            base.OnKill(timeLeft);
        }
        public virtual void OnHit()
        {

        }

        public virtual void UpdatePlayerArm()
        {
            //让弹幕图层在在玩家手中
            Owner.heldProj = Projectile.whoAmI;
            //保持物品使用
            Owner.itemTime = Owner.itemAnimation = 2;

            float ArmToward = (Main.MouseWorld - Owner.Center).ToRotation();
            //控制手臂旋转
            Owner.itemRotation = ArmToward;
            //记录手臂位置
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, ArmToward - MathHelper.ToRadians(90f)); // set arm position (90 degree offset since arm starts lowered)
            HandPos = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, ArmToward - (float)Math.PI / 2); // get position of hand
            HandPos.Y += Owner.gfxOffY;
        }
        public virtual void UpdatePosition()
        {
            Projectile.Center = Owner.Center;
        }
        public virtual void UpdateAngle()
        {
            if (OnHitFreezeTimer != 0 && VisualEffectSystem.HitEffect_AttackFreeze && OnHitFreezeTimer < HitFreezeTime)
            {
                FreezeTimer--;
                OnHitFreezeTimer++;
                return;
            }
            if (OnHitFreezeTimer >= HitFreezeTime)
                OnHitFreezeTimer = 0;
            AttackAngle += MaxSwingSpeed * OwnerDirection * MathF.Sqrt(Math.Clamp(Timer / SwingAccTime, 0, 1));

        }
        public virtual void UpdateCanSweep()
        {
            if (MathF.Sqrt(Math.Clamp(Timer / SwingAccTime, 0, 1)) == 1 && !CanSweep)
            {
                OnCanSweep();
                CanSweep = true;
            }
        }
        public virtual void UpdateProjectileAlive()
        {
            if (active)
                Projectile.timeLeft = 2;
        }

        public virtual void UpdateSlashCache()
        {
            if (SlashTop is null)
            {
                SlashTop = new Vector2[SlashLength];
                for (int i = 0; i < SlashLength; i++)
                {
                    SlashTop[i] = DrawUtils.MartixTrans(AttackAngle.ToRotationVector2() * WeaponTopPosition.Length() * 0.75f, AngleH, AngleV);
                }
            }
            if (SlashBottom is null)
            {
                SlashBottom = new Vector2[SlashLength];
                for (int i = 0; i < SlashLength; i++)
                {
                    SlashBottom[i] = DrawUtils.MartixTrans(AttackAngle.ToRotationVector2() * WeaponTopPosition.Length() * 0.4f * 0.75f, AngleH, AngleV);
                }
            }
            for (int i = SlashLength - 1; i > 0; i--)
            {
                SlashTop[i] = SlashTop[i - 1];
            }
            SlashTop[0] = DrawUtils.MartixTrans(AttackAngle.ToRotationVector2() * WeaponTopPosition.Length() * 0.75f, AngleH, AngleV);
            for (int i = SlashLength - 1; i > 0; i--)
            {
                SlashBottom[i] = SlashBottom[i - 1];
            }
            SlashBottom[0] = DrawUtils.MartixTrans(AttackAngle.ToRotationVector2() * WeaponTopPosition.Length() * 0.4f * 0.75f, AngleH, AngleV);
        }
        public virtual void PlaySound()
        {
            if (AttackAngle % (MathHelper.TwoPi / 3f) < 0.2f)
                SoundEngine.PlaySound(MASoundID.Swing_Item1);
        }



        public virtual void WeaponDraw(SpriteBatch spriteBatch)
        {
            //贴图旋转修正
            float TexRotationFix = MathHelper.PiOver4 * OwnerDirection;
            //武器贴图
            Texture2D weapon = TextureAssets.Projectile[Type].Value;

            if (OwnerDirection == 1)
            {
                DrawUtils.DrawEntityInWorld(
                    spriteBatch,
                    weapon,
                    HandPos - Main.screenPosition + WeaponHeldPostion,
                    Color.White,
                    default,
                    AttackAngle + TexRotationFix * OwnerDirection,
                    DrawUtils.GetCurrectScale(weapon, WeaponCurretSize),
                    AngleH,
                    AngleV
                    );
            }
            else
            {
                DrawUtils.DrawEntityInWorld(
                    spriteBatch,
                    weapon,
                    HandPos - Main.screenPosition + WeaponHeldPostion,
                    Color.White,
                    default,
                    AttackAngle + TexRotationFix * OwnerDirection,
                    DrawUtils.GetCurrectScale(weapon, WeaponCurretSize),
                    AngleH,
                    AngleV,
                    Enum.ModifySpriteEffect.FlipDiagonally
                    );
            }
        }
        public virtual void DrawSlash(Color color, Texture2D shape, Texture2D colorBar)
        {

        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public virtual void DrawPrimitives(SpriteBatch spriteBatch)
        {
            DrawSlash(Color.White, AssetUtils.GetTexture2D(AssetUtils.Extra + "Slash_1"), AssetUtils.GetColorBar("Frost3"));
            VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.BloomAreaDraw, DrawBloom);
        }
        public virtual void DrawFlowGraph()
        {

        }

        public virtual void DrawBloom()
        {

        }

        public virtual void OnCanSweep()
        {

        }

        public virtual void OnSwitchStage()
        {

        }

        protected class SwingState : ProjState<BaseQuickScythe>
        {
            public SwingState(ProjStateMachine projStateMachine) : base(projStateMachine)
            {
            }
            public override void EntryState(IStateMachine stateMachine)
            {
                Projectile.active = true;
                base.EntryState(stateMachine);
            }
            public override void OnState(IStateMachine stateMachine)
            {
                Projectile.Timer++;
                //PlaySound();
                Projectile.UpdateProjectileAlive();
                Projectile.UpdatePlayerArm();
                Projectile.UpdateAngle();
                Projectile.UpdateCanSweep();
                Projectile.UpdatePosition();
                Projectile.UpdateSlashCache();
                CheckSwitchState();
                base.OnState(stateMachine);
            }
            private void CheckSwitchState()
            {
                if (Projectile.CanSweep && !Projectile.Owner.controlUseItem)
                {
                    Projectile.SwitchState<SweepState>();
                    return;
                }

                if (!Projectile.CanSweep && !Projectile.Owner.controlUseItem)
                {
                    Projectile.active = false;
                    return;
                }

            }
        }
        protected class SweepState : ProjState<BaseQuickScythe>
        {
            public SweepState(ProjStateMachine projStateMachine) : base(projStateMachine)
            {
            }
            public override void EntryState(IStateMachine stateMachine)
            {
                float SpownAngle = MathHelper.Pi * (Projectile.OwnerDirection == 1 ? 0 : 1);
                Projectile.AngleH = MathHelper.PiOver4;
                Projectile.AngleV = MathHelper.PiOver2;
                SoundEngine.PlaySound(MASoundID.Slash_Item71);
                DebugUtils.NewText(Projectile.WeaponStaticSize);
                Projectile.AttackAngle = -MathHelper.PiOver2 * 1.5f * Projectile.OwnerDirection + SpownAngle;
                for (int i = 0; i < Projectile.SlashLength; i++)
                {
                    Projectile.SlashTop[i] = DrawUtils.MartixTrans(Projectile.AttackAngle.ToRotationVector2() * Projectile.WeaponTopPosition.Length() * 0.75f, Projectile.AngleH, Projectile.AngleV);
                }
                for (int i = 0; i < Projectile.SlashLength; i++)
                {
                    Projectile.SlashBottom[i] = DrawUtils.MartixTrans(Projectile.AttackAngle.ToRotationVector2() * Projectile.WeaponTopPosition.Length() * 0.4f * 0.75f, Projectile.AngleH, Projectile.AngleV);
                }
                Projectile.Timer = 0;
                Projectile.OnSwitchStage();
                base.EntryState(stateMachine);
            }
            public override void OnState(IStateMachine stateMachine)
            {
                Projectile.Timer++;
                updatAngle();
                //PlaySound();
                Projectile.UpdateProjectileAlive();
                Projectile.UpdatePlayerArm();
                Projectile.UpdatePosition();
                Projectile.UpdateSlashCache();
                base.OnState(stateMachine);
                if (Projectile.Timer > 20)
                    Projectile.active = false;
            }
            private void updatAngle()
            {
                float SpownAngle = MathHelper.Pi * (Projectile.OwnerDirection == 1 ? 0 : 1);
                DebugUtils.NewText(Projectile.OwnerDirection);
                Projectile.AttackAngle = MathHelper.Lerp(-MathHelper.PiOver2 * 1.5f * Projectile.OwnerDirection + SpownAngle, MathHelper.PiOver2 * 1.3f * Projectile.OwnerDirection + SpownAngle, MathF.Pow(Projectile.Timer / 20f, 1.7f));
            }
        }
    }
    public enum QuickScytheSytle
    {
        Swing, Sweep
    }
}
