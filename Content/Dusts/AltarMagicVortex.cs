using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Utils;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Dusts
{
    public class AltarMagicVortex : ModDust
    {
        public override string Texture => AssetUtils.Dusts + Name;
        public override void OnSpawn(Dust dust)
        {// Multiply the dust's start velocity by 0.4, slowing it down
            dust.noGravity = true; // Makes the dust have no gravity.
            dust.noLight = true; // Makes the dust emit no light.
            dust.scale *= 0.7f; // Multiplies the dust's initial scale by 1.5
            dust.alpha = 255;

        }

        public override bool Update(Dust dust)
        { // Calls every frame the dust is active
            dust.position += dust.velocity;
            dust.velocity = (new Vector2(-dust.velocity.Y, dust.velocity.X) * -0.13f + dust.velocity).SafeNormalize(Vector2.Zero) * dust.scale * 4;
            dust.scale *= 0.985f;
            if (dust.scale < 0.35f)
            {
                dust.active = false;
            }

            return false; // Return false to prevent vanilla behavior.
        }

        public static void DrawAll(SpriteBatch sb)
        {
            foreach (Dust d in Main.dust)
            {
                if (d.type == ModContent.DustType<AltarMagicVortex>() && d.active)
                {
                    Texture2D tex = AssetUtils.GetTexture2D(AssetUtils.Extra + "CosmicFlame");
                    sb.Draw(tex, d.position - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, d.scale * 0.4f, SpriteEffects.None, 0);
                }
            }
        }
    }
}