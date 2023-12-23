using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Dusts;
using MysteriousAlchemy.Modsystem;
using MysteriousAlchemy.Utils;
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
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MysteriousAlchemy.Core.Systems
{
    /// <summary>
    /// �Ӿ�����(VisualPostPorcessing)
    /// </summary>
    public class VisualPPSystem : Hook
    {
        public static Terraria.Graphics.Effects.On_FilterManager.hook_EndCapture EndCaptureGroup;

        //����Ť������ͼ�õ�render
        private RenderTarget2D DistortFlowGraphRender;
        private static Action FlowGraphDraw = null;
        //���Ʊ�Ť��ͼ���render
        private RenderTarget2D DistortRender;
        private static Action DistortedGraphDraw = null;
        private RenderTarget2D DIstortFinalBloomText;
        //����Bloom������ͼ
        private RenderTarget2D OldBloomRender;
        private RenderTarget2D OldBloomBlend;
        private static Action OldBloomAreaDraw = null;
        private static int OldBloomIntensity = 7;
        //��������Bloom
        //��������map�������ɸ���������Բ��ģ��Ч��,�²���
        //private RenderTarget2D[] MipMaps = new RenderTarget2D[DownSampleStep];
        ////�ϲ���
        //private RenderTarget2D[] BloomUp = new RenderTarget2D[DownSampleStep - 1];
        //���ⲿ�֣�ֱ�Ӷ���draw����ȡyuan
        private RenderTarget2D Glow;
        private static Action BloomDraw = null;

        private static int DownSampleStep = 10;
        private static int DownSize = 2;
        public int LoaderIndex => 5;
        public void Load()
        {
            var _drawmethod = typeof(Terraria.Graphics.Effects.FilterManager).GetMethod("EndCapture", BindingFlags.Instance | BindingFlags.Public);
            MonoModHooks.Add(_drawmethod, FilterManager_EndCapture);
            MonoModHooks.Add(_drawmethod, DistortRenderDraw);
            //MonoModHooks.Add(_drawmethod, OldBloomRenderDraw);
            //MonoModHooks.Add(_drawmethod, BloomRenderDraw);
        }

        public void Unload()
        {
            DistortFlowGraphRender = null;
            DistortRender = null;
            DIstortFinalBloomText = null;
            OldBloomRender = null;
            OldBloomBlend = null;
            Glow = null;
        }
        public static void PutRT2Ddelegate(Action actionDraw, Action EffectApply)
        {
            EndCaptureGroup += (orig, self, finalTexture, screenTarget1, screenTarget2, clearColor) =>
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
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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
            DistortEffect.Parameters["distortLength"].SetValue(32);
            DistortEffect.Parameters["screenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            //������Ҫ��Ť����ͼ��
            sb.Draw(DistortRender, Vector2.Zero, Color.White);
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            // sb.Draw(DistortFlowGraphRender, Vector2.Zero, Color.White);
            sb.End();

            //gd.SetRenderTarget(DIstortFinalBloomText);
            //gd.Clear(Color.Transparent);
            //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            ////������������ͼ
            //gd.Textures[1] = DistortFlowGraphRender;
            ////����Ť��shader
            //DistortEffect.CurrentTechnique.Passes[0].Apply();
            ////������Ҫ��Ť����ͼ��
            //sb.Draw(DistortRender, Vector2.Zero, Color.White);
            ////sb.Draw(DistortFlowGraphRender, Vector2.Zero, Color.White);
            //sb.End();
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }

        private void OldBloomRenderDraw(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            //����rt2dʵ��
            if (OldBloomRender == null)
            {
                OldBloomRender = CreateRender();
            }
            if (OldBloomBlend == null)
            {
                OldBloomBlend = CreateRender();
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
            gd.SetRenderTarget(OldBloomRender);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            if (OldBloomAreaDraw != null)
            {
                OldBloomAreaDraw.Invoke();
                ClearAction(ref OldBloomAreaDraw);
            }
            sb.End();

            //�޳�����ҪBloom������
            gd.SetRenderTarget(OldBloomBlend);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            //�޳�shader
            gd.Textures[0] = Main.screenTargetSwap;
            gd.Textures[1] = OldBloomRender;
            Bloom.CurrentTechnique.Passes[0].Apply();
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();

            //����BloomЧ��
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Bloom.Parameters["uIntensity"].SetValue(0.88f);
            Bloom.Parameters["uRange"].SetValue(2f);
            Bloom.Parameters["uScreenSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            //�����ʼ������
            for (int i = 0; i < OldBloomIntensity; i++)
            {
                //����Bloom
                gd.Textures[0] = Main.screenTargetSwap;
                gd.Textures[1] = OldBloomRender;
                Bloom.CurrentTechnique.Passes[1].Apply();
                gd.SetRenderTarget(Main.screenTarget);
                gd.Clear(Color.Transparent);
                sb.Draw(OldBloomBlend, Vector2.Zero, Color.White);
                //����Bloom
                gd.Textures[0] = Main.screenTargetSwap;
                gd.Textures[1] = OldBloomRender;
                Bloom.CurrentTechnique.Passes[2].Apply();
                gd.SetRenderTarget(OldBloomBlend);
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
            sb.Draw(OldBloomBlend, Vector2.Zero, Color.White);
            //sb.Draw(BloomRender, Vector2.Zero, Color.White);
            sb.End();
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        //private void BloomRenderDraw(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        //{
        //    //����rt2dʵ��

        //    Glow ??= CreateRender();
        //    MipMaps ??= new RenderTarget2D[DownSampleStep];
        //    BloomUp ??= new RenderTarget2D[DownSampleStep - 1];
        //    for (int i = 0; i < DownSampleStep; i++)
        //    {

        //        //����mipmap
        //        MipMaps[i] ??= new RenderTarget2D(Main.graphics.GraphicsDevice, (int)(Main.screenWidth / Math.Pow(DownSize, i)), (int)(Main.screenHeight / Math.Pow(DownSize, i)));
        //    }
        //    for (int i = 0; i < DownSampleStep - 1; i++)
        //    {
        //        //����mipmap
        //        BloomUp[i] ??= new RenderTarget2D(Main.graphics.GraphicsDevice, MipMaps[i].Width, MipMaps[i].Height);
        //    }
        //    GraphicsDevice gd = Main.instance.GraphicsDevice;
        //    SpriteBatch sb = Main.spriteBatch;
        //    Effect Bloom = AssetUtils.GetEffect("Bloom");
        //    Effect GenerateMipmap = AssetUtils.GetEffect("GenerateMipmap");
        //    //�����ʼ
        //    gd.SetRenderTarget(Main.screenTargetSwap);
        //    gd.Clear(Color.Transparent);
        //    sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        //    sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
        //    sb.End();

        //    //������Ҫ��Bloom�Ĳ���
        //    gd.SetRenderTarget(Glow);
        //    gd.Clear(Color.Transparent);
        //    sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        //    if (BloomDraw != null)
        //    {
        //        BloomDraw.Invoke();
        //        ClearAction(ref BloomDraw);
        //    }
        //    sb.End();

        //    //ģ��ͼ��

        //    for (int i = 0; i < DownSampleStep; i++)
        //    {

        //        if (i == 0)
        //        {

        //            gd.SetRenderTarget(MipMaps[i]);
        //            gd.Clear(Color.Transparent);
        //            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);
        //            sb.Draw(Glow, new Rectangle(0, 0, MipMaps[i].Width, MipMaps[i].Height), Color.White);
        //            sb.End();
        //        }
        //        else
        //        {
        //            gd.SetRenderTarget(MipMaps[i]);
        //            gd.Clear(Color.Transparent);
        //            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);
        //            gd.Textures[0] = MipMaps[i - 1];
        //            //GenerateMipmap.Parameters["TexSize"].SetValue(MipMaps[i - 1].Size());
        //            //GenerateMipmap.CurrentTechnique.Passes[0].Apply();
        //            sb.Draw(MipMaps[i - 1], new Rectangle(0, 0, MipMaps[i].Width, MipMaps[i].Height), Color.White);
        //            sb.End();
        //        }
        //    }

        //    //�ϲ�����������

        //    for (int i = DownSampleStep - 2; i >= 0; i--)
        //    {
        //        if (i == DownSampleStep - 2)
        //        {
        //            gd.SetRenderTarget(BloomUp[i]);
        //            gd.Clear(Color.Transparent);
        //            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);
        //            sb.Draw(MipMaps[i + 1], new Rectangle(0, 0, BloomUp[i].Width, BloomUp[i].Height), Color.White);
        //            sb.Draw(MipMaps[i], new Rectangle(0, 0, BloomUp[i].Width, BloomUp[i].Height), Color.White);
        //            sb.End();
        //        }
        //        else
        //        {
        //            gd.SetRenderTarget(BloomUp[i]);
        //            gd.Clear(Color.Transparent);
        //            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);
        //            sb.Draw(BloomUp[i + 1], new Rectangle(0, 0, BloomUp[i].Width, BloomUp[i].Height), Color.White);
        //            sb.Draw(MipMaps[i], new Rectangle(0, 0, BloomUp[i].Width, BloomUp[i].Height), Color.White);
        //            sb.End();
        //        }
        //    }


        //    //���Ч��
        //    gd.SetRenderTarget(Main.screenTarget);
        //    gd.Clear(Color.Transparent);
        //    sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone);
        //    //����ԭʼͼ��
        //    sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
        //    //����bloomͼ��
        //    DebugUtils.NewText(MipMaps[7].Size());
        //    //sb.Draw(MipMaps[0], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        //    //sb.Draw(MipMaps[1], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        //    //sb.Draw(MipMaps[2], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        //    //sb.Draw(MipMaps[3], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        //    //sb.Draw(MipMaps[4], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        //    //sb.Draw(MipMaps[5], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        //    //sb.Draw(MipMaps[6], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        //    //sb.Draw(MipMaps[7], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        //    //sb.Draw(MipMaps[8], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        //    //sb.Draw(MipMaps[9], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        //    sb.Draw(BloomUp[0], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

        //    sb.End();
        //    orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        //}
        private static RenderTarget2D CreateRender()
        {
            return new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
        }



        public static void AddAction(VisualPPActionType actionType, Action action)
        {
            switch (actionType)
            {
                case VisualPPActionType.FlowGraphDraw:
                    FlowGraphDraw += action;
                    break;
                case VisualPPActionType.DistortedGraphDraw:
                    DistortedGraphDraw += action;
                    break;
                case VisualPPActionType.BloomAreaDraw:
                    BloomDraw += action;
                    break;
                default:
                    break;
            }
        }

        private static void ClearAction(ref Action action)
        {
            Delegate[] allAction = action.GetInvocationList();
            for (int i = 0; i < allAction.Length; i++)
            {
                action -= allAction[i] as Action;
            }
        }



        public enum VisualPPActionType
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