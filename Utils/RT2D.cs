using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Dusts;
using MysteriousAlchemy.Modsystem;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;

namespace MysteriousAlchemy.Utils
{
    public class RT2D : ModSystem
    {
        public static Terraria.Graphics.Effects.On_FilterManager.hook_EndCapture EndCaptureGroup;

        //绘制扭曲流向图用的render
        private static RenderTarget2D DistortFlowGraphRender;
        private static Action FlowGraphDraw = null;
        //绘制被扭曲图像的render
        private static RenderTarget2D DistortRender;
        private static Action DistortedGraphDraw = null;
        private static RenderTarget2D DIstortFinalBloomText;

        //绘制Bloom的属性图
        private static RenderTarget2D BloomRender;
        private static RenderTarget2D BloomBlend;
        private static Action BloomAreaDraw = null;
        private static int BloomIntensity = 30;
        public override void Load()
        {
            var _drawmethod = typeof(Terraria.Graphics.Effects.FilterManager).GetMethod("EndCapture", BindingFlags.Instance | BindingFlags.Public);
            MonoModHooks.Add(_drawmethod, FilterManager_EndCapture);
            MonoModHooks.Add(_drawmethod, DistortRenderDraw);
            MonoModHooks.Add(_drawmethod, BloomRenderDraw);
            base.Load();
        }

        public static void PutRT2Ddelegate(Action actionDraw, Action EffectApply)
        {
            EndCaptureGroup += (Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor) =>
            {
                GraphicsDevice gd = Main.instance.GraphicsDevice;
                SpriteBatch sb = Main.spriteBatch;


                gd.SetRenderTarget(Main.screenTargetSwap);
                gd.Clear(Color.Transparent);
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
                sb.End();

                gd.SetRenderTarget(MysteriousAlchemy.render);
                gd.Clear(Color.Transparent);
                sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                actionDraw();
                sb.End();

                gd.SetRenderTarget(Main.screenTarget);
                gd.Clear(Color.Transparent);
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
                sb.End();
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                EffectApply();
                sb.Draw(MysteriousAlchemy.render, Vector2.Zero, Color.White);
                sb.End();

                orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
            };
        }

        /// <summary>
        /// 彩虹字体
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="finalTexture"></param>
        /// <param name="screenTarget1"></param>
        /// <param name="screenTarget2"></param>
        /// <param name="clearColor"></param>
        private void FilterManager_EndCapture(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;


            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(MysteriousAlchemy.render);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ColorfulStringSystem.DrawAllColorfulString();
            sb.End();


            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Effect effect = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Rainbow").Value;
            effect.CurrentTechnique.Passes[0].Apply();
            effect.Parameters["timer"].SetValue((float)Main.time / 60f);
            sb.Draw(MysteriousAlchemy.render, Vector2.Zero, Color.White);
            sb.End();

            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);

        }

        //扭曲效果
        private void DistortRenderDraw(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            //创建rt2d实例
            if (DistortFlowGraphRender == null)
            {
                DistortFlowGraphRender = CreateRender();
            }
            if (DistortRender == null)
            {
                DistortRender = CreateRender();
            }
            if (DIstortFinalBloomText == null)
            {
                DIstortFinalBloomText = CreateRender();
            }

            GraphicsDevice gd = Main.instance.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;
            Effect DistortEffect = AssetUtils.GetEffect("Distort");
            //保存原本屏幕内容
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            //绘制流向图
            gd.SetRenderTarget(DistortFlowGraphRender);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (FlowGraphDraw != null)
            {
                FlowGraphDraw.Invoke();
                //清空委托
                ClearAction(ref FlowGraphDraw);
            }

            sb.End();

            //绘制需要被扭曲的图像
            gd.SetRenderTarget(DistortRender);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (DistortedGraphDraw != null)
            {
                DistortedGraphDraw.Invoke();
                //清空委托
                ClearAction(ref DistortedGraphDraw);
            }


            sb.End();

            //绘制最终结果
            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //输入最终流向图
            gd.Textures[1] = DistortFlowGraphRender;
            //导入扭曲shader
            DistortEffect.CurrentTechnique.Passes[0].Apply();
            //绘制需要被扭曲的图像
            sb.Draw(DistortRender, Vector2.Zero, Color.White);
            //sb.Draw(DistortFlowGraphRender, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(DIstortFinalBloomText);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //输入最终流向图
            gd.Textures[1] = DistortFlowGraphRender;
            //导入扭曲shader
            DistortEffect.CurrentTechnique.Passes[0].Apply();
            //绘制需要被扭曲的图像
            sb.Draw(DistortRender, Vector2.Zero, Color.White);
            //sb.Draw(DistortFlowGraphRender, Vector2.Zero, Color.White);
            sb.End();
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }

        private void BloomRenderDraw(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            //创建rt2d实例
            if (BloomRender == null)
            {
                BloomRender = CreateRender();
            }
            if (BloomBlend == null)
            {
                BloomBlend = CreateRender();
            }
            if (DIstortFinalBloomText == null)
            {
                DIstortFinalBloomText = CreateRender();
            }

            GraphicsDevice gd = Main.instance.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;
            Effect Bloom = AssetUtils.GetEffect("Bloom");
            //保存初始
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            //绘制需要被Bloom的部分
            gd.SetRenderTarget(BloomRender);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            if (BloomAreaDraw != null)
            {
                BloomAreaDraw.Invoke();
                ClearAction(ref BloomAreaDraw);
            }
            sb.End();

            //剔除不需要Bloom的区域
            gd.SetRenderTarget(BloomBlend);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //剔除shader
            gd.Textures[0] = Main.screenTargetSwap;
            gd.Textures[1] = BloomRender;
            Bloom.CurrentTechnique.Passes[0].Apply();
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();

            //创建Bloom效果
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Bloom.Parameters["uIntensity"].SetValue(0.88f);
            Bloom.Parameters["uRange"].SetValue(2f);
            Bloom.Parameters["uScreenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            //导入初始化参数
            for (int i = 0; i < BloomIntensity; i++)
            {
                //横向Bloom
                gd.Textures[0] = Main.screenTargetSwap;
                gd.Textures[1] = BloomRender;
                Bloom.CurrentTechnique.Passes[1].Apply();
                gd.SetRenderTarget(Main.screenTarget);
                gd.Clear(Color.Transparent);
                sb.Draw(BloomBlend, Vector2.Zero, Color.White);
                //纵向Bloom
                gd.Textures[0] = Main.screenTargetSwap;
                gd.Textures[1] = BloomRender;
                Bloom.CurrentTechnique.Passes[2].Apply();
                gd.SetRenderTarget(BloomBlend);
                gd.Clear(Color.Transparent);
                sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            }
            sb.End();

            //混合效果
            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            //绘制原始图像
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.Draw(DIstortFinalBloomText, Vector2.Zero, Color.White);
            //绘制bloom图像
            sb.Draw(BloomBlend, Vector2.Zero, Color.White);
            // sb.Draw(BloomRender, Vector2.Zero, Color.White);
            sb.End();
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        private RenderTarget2D CreateRender()
        {
            return new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
        }



        public static void AddAction(RTActionType actionType, Action action)
        {
            switch (actionType)
            {
                case RTActionType.FlowGraphDraw:
                    FlowGraphDraw += action;
                    break;
                case RTActionType.DistortedGraphDraw:
                    DistortedGraphDraw += action;
                    break;
                case RTActionType.BloomAreaDraw:
                    BloomAreaDraw += action;
                    break;
                default:
                    break;
            }
        }

        private void ClearAction(ref Action action)
        {
            Delegate[] allAction = action.GetInvocationList();
            for (int i = 0; i < allAction.Length; i++)
            {
                action -= allAction[i] as Action;
            }
        }

        public enum RTActionType
        {
            /// <summary>
            /// 扭曲流向图Acion
            /// </summary>
            FlowGraphDraw,
            /// <summary>
            /// 被扭曲的图像Action
            /// </summary>
            DistortedGraphDraw,
            /// <summary>
            /// 反光区域绘制
            /// </summary>
            BloomAreaDraw
        }
    }
}