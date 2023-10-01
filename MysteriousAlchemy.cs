using Humanizer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Mapping;
using MysteriousAlchemy.Dusts;
using MysteriousAlchemy.UI;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace MysteriousAlchemy
{
    public class MysteriousAlchemy : Mod
    {
        //单例
        public static MysteriousAlchemy Instance { get; private set; }

        public MysteriousAlchemy()
        {
            Instance = this;
        }

        public static MysteriousAlchemy GetInstance()
        {
            return Instance;
        }
        //自动加载
        List<IOrderLoadable> Loadables;

        public static RenderTarget2D render;
        public static Effect AltarMagicTransform;
        public static Effect Default;
        public static Effect BigTentacle;
        public static Effect AltarTransform;
        public static Effect IngredientStroke;
        public static Effect Rainbow;
        public static Effect Polortest;
        public static Effect CommenPolarVortex;
        public static Effect Trail;
        public static Effect Bloom1;
        public override void Load()
        {
            AltarTransform = ModContent.Request<Effect>("MysteriousAlchemy/Effects/AltarTransform").Value;
            BigTentacle = ModContent.Request<Effect>("MysteriousAlchemy/Effects/BigTentacle").Value;
            AltarMagicTransform = ModContent.Request<Effect>("MysteriousAlchemy/Effects/AltarMagicTransform").Value;
            IngredientStroke = ModContent.Request<Effect>("MysteriousAlchemy/Effects/IngredientStroke").Value;
            Rainbow = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Rainbow").Value;
            Polortest = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Polortest").Value;
            Trail = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Trail").Value;
            CommenPolarVortex = ModContent.Request<Effect>("MysteriousAlchemy/Effects/CommenPolarVortex", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Bloom1 = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Bloom1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Terraria.Graphics.Effects.On_FilterManager.EndCapture += FilterManager_EndCapture;
            Main.OnResolutionChanged += Main_OnResolutionChanged;
            Main.QueueMainThreadAction(() =>
            {
                CreateRender();

            });

            //
            Loadables = new List<IOrderLoadable>();

            foreach (var entity in Code.GetTypes())
            {
                if (!entity.IsAbstract && entity.GetInterfaces().Contains(typeof(IOrderLoadable)))
                {
                    //创建实例
                    var instance = Activator.CreateInstance(entity);
                    //添加到列表
                    Loadables.Add(instance as IOrderLoadable);
                }
                //按加载顺序排序
                Loadables.Sort((n, t) => n.LoaderIndex.CompareTo(t.LoaderIndex));
            }

            //自动加载
            foreach (var NeedLoad in Loadables)
            {
                NeedLoad.Load();
            }
            base.Load();
        }
        public override void Unload()
        {
            Main.OnResolutionChanged -= Main_OnResolutionChanged;
            Terraria.Graphics.Effects.On_FilterManager.EndCapture -= FilterManager_EndCapture;

            //自动卸载
            if (Loadables is not null)
            {
                foreach (var loadable in Loadables)
                {
                    loadable.Unload();
                }
                Loadables = null;
            }
            base.Unload();
        }
        public override void PostSetupContent()
        {
            AltarTransform = ModContent.Request<Effect>("MysteriousAlchemy/Effects/AltarTransform").Value;
            AltarMagicTransform = ModContent.Request<Effect>("MysteriousAlchemy/Effects/AltarMagicTransform").Value;
            Default = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Default").Value;
            BigTentacle = ModContent.Request<Effect>("MysteriousAlchemy/Effects/BigTentacle").Value;
            IngredientStroke = ModContent.Request<Effect>("MysteriousAlchemy/Effects/IngredientStroke").Value;
            Rainbow = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Rainbow").Value;
            Polortest = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Polortest").Value;
            Trail = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Trail").Value;
            CommenPolarVortex = ModContent.Request<Effect>("MysteriousAlchemy/Effects/CommenPolarVortex", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Bloom1 = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Bloom1", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            base.PostSetupContent();
            //ModItemIDMap.LoadModItemIDMaping();
            Main.QueueMainThreadAction(() =>
            {
                CreateRender();
            });
        }
        private void Main_OnResolutionChanged(Vector2 obj)
        {
            Main.QueueMainThreadAction(() => { CreateRender(); });
        }

        private void FilterManager_EndCapture(Terraria.Graphics.Effects.On_FilterManager.orig_EndCapture orig, Terraria.Graphics.Effects.FilterManager self, RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
        {
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            SpriteBatch sb = Main.spriteBatch;


            gd.SetRenderTarget(Main.screenTargetSwap);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            sb.End();

            gd.SetRenderTarget(render);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            AltarMagicVortex.DrawAll(sb);
            IngredientStrokeDustcs.DrawAll(sb);
            sb.End();

            gd.SetRenderTarget(Main.screenTarget);
            gd.Clear(Color.Transparent);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sb.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            gd.Textures[1] = AssetUtils.GetTexture2D(AssetUtils.Extra + "Cosmic");
            AltarMagicTransform.CurrentTechnique.Passes[0].Apply();
            AltarMagicTransform.Parameters["m"].SetValue(0.62f);
            AltarMagicTransform.Parameters["n"].SetValue(0.05f);
            AltarMagicTransform.Parameters["timer"].SetValue((float)Main.time / 180f % 1);
            sb.Draw(render, Vector2.Zero, Color.White);
            sb.End();


            //UseBloom(gd);
            orig(self, finalTexture, screenTarget1, screenTarget2, clearColor);
        }
        private void UseBloom(GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();

            //取样
            graphicsDevice.SetRenderTarget(render);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Bloom1.CurrentTechnique.Passes[0].Apply();//取亮度超过m值的部分

            Bloom1.Parameters["m"].SetValue(0.75f);

            Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            Main.spriteBatch.End();

            //处理
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Bloom1.Parameters["uScreenResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            Bloom1.Parameters["uRange"].SetValue(2.5f);
            Bloom1.Parameters["uIntensity"].SetValue(0.94f);
            for (int i = 0; i < 3; i++)//交替使用两个RenderTarget2D，进行多次模糊
            {
                Bloom1.CurrentTechnique.Passes["GlurV"].Apply();//横向
                graphicsDevice.SetRenderTarget(Main.screenTarget);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Draw(render, Vector2.Zero, Color.White);

                Bloom1.CurrentTechnique.Passes["GlurH"].Apply();//纵向
                graphicsDevice.SetRenderTarget(render);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
            }
            Main.spriteBatch.End();

            //叠加到原图上
            graphicsDevice.SetRenderTarget(Main.screenTarget);
            graphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);//Additive把模糊后的部分加到Main.screenTarget里
            Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
            Main.spriteBatch.Draw(render, Vector2.Zero, Color.White);
            Main.spriteBatch.End();
        }
        public void CreateRender()
        {
            render = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight);
        }

    }

    public class UIsystem : ModSystem
    {
        //创建变量
        internal UI_Extractor UI_Extractor;
        internal UserInterface UI_ExtractorUserInterface;

        internal UI_MysteriousAlterOld UI_MysteriousAlter;
        internal UserInterface UI_MysteriousAlterUserInterface;

        internal UI_AltarCompose UI_AltarCompose;
        internal UserInterface UI_AltarComposeUserInterface;
        public override void Load()
        {
            UI_Extractor = new UI_Extractor();

            UI_Extractor.Activate();

            UI_ExtractorUserInterface = new UserInterface();

            UI_ExtractorUserInterface.SetState(UI_Extractor);

            UI_MysteriousAlter = new UI_MysteriousAlterOld();

            UI_MysteriousAlter.Activate();

            UI_MysteriousAlterUserInterface = new UserInterface();

            UI_MysteriousAlterUserInterface.SetState(UI_MysteriousAlter);

            UI_AltarCompose = new UI_AltarCompose();

            UI_AltarCompose.Activate();

            UI_AltarComposeUserInterface = new UserInterface();

            UI_AltarComposeUserInterface.SetState(UI_AltarCompose);
            base.Load();



        }
        public override void UpdateUI(GameTime gameTime)
        {
            UI_Extractor?.Update(gameTime);
            UI_MysteriousAlter?.Update(gameTime);
            // UI_AltarCompose?.Update(gameTime);
            UI_AltarComposeUserInterface?.Update(gameTime);
            base.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                //往绘制层集合插入一个成员，第一个参数是插入的地方的索引，第二个参数是绘制层
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer("UI: MAUI",
                delegate
                {
                    //绘制UI（运行UI的Draw方法）
                    if (UI_Extractor.Visable)
                    {
                        UI_Extractor.Draw(Main.spriteBatch);
                    }
                    if (UI_MysteriousAlterOld.Visable)
                    {
                        UI_MysteriousAlter.Draw(Main.spriteBatch);
                    }
                    if (UI_AltarCompose.Visable)
                    {
                        UI_AltarCompose.Draw(Main.spriteBatch);
                    }
                    return true;
                },
                //绘制层的类型
                InterfaceScaleType.UI)
                );
            }
            base.ModifyInterfaceLayers(layers);
        }

        public override void PostUpdateEverything()
        {
            base.PostUpdateEverything();

        }
    }

}