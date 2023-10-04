using Microsoft.Xna.Framework;
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

        List<EtherCrystal> etherCrystalList;

        public override void Initialize()
        {
            base.Initialize();
            etherCrystalList = new List<EtherCrystal>();
            RegisterState<Standby>(new Standby(this));


            SetState<Standby>();

            for (int i = 0; i < 4; i++)
            {
                EtherCrystal etherCrystal = RegisterDrawUnit<EtherCrystal>();
                etherCrystal.Pivot = Main.MouseWorld;
                etherCrystalList.Add(etherCrystal);
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
                for (int i = 0; i < Animator.etherCrystalList.Count; i++)
                {
                    Animator.etherCrystalList[i].Pivot = Animator.Position + DrawUtils.MartixTrans(new Vector2(MathF.Sin((float)Main.time / 60 + MathHelper.PiOver2 * i), MathF.Cos((float)Main.time / 60 + MathHelper.PiOver2 * i)) * 100, MathHelper.PiOver4, MathHelper.PiOver2);
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
        private class EtherCrystal : DrawUnit
        {
            public override void SetDefaults()
            {
                Texture = AssetUtils.GetTexture2D(AssetUtils.Ether + "EtherCrystal");
                DrawMode = DrawMode.Default;
                ModifyBlendState = ModifyBlendState.AlphaBlend;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Front;
                SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Origin = Texture.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One;
                UpdateAction += Shake;
                base.SetDefaults();
            }

            private void Shake(DrawUnit drawUnit)
            {
                drawUnit.Offest = new Vector2(0, 10 * MathF.Sin((float)Main.time / 60));
            }
        }
        #endregion
    }
}