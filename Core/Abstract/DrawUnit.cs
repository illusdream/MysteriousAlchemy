using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MysteriousAlchemy.Core.Abstract
{
    /// <summary>
    /// 一个绘制基类，主要用来记录绘制元素的基础信息，并绘制内容
    /// 需要保存图片，shader属于后处理，用委托注入
    /// 如果存在需要使用的shader，需要begin end 一遍（比较麻烦，要不专门写个字段，用于将用shader和不用shader分开来）
    /// 用shader的可以直接apply而不用end，begin
    /// </summary>
    public class DrawUnit
    {

        public Texture2D Texture;

        public Vector2 Pivot;

        public Vector2 Offest;
        public Vector2 PositionInScreen { get { return DrawUtils.ToScreenPosition(Pivot + Offest); } }

        public bool active;

        public DrawMode DrawMode;
        //混合模式
        public ModifyBlendState ModifyBlendState;
        //判断这个元素该在其他元素前面还是后面绘制
        public DrawSortWithUnits DrawSortWithUnits;
        /// <summary>
        /// 判断
        /// </summary>
        #region //xml原版绘制相关参数
        //暂时不想写到顶点绘制里（貌似没什么大用，有用的时候再说
        public Vector2 Origin;
        //由于多出一个沿对角线反转，不合并了
        public SpriteEffects SpriteEffect;
        public float layers = 0;
        #endregion

        #region //自定义的3d绘制,origin始终在中心
        public float AngleH;
        public float AngleV;
        public ModifySpriteEffect ModifySpriteEffect;
        #endregion


        #region //通用参数
        public Rectangle SourceRectangle;
        public Color color;
        public float Rotation;
        public Vector2 Scale;
        #endregion

        public bool UseShader;
        public Action<DrawUnit> ShaderAciton;

        //委托注入对应的行为
        public Action<DrawUnit> UpdateAction;

        public int OldPositionLength;
        public Vector2[] OldPosition;

        public int OldRotationLength;
        public float[] OldRotation;

        /// <summary>
        /// 最好永远不要使用该构造函数 ,如要创建对象，应该在对应的<see cref="Animator"/>内使用<see cref="Animator.RegisterDrawUnit{T}"/>注册新对象<br/>;
        /// 不要使用基类，用继承创建新类，在<see cref="DrawUnit.SetDefaults"/>内设定初始参数
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="drawMode"></param>
        /// <param name="modifyBlendState"></param>
        /// <param name="drawSortWithUnits"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="origin"></param>
        /// <param name="spriteEffects"></param>
        /// <param name="layer"></param>
        /// <param name="updateAction"></param>
        /// <param name="useShader"></param>
        /// <param name="shaderAction"></param>
        public DrawUnit(Texture2D texture, Vector2 position, DrawMode drawMode, ModifyBlendState modifyBlendState, DrawSortWithUnits drawSortWithUnits, Color color, float rotation, Vector2 scale, Rectangle? sourceRectangle, Vector2 origin, SpriteEffects spriteEffects, int layer, Action<DrawUnit> updateAction, bool useShader, Action<DrawUnit> shaderAction)
        {
            Texture = texture;
            Pivot = position;
            DrawMode = drawMode;
            ModifyBlendState = modifyBlendState;
            DrawSortWithUnits = drawSortWithUnits;
            this.color = color;
            Rotation = rotation;
            Scale = scale;
            SourceRectangle = (Rectangle)sourceRectangle;
            Origin = origin;
            SpriteEffect = spriteEffects;
            layers = layer;
            UpdateAction = updateAction;
            UseShader = useShader;
            ShaderAciton = shaderAction;
        }

        public DrawUnit()
        {

        }

        /// <summary>
        /// 设定初始参数
        /// </summary>
        public virtual void SetDefaults()
        {
            if (OldPositionLength != 0)
            {
                OldPosition = new Vector2[OldPositionLength];
            }
            if (OldPositionLength != 0)
            {
                OldPosition = new Vector2[OldPositionLength];
            }
        }
        //更新函数
        public virtual void Update()
        {
            UpdateAction?.Invoke(this);
            UpdateOldInfoCache();
        }

        /// <summary>
        /// 绘制
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            CheckCanDraw();

            //原版绘制
            if (DrawMode == DrawMode.Default)
            {
                ShaderAciton?.Invoke(this);
                spriteBatch.Draw(Texture, PositionInScreen, SourceRectangle, color, Rotation, Origin, Scale, SpriteEffect, layers);
            }
            else //3d绘制
            {
                //重写3d绘制得了，之前写的已经成屎山（已改完）原函数未改，哪天看到这个赶紧去改
                //DrawUtil.DrawEntityInWorld(SpriteBatch, Texture, PositionInScreen, color, Shader, Rotation, scale, AngleH, AngleV, ShaderAciton, null, 0);


                //对角线一半的长度，用来矫正位置
                Vector2 TextureHalfDiagonalLength = Texture.Size() / 2f;
                //
                float VertexPosAngleFix = -MathHelper.PiOver4 * 3;

                //按泰拉的顺时针取顶点
                Vector2 NonTransVertex_Topleft = new Vector2(MathF.Cos(Rotation + VertexPosAngleFix) * Scale.X, MathF.Sin(Rotation + VertexPosAngleFix) * Scale.Y) * TextureHalfDiagonalLength;
                Vector2 vertex_Topleft = DrawUtils.MartixTrans(NonTransVertex_Topleft, AngleH, AngleV);

                Vector2 NonTransVertex_Topright = new Vector2(MathF.Cos(Rotation + VertexPosAngleFix + MathHelper.PiOver2) * Scale.X, MathF.Sin(Rotation + VertexPosAngleFix + MathHelper.PiOver2) * Scale.Y) * TextureHalfDiagonalLength;
                Vector2 vertex_Topright = DrawUtils.MartixTrans(NonTransVertex_Topright, AngleH, AngleV);

                Vector2 NonTransVertex_Buttomright = new Vector2(MathF.Cos(Rotation + VertexPosAngleFix + MathHelper.PiOver2 * 2) * Scale.X, MathF.Sin(Rotation + VertexPosAngleFix + MathHelper.PiOver2 * 2) * Scale.Y) * TextureHalfDiagonalLength;
                Vector2 vertex_Buttomright = DrawUtils.MartixTrans(NonTransVertex_Buttomright, AngleH, AngleV);

                Vector2 NonTransVertex_Buttomleft = new Vector2(MathF.Cos(Rotation + VertexPosAngleFix + MathHelper.PiOver2 * 3) * Scale.X, MathF.Sin(Rotation + VertexPosAngleFix + MathHelper.PiOver2 * 3) * Scale.Y) * TextureHalfDiagonalLength;
                Vector2 vertex_Buttomleft = DrawUtils.MartixTrans(NonTransVertex_Buttomleft, AngleH, AngleV);


                //获取正确的顶点采样坐标
                RectangleF currectTexcoords = DrawUtils.GetCurrectRectangleInVertexPaint(SourceRectangle, Texture);
                Vector2 TopLeftTexcoord = new Vector2(currectTexcoords.X, currectTexcoords.Y);
                Vector2 TopRightTexcoord = new Vector2(currectTexcoords.X + currectTexcoords.Width, currectTexcoords.Y);
                Vector2 ButtomLeftTexcoord = new Vector2(currectTexcoords.X, currectTexcoords.Y + currectTexcoords.Height);
                Vector2 ButtomRightTexcoord = new Vector2(currectTexcoords.X + currectTexcoords.Width, currectTexcoords.Y + currectTexcoords.Height);

                List<CustomVertexInfo> triangleList = new List<CustomVertexInfo>();
                //逆时针连接各个顶点
                switch (ModifySpriteEffect)
                {
                    case ModifySpriteEffect.None:
                        //      Topleft ---- Topright
                        //          |   \        |
                        //          |      \     |
                        //          |          \ |
                        //      Buttomleft---Buttomright
                        triangleList.Add(new CustomVertexInfo(vertex_Topleft + PositionInScreen, color, new Vector3(TopLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomleft + PositionInScreen, color, new Vector3(ButtomLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomright + PositionInScreen, color, new Vector3(ButtomRightTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Topleft + PositionInScreen, color, new Vector3(TopLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomright + PositionInScreen, color, new Vector3(ButtomRightTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Topright + PositionInScreen, color, new Vector3(TopRightTexcoord, 1)));

                        break;
                    case ModifySpriteEffect.FlipHorizontally:
                        //      Topright ---- Topleft
                        //          |   \        |
                        //          |      \     |
                        //          |          \ |
                        //      Buttomright---Buttomleft
                        triangleList.Add(new CustomVertexInfo(vertex_Topright + PositionInScreen, color, new Vector3(TopLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomright + PositionInScreen, color, new Vector3(ButtomLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomleft + PositionInScreen, color, new Vector3(ButtomRightTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Topright + PositionInScreen, color, new Vector3(TopLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomleft + PositionInScreen, color, new Vector3(ButtomRightTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Topleft + PositionInScreen, color, new Vector3(TopRightTexcoord, 1)));
                        break;
                    case ModifySpriteEffect.FlipVertically:
                        //      Buttomleft ---- Buttomright
                        //          |   \        |
                        //          |      \     |
                        //          |          \ |
                        //      Topleft---------Topright
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomleft + PositionInScreen, color, new Vector3(TopLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Topleft + PositionInScreen, color, new Vector3(ButtomLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Topright + PositionInScreen, color, new Vector3(ButtomRightTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomleft + PositionInScreen, color, new Vector3(TopLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Topright + PositionInScreen, color, new Vector3(ButtomRightTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomright + PositionInScreen, color, new Vector3(TopRightTexcoord, 1)));
                        break;
                    case ModifySpriteEffect.FlipDiagonally:
                        //      Buttomright ---- Topright
                        //          |   \        |
                        //          |      \     |
                        //          |          \ |
                        //      Buttomleft---Topleft
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomright + PositionInScreen, color, new Vector3(TopLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomleft + PositionInScreen, color, new Vector3(ButtomLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Topleft + PositionInScreen, color, new Vector3(1, 1, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Buttomright + PositionInScreen, color, new Vector3(TopLeftTexcoord, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Topleft + PositionInScreen, color, new Vector3(1, 1, 1)));
                        triangleList.Add(new CustomVertexInfo(vertex_Topright + PositionInScreen, color, new Vector3(TopRightTexcoord, 1)));
                        break;
                    default:
                        break;
                }



                //绘制

                //使用shader
                ShaderAciton?.Invoke(this);

                Main.graphics.GraphicsDevice.Textures[0] = Texture;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, triangleList.ToArray(), 0, triangleList.Count - 2);

            }
        }



        /// <summary>
        /// 防止null导致地报错使动画机整体停止工作,没写完
        /// </summary>
        private void CheckCanDraw()
        {
            //防止null报错
            if (Texture == null)
            {
                return;
            }
            //初始化sourseRectangle
            if (SourceRectangle == new Rectangle())
            {
                SourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            }
        }

        private void ClearAction(ref Action<DrawUnit> action)
        {
            Delegate[] allAction = action.GetInvocationList();
            for (int i = 0; i < allAction.Length; i++)
            {
                action -= allAction[i] as Action<DrawUnit>;
            }
        }
        private void UpdateOldInfoCache()
        {
            if (OldPositionLength != 0)
            {
                for (int i = OldPositionLength - 1; i > 0; i++)
                {
                    OldPosition[i] = OldPosition[i - 1];
                }
                OldPosition[0] = Pivot + Offest;
            }
            if (OldRotationLength != 0)
            {
                for (int i = OldRotationLength - 1; i > 0; i++)
                {
                    OldRotation[i] = OldRotation[i - 1];
                }
                OldRotation[0] = Rotation;
            }
        }
    }

}
