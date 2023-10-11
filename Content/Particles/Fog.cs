using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Content.Particles
{
    public class Fog : Particle, IDrawAddtive
    {
        int frameCount;
        public override Texture2D Texture => AssetUtils.GetTexture2DImmediate(AssetUtils.Dusts + "Fog");
        public SpriteSortMode Sort { get => SpriteSortMode.Deferred; set => Sort = value; }

        public override void Onspown()
        {
            alpha = Main.rand.NextFloat(0.7f, 1);
            frameCount = Main.rand.Next(4);
            base.Onspown();
        }
        public override void Update()
        {
            velocity *= 0.7f;
            velocity = velocity.RotatedBy(rotation);
            rotation += 0.01f;
            alpha *= 0.95f;
            if (alpha < 0.1f)
                active = false;
            if (scale < 0f)
                active = false;
            base.Update();
        }
        public void DrawAddtive(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DrawUtils.ToScreenPosition(position), new Rectangle(0, 256 * frameCount, 256, 256), color * alpha, rotation, new Vector2(256, 256) / 2f, scale, SpriteEffects.None, 0);
        }
    }
}
