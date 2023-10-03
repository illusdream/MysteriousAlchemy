using Microsoft.CodeAnalysis.Operations;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.System
{
    public class AnimatorManager : IOrderLoadable, IUpdate
    {
        public List<Animator> Animators;
        public int LoaderIndex => 3;

        public int UpdateIndex => 4;

        public static AnimatorManager Instance;

        public void Load()
        {
            Animators = new List<Animator>();
            Instance = this;
        }

        public void Unload()
        {
            if (Animators != null)
            {
                Animators = null;
            }
        }
        public void NoShaderDraw_AlphaBlend(SpriteBatch spriteBatch)
        {
            if (Animators == null)
                return;
            foreach (var animators in Animators)
            {
                animators.NoShaderDraw_AlphaBlend(spriteBatch);
            }
        }
        public void NoShaderDraw_Additive(SpriteBatch spriteBatch)
        {
            if (Animators == null)
                return;
            foreach (var animators in Animators)
            {
                animators.NoShaderDraw_Additive(spriteBatch);
            }
        }
        public void NoShaderDraw_NonPremultiplied(SpriteBatch spriteBatch)
        {
            if (Animators == null)
                return;
            foreach (var animators in Animators)
            {
                animators.NoShaderDraw_NonPremultiplied(spriteBatch);
            }
        }
        public void ShaderDraw_AlphaBlend(SpriteBatch spriteBatch)
        {
            if (Animators == null)
                return;
            foreach (var animators in Animators)
            {
                animators.ShaderDraw_AlphaBlend(spriteBatch);
            }
        }
        public void ShaderDraw__Additive(SpriteBatch spriteBatch)
        {
            if (Animators == null)
                return;
            foreach (var animators in Animators)
            {
                animators.ShaderDraw__Additive(spriteBatch);
            }
        }
        public void ShaderDraw_NonPremultiplied(SpriteBatch spriteBatch)
        {
            if (Animators == null)
                return;
            foreach (var animators in Animators)
            {
                animators.ShaderDraw_NonPremultiplied(spriteBatch);
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
    }
}
