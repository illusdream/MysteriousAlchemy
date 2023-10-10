using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MysteriousAlchemy.Utils
{
    public class DrawUtils
    {
        public static Vector2 MartixTrans(Vector2 input, float angleH, float angleV)
        {
            return new Vector2(input.X * (float)Math.Sin(angleV) + input.Y * (float)(Math.Sin(angleH) * Math.Cos(angleV)), input.Y * (float)(Math.Cos(angleH)));
        }

        public static Vector2 ToScreenPosition(Vector2 vector2)
        {
            return vector2 - Main.screenPosition;
        }

        public static Vector2 GetCurrectScale(Texture2D texture2D, Vector2 target)
        {
            return target / texture2D.Size();
        }

        /// <summary>
        /// 用于使用顶点绘制绘制伪3d图像，获得正确的顶点坐标
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="sourseTex"></param>
        /// <returns></returns>
        public static RectangleF GetCurrectRectangleInVertexPaint(Rectangle sourse, Texture2D sourseTex)
        {
            Rectangle TexFullRectangle = new Rectangle(0, 0, sourseTex.Width, sourseTex.Height);
            Vector2 currectTopleft = new Vector2(sourse.X, sourse.Y) / sourseTex.Size();
            RectangleF currectRectrangleF = new RectangleF(currectTopleft.X, currectTopleft.Y, sourse.Width / (float)sourseTex.Width, sourse.Height / (float)sourseTex.Height);
            return currectRectrangleF;

        }
        public static bool OnScreen(Vector2 pos)
        {
            return pos.X > -16 && pos.X < Main.screenWidth + 16 && pos.Y > -16 && pos.Y < Main.screenHeight + 16;
        }

        /// <summary>
        /// 当实体在前部时返回<see langword="true"/>,否则返回<see langword="false"/>
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static bool GetEntityContextInCircle(float angle)
        {
            float AngleInTwoPI = angle % MathHelper.TwoPi;
            if (AngleInTwoPI > MathHelper.PiOver2 && AngleInTwoPI < MathHelper.Pi + MathHelper.PiOver2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void DrawTile(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Effect effect, float rotation = 0, float scale = 1, float angleH = 0, float angleV = 0, Action effectAction = null)
        {
            if (texture == null)
            {
                return;
            }
            if (effect == null)
            {
                effect = (Effect)ModContent.Request<Effect>("MysteriousAlchemy/Effects/Default").Value;
            }
            int TextureWidth = texture.Width;
            int TextureHeight = texture.Height;
            float TextureHalfDiagonalDistance = texture.Size().Length() / 2f * scale;

            Vector2 vertex1 = MartixTrans(new Vector2((float)Math.Cos(0 + rotation + MathHelper.PiOver4), (float)Math.Sin(0 + rotation + MathHelper.PiOver4)) * TextureHalfDiagonalDistance, angleH, angleV);
            Vector2 vertex2 = MartixTrans(new Vector2((float)Math.Cos(MathHelper.PiOver2 + rotation + MathHelper.PiOver4), (float)Math.Sin(MathHelper.PiOver2 + rotation + MathHelper.PiOver4)) * TextureHalfDiagonalDistance, angleH, angleV);
            Vector2 vertex3 = MartixTrans(new Vector2((float)Math.Cos(MathHelper.Pi + rotation + MathHelper.PiOver4), (float)Math.Sin(MathHelper.Pi + rotation + MathHelper.PiOver4)) * TextureHalfDiagonalDistance, angleH, angleV);
            Vector2 vertex4 = MartixTrans(new Vector2((float)Math.Cos(MathHelper.Pi * 1.5f + rotation + MathHelper.PiOver4), (float)Math.Sin(MathHelper.Pi * 1.5f + rotation + MathHelper.PiOver4)) * TextureHalfDiagonalDistance, angleH, angleV);
            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();


            // 按照顺序连接三角形
            triangleList.Add(new CustomVertexInfo(position + vertex1, Color.White, new Vector3(0, 0, 1)));
            triangleList.Add(new CustomVertexInfo(position + vertex2, Color.White, new Vector3(0, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(position + vertex3, Color.White, new Vector3(1, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(position + vertex1, Color.White, new Vector3(0, 0, 1)));
            triangleList.Add(new CustomVertexInfo(position + vertex3, Color.White, new Vector3(1f, 1f, 1)));
            triangleList.Add(new CustomVertexInfo(position + vertex4, Color.White, new Vector3(1f, 0, 1)));



            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null);
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            // 干掉注释掉就可以只显示三角形栅格
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //rasterizerState.FillMode = FillMode.WireFrame;
            // Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            // 把变换和所需信息丢给shader

            effect.Parameters["UTransform"].SetValue(model * projection);
            if (effectAction != null)
            {
                effectAction.Invoke();
            }
            Main.graphics.GraphicsDevice.Textures[0] = texture;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            effect.CurrentTechnique.Passes[0].Apply();

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
        }
        public static void DrawEntityInWorld(SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, Effect effect, float rotation = 0, float scale = 1, float angleH = 0, float angleV = MathHelper.PiOver2, Action effectAction = null, BlendState blendState = null, int Flip = 0
            )
        {
            if (texture == null)
            {
                return;
            }
            if (effect == null)
            {
                effect = (Effect)ModContent.Request<Effect>("MysteriousAlchemy/Effects/Default").Value;
            }
            if (blendState == null)
            {
                blendState = BlendState.Additive;
            }
            int TextureWidth = texture.Width;
            int TextureHeight = texture.Height;
            float TextureHalfDiagonalDistance = texture.Size().Length() / 2f * scale;
            float TexPosFix = -MathHelper.PiOver4 * 3;
            Vector2 vertex1 = MartixTrans(new Vector2((float)Math.Cos(0 + rotation + TexPosFix), (float)Math.Sin(0 + rotation + TexPosFix)) * TextureHalfDiagonalDistance, angleH, angleV);
            Vector2 vertex2 = MartixTrans(new Vector2((float)Math.Cos(MathHelper.PiOver2 + rotation + TexPosFix), (float)Math.Sin(MathHelper.PiOver2 + rotation + TexPosFix)) * TextureHalfDiagonalDistance, angleH, angleV);
            Vector2 vertex3 = MartixTrans(new Vector2((float)Math.Cos(MathHelper.Pi + rotation + TexPosFix), (float)Math.Sin(MathHelper.Pi + rotation + TexPosFix)) * TextureHalfDiagonalDistance, angleH, angleV);
            Vector2 vertex4 = MartixTrans(new Vector2((float)Math.Cos(MathHelper.Pi * 1.5f + rotation + TexPosFix), (float)Math.Sin(MathHelper.Pi * 1.5f + rotation + TexPosFix)) * TextureHalfDiagonalDistance, angleH, angleV);
            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

            if (Flip == 0)
            {
                // 按照顺序连接三角形
                triangleList.Add(new CustomVertexInfo(position + vertex1, color, new Vector3(0, 0, 1)));
                triangleList.Add(new CustomVertexInfo(position + vertex4, color, new Vector3(0, 1f, 1)));
                triangleList.Add(new CustomVertexInfo(position + vertex3, color, new Vector3(1, 1f, 1)));
                triangleList.Add(new CustomVertexInfo(position + vertex1, color, new Vector3(0, 0, 1)));
                triangleList.Add(new CustomVertexInfo(position + vertex3, color, new Vector3(1f, 1f, 1)));
                triangleList.Add(new CustomVertexInfo(position + vertex2, color, new Vector3(1f, 0, 1)));
            }
            else
            {
                // 按照顺序连接三角形
                triangleList.Add(new CustomVertexInfo(position + vertex3, color, new Vector3(0, 0, 1)));
                triangleList.Add(new CustomVertexInfo(position + vertex4, color, new Vector3(0, 1f, 1)));
                triangleList.Add(new CustomVertexInfo(position + vertex1, color, new Vector3(1, 1f, 1)));
                triangleList.Add(new CustomVertexInfo(position + vertex3, color, new Vector3(0, 0, 1)));
                triangleList.Add(new CustomVertexInfo(position + vertex1, color, new Vector3(1f, 1f, 1)));
                triangleList.Add(new CustomVertexInfo(position + vertex2, color, new Vector3(1f, 0, 1)));

            }




            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, blendState, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
            // 干掉注释掉就可以只显示三角形栅格
            //RasterizerState rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //rasterizerState.FillMode = FillMode.WireFrame;
            // Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            // 把变换和所需信息丢给shader

            effect.Parameters["UTransform"].SetValue(model * projection);
            if (effectAction != null)
            {
                effectAction.Invoke();
            }
            Main.graphics.GraphicsDevice.Textures[0] = texture;
            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
            effect.CurrentTechnique.Passes[0].Apply();

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
            Main.graphics.GraphicsDevice.RasterizerState = originalState;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin();
        }

        public static Vector2 GetDrawMartixParameter(Texture2D texture, Vector2 position, float rotation = 0, float scale = 1, float angleH = 0, float angleV = 0)
        {
            float TextureHalfDiagonalDistance = texture.Size().Length() / 2f * scale;
            return MartixTrans(new Vector2((float)Math.Cos(0 + rotation + MathHelper.PiOver4), (float)Math.Sin(0 + rotation + MathHelper.PiOver4)) * TextureHalfDiagonalDistance, angleH, angleV) * (texture.Width * scale / 2f / TextureHalfDiagonalDistance) + position;
        }

        public static void DrawMagicLighting(SpriteBatch spriteBatch, float time, Texture2D texture, Vector2 position, float scale = 1, float angleH = 0, float angleV = 0, float offest = 0, int Length = 100)
        {
            List<CustomVertexInfo> bars = new List<CustomVertexInfo>();

            // 把所有的点都生成出来，按照顺序
            for (int i = 0; i < Length; ++i)
            {
                //Main.spriteBatch.Draw(Main.magicPixel, oldPosi[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                int width = 10;
                var normalDir = GetDrawMartixParameter(texture, position, i * MathHelper.TwoPi / Length + 1, scale, angleH, angleV) - GetDrawMartixParameter(texture, position, i * MathHelper.TwoPi / Length, scale, angleH, angleV);
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)Length;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                Random random = new Random();
                Vector2 Center = GetDrawMartixParameter(texture, position, i * MathHelper.TwoPi / Length, scale * random.Next((int)(100 - offest * 100), (int)(100 + offest * 100)) / 100f, angleH, angleV);
                bars.Add(new CustomVertexInfo(Center + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new CustomVertexInfo(Center + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

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


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                // 干掉注释掉就可以只显示三角形栅格
                //RasterizerState rasterizerState = new RasterizerState();
                //rasterizerState.CullMode = CullMode.None;
                //rasterizerState.FillMode = FillMode.WireFrame;
                //Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));
                Texture2D LightingTex = (Texture2D)ModContent.Request<Texture2D>(AssetUtils.Extra + "Extra_194").Value;
                //// 把变换和所需信息丢给shader
                Effect AltarTransform = ModContent.Request<Effect>("MysteriousAlchemy/Effects/AltarTransform").Value;
                //AltarTransform.Parameters["UTransform"].SetValue(model * Main.Transform * projection);
                AltarTransform.Parameters["timer"].SetValue(time);
                AltarTransform.Parameters["alpha"].SetValue(1);
                Main.graphics.GraphicsDevice.Textures[0] = LightingTex;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                AltarTransform.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin();

            }
        }

        public static void DrawProjectileTrail(Vector2[] trailArray, Texture2D TrailShapeTex, Texture2D TrailMaskTex, Texture2D MainColorTex, float width = 20, float _shapeRowTime = 0)
        {
            List<CustomVertexInfo> bars = new List<CustomVertexInfo>();

            // 把所有的点都生成出来，按照顺序
            for (int i = 1; i < trailArray.Length; ++i)
            {
                if (trailArray[i] == Vector2.Zero) break;
                //Main.spriteBatch.Draw(Main.magicPixel, oldPosi[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                var normalDir = trailArray[i - 1] - trailArray[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));

                var factor = i / (float)trailArray.Length;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);

                bars.Add(new CustomVertexInfo(trailArray[i] + normalDir * width, color, new Vector3((float)Math.Sqrt(factor), 1, w)));
                bars.Add(new CustomVertexInfo(trailArray[i] + normalDir * -width, color, new Vector3((float)Math.Sqrt(factor), 0, w)));
            }

            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

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


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                Effect trail = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Trail").Value;
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));

                // 把变换和所需信息丢给shader
                trail.Parameters["uTransform"].SetValue(model * Main.Transform * projection);
                trail.Parameters["ShapeRowTime"].SetValue(_shapeRowTime);
                Main.graphics.GraphicsDevice.Textures[0] = TrailShapeTex;
                Main.graphics.GraphicsDevice.Textures[1] = TrailMaskTex;
                Main.graphics.GraphicsDevice.Textures[2] = MainColorTex;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                trail.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin();
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }



        }

        public static void DrawTrail(Vector2[] trailUpVertex, Vector2[] trailDownVertex, Texture2D TrailShapeTex, Texture2D TrailMaskTex, Texture2D MainColorTex, float _enhanceScale = 1, float _shapeRowTime = 0)
        {
            List<CustomVertexInfo> bars = new List<CustomVertexInfo>();

            // 把所有的点都生成出来，按照顺序
            for (int i = 1; i < trailUpVertex.Length; ++i)
            {
                if (trailUpVertex[i] == Vector2.Zero)
                {
                    break;
                }
                if (trailDownVertex[i] == Vector2.Zero) break;
                //Main.spriteBatch.Draw(Main.magicPixel, oldPosi[i] - Main.screenPosition,
                //    new Rectangle(0, 0, 1, 1), Color.White, 0f, new Vector2(0.5f, 0.5f), 5f, SpriteEffects.None, 0f);

                var factor = i / (float)trailUpVertex.Length;
                var color = Color.Lerp(Color.White, Color.Red, factor);
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                bars.Add(new CustomVertexInfo(trailUpVertex[i], Color.White, new Vector3(factor, 1, 1)));
                bars.Add(new CustomVertexInfo(trailDownVertex[i], Color.White, new Vector3(factor, 0, 1)));
            }

            List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();

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


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                RasterizerState originalState = Main.graphics.GraphicsDevice.RasterizerState;
                Effect trail = ModContent.Request<Effect>("MysteriousAlchemy/Effects/Trail").Value;
                var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
                var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0));

                // 把变换和所需信息丢给shader
                trail.Parameters["uTransform"].SetValue(model * Main.Transform * projection);
                trail.Parameters["ShapeRowTime"].SetValue(_shapeRowTime);
                trail.Parameters["enhanceScale"].SetValue(_enhanceScale);
                Main.graphics.GraphicsDevice.Textures[0] = TrailShapeTex;
                Main.graphics.GraphicsDevice.Textures[1] = TrailMaskTex;
                Main.graphics.GraphicsDevice.Textures[2] = MainColorTex;
                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointWrap;
                Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointWrap;
                trail.CurrentTechnique.Passes[0].Apply();


                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, triangleList.ToArray(), 0, triangleList.Count / 3);
                Main.graphics.GraphicsDevice.RasterizerState = originalState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }

        public static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawPos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            Texture2D value = TextureAssets.Extra[98].Value;
            Color color = shineColor * opacity * 0.5f;
            color.A = 0;
            Vector2 origin = value.Size() / 2f;
            Color color2 = drawColor * 0.5f;
            float num = MathUtils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * MathUtils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 vector = new Vector2(fatness.X * 0.5f, scale.X) * num;
            Vector2 vector2 = new Vector2(fatness.Y * 0.5f, scale.Y) * num;
            color *= num;
            color2 *= num;
            Main.EntitySpriteDraw(value, drawPos, null, color, (float)Math.PI / 2f + rotation, origin, vector, dir, 0);
            Main.EntitySpriteDraw(value, drawPos, null, color, 0f + rotation, origin, vector2, dir, 0);
            Main.EntitySpriteDraw(value, drawPos, null, color2, (float)Math.PI / 2f + rotation, origin, vector * 0.6f, dir, 0);
            Main.EntitySpriteDraw(value, drawPos, null, color2, 0f + rotation, origin, vector2 * 0.6f, dir, 0);
        }

        public static void DrawPrettyLine(float opacity, SpriteEffects dir, Vector2 drawPos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, float scale, Vector2 fatness)
        {
            Texture2D value = TextureAssets.Extra[98].Value;
            Color color = shineColor * opacity * 0.5f;
            color.A = 0;
            Vector2 origin = value.Size() / 2f;
            Color color2 = drawColor * 0.5f;
            float num = Terraria.Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Terraria.Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
            Vector2 vector = new Vector2(fatness.X * 0.5f, scale) * num;
            color *= num;
            color2 *= num;
            Main.EntitySpriteDraw(value, drawPos, null, color, (float)Math.PI / 2f + rotation, origin, vector, dir, 0);
            Main.EntitySpriteDraw(value, drawPos, null, color2, (float)Math.PI / 2f + rotation, origin, vector * 0.6f, dir, 0);
        }
    }
    struct CustomVertexInfo : IVertexType
    {
        private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement[3]
        {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
        });
        public Vector2 Position;
        public Color Color;
        public Vector3 TexCoord;

        public CustomVertexInfo(Vector2 position, Color color, Vector3 texCoord)
        {
            this.Position = position;
            this.Color = color;
            this.TexCoord = texCoord;
        }

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return _vertexDeclaration;
            }
        }

    }
}