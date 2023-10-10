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
            frameCount = Main.rand.Next(4);
            base.Onspown();
        }
        public void DrawAddtive(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, DrawUtils.ToScreenPosition(position), new Rectangle(0, 256 * frameCount, 256, 256), color, rotation, new Vector2(256, 256) / 2f, scale, SpriteEffects.None, 0);
        }
    }
}
