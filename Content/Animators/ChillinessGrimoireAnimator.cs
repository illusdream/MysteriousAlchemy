using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.Items.Chilliness;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Animators
{
    public class ChillinessGrimoireAnimator : Animator
    {
        public Player Player { get; set; }
        Book book;
        MagicRing BookMagicRing;
        public override void Initialize()
        {
            RegisterState<DefaultState>(new DefaultState(this));
            SetState<DefaultState>();
            base.Initialize();
        }

        public void AddMagicCircle(Projectile projectile)
        {
            var instance = RegisterDrawUnit<MagicRing>(projectile.position);
            instance.magicRingStage = MagicRing.MagicRingStage.end;
        }



        #region //状态
        private class DefaultState : AnimationState<ChillinessGrimoireAnimator>
        {
            public DefaultState(IStateMachine stateMachine) : base(stateMachine)
            {
            }
            public override void OnState(IStateMachine animator)
            {
                Vector2 MouseToward = (Animator.Player.Center - Main.MouseWorld).SafeNormalize(Vector2.One) * -50;

                if (Animator.Player == null)
                    return;
                if (Animator.Player.controlUseItem && Animator.Player.HeldItem.type == ModContent.ItemType<FrostEssenceGrimoire>())
                {
                    Animator.book ??= Animator.RegisterDrawUnit<Book>(Animator.Player.Center + MouseToward);
                    Animator.BookMagicRing ??= Animator.RegisterDrawUnit<MagicRing>(Animator.Player.Center + MouseToward);
                    Animator.book.Pivot = Animator.Player.Center + MouseToward;
                    Animator.BookMagicRing.Pivot = Animator.Player.Center + MouseToward;
                    if (Animator.BookMagicRing.active == false)
                    {
                        Animator.BookMagicRing = Animator.RegisterDrawUnit<MagicRing>();
                    }
                }
                if (Animator.Player.releaseUseItem && Animator.Player.HeldItem.type == ModContent.ItemType<FrostEssenceGrimoire>())
                {
                    if (Animator.book != null)
                    {
                        Animator.book.active = false;
                        Animator.book = null;
                    }

                    Animator.BookMagicRing.StageToEnd();
                }
                base.OnState(animator);
            }
        }
        #endregion

        #region //绘制元素
        private class Book : DrawUnit
        {
            public override void SetDefaults()
            {
                Texture = AssetUtils.GetTexture2DImmediate(AssetUtils.Weapons + "FrostEssenceGrimoire");
                DrawMode = DrawMode.Default;
                ModifyBlendState = ModifyBlendState.AlphaBlend;
                color = Color.White;
                DrawSortWithUnits = DrawSortWithUnits.Front;
                SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Origin = Texture.Size() / 2f;
                SpriteEffect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                Scale = Vector2.One * 25 / Texture.Size();
                base.SetDefaults();
            }
        }
        private class MagicRing : DrawUnit
        {
            public MagicRingStage magicRingStage = MagicRingStage.start;
            int Timer = 0;
            int StartTime = 30;
            int EndTime = 30;
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
                Scale = Vector2.One * 60 / Texture.Size();
                ShaderAciton += shader;
                UpdateAction += update;
                AngleV = MathHelper.PiOver2;
                AngleH = 0;
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
                        {
                            Timer = 0;
                            magicRingStage = MagicRingStage.standby;
                        }
                        break;
                    case MagicRingStage.standby:
                        break;
                    case MagicRingStage.end:
                        Timer++;
                        Fliter = (Timer / (float)EndTime);
                        if (Timer > EndTime)
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
            public void StageToEnd()
            {
                if (magicRingStage != MagicRingStage.end)
                {
                    Timer = 0;
                    magicRingStage = MagicRingStage.end;
                }
            }
        }
        #endregion
    }
}
