using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Particles;
using MysteriousAlchemy.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Utils
{
    public class ParticleUtils
    {
        public static void SpwonFog(Vector2 center, Vector2 velocity, Color newColor = default, float scale = 0.2f, float scatterRange = 0, float scaleRange = 0)
        {
            Vector2 currectVel = velocity.RotatedBy(Main.rand.NextFloat(-scatterRange, scatterRange));
            float currectScale = scale * Main.rand.NextFloat(1 - scaleRange, 1 + scaleRange);
            Particle.NewParticle<Fog>(center, currectVel, newColor, currectScale);
        }
    }
}
