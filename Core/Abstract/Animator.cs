using log4net.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Core.Abstract
{
    public class Animator : IStateMachine
    {
        // 通用计时器
        public int Timer;
        public IState CurrectState { get; set; }
        public Dictionary<string, IState> States { get; set; }
        //存储所有绘制元素，记得在Initialize内添加他们;
        public List<DrawUnit> drawUnits;

        #region 将DrawUnit按绘制顺序分别存储，内存翻倍，但是好处是少了大量遍历，如果绘制元素多，优化应该比较明显
        public List<DrawUnit> Default_Behind_AlphaBlend;
        public List<DrawUnit> Default_Behind_Additive;
        public List<DrawUnit> Default_Behind_NonPremultiplied;
        public List<DrawUnit> Immediate_Behind_AlphaBlend;
        public List<DrawUnit> Immediate_Behind_Additive;
        public List<DrawUnit> Immediate_Behind_NonPremultiplied;

        public List<DrawUnit> Default_Middle_AlphaBlend;
        public List<DrawUnit> Default_Middle_Additive;
        public List<DrawUnit> Default_Middle_NonPremultiplied;
        public List<DrawUnit> Immediate_Middle_AlphaBlend;
        public List<DrawUnit> Immediate_Middle_Additive;
        public List<DrawUnit> Immediate_Middle_NonPremultiplied;

        public List<DrawUnit> Default_Front_AlphaBlend;
        public List<DrawUnit> Default_Front_Additive;
        public List<DrawUnit> Default_Front_NonPremultiplied;
        public List<DrawUnit> Immediate_Front_AlphaBlend;
        public List<DrawUnit> Immediate_Front_Additive;
        public List<DrawUnit> Immediate_Front_NonPremultiplied;
        #endregion
        public virtual DrawSortWithPlayer DrawSortWithPlayer { get; }

        public bool active;

        public Vector2 Position;
        public string Name;
        /// <summary>
        /// 不要直接用构造函数add到Manager里！！！！！
        /// </summary>
        public Animator()
        {
            drawUnits = new List<DrawUnit>();
            InitDrawUnitLists();
            Initialize();

        }
        public Animator(Vector2 Position)
        {
            drawUnits = new List<DrawUnit>();
            InitDrawUnitLists();
            Initialize();
            this.Position = Position;
        }
        protected void InitDrawUnitLists()
        {
            Default_Behind_Additive = new List<DrawUnit>();
            Default_Behind_AlphaBlend = new List<DrawUnit>();
            Default_Behind_NonPremultiplied = new List<DrawUnit>();
            Immediate_Behind_Additive = new List<DrawUnit>();
            Immediate_Behind_AlphaBlend = new List<DrawUnit>();
            Immediate_Behind_NonPremultiplied = new List<DrawUnit>();


            Default_Middle_AlphaBlend = new List<DrawUnit>();
            Default_Middle_Additive = new List<DrawUnit>();
            Default_Middle_NonPremultiplied = new List<DrawUnit>();
            Immediate_Middle_AlphaBlend = new List<DrawUnit>();
            Immediate_Middle_Additive = new List<DrawUnit>();
            Immediate_Middle_NonPremultiplied = new List<DrawUnit>();

            Default_Front_AlphaBlend = new List<DrawUnit>();
            Default_Front_Additive = new List<DrawUnit>();
            Default_Front_NonPremultiplied = new List<DrawUnit>();
            Immediate_Front_AlphaBlend = new List<DrawUnit>();
            Immediate_Front_Additive = new List<DrawUnit>();
            Immediate_Front_NonPremultiplied = new List<DrawUnit>();
        }

        public virtual void InitStartDrawUnit()
        {

        }
        /// <summary>
        /// 更新，update
        /// </summary>
        public virtual void AI()
        {

            CurrectState?.OnState(this);
            if (drawUnits != null)
            {
                foreach (var unit in drawUnits)
                {
                    unit.Update();
                }
            }
            ClearDeactiveDrawUnit();
        }
        #region //绘制drawunits
        #region //Front绘制
        public virtual void NoShaderDraw_AlphaBlend_Front(SpriteBatch spriteBatch)
        {
            if (Default_Front_AlphaBlend == null)
                return;
            foreach (var drawUnit in Default_Front_AlphaBlend)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void NoShaderDraw_Additive_Front(SpriteBatch spriteBatch)
        {
            if (Default_Front_Additive == null)
                return;
            foreach (var drawUnit in Default_Front_Additive)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void NoShaderDraw_NonPremultipliede_Front(SpriteBatch spriteBatch)
        {
            if (Default_Front_NonPremultiplied == null)
                return;
            foreach (var drawUnit in Default_Front_NonPremultiplied)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void ShaderDraw_AlphaBlend_Front(SpriteBatch spriteBatch)
        {
            if (Immediate_Front_AlphaBlend == null)
                return;
            foreach (var drawUnit in Immediate_Front_AlphaBlend)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void ShaderDraw_Additive_Front(SpriteBatch spriteBatch)
        {
            if (Immediate_Front_Additive == null)
                return;
            foreach (var drawUnit in Immediate_Front_Additive)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void ShaderDraw_NonPremultipliede_Front(SpriteBatch spriteBatch)
        {
            if (Immediate_Front_NonPremultiplied == null)
                return;
            foreach (var drawUnit in Immediate_Front_NonPremultiplied)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        #endregion
        #region //Middle
        public virtual void NoShaderDraw_AlphaBlend_Middle(SpriteBatch spriteBatch)
        {
            if (Default_Middle_AlphaBlend == null)
                return;
            foreach (var drawUnit in Default_Middle_AlphaBlend)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void NoShaderDraw_Additive_Middle(SpriteBatch spriteBatch)
        {
            if (Default_Middle_Additive == null)
                return;
            foreach (var drawUnit in Default_Middle_Additive)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void NoShaderDraw_NonPremultipliede_Middle(SpriteBatch spriteBatch)
        {
            if (Default_Middle_NonPremultiplied == null)
                return;
            foreach (var drawUnit in Default_Middle_NonPremultiplied)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void ShaderDraw_AlphaBlend_Middle(SpriteBatch spriteBatch)
        {
            if (Immediate_Middle_AlphaBlend == null)
                return;
            foreach (var drawUnit in Immediate_Middle_AlphaBlend)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void ShaderDraw_Additive_Middle(SpriteBatch spriteBatch)
        {
            if (Immediate_Middle_Additive == null)
                return;
            foreach (var drawUnit in Immediate_Middle_Additive)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void ShaderDraw_NonPremultipliede_Middle(SpriteBatch spriteBatch)
        {
            if (Immediate_Middle_NonPremultiplied == null)
                return;
            foreach (var drawUnit in Immediate_Middle_NonPremultiplied)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        #endregion
        #region //Behind
        public virtual void NoShaderDraw_AlphaBlend_Behind(SpriteBatch spriteBatch)
        {
            if (Default_Behind_AlphaBlend == null)
                return;
            foreach (var drawUnit in Default_Behind_AlphaBlend)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void NoShaderDraw_Additive_Behind(SpriteBatch spriteBatch)
        {
            if (Default_Behind_Additive == null)
                return;
            foreach (var drawUnit in Default_Behind_Additive)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void NoShaderDraw_NonPremultipliede_Behind(SpriteBatch spriteBatch)
        {
            if (Default_Behind_NonPremultiplied == null)
                return;
            foreach (var drawUnit in Default_Behind_NonPremultiplied)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void ShaderDraw_AlphaBlende_Behind(SpriteBatch spriteBatch)
        {
            if (Immediate_Behind_AlphaBlend == null)
                return;
            foreach (var drawUnit in Immediate_Behind_AlphaBlend)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void ShaderDraw_Additive_Behind(SpriteBatch spriteBatch)
        {
            if (Immediate_Behind_Additive == null)
                return;
            foreach (var drawUnit in Immediate_Behind_Additive)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        public virtual void ShaderDraw_NonPremultipliede_Behind(SpriteBatch spriteBatch)
        {
            if (Immediate_Behind_NonPremultiplied == null)
                return;
            foreach (var drawUnit in Immediate_Behind_NonPremultiplied)
            {
                drawUnit.Draw(spriteBatch);
            }
        }
        #endregion

        public void NoShaderDraw_Front(SpriteBatch spriteBatch)
        {
            BlendState blendState = spriteBatch.GraphicsDevice.BlendState;
            spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            NoShaderDraw_AlphaBlend_Front(spriteBatch);


            spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;
            NoShaderDraw_Additive_Front(spriteBatch);


            spriteBatch.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            NoShaderDraw_NonPremultipliede_Front(spriteBatch);
            spriteBatch.GraphicsDevice.BlendState = blendState;
        }
        public void NoShaderDraw_Middle(SpriteBatch spriteBatch)
        {
            BlendState blendState = spriteBatch.GraphicsDevice.BlendState;
            spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            NoShaderDraw_AlphaBlend_Middle(spriteBatch);



            spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;
            NoShaderDraw_Additive_Middle(spriteBatch);


            spriteBatch.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            NoShaderDraw_NonPremultipliede_Middle(spriteBatch);
            spriteBatch.GraphicsDevice.BlendState = blendState;
        }
        public void NoShaderDraw_Behind(SpriteBatch spriteBatch)
        {
            BlendState blendState = spriteBatch.GraphicsDevice.BlendState;
            spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            NoShaderDraw_AlphaBlend_Behind(spriteBatch);


            spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;
            NoShaderDraw_Additive_Behind(spriteBatch);




            spriteBatch.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            NoShaderDraw_NonPremultipliede_Behind(spriteBatch);
            spriteBatch.GraphicsDevice.BlendState = blendState;
        }
        public void ShaderDraw_Front(SpriteBatch spriteBatch)
        {
            BlendState blendState = spriteBatch.GraphicsDevice.BlendState;
            spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            ShaderDraw_AlphaBlend_Front(spriteBatch);


            spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;
            ShaderDraw_Additive_Front(spriteBatch);


            spriteBatch.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            ShaderDraw_NonPremultipliede_Front(spriteBatch);
            spriteBatch.GraphicsDevice.BlendState = blendState;
        }
        public void ShaderDraw_Middle(SpriteBatch spriteBatch)
        {
            BlendState blendState = spriteBatch.GraphicsDevice.BlendState;
            spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            ShaderDraw_AlphaBlend_Middle(spriteBatch);




            spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;
            ShaderDraw_Additive_Middle(spriteBatch);



            spriteBatch.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            ShaderDraw_NonPremultipliede_Middle(spriteBatch);
            spriteBatch.GraphicsDevice.BlendState = blendState;
        }
        public void ShaderDraw_Behind(SpriteBatch spriteBatch)
        {
            BlendState blendState = spriteBatch.GraphicsDevice.BlendState;
            spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            ShaderDraw_AlphaBlende_Behind(spriteBatch);


            spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;
            ShaderDraw_Additive_Behind(spriteBatch);


            spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;
            ShaderDraw_NonPremultipliede_Behind(spriteBatch);
            spriteBatch.GraphicsDevice.BlendState = blendState;
        }
        #endregion
        public virtual void Initialize()
        {

        }

        #region 状态机相关
        public void RegisterState<T>(T state) where T : IState
        {
            States ??= new Dictionary<string, IState>();
            if (state.ModifyName(out string name))
            {
                if (States.ContainsKey(name))
                    throw new ArgumentException("已被注册");
                States.Add(name, state);
                return;
            }
            if (States.ContainsKey(typeof(T).ToString()))
                throw new ArgumentException("已被注册");
            States.Add(typeof(T).ToString(), state);
        }

        public void SetState(string Name)
        {
            if (!States.ContainsKey(Name)) throw new ArgumentException("该状态并不存在");
            if (States.ContainsKey(Name))
                CurrectState = States[Name];
        }

        public void SwitchState(string Name)
        {
            CurrectState?.ExitState(this);
            if (!States.ContainsKey(Name)) throw new ArgumentException("该状态并不存在");
            States[Name]?.EntryState(this);
            SetState(Name);
        }
        #endregion


        #region DrawUnit相关，添加，删除，查找需要加入，但必要性不高，常用的直接存引用不是更好？
        /// <summary>
        /// 一堆if不知道有没有优化空间
        /// </summary>
        /// <param name="drawUnit"></param>
        protected virtual void InsertDrawUnitToCurrectList(DrawUnit drawUnit)
        {
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                Default_Behind_AlphaBlend.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                Default_Behind_Additive.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                Default_Behind_NonPremultiplied.Add(drawUnit);
            }

            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                Immediate_Behind_AlphaBlend.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                Immediate_Behind_Additive.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                Immediate_Behind_NonPremultiplied.Add(drawUnit);
            }


            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                Default_Middle_AlphaBlend.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                Default_Middle_Additive.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                Default_Middle_NonPremultiplied.Add(drawUnit);
            }

            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                Immediate_Middle_AlphaBlend.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                Immediate_Middle_Additive.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                Immediate_Middle_NonPremultiplied.Add(drawUnit);
            }


            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                Default_Front_AlphaBlend.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                Default_Front_Additive.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                Default_Front_NonPremultiplied.Add(drawUnit);
            }

            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                Immediate_Front_AlphaBlend.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                Immediate_Front_Additive.Add(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                Immediate_Front_NonPremultiplied.Add(drawUnit);
            }
        }
        public T RegisterDrawUnit<T>() where T : DrawUnit, new()
        {
            T instance = new T();
            instance.SetDefaults();
            instance.active = true;
            instance.Animator = this;
            drawUnits.Add(instance);
            InsertDrawUnitToCurrectList(instance);
            return instance;
        }
        /// <summary>
        /// 已内置<see cref="DrawUtils.GetCurrectScale(Texture2D, Vector2)"/>,<see href="scale"/>只需填目标值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pivot"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public T RegisterDrawUnit<T>(Vector2 pivot, Vector2 scale) where T : DrawUnit, new()
        {
            var instance = RegisterDrawUnit<T>();
            instance.Pivot = pivot;
            Vector2 _scale = DrawUtils.GetCurrectScale(instance.TextureInstance, scale);
            instance.Scale = _scale;
            instance.Animator = this;
            return instance;
        }
        public T RegisterDrawUnit<T>(Vector2 pivot) where T : DrawUnit, new()
        {
            T instance = new T();

            instance.Pivot = pivot;
            instance.SetDefaults();
            instance.active = true;
            instance.Animator = this;
            drawUnits.Add(instance);
            InsertDrawUnitToCurrectList(instance);
            return instance;
        }
        public int RegisterDrawUnitOutInt<T>() where T : DrawUnit, new()
        {
            T instance = new T();
            instance.SetDefaults();
            instance.active = true;
            instance.Animator = this;
            int index = drawUnits.Count;
            drawUnits.Add(instance);
            InsertDrawUnitToCurrectList(instance);
            return index;
        }
        protected virtual bool RemoveDrawUnitInCurrectList(DrawUnit drawUnit)
        {
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                return Default_Behind_AlphaBlend.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                return Default_Behind_Additive.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                return Default_Behind_NonPremultiplied.Remove(drawUnit);
            }

            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                return Immediate_Behind_AlphaBlend.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                return Immediate_Behind_Additive.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
            {
                return Immediate_Behind_NonPremultiplied.Remove(drawUnit);
            }


            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                return Default_Middle_AlphaBlend.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                return Default_Middle_Additive.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                return Default_Middle_NonPremultiplied.Remove(drawUnit);
            }

            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                return Immediate_Middle_AlphaBlend.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                return Immediate_Middle_Additive.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
            {
                return Immediate_Middle_NonPremultiplied.Remove(drawUnit);
            }


            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                return Default_Front_AlphaBlend.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                return Default_Front_Additive.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                return Default_Front_NonPremultiplied.Remove(drawUnit);
            }

            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                return Immediate_Front_AlphaBlend.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                return Immediate_Front_Additive.Remove(drawUnit);
            }
            if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
            {
                return Immediate_Front_NonPremultiplied.Remove(drawUnit);
            }
            return false;
        }
        public void ClearDeactiveDrawUnit()
        {
            if (drawUnits != null)
            {
                for (int i = 0; i < drawUnits.Count; i++)
                {
                    if (!drawUnits[i].active)
                    {
                        drawUnits.Remove(drawUnits[i]);
                        RemoveDrawUnitInCurrectList(drawUnits[i]);
                    }
                }
            }
        }
        public bool RemoveDrawUnit(DrawUnit drawUnit)
        {
            return drawUnits.Remove(drawUnit) && RemoveDrawUnitInCurrectList(drawUnit);
        }
        public void RemoveAllDrawUnits()
        {
            Default_Behind_AlphaBlend.Clear();
            Default_Behind_Additive.Clear();
            Default_Behind_NonPremultiplied.Clear();
            Immediate_Behind_AlphaBlend.Clear();
            Immediate_Behind_Additive.Clear();
            Immediate_Behind_NonPremultiplied.Clear();

            Default_Middle_AlphaBlend.Clear();
            Default_Middle_Additive.Clear();
            Default_Middle_NonPremultiplied.Clear();
            Immediate_Middle_AlphaBlend.Clear();
            Immediate_Middle_Additive.Clear();
            Immediate_Middle_NonPremultiplied.Clear();

            Default_Front_AlphaBlend.Clear();
            Default_Front_Additive.Clear();
            Default_Front_NonPremultiplied.Clear();
            Immediate_Front_AlphaBlend.Clear();
            Immediate_Front_Additive.Clear();
            Immediate_Front_NonPremultiplied.Clear();


            drawUnits.Clear();
        }
        #endregion


        public void Kill()
        {
            OnKill();
            AnimatorManager.Instance.Animators.Remove(this);
        }
        public virtual void OnKill()
        {

        }

    }
}
