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

        //����Ť������ͼ�õ�render
        private static RenderTarget2D DistortFlowGraphRender;
        private static Action FlowGraphDraw = null;
        //���Ʊ�Ť��ͼ���render
        private static RenderTarget2D DistortRender;
        private static Action DistortedGraphDraw = null;
        private static RenderTarget2D DIstortFinalBloomText;

        //����Bloom������ͼ
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
        /// �ʺ�����
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

        //Ť��Ч��
        private void DistortRenderDraw(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            //����rt2dʵ��
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
            //����ԭ����Ļ����
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            //��������ͼ
            gd.SetRenderTarget(DistortFlowGraphRender);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (FlowGraphDraw != null)
            {
                FlowGraphDraw.Invoke();
                //���ί��
                ClearAction(ref FlowGraphDraw);
            }

            sb.End();

            //������Ҫ��Ť����ͼ��
            gd.SetRenderTarget(DistortRender);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (DistortedGraphDraw != null)
            {
                DistortedGraphDraw.Invoke();
                //���ί��
                ClearAction(ref DistortedGraphDraw);
            }


            sb.End();

            //�������ս��
            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //������������ͼ
            gd.Textures[1] = DistortFlowGraphRender;
            //����Ť��shader
            DistortEffect.CurrentTechnique.Passes[0].Apply();
            //������Ҫ��Ť����ͼ��
            sb.Draw(DistortRender, Vector2.Zero, Color.White);
            //sb.Draw(DistortFlowGraphRender, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(DIstortFinalBloomText);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //������������ͼ
            gd.Textures[1] = DistortFlowGraphRender;
            //����Ť��shader
            DistortEffect.CurrentTechnique.Passes[0].Apply();
            //������Ҫ��Ť����ͼ��
            sb.Draw(DistortRender, Vector2.Zero, Color.White);
            //sb.Draw(DistortFlowGraphRender, Vector2.Zero, Color.White);
            sb.End();
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }

        private void BloomRenderDraw(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            //����rt2dʵ��
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
            //�����ʼ
            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            //������Ҫ��Bloom�Ĳ���
            gd.SetRenderTarget(BloomRender);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            if (BloomAreaDraw != null)
            {
                BloomAreaDraw.Invoke();
                ClearAction(ref BloomAreaDraw);
            }
            sb.End();

            //�޳�����ҪBloom������
            gd.SetRenderTarget(BloomBlend);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //�޳�shader
            gd.Textures[0] = Main.screenTargetSwap;
            gd.Textures[1] = BloomRender;
            Bloom.CurrentTechnique.Passes[0].Apply();
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();

            //����BloomЧ��
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Bloom.Parameters["uIntensity"].SetValue(0.88f);
            Bloom.Parameters["uRange"].SetValue(2f);
            Bloom.Parameters["uScreenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            //�����ʼ������
            for (int i = 0; i < BloomIntensity; i++)
            {
                //����Bloom
                gd.Textures[0] = Main.screenTargetSwap;
                gd.Textures[1] = BloomRender;
                Bloom.CurrentTechnique.Passes[1].Apply();
                gd.SetRenderTarget(Main.screenTarget);
                gd.Clear(Color.Transparent);
                sb.Draw(BloomBlend, Vector2.Zero, Color.White);
                //����Bloom
                gd.Textures[0] = Main.screenTargetSwap;
                gd.Textures[1] = BloomRender;
                Bloom.CurrentTechnique.Passes[2].Apply();
                gd.SetRenderTarget(BloomBlend);
                gd.Clear(Color.Transparent);
                sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            }
            sb.End();

            //���Ч��
            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            //����ԭʼͼ��
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.Draw(DIstortFinalBloomText, Vector2.Zero, Color.White);
            //����bloomͼ��
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
            /// Ť������ͼAcion
            /// </summary>
            FlowGraphDraw,
            /// <summary>
            /// ��Ť����ͼ��Action
            /// </summary>
            DistortedGraphDraw,
            /// <summary>
            /// �����������
            /// </summary>
            BloomAreaDraw
        }
    }
}