using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Utils;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Core.Trails;
using MysteriousAlchemy.UI;
using MysteriousAlchemy.Utils;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Drawing;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.RGB;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MysteriousAlchemy.Content.Animators
{
    public class AltarAnimator : Animator
    {
        public override DrawSortWithPlayer DrawSortWithPlayer => DrawSortWithPlayer.Behind;

        public List<EtherCrystal> etherCrystalList_Ready;
        public List<EtherCrystal> etherCrystalList_Prepare;

        MagicRing magicRing_1;
        MagicRing magicRing_2;
        MagicRing magicRing_3;

        float Raduim_1 = 200;
        float Raduim_2 = 250;
        float Raduim_3 = 300;

        public Item Product;
        public override void Initialize()
        {
            base.Initialize();

            RegisterState<Standby>(new Standby(this));
            RegisterState<OpenUI>(new OpenUI(this));
            RegisterState<EntryOpenUI>(new EntryOpenUI(this));
            RegisterState<ExitOpenUI>(new ExitOpenUI(this));
            RegisterState<EntryCompose>(new EntryCompose(this));
            RegisterState<Compose>(new Compose(this));


            SetState(typeof(Standby).ToString());
            etherCrystalList_Ready = new List<EtherCrystal>();
            etherCrystalList_Prepare = new List<EtherCrystal>();
        }
        public override void OnKill()
        {
            if (UI_AltarCompose.AltarAnimator == this)
            {
                UI_AltarCompose.visable = false;
            }
            base.OnKill();
        }
        public bool AddEtherCrystal()
        {
            if (CurrectState is Standby && etherCrystalList_Ready.Count < 12)
            {
                SoundEngine.PlaySound(MASoundID.Bell_Item35, Position);
                EtherCrystal etherCrystal = RegisterDrawUnit<EtherCrystal>(Position);
                etherCrystalList_Prepare.Add(etherCrystal);
                return true;
            }
            return false;
        }
        public void AddMagicCircle()
        {
            switch ((int)(etherCrystalList_Ready.Count / 4))
            {
                case 1:
                    magicRing_1 = RegisterDrawUnit<MagicRing>(Position, Vector2.One * Raduim_1);
                    break;
                case 2:
                    magicRing_2 = RegisterDrawUnit<MagicRing>(Position, Vector2.One * Raduim_2);
                    if (magicRing_1 == null)
                        magicRing_1 = RegisterDrawUnit<MagicRing>(Position, Vector2.One * Raduim_1);
                    break;
                case 3:
                    magicRing_3 = RegisterDrawUnit<MagicRing>(Position, Vector2.One * Raduim_3);
                    if (magicRing_1 == null)
                        magicRing_1 = RegisterDrawUnit<MagicRing>(Position, Vector2.One * Raduim_1);
                    if (magicRing_2 == null)
                        magicRing_2 = RegisterDrawUnit<MagicRing>(Position, Vector2.One * Raduim_2);
                    break;
                default:
                    break;
            }
        }
        public void SwitchUI_AltarComposeVisable()
        {
            if (CurrectState is Standby && magicRing_1 != null && magicRing_2 != null && magicRing_3 != null && etherCrystalList_Ready.Count > 0)
            {
                SwitchState(typeof(EntryOpenUI).ToString());
            }
            if (CurrectState is OpenUI)
            {
                SwitchState(typeof(ExitOpenUI).ToString());
            }
        }

        public void CommonElementAI()
        {
            for (int i = 0; i < etherCrystalList_Ready.Count; i++)
            {
                Vector2 target = Position + DrawUtils.MartixTrans(new Vector2(MathF.Sin((float)Main.time / 600 + MathHelper.TwoPi / etherCrystalList_Ready.Count * i), MathF.Cos((float)Main.time / 600 + MathHelper.TwoPi / etherCrystalList_Ready.Count * i)) * 175, MathHelper.PiOver4 * 1.75f, MathHelper.PiOver2);
                etherCrystalList_Ready[i].Pivot = MathUtils.SteppingTrack(etherCrystalList_Ready[i].Pivot, target, 0.05f);

                etherCrystalList_Ready[i].Offest = DrawUtils.MartixTrans(new Vector2(MathF.Sin((float)Main.time / 300 + MathHelper.TwoPi / etherCrystalList_Ready.Count * i), MathF.Cos((float)Main.time / 300 + MathHelper.TwoPi / etherCrystalList_Ready.Count * i)) * 3, MathHelper.PiOver4, MathHelper.PiOver2);
                if (DrawUtils.GetEntityContextInCircle((float)Main.time / 600 + MathHelper.TwoPi / etherCrystalList_Ready.Count * i))
                {
                    etherCrystalList_Ready[i].DrawSortWithUnits = DrawSortWithUnits.Front;
                }
                else
                {
                    etherCrystalList_Ready[i].DrawSortWithUnits = DrawSortWithUnits.Behind;
                }
            }
            for (int i = 0; i < etherCrystalList_Prepare.Count; i++)
            {
                Vector2 target = Position + DrawUtils.MartixTrans(new Vector2(MathF.Sin((float)Main.time / 600 + MathHelper.TwoPi / etherCrystalList_Prepare.Count * i), MathF.Cos((float)Main.time / 300 + MathHelper.TwoPi / etherCrystalList_Prepare.Count * i)) * Raduim_1 * 0.1f, 0, MathHelper.PiOver2);
                etherCrystalList_Prepare[i].Pivot = MathUtils.SteppingTrack(etherCrystalList_Prepare[i].Pivot, target, 0.05f);
            }
            if (magicRing_1 != null)
            {
                magicRing_1.Pivot = Position;
                magicRing_1.Rotation += MathHelper.PiOver4 * 0.05f;
            }
            if (magicRing_2 != null)
            {
                magicRing_2.Pivot = Position;
                magicRing_2.Rotation += MathHelper.PiOver4 * 0.03f;
                magicRing_2.AngleV += MathHelper.PiOver4 * 0.01f;
            }
            if (magicRing_3 != null)
            {
                magicRing_3.Pivot = Position;
                magicRing_3.Rotation += MathHelper.PiOver4 * 0.01f;
                magicRing_3.AngleV += MathHelper.PiOver4 * 0.007f; ;
                magicRing_3.AngleH += MathHelper.PiOver4 * 0.01f;
            }
        }

        public void ShakeCrsytal()
        {

        }
        public void DistoryCrtstal()
        {
            for (int i = 0; i < etherCrystalList_Ready.Count; i++)
            {
                var instance = RegisterDrawUnit<TrailDrawunit>(etherCrystalList_Ready[i].Pivot + etherCrystalList_Ready[i].Offest);
                instance.TargetPos = Position;
                instance.velocity = (etherCrystalList_Ready[i].Pivot - Position).SafeNormalize(Vector2.One).RotatedBy(MathHelper.Pi * Main.rand.NextFloat()) * 15 * Main.rand.NextFloat(1, 1.5f);
                etherCrystalList_Ready[i].active = false;
                SoundEngine.PlaySound(MASoundID.Ding_Item4);
            }
            etherCrystalList_Ready.Clear();
        }

        public void AddCorona()
        {
            var instance = RegisterDrawUnit<ShimmerCorona>(Position);
            instance.AngleV = MathHelper.PiOver2;
            instance.AngleH = 0;
            instance.startAngle = MathHelper.TwoPi * Main.rand.NextFloat();
            instance.raduim = Raduim_1 * 0.5f;
        }
        public override void AI()
        {

            base.AI();
        }
        #region //×´Ì¬
        private class Standby : AnimationState<AltarAnimator>
        {

            public Standby(IStateMachine stateMachine) : base(stateMachine)
            {
            }
            public override void EntryState(IStateMachine animator)
            {

                base.EntryState(animator);
            }
            public override void OnState(IStateMachine animator)
            {
                Timer++;
                Animator.CommonElementAI();
                AddMagicCircle();
                base.OnState(animator);
            }
            public override void ExitState(IStateMachine animator)
            {
                base.ExitState(animator);
            }
            public void AddMagicCircle()
            {
                if (Animator.etherCrystalList_Prepare.Count >= 4)
                {
                    Animator.etherCrystalList_Ready.AddRange(Animator.etherCrystalList_Prepare.GetRange(0, 4));
                    Animator.AddMagicCircle();
                    Animator.etherCrystalList_Prepare.RemoveRange(0, 4);

                }
            }
        }
        private class EntryOpenUI : AnimationState<AltarAnimator>
        {
            Vector2 MagicRing_1TargetScale;
            Vector2 MagicRing_2TargetScale;
            Vector2 MagicRing_3TargetScale;
            float MagicRing_1TargetAngleH;
            float Timer;
            float TotalTime = 60;
            public EntryOpenUI(IStateMachine stateMachine) : base(stateMachine)
            {
            }
            public override void EntryState(IStateMachine animator)
            {
                MagicRing_1TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_1.TextureInstance, Vector2.One * Animator.Raduim_3);
                MagicRing_2TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_2.TextureInstance, Vector2.One * Animator.Raduim_2);
                MagicRing_3TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_3.TextureInstance, Vector2.One * Animator.Raduim_1);
                MagicRing_1TargetAngleH = 0;
                base.EntryState(animator);
            }
            public override void OnState(IStateMachine animator)
            {
                Timer++;
                if (Animator.magicRing_1 != null)
                {
                    Animator.magicRing_1.Scale = MathUtils.SteppingTrack(Animator.magicRing_1.Scale, MagicRing_1TargetScale, 0.05f);
                    Animator.magicRing_1.AngleH = MathUtils.SteppingTrack(Animator.magicRing_1.AngleH, MagicRing_1TargetAngleH, 0.05f);
                }
                if (Animator.magicRing_2 != null)
                {
                    Animator.magicRing_2.Scale = MathUtils.SteppingTrack(Animator.magicRing_2.Scale, MagicRing_2TargetScale, 0.05f);
                }
                if (Animator.magicRing_3 != null)
                {
                    Animator.magicRing_3.Scale = MathUtils.SteppingTrack(Animator.magicRing_3.Scale, MagicRing_3TargetScale, 0.05f);
                }
                Animator.CommonElementAI();
                Animator.magicRing_1.Rotation -= MathHelper.PiOver4 * 0.05f * (Timer / TotalTime);
                if (Timer > TotalTime)
                {
                    Timer = 0;
                    Animator.SwitchState(typeof(OpenUI).ToString());
                }

                base.OnState(animator);
            }
            public override void ExitState(IStateMachine animator)
            {
                UI_AltarCompose.visable = true;
                UI_AltarCompose.AltarAnimator = Animator;
                base.ExitState(animator);
            }
        }
        private class OpenUI : AnimationState<AltarAnimator>
        {
            public OpenUI(IStateMachine stateMachine) : base(stateMachine)
            {
            }
            public override void EntryState(IStateMachine animator)
            {
                base.EntryState(animator);
            }
            public override void OnState(IStateMachine animator)
            {
                Animator.CommonElementAI();
                Animator.magicRing_1.Rotation -= MathHelper.PiOver4 * 0.05f;
                UICheckInCurrcetAnimator();
                base.OnState(animator);
            }
            public override void ExitState(IStateMachine animator)
            {
                if (UI_AltarCompose.AltarAnimator == Animator)
                {
                    UI_AltarCompose.visable = false;
                }
                base.ExitState(animator);
            }
            public void UICheckInCurrcetAnimator()
            {
                if (UI_AltarCompose.AltarAnimator != Animator)
                {
                    Animator.SwitchState(typeof(ExitOpenUI).ToString());
                }
            }
        }
        private class ExitOpenUI : AnimationState<AltarAnimator>
        {
            Vector2 MagicRing_1TargetScale;
            Vector2 MagicRing_2TargetScale;
            Vector2 MagicRing_3TargetScale;
            float MagicRing_1TargetAngleH;
            float TotalTime = 60;
            public ExitOpenUI(IStateMachine stateMachine) : base(stateMachine)
            {
            }
            public override void EntryState(IStateMachine animator)
            {
                MagicRing_1TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_1.TextureInstance, Vector2.One * Animator.Raduim_1);
                MagicRing_2TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_2.TextureInstance, Vector2.One * Animator.Raduim_2);
                MagicRing_3TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_3.TextureInstance, Vector2.One * Animator.Raduim_3);
                MagicRing_1TargetAngleH = MathHelper.PiOver4;
                base.EntryState(animator);
            }
            public override void OnState(IStateMachine animator)
            {
                Timer++;
                if (Animator.magicRing_1 != null)
                {
                    Animator.magicRing_1.Scale = MathUtils.SteppingTrack(Animator.magicRing_1.Scale, MagicRing_1TargetScale, 0.05f);
                    Animator.magicRing_1.AngleH = MathUtils.SteppingTrack(Animator.magicRing_1.AngleH, MagicRing_1TargetAngleH, 0.05f);
                }
                if (Animator.magicRing_2 != null)
                {
                    Animator.magicRing_2.Scale = MathUtils.SteppingTrack(Animator.magicRing_2.Scale, MagicRing_2TargetScale, 0.05f);
                }
                if (Animator.magicRing_3 != null)
                {
                    Animator.magicRing_3.Scale = MathUtils.SteppingTrack(Animator.magicRing_3.Scale, MagicRing_3TargetScale, 0.05f);
                }
                Animator.CommonElementAI();
                Animator.magicRing_1.Rotation -= MathHelper.PiOver4 * 0.05f * ((TotalTime - Timer) / TotalTime);
                if (Timer > TotalTime)
                {
                    Timer = 0;
                    if (UI_AltarCompose.CheckCanCompose())
                    {
                        Animator.SwitchState(typeof(EntryCompose).ToString());

                    }
                    else
                    {
                        Animator.SwitchState(typeof(Standby).ToString());
                    }

                }
                base.OnState(animator);
            }

        }
        private class EntryCompose : AnimationState<AltarAnimator>
        {
            float TotalTime = 60;
            public EntryCompose(IStateMachine stateMachine) : base(stateMachine)
            {
            }
            public override void OnState(IStateMachine animator)
            {
                Timer++;
                float interget = 1 - (float)(Timer) / TotalTime;
                if (Timer > TotalTime)
                {
                    Animator.SwitchState(typeof(Compose).ToString());

                }
                Animator.CommonElementAI();
                base.OnState(animator);
            }
            public override void ExitState(IStateMachine animator)
            {
                Timer = 0;
                Animator.DistoryCrtstal();
                base.ExitState(animator);
            }
            public override void EntryState(IStateMachine animator)
            {
                Animator.magicRing_1.magicRingStage = MagicRing.MagicRingStage.end;
                Animator.magicRing_2.magicRingStage = MagicRing.MagicRingStage.end;
                Animator.magicRing_3.magicRingStage = MagicRing.MagicRingStage.end;
                base.EntryState(animator);
            }
        }
        private class Compose : AnimationState<AltarAnimator>
        {
            int ComposeProgress;
            int MaxComposeProgress = 12;
            public Compose(IStateMachine stateMachine) : base(stateMachine)
            {
            }
            public override void OnState(IStateMachine animator)
            {
                Animator.CommonElementAI();
                foreach (var item in Animator.drawUnits)
                {
                    if (item is TrailDrawunit)
                    {
                        TrailDrawunit instance = item as TrailDrawunit;
                        if ((instance.TargetPos - instance.Pivot).Length() < 10)
                        {
                            instance.active = false;
                            ComposeProgress++;
                        }
                    }
                }
                if (ComposeProgress >= MaxComposeProgress)
                {
                    Item.NewItem(new EntitySource_DropAsItem(Main.LocalPlayer), Animator.Position, 10, 10, UI_AltarCompose.ComposeItem());
                    Animator.SwitchState(typeof(Standby).ToString());
                }
                base.OnState(animator);
            }
            public override void ExitState(IStateMachine animator)
            {
                base.ExitState(animator);
            }
        }
        #endregion

        #region //»æÖÆÔªËØ
        public class EtherCrystal : DrawUnit
        {
            float shakescale;
            float randomPhase;
            int index;
            public override void SetDefaults()
            {
                texture = AssetUtils.Ether + "EtherCrystal";
                DrawMode = DrawMode.Default;
                ModifyBlendState = ModifyBlendState.NonPremultiplied;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Front;
                SourceRectangle = new Rectangle(0, 0, TextureInstance.Width, TextureInstance.Height);
                Origin = TextureInstance.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One * 0.55f;
                UpdateAction += Shake;
                shakescale = Main.rand.NextFloat(2, 4);
                randomPhase = Main.rand.NextFloat(0, 2) * MathHelper.Pi;
                base.SetDefaults();
            }

            private void Shake(DrawUnit drawUnit)
            {
                drawUnit.Offest = new Vector2(0, MathF.Sin((float)Main.time / 60 + randomPhase) * shakescale);
            }
        }

        private class MagicRing : DrawUnit
        {
            public MagicRingStage magicRingStage = MagicRingStage.start;
            int Timer = 0;
            int StartTime = 75;
            int EndTime = 75;
            float Fliter = 0;
            public override void SetDefaults()
            {
                texture = AssetUtils.Texture + "Projectile_490";
                DrawMode = DrawMode.Custom3D;
                ModifyBlendState = ModifyBlendState.AlphaBlend;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Middle;
                SourceRectangle = new Rectangle(0, 0, TextureInstance.Width, TextureInstance.Height);
                Origin = TextureInstance.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One * 200 / TextureInstance.Size();
                ShaderAciton += shader;
                UpdateAction += update;
                AngleV = MathHelper.PiOver2;
                AngleH = MathHelper.PiOver4;
                ModifySpriteEffect = ModifySpriteEffect.None;
                UseShader = true;
                base.SetDefaults();
            }
            private void shader(DrawUnit drawUnit)
            {
                Effect effect = AssetUtils.GetEffect("AltarTransform");
                effect.Parameters["timer"].SetValue((float)Main.time / 240f);
                effect.Parameters["alpha"].SetValue(1);
                effect.Parameters["Fliter"].SetValue(Fliter);
                effect.CurrentTechnique.Passes[0].Apply();
            }
            private void update(DrawUnit drawUnit)
            {
                switch (magicRingStage)
                {
                    case MagicRingStage.start:
                        Timer++;
                        Fliter = 1 - (Timer / (float)StartTime);
                        if (Timer > StartTime)
                            magicRingStage = MagicRingStage.standby;
                        break;
                    case MagicRingStage.standby:
                        Timer = 0;
                        break;
                    case MagicRingStage.end:
                        Timer++;
                        Fliter = (Timer / (float)StartTime);
                        if (Timer > StartTime)
                            active = false;
                        break;
                    default:
                        break;
                }
            }
            public enum MagicRingStage
            {
                start, standby, end
            }
        }

        public class TrailDrawunit : DrawUnit
        {
            Trail trail;
            Trail Bloom;
            Vector2[] Oldpos = new Vector2[20];
            public Vector2 TargetPos;
            public Vector2 velocity;
            int timer;
            public override void SetDefaults()
            {
                texture = AssetUtils.Extra + "Extra_194";
                DrawMode = DrawMode.Default;
                ModifyBlendState = ModifyBlendState.Additive;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Middle;
                SourceRectangle = new Rectangle(0, 0, TextureInstance.Width, TextureInstance.Height);
                Origin = TextureInstance.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One * 40 / TextureInstance.Size();
                AngleV = MathHelper.PiOver2;
                AngleH = MathHelper.PiOver4;
                ModifySpriteEffect = ModifySpriteEffect.None;
                UseShader = true;
                UpdateAction += update;
                for (int i = 0; i < Oldpos.Length; i++)
                {
                    Oldpos[i] = Pivot + Offest;
                }
                base.SetDefaults();
            }

            private void update(DrawUnit drawUnit)
            {
                timer++;
                velocity = MathUtils.LerpVelocity(velocity, (TargetPos - Pivot).SafeNormalize(Vector2.One) * 10, 0.03f);
                Pivot += velocity;
                for (int i = Oldpos.Length - 1; i > 0; i--)
                {
                    Oldpos[i] = Oldpos[i - 1];
                }
                Oldpos[0] = Pivot + Offest;
                trail ??= new Trail(Main.graphics.GraphicsDevice, 20, new NoTip(), factor =>
                {
                    return 24 * (1 - factor);
                },
                null);
                trail.Positions = Oldpos;
                Bloom ??= new Trail(Main.graphics.GraphicsDevice, 20, new NoTip(), factor =>
                {
                    return 48 * (1 - factor);
                },
                null);
                Bloom.Positions = Oldpos;
                if (timer > 1000)
                    active = false;
            }
            public override void Draw(SpriteBatch spriteBatch)
            {

                trail?.Render(shader);
                trail?.Render(shader2);
                VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.BloomAreaDraw, bloom);
                base.Draw(spriteBatch);
            }
            private void shader()
            {
                Effect effect = AssetUtils.GetEffect("AltarShiningTrail");
                Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
                Matrix view = Main.GameViewMatrix.TransformationMatrix;
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

                effect.Parameters["transformMatrix"].SetValue(world * view * projection);
                effect.Parameters["time"].SetValue((float)Main.time / 60);
                effect.Parameters["sampleTexture"].SetValue(AssetUtils.GetTexture2D(AssetUtils.Extra + "Extra_3"));

                effect.CurrentTechnique.Passes[0].Apply();
            }
            private void shader2()
            {
                Effect effect = AssetUtils.GetEffect("AltarShiningTrail");
                Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
                Matrix view = Main.GameViewMatrix.TransformationMatrix;
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

                effect.Parameters["transformMatrix"].SetValue(world * view * projection);
                effect.Parameters["time"].SetValue((float)Main.time / 60);
                effect.Parameters["sampleTexture"].SetValue(AssetUtils.GetTexture2D(AssetUtils.Extra + "Extra_197"));

                effect.CurrentTechnique.Passes[0].Apply();
            }
            private void bloom()
            {
                Bloom?.Render(shader2);
            }
        }

        public class ShimmerCorona : DrawUnit
        {
            public float raduim;
            public float startAngle;
            int timer;
            public override void SetDefaults()
            {
                texture = AssetUtils.Extra + "Extra_60";
                DrawMode = DrawMode.Default;
                ModifyBlendState = ModifyBlendState.Additive;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Middle;
                SourceRectangle = new Rectangle(0, 0, TextureInstance.Width, TextureInstance.Height);
                Origin = TextureInstance.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = new Vector2(1, 10) * 20 / TextureInstance.Size();
                AngleV = 0;
                AngleH = MathHelper.PiOver4;
                ModifySpriteEffect = ModifySpriteEffect.None;
                UseShader = true;
                base.SetDefaults();
            }
            public override void Update()
            {
                timer++;
                Offest = MathUtils.GetVector2InCircle(timer * MathHelper.PiOver4 / 15 + startAngle, raduim);
                Rotation = MathHelper.PiOver2 + timer * MathHelper.PiOver4 / 15 + startAngle;
                base.Update();
            }
        }


        #endregion
    }
}