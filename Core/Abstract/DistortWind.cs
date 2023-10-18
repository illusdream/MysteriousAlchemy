using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Core.Abstract
{
    public class DistortWind
    {
        public bool active;

        public Vector2 postion;

        public Vector2[] vertexTop;

        public Vector2[] vertexBottom;

        public string distortTexture;

        public float flipAlpha;

        public float baseTimeScale;

        //扭曲特效应该向哪边扭曲
        public Vector2 flowDirection;

        public Action<DistortWind> drawDistort;

        public Action<DistortWind> update;

        public static int NewDistortWind(Vector2 position, Vector2[] vertexTop, Vector2[] vertexBottom, float baseTimeScale, Vector2 flowDirection, Action<DistortWind> drawDistort, Action<DistortWind> update)
        {
            int result = VisualEffectSystem.DistortEffect_DistortWindCount - 1;
            DistortWind[] distortWinds = VisualEffectEntitySystem.distortWinds;
            for (int i = 0; i < VisualEffectSystem.DistortEffect_DistortWindCount; i++)
            {
                if (distortWinds[i].active)
                    continue;
                distortWinds[i] = new DistortWind();
                distortWinds[i].active = true;
                distortWinds[i].postion = position;
                distortWinds[i].vertexTop = vertexTop;
                distortWinds[i].vertexBottom = vertexBottom;
                distortWinds[i].baseTimeScale = baseTimeScale;
                distortWinds[i].flowDirection = flowDirection;
                distortWinds[i].drawDistort = drawDistort;
                distortWinds[i].update = update;
                break;
            }
            return result;
        }

        public static void DefaultUpdate(DistortWind distortWind)
        {

        }
        public static void DefaultDraw(DistortWind distortWind)
        {
            List<VertexPositionColorTexture> bars = new List<VertexPositionColorTexture>();
            int ArrayLength = Math.Min(distortWind.vertexTop.Length, distortWind.vertexBottom.Length);
            // 把所有的点都生成出来，按照顺序
            for (int i = 0; i < ArrayLength; ++i)
            {
                if (distortWind.vertexTop[i] == Vector2.Zero)
                {
                    break;
                }
                if (distortWind.vertexBottom[i] == Vector2.Zero) break;
                //Main.spriteBatch.Draw(Main.magicPixel, oldPosi[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                var factor = i / (float)ArrayLength;
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                bars.Add(new VertexPositionColorTexture((distortWind.vertexTop[i] + distortWind.postion).Vec3(), new Color(distortWind.vertexTop[i].ToRotation(), 1, 0), new Vector2(factor, 0) + distortWind.flowDirection * MathUtils.GetTime(distortWind.baseTimeScale)));
                bars.Add(new VertexPositionColorTexture((distortWind.vertexTop[i] + distortWind.postion).Vec3(), new Color(distortWind.vertexBottom[i].ToRotation(), 1, 0), new Vector2(factor, 1) + distortWind.flowDirection * MathUtils.GetTime(distortWind.baseTimeScale)));
            }
            List<VertexPositionColorTexture> triangleList = new List<VertexPositionColorTexture>();

            if (bars.Count > 2)
            {

                // 按照顺序连接三角形

                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    triangleList.Add(bars[i]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 1]);

                    triangleList.Add(bars[i + 1]);
                    triangleList.Add(bars[i + 2]);
                    triangleList.Add(bars[i + 3]);
                }

                Effect effect = AssetUtils.GetEffect("DefaultSlash");
                Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
                Matrix view = Main.GameViewMatrix.TransformationMatrix;
                Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

                effect.Parameters["transformMatrix"].SetValue(world * view * projection);
                effect.Parameters["shapeTexture"].SetValue(AssetUtils.GetTexture2D(AssetUtils.Flow + "Flow_1"));
                effect.Parameters["colorTexture"].SetValue(AssetUtils.GetColorBar("White"));
                effect.CurrentTechnique.Passes[0].Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
            }
        }
    }
}
