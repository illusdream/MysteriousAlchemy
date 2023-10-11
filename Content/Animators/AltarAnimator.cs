using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Utils;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Trails;
using MysteriousAlchemy.UI;
using MysteriousAlchemy.Utils;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;

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
        public override void Initialize()
        {
            base.Initialize();

            RegisterState<Standby>(new Standby(this));
            RegisterState<OpenUI>(new OpenUI(this));
            RegisterState<EntryOpenUI>(new EntryOpenUI(this));
            RegisterState<ExitOpenUI>(new ExitOpenUI(this));


            SetState<Standby>();
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
            if (CurrectState is Standby && magicRing_1 != null && magicRing_2 != null && magicRing_3 != null)
            {
                SwitchState<EntryOpenUI>();
            }
            if (CurrectState is OpenUI)
            {
                SwitchState<ExitOpenUI>();
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


        #region //状态
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
                MagicRing_1TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_1.Texture, Vector2.One * Animator.Raduim_3);
                MagicRing_2TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_2.Texture, Vector2.One * Animator.Raduim_2);
                MagicRing_3TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_3.Texture, Vector2.One * Animator.Raduim_1);
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
                    Animator.SwitchState<OpenUI>();
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
                    Animator.SwitchState<ExitOpenUI>();
                }
            }
        }
        private class ExitOpenUI : AnimationState<AltarAnimator>
        {
            Vector2 MagicRing_1TargetScale;
            Vector2 MagicRing_2TargetScale;
            Vector2 MagicRing_3TargetScale;
            float MagicRing_1TargetAngleH;
            float Timer;
            float TotalTime = 60;
            public ExitOpenUI(IStateMachine stateMachine) : base(stateMachine)
            {
            }
            public override void EntryState(IStateMachine animator)
            {
                MagicRing_1TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_1.Texture, Vector2.One * Animator.Raduim_1);
                MagicRing_2TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_2.Texture, Vector2.One * Animator.Raduim_2);
                MagicRing_3TargetScale = DrawUtils.GetCurrectScale(Animator.magicRing_3.Texture, Vector2.One * Animator.Raduim_3);
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
                    Animator.SwitchState<Standby>();
                }
                base.OnState(animator);
            }
        }
        #endregion

        #region //绘制元素
        public class EtherCrystal : DrawUnit
        {
            float shakescale;
            float randomPhase;
            int index;
            public override void SetDefaults()
            {
                Texture = AssetUtils.GetTexture2DImmediate(AssetUtils.Ether + "EtherCrystal");
                DrawMode = DrawMode.Default;
                ModifyBlendState = ModifyBlendState.NonPremultiplied;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Front;
                SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Origin = Texture.Size() / 2f;
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
            MagicRingStage magicRingStage = MagicRingStage.start;
            int Timer = 0;
            int StartTime = 75;
            int EndTime = 75;
            float Fliter = 0;
            public override void SetDefaults()
            {

                Texture = AssetUtils.GetTexture2DImmediate(AssetUtils.Texture + "Projectile_490");
                DrawMode = DrawMode.Custom3D;
                ModifyBlendState = ModifyBlendState.AlphaBlend;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Middle;
                SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Origin = Texture.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One * 200 / Texture.Size();
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
                        break;
                    case MagicRingStage.end:
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

        public class RandomLighting : DrawUnit
        {
            public Vector2 LightStart;
            public Vector2 LightEnd;
            public override void SetDefaults()
            {
                Texture = AssetUtils.GetTexture2DImmediate(AssetUtils.Ether + "EtherCrystal");
                DrawMode = DrawMode.Default;
                ModifyBlendState = ModifyBlendState.NonPremultiplied;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Front;
                SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Origin = Texture.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One * 0.55f;
                base.SetDefaults();
            }
            public override void Draw(SpriteBatch spriteBatch)
            {

            }

        }
        public class ItemHolding : DrawUnit
        {
            ItemHoldingStage itemHoldingStage = ItemHoldingStage.start;
            int Timer = 0;
            int StartTime = 75;
            int EndTime = 75;
            float Fliter = 0;
            public override void SetDefaults()
            {

                Texture = AssetUtils.GetTexture2DImmediate(AssetUtils.Extra + "Extra_2");
                DrawMode = DrawMode.Custom3D;
                ModifyBlendState = ModifyBlendState.AlphaBlend;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Middle;
                SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Origin = Texture.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One * 40 / Texture.Size();
                ShaderAciton += shader;
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
                switch (itemHoldingStage)
                {
                    case ItemHoldingStage.start:
                        Timer++;
                        Fliter = 1 - (Timer / (float)StartTime);
                        if (Timer > StartTime)
                            itemHoldingStage = ItemHoldingStage.standby;
                        break;
                    case ItemHoldingStage.standby:
                        break;
                    case ItemHoldingStage.end:
                        break;
                    default:
                        break;
                }
            }
            public enum ItemHoldingStage
            {
                start, standby, end
            }
        }

        public class TrailDrawunit : DrawUnit
        {
            Trail trail;
            Vector2[] Oldpos = new Vector2[20];
            public override void SetDefaults()
            {

                Texture = AssetUtils.GetTexture2DImmediate(AssetUtils.Extra + "Extra_194");
                DrawMode = DrawMode.Default;
                ModifyBlendState = ModifyBlendState.Additive;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Middle;
                SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Origin = Texture.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One * 40 / Texture.Size();
                AngleV = MathHelper.PiOver2;
                AngleH = MathHelper.PiOver4;
                ModifySpriteEffect = ModifySpriteEffect.None;
                UseShader = true;
                UpdateAction += update;
                base.SetDefaults();
            }
            private void update(DrawUnit drawUnit)
            {
                for (int i = Oldpos.Length - 1; i > 0; i--)
                {
                    Oldpos[i] = Oldpos[i - 1];
                }
                Oldpos[0] = Main.MouseWorld;
                trail ??= new Trail(Main.graphics.GraphicsDevice, 20, new NoTip(), factor =>
                {
                    return 8 * (1 - factor);
                },
                factor =>
                {
                    return Color.White * (1 - factor.X);
                });
                trail.Positions = Oldpos;
                drawUnit.Pivot = Main.MouseWorld;
                //ParticleOrchestrator.SpawnParticlesDirect(ParticleOrchestraType.StellarTune, new ParticleOrchestraSettings()
                //{
                //    PositionInWorld = Pivot,
                //    MovementVector = Main.rand.NextVector2Circular(1, 1) * Main.rand.NextFloat(2f, 4f) * 10,

                //});
            }
            public override void Draw(SpriteBatch spriteBatch)
            {
                //                for (int i = 0; i < 3; i++)
                //                {
                //                    DrawUtils.DrawPrettyStarSparkle(1f, 0, Main.MouseScreen + new Vector2(10, 20), new Color(187, 166, 250) * 0.7f,
                //new Color(157, 113, 236), 0.3f, 0f, 0.5f, 0.5f, 1f, 0, new Vector2(0.33f, 1), new Vector2(1, 1) / 2f);
                //                }
                //trail?.Render(shader);
                //t/rail?.Render(shader);
                //trail?.Render(shader);
                //base.Draw(spriteBatch);
            }
            //离奇的bug，剔除不管用了，打算改为彩虹魔杖那个特效
            private void shader()
            {
                Effect effect = AssetUtils.GetEffect("AltarShiningTrail");
                Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
                Matrix view = Main.GameViewMatrix.TransformationMatrix;
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

                effect.Parameters["transformMatrix"].SetValue(world * view * projection);
                effect.Parameters["UVoffest"].SetValue(new Vector2((float)Main.time / 60, 0));
                effect.Parameters["UVScale"].SetValue(10);
                effect.Parameters["Fliter"].SetValue(0.5f);
                effect.Parameters["sampleTexture"].SetValue(AssetUtils.GetTexture2D(AssetUtils.Extra + "Extra_194"));
                effect.Parameters["colorTexture"].SetValue(AssetUtils.GetColorBar("Shimmer"));
                effect.CurrentTechnique.Passes[0].Apply();
            }
        }
        #endregion
    }
}