using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Utils;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Animators
{
    public class AltarAnimator : Animator
    {
        public override DrawSortWithPlayer DrawSortWithPlayer => DrawSortWithPlayer.Behind;

        List<EtherCrystal> etherCrystalList_1;
        List<EtherCrystal> etherCrystalList_2;
        List<EtherCrystal> etherCrystalList_3;
        MagicRing magicRing_1;
        MagicRing magicRing_2;
        MagicRing magicRing_3;
        MagicInsideCirecle magicInsideCirecle;

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
            etherCrystalList_1 = new List<EtherCrystal>();
            for (int i = 0; i < 12; i++)
            {
                EtherCrystal etherCrystal = RegisterDrawUnit<EtherCrystal>(Main.MouseWorld);
                etherCrystalList_1.Add(etherCrystal);
            }
            magicRing_1 = RegisterDrawUnit<MagicRing>(Main.MouseWorld, Vector2.One * Raduim_1);
            magicRing_2 = RegisterDrawUnit<MagicRing>(Main.MouseWorld, Vector2.One * Raduim_2);
            magicRing_3 = RegisterDrawUnit<MagicRing>(Main.MouseWorld, Vector2.One * Raduim_3);
            magicInsideCirecle = RegisterDrawUnit<MagicInsideCirecle>(Main.MouseWorld);
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
                for (int i = 0; i < Animator.etherCrystalList_1.Count; i++)
                {
                    DebugUtils.NewText(((float)Main.time / 600 + MathHelper.TwoPi / Animator.etherCrystalList_1.Count * i) % MathHelper.TwoPi < MathHelper.Pi);
                    Animator.etherCrystalList_1[i].Pivot = Animator.Position + DrawUtils.MartixTrans(new Vector2(MathF.Sin((float)Main.time / 600 + MathHelper.TwoPi / Animator.etherCrystalList_1.Count * i), MathF.Cos((float)Main.time / 600 + MathHelper.TwoPi / Animator.etherCrystalList_1.Count * i)) * 175, MathHelper.PiOver4 * 1.75f, MathHelper.PiOver2);

                    Animator.etherCrystalList_1[i].Offest = DrawUtils.MartixTrans(new Vector2(MathF.Sin((float)Main.time / 300 + MathHelper.TwoPi / Animator.etherCrystalList_1.Count * i), MathF.Cos((float)Main.time / 300 + MathHelper.TwoPi / Animator.etherCrystalList_1.Count * i)) * 3, MathHelper.PiOver4, MathHelper.PiOver2);
                }
                Animator.AngleV_2 += MathHelper.PiOver4 * 0.01f;
                Animator.AngleV_3 += MathHelper.PiOver4 * 0.007f;
                Animator.AngleH_3 += MathHelper.PiOver4 * 0.01f;
                Animator.magicRing_1.Rotation = (float)Main.time / 60;
                Animator.magicRing_2.Rotation = (float)Main.time / 150;
                Animator.magicRing_3.Rotation = (float)Main.time / 240;
                Animator.magicRing_2.AngleV = Animator.AngleV_2;
                Animator.magicRing_3.AngleV = Animator.AngleV_3;
                Animator.magicRing_3.AngleH = Animator.AngleH_3;
                Animator.magicInsideCirecle.Rotation = -(float)Main.time / 60;
                base.OnState(animator);
            }
            public override void ExitState(IStateMachine animator)
            {
                base.ExitState(animator);
            }
        }
        #endregion

        #region //»æÖÆÔªËØ
        private class EtherCrystal : DrawUnit
        {
            float shakescale;
            int index;
            public override void SetDefaults()
            {
                Texture = AssetUtils.GetTexture2D(AssetUtils.Ether + "EtherCrystal");
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
                base.SetDefaults();
            }

            private void Shake(DrawUnit drawUnit)
            {
                drawUnit.Offest = new Vector2(0, 3 * MathF.Sin((float)Main.time / 60) * shakescale);
            }
        }

        private class MagicRing : DrawUnit
        {
            public override void SetDefaults()
            {
                Texture = AssetUtils.GetTexture2D(AssetUtils.Texture + "Projectile_490");
                DrawMode = DrawMode.Custom3D;
                ModifyBlendState = ModifyBlendState.AlphaBlend;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Middle;
                SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Origin = Texture.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One * 200 / Texture.Size();
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
                effect.Parameters["timer"].SetValue((float)Main.time / 60f);
                effect.Parameters["alpha"].SetValue(1);
                effect.CurrentTechnique.Passes[0].Apply();
            }
        }

        private class MagicInsideCirecle : DrawUnit
        {
            public override void SetDefaults()
            {
                Texture = AssetUtils.GetTexture2D(AssetUtils.Extra + "Extra_34");
                DrawMode = DrawMode.Custom3D;
                ModifyBlendState = ModifyBlendState.Additive;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Middle;
                SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Origin = Texture.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One * 175 / Texture.Size();
                ShaderAciton += shader;
                AngleV = MathHelper.PiOver2;
                AngleH = MathHelper.PiOver4;
                ModifySpriteEffect = ModifySpriteEffect.None;
                UseShader = true;
                base.SetDefaults();
            }
            private void shader(DrawUnit drawUnit)
            {
                MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1);
                MysteriousAlchemy.AltarTransform.CurrentTechnique.Passes[0].Apply();
            }
        }
        #endregion
    }
}