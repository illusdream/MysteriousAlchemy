using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Dusts
{
    public class IngredientStrokeDustcs : ModDust
    {
        public override string Texture => AssetUtils.Dusts + Name;
        int k = 0;
        public override void OnSpawn(Dust dust)
        {// Multiply the dust's start velocity by 0.4, slowing it down
            dust.noGravity = true; // Makes the dust have no gravity.
            dust.noLight = true; // Makes the dust emit no light.
            dust.scale *= 0.7f; // Multiplies the dust's initial scale by 1.5
            dust.alpha = 255;
            k = 0;

        }

        public override bool Update(Dust dust)
        { // Calls every frame the dust is active
            dust.position += dust.velocity;
            Vector2 FinalVel = (dust.velocity + ((dust.customData as CustomData_CenterPos).Vector2 - dust.position).SafeNormalize(Vector2.Zero) * 2).SafeNormalize(Vector2.Zero) * 5;
            dust.velocity = FinalVel;
            k++;
            if (k % 10 == 0)
            {
                Dust _dust;
                _dust = Main.dust[Terraria.Dust.NewDust(dust.position, 30, 30, 228, 0f, 0f, 0, new Color(255, 255, 255), 0.9302325f)];
                _dust.noGravity = true;
                _dust.shader = GameShaders.Armor.GetSecondaryShader(77, Main.LocalPlayer);
            }
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
                if (d.type == ModContent.DustType<IngredientStrokeDustcs>() && d.active)
                {
                    Texture2D tex = AssetUtils.GetTexture2D(AssetUtils.Texture + "IngredientStrokeEffect");
                    sb.Draw(tex, d.position - Main.screenPosition, null, Color.White, 0, tex.Size() / 2, d.scale * 0.4f, SpriteEffects.None, 0);
                }
            }
        }
    }
    public class CustomData_CenterPos
    {
        public CustomData_CenterPos(Vector2 vector2)
        {
            this.Vector2 = vector2;
        }
        public Vector2 Vector2;
    }
}