using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Renderers;

namespace MysteriousAlchemy.Core.Abstract
{
    /// <summary>
    /// 其实这个基类和<see href="DrawUnit"/>的区别只有不能使用shader，无updateAction与3D绘制
    /// 外加多出来的粒子相关参数
    /// 绘制用接口绘制，类本身不用写,只存储基本数据
    /// </summary>
    public class Particle
    {
        public virtual Texture2D Texture { get; }
        public Vector2 position;
        public Vector2 velocity;
        public float rotation;
        public float scale;
        public float alpha;
        public Color color;
        public int timer;
        public bool active;

        public virtual void SetDefaults()
        {

        }
        public virtual void Onspown()
        {

        }
        public virtual void Update()
        {
            timer++;
        }
        public virtual bool ShouldUpdatePosition() => true;

        public static Particle NewParticleDirect<T>(Vector2 center, Vector2 velocity, Color newColor = default, float Scale = 1f) where T : Particle, new()
        {
            return ParticleSystem.particles[NewParticle<T>(center, velocity, newColor, Scale)];
        }
        public static int NewParticle<T>(Vector2 center, Vector2 velocity, Color newColor = default, float Scale = 1f) where T : Particle, new()
        {
            int result = ParticleSystem.ParticleCount - 1;
            Particle[] particles = ParticleSystem.particles;
            for (int i = 0; i < ParticleSystem.ParticleCount; i++)
            {
                if (particles[i].active)
                    continue;
                particles[i] = new T();
                result = i;
                //设置各种初始值
                particles[i].timer = 0;
                particles[i].active = true;
                particles[i].color = newColor;
                //particle.alpha = Alpha;
                particles[i].position = center;
                particles[i].velocity = velocity;
                particles[i].rotation = 0f;
                particles[i].scale = Scale;

                particles[i].SetDefaults();
                particles[i].Onspown();
                break;
            }
            return result;
        }
    }
}
