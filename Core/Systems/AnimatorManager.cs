using Microsoft.CodeAnalysis.Operations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.Animators;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Core.Systems
{
    public class AnimatorManager : IOrderLoadable, IUpdate
    {
        public List<Animator> Animators;
        public int LoaderIndex => 3;

        public int UpdateIndex => 4;

        public static AnimatorManager Instance;

        public AnimatorManager() : base()
        {

            Instance = this;
        }
        public void Load()
        {
            Animators = new List<Animator>();
        }

        public void Unload()
        {
            if (Animators != null)
            {
                Animators = null;
            }
        }
        public void DrawBehindPlayer(SpriteBatch spriteBatch)
        {
            List<Animator> animators = new List<Animator>();
            if (Animators != null)
            {
                foreach (var animator in Animators)
                {
                    if (animator.DrawSortWithPlayer == Enum.DrawSortWithPlayer.Behind)
                    {
                        animators.Add(animator);
                    }
                }
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.NoShaderDraw_Behind(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.ShaderDraw_Behind(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.NoShaderDraw_Middle(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.ShaderDraw_Middle(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.NoShaderDraw_Front(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.ShaderDraw_Front(spriteBatch);
                }
                spriteBatch.End();
            }
            animators.Clear();
        }
        public void DrawFrontPlayer(SpriteBatch spriteBatch)
        {
            List<Animator> animators = new List<Animator>();
            if (Animators != null)
            {
                foreach (var animator in Animators)
                {
                    if (animator.DrawSortWithPlayer == Enum.DrawSortWithPlayer.Front)
                    {
                        animators.Add(animator);
                    }
                }
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.NoShaderDraw_Behind(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.ShaderDraw_Behind(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.NoShaderDraw_Middle(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.ShaderDraw_Middle(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.NoShaderDraw_Front(spriteBatch);
                }
                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                foreach (var animator in animators)
                {
                    animator.ShaderDraw_Front(spriteBatch);
                }
                spriteBatch.End();
            }
            animators.Clear();
        }

        public void Update()
        {

            if (Animators != null)
            {
                foreach (var animator in Animators)
                {
                    animator.AI();
                }
            }
        }

        public void AddAnimator(Animator animator)
        {
            animator.active = true;
            Animators.Add(animator);
        }
        public T Register<T>() where T : Animator, new()
        {
            if (Animators == null)
            {
                Animators = new List<Animator>();
            }
            T instance = new T();
            instance.active = true;
            Animators.Add(instance);
            return instance;
        }
        public Animator FindAnimator(string Name)
        {
            return Animators.Find((o) => o.Name == Name);
        }
        public bool RemoveAnimator(string Name)
        {
            Animator target = FindAnimator(Name);
            return Animators.Remove(target);
        }
        public bool RemoveAnimator(Animator target)
        {
            return Animators.Remove(target);
        }
    }
}
