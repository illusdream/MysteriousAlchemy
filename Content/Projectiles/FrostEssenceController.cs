using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Dusts;
using MysteriousAlchemy.Items;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Projectiles
{
    public class FrostEssenceController : ModProjectile
    {
        public override string Texture => AssetUtils.Projectiles + Name;
        Vector2[] LaserPosition = new Vector2[150];
        Vector2 Toward = Vector2.Zero;
        Vector2 LaserStartPoint = Vector2.Zero;
        Vector2 EndPoint = Vector2.Zero;
        //转向速度
        float TurnRate = 0.1f;
        public override void SetDefaults()
        {
            Projectile.friendly = true; // 弹幕是否攻击敌方，默认false
            Projectile.hostile = false; // 弹幕是否攻击友方和城镇NPC，默认false
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            base.SetDefaults();
        }
        public override void AI()
        {
            if (Main.mouseRight)
            {
                Projectile.timeLeft++;
            }
            else
            {
                Projectile.timeLeft = 0;
            }
            Vector2 ToMouse = (Main.MouseWorld - Main.LocalPlayer.Center).SafeNormalize(Vector2.Zero);
            Toward = ((1 - TurnRate) * Toward + TurnRate * ToMouse).SafeNormalize(Vector2.Zero);
            LaserStartPoint = Main.LocalPlayer.Center + Toward * 50;
            for (int i = 0; i < LaserPosition.Length; i++)
            {
                LaserPosition[i] = LaserStartPoint + Toward * 10 * i;
            }
            EndPoint = LaserPosition[LaserPosition.Length - 1];
            Projectile.position = Main.LocalPlayer.position;
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {

            DrawVortex(LaserStartPoint, Toward, 1, 1);
            DrawLaser(LaserPosition);
            SpownVortexDust(LaserStartPoint, Toward, 50);
            SpownLaserDust(LaserPosition, 10, 10, 10);
            Vector2 ToMouse = (Main.MouseWorld - Main.LocalPlayer.Center).SafeNormalize(Vector2.Zero);
            //核心蓄力漩涡


            //激光
            base.PostDraw(lightColor);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), LaserStartPoint, EndPoint, 20, ref point);
        }
        private void DrawVortex(Vector2 Center, Vector2 Toward, float scale, float VortexRate)
        {

            Texture2D CircleTex = AssetUtils.GetFlow("flow_4");
            Vector2 ToMouse = Toward.SafeNormalize(Vector2.Zero);
            Effect CommenPolarVortex = ModContent.Request<Effect>("MysteriousAlchemy/Effects/CommenPolarVortex").Value;
            Texture2D VortexColor = AssetUtils.GetTexture2D(AssetUtils.Extra + "BlueVortex");
            DrawUtil.DrawEntityInWorld(Main.spriteBatch, CircleTex, Center - Main.screenPosition, Color.White, CommenPolarVortex, 0, scale, -ToMouse.ToRotation(), MathHelper.PiOver4 * ((ToMouse.SafeNormalize(Vector2.One).X) < 0 ? -1 : 1), () =>
            {
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;
                CommenPolarVortex.Parameters["UTransform"].SetValue(model * projection);
                CommenPolarVortex.Parameters["lengthOffest"].SetValue((float)Main.time / 600f * VortexRate);
                CommenPolarVortex.Parameters["repeat"].SetValue(7);
                CommenPolarVortex.Parameters["zoom"].SetValue(0.7f);
                CommenPolarVortex.Parameters["distortScale"].SetValue(1);
                CommenPolarVortex.Parameters["lengthDistortScale"].SetValue(0.5f);
                Main.graphics.GraphicsDevice.Textures[1] = VortexColor;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
            });
        }
        private void SpownVortexDust(Vector2 Center, Vector2 Toward, float Radium)
        {
            Vector2 ToMouse = Toward.SafeNormalize(Vector2.Zero);
            for (int i = 0; i < 3; i++)
            {
                Vector2 DustSpownPosition = Main.rand.NextVector2Circular(Radium, Radium);
                Vector2 FinalPosition = DrawUtil.MartixTrans(DustSpownPosition, -ToMouse.ToRotation(), MathHelper.PiOver4 * ((ToMouse.SafeNormalize(Vector2.One).X) < 0 ? -1 : 1));
                Vector2 FinalVel = FinalPosition.RotatedBy(-MathHelper.PiOver2).SafeNormalize(Vector2.Zero) * 3;
                Dust.NewDustDirect(FinalPosition + Center, 5, 5, 307, FinalVel.X, FinalVel.Y).noGravity = true;
            }
        }
        private void DrawLaser(Vector2[] LaserCenter)
        {
            Texture2D Mainshape = AssetUtils.GetTexture2D(AssetUtils.Extra + "Extra_1");
            Texture2D MainMask = AssetUtils.GetMask("Mask2");
            Texture2D MainColor = AssetUtils.GetColorBar("Frost");
            DrawUtil.DrawProjectileTrail(LaserCenter, Mainshape, MainMask, MainColor, 35, -(float)Main.time / 600f);
        }
        private void SpownLaserDust(Vector2[] LaserCenter, float SpownRate, float VelMin, float VelMax)
        {
            for (int i = 0; i < LaserCenter.Length - 1; i++)
            {
                if (Main.rand.Next(0, 25) == 0)
                {
                    Vector2 Vel = (LaserCenter[i + 1] - LaserCenter[i]).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(3, 6);
                    Dust.NewDustDirect(LaserCenter[i], 5, 5, 307, Vel.X, Vel.Y).noGravity = true;
                }
            }
        }
    }
}