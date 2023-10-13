using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.System;
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
        public virtual DrawSortWithPlayer DrawSortWithPlayer { get; }

        public bool active;

        public Vector2 Position;

        public Animator()
        {
            Initialize();
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
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void NoShaderDraw_Additive_Front(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void NoShaderDraw_NonPremultipliede_Front(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void ShaderDraw_AlphaBlende_Front(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void ShaderDraw_Additive_Front(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void ShaderDraw_NonPremultipliede_Front(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        #endregion
        #region //Middle
        public virtual void NoShaderDraw_AlphaBlend_Middle(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void NoShaderDraw_Additive_Middle(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void NoShaderDraw_NonPremultipliede_Middle(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void ShaderDraw_AlphaBlende_Middle(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void ShaderDraw_Additive_Middle(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
                {
                    drawUnit.Draw(spriteBatch);
                }

            }
        }
        public virtual void ShaderDraw_NonPremultipliede_Middle(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        #endregion
        #region //Behind
        public virtual void NoShaderDraw_AlphaBlend_Behind(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void NoShaderDraw_Additive_Behind(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void NoShaderDraw_NonPremultipliede_Behind(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void ShaderDraw_AlphaBlende_Behind(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void ShaderDraw_Additive_Behind(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void ShaderDraw_NonPremultipliede_Behind(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        #endregion

        public void NoShaderDraw_Front(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            NoShaderDraw_AlphaBlend_Front(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            NoShaderDraw_Additive_Front(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            NoShaderDraw_NonPremultipliede_Front(spriteBatch);
            spriteBatch.End();
        }
        public void NoShaderDraw_Middle(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            NoShaderDraw_AlphaBlend_Middle(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            NoShaderDraw_Additive_Middle(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            NoShaderDraw_NonPremultipliede_Middle(spriteBatch);
            spriteBatch.End();
        }
        public void NoShaderDraw_Behind(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            NoShaderDraw_AlphaBlend_Behind(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            NoShaderDraw_Additive_Behind(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);
            NoShaderDraw_NonPremultipliede_Behind(spriteBatch);
            spriteBatch.End();
        }
        public void ShaderDraw_Front(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ShaderDraw_AlphaBlende_Front(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ShaderDraw_Additive_Front(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ShaderDraw_NonPremultipliede_Front(spriteBatch);
            spriteBatch.End();
        }
        public void ShaderDraw_Middle(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ShaderDraw_AlphaBlende_Middle(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ShaderDraw_Additive_Middle(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ShaderDraw_NonPremultipliede_Middle(spriteBatch);
            spriteBatch.End();
        }
        public void ShaderDraw_Behind(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ShaderDraw_AlphaBlende_Behind(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ShaderDraw_Additive_Behind(spriteBatch);
            spriteBatch.End();


            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            ShaderDraw_NonPremultipliede_Behind(spriteBatch);
            spriteBatch.End();
        }
        #endregion
        public virtual void Initialize()
        {
            drawUnits = new List<DrawUnit>();
        }

        public void RegisterState<T>(T state) where T : IState
        {
            if (States == null)
                States = new Dictionary<string, IState>();
            if (States.ContainsKey(typeof(T).ToString()))
                throw new ArgumentException("已被注册");
            States.Add(typeof(T).ToString(), state);
        }

        public void SetState<T>() where T : IState
        {
            var name = typeof(T).ToString();
            if (!States.ContainsKey(name)) throw new ArgumentException("该状态并不存在");
            if (States.ContainsKey(name))
                CurrectState = States[name];
        }

        public void SwitchState<T>() where T : IState
        {
            var name = typeof(T).ToString();
            CurrectState.ExitState(this);
            if (!States.ContainsKey(name)) throw new ArgumentException("该状态并不存在");
            States[name].EntryState(this);
            SetState<T>();
        }


        /// <summary>
        /// 不要直接用构造函数add到Manager里！！！！！
        /// </summary>


        public T RegisterDrawUnit<T>() where T : DrawUnit, new()
        {
            T instance = new T();
            instance.SetDefaults();
            instance.active = true;
            drawUnits.Add(instance);
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
            Vector2 _scale = DrawUtils.GetCurrectScale(instance.Texture, scale);
            instance.Scale = _scale;
            return instance;
        }
        public T RegisterDrawUnit<T>(Vector2 pivot) where T : DrawUnit, new()
        {
            T instance = new T();

            instance.Pivot = pivot;
            instance.SetDefaults();
            instance.active = true;
            drawUnits.Add(instance);

            return instance;
        }
        public int RegisterDrawUnitOutInt<T>() where T : DrawUnit, new()
        {
            T instance = new T();
            instance.SetDefaults();
            instance.active = true;
            int index = drawUnits.Count;
            drawUnits.Add(instance);
            return index;
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
                    }
                }
            }
        }
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
