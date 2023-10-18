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
            if (Animators != null)
            {
                foreach (var animator in Animators)
                {
                    if (animator.DrawSortWithPlayer == Enum.DrawSortWithPlayer.Behind)
                    {
                        animator.NoShaderDraw_Behind(spriteBatch);
                        animator.ShaderDraw_Behind(spriteBatch);
                        animator.NoShaderDraw_Middle(spriteBatch);
                        animator.ShaderDraw_Middle(spriteBatch);
                        animator.NoShaderDraw_Front(spriteBatch);
                        animator.ShaderDraw_Front(spriteBatch);
                    }
                }
            }
        }
        public void DrawFrontPlayer(SpriteBatch spriteBatch)
        {
            if (Animators != null)
            {
                foreach (var animator in Animators)
                {
                    if (animator.DrawSortWithPlayer == Enum.DrawSortWithPlayer.Front)
                    {
                        animator.NoShaderDraw_Behind(spriteBatch);
                        animator.ShaderDraw_Behind(spriteBatch);
                        animator.NoShaderDraw_Middle(spriteBatch);
                        animator.ShaderDraw_Middle(spriteBatch);
                        animator.NoShaderDraw_Front(spriteBatch);
                        animator.ShaderDraw_Front(spriteBatch);
                    }
                }
            }
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
    }
}
