using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Utils;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Animators
{
    public class AltarAnimator : Animator
    {
        public override DrawSortWithPlayer DrawSortWithPlayer => DrawSortWithPlayer.Behind;

        public List<EtherCrystal> etherCrystalList_Ready;
        private List<EtherCrystal> etherCrystalList_Prepare;
        MagicRing magicRing_1;
        MagicRing magicRing_2;
        MagicRing magicRing_3;

        float AngleV_2;

        float AngleV_3;
        float AngleH_3;

        float Raduim_1 = 200;
        float Raduim_2 = 250;
        float Raduim_3 = 300;
        public override void Initialize()
        {
            base.Initialize();

            RegisterState<Standby>(new Standby(this));


            SetState<Standby>();
            etherCrystalList_Ready = new List<EtherCrystal>();
            etherCrystalList_Prepare = new List<EtherCrystal>();

        }
        public void AddEtherCrystal()
        {
            if (CurrectState is Standby && etherCrystalList_Ready.Count < 12)
            {
                SoundEngine.PlaySound(MASoundID.Bell_Item35, Position);
                EtherCrystal etherCrystal = RegisterDrawUnit<EtherCrystal>(Position);
                etherCrystalList_Prepare.Add(etherCrystal);
            }
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

                for (int i = 0; i < Animator.etherCrystalList_Ready.Count; i++)
                {
                    Vector2 target = Animator.Position + DrawUtils.MartixTrans(new Vector2(MathF.Sin((float)Main.time / 600 + MathHelper.TwoPi / Animator.etherCrystalList_Ready.Count * i), MathF.Cos((float)Main.time / 600 + MathHelper.TwoPi / Animator.etherCrystalList_Ready.Count * i)) * 175, MathHelper.PiOver4 * 1.75f, MathHelper.PiOver2);
                    Animator.etherCrystalList_Ready[i].Pivot = MathUtils.SteppingTrack(Animator.etherCrystalList_Ready[i].Pivot, target, 0.05f);

                    Animator.etherCrystalList_Ready[i].Offest = DrawUtils.MartixTrans(new Vector2(MathF.Sin((float)Main.time / 300 + MathHelper.TwoPi / Animator.etherCrystalList_Ready.Count * i), MathF.Cos((float)Main.time / 300 + MathHelper.TwoPi / Animator.etherCrystalList_Ready.Count * i)) * 3, MathHelper.PiOver4, MathHelper.PiOver2);
                    if (DrawUtils.GetEntityContextInCircle((float)Main.time / 600 + MathHelper.TwoPi / Animator.etherCrystalList_Ready.Count * i))
                    {
                        Animator.etherCrystalList_Ready[i].DrawSortWithUnits = DrawSortWithUnits.Front;
                    }
                    else
                    {
                        Animator.etherCrystalList_Ready[i].DrawSortWithUnits = DrawSortWithUnits.Behind;
                    }
                }
                for (int i = 0; i < Animator.etherCrystalList_Prepare.Count; i++)
                {
                    Vector2 target = Animator.Position + DrawUtils.MartixTrans(new Vector2(MathF.Sin((float)Main.time / 600 + MathHelper.TwoPi / Animator.etherCrystalList_Prepare.Count * i), MathF.Cos((float)Main.time / 300 + MathHelper.TwoPi / Animator.etherCrystalList_Prepare.Count * i)) * Animator.Raduim_1 * 0.1f, 0, MathHelper.PiOver2);
                    Animator.etherCrystalList_Prepare[i].Pivot = MathUtils.SteppingTrack(Animator.etherCrystalList_Prepare[i].Pivot, target, 0.05f);
                }
                if (Animator.etherCrystalList_Prepare.Count >= 4)
                {
                    Animator.etherCrystalList_Ready.AddRange(Animator.etherCrystalList_Prepare);
                    Animator.AddMagicCircle();
                    Animator.etherCrystalList_Prepare.Clear();

                }
                if (Animator.magicRing_1 != null)
                {
                    Animator.magicRing_1.Pivot = Animator.Position;
                    Animator.magicRing_1.Rotation = (float)Main.time / 60;
                }
                if (Animator.magicRing_2 != null)
                {
                    Animator.magicRing_2.Pivot = Animator.Position;
                    Animator.magicRing_2.Rotation = (float)Main.time / 150;
                    Animator.magicRing_2.AngleV += MathHelper.PiOver4 * 0.01f;
                }
                if (Animator.magicRing_3 != null)
                {
                    Animator.magicRing_3.Pivot = Animator.Position;
                    Animator.magicRing_3.Rotation = (float)Main.time / 240;
                    Animator.magicRing_3.AngleV += MathHelper.PiOver4 * 0.007f; ;
                    Animator.magicRing_3.AngleH += MathHelper.PiOver4 * 0.01f;
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
        #endregion
    }
}