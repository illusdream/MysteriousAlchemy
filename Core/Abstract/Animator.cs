using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Abstract
{
    public class Animator : IStateMachine
    {
        /// <summary>
        /// 通用计时器
        /// </summary>
        public int Timer;
        public IState CurrectState { get; set; }
        public Dictionary<string, IState> States { get; set; }
        //存储所有绘制元素，记得在Initialize内添加他们;
        public List<DrawUnit> drawUnits;
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
        }
        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="spriteBatch"></param>
        #region //绘制drawunits
        public virtual void NoShaderDraw_AlphaBlend(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
                {
                    drawUnit.Draw(spriteBatch);
                }
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
                {
                    drawUnit.Draw(spriteBatch);
                }
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void NoShaderDraw_Additive(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
                {
                    drawUnit.Draw(spriteBatch);
                }
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
                {
                    drawUnit.Draw(spriteBatch);
                }
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void NoShaderDraw_NonPremultiplied(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
                {
                    drawUnit.Draw(spriteBatch);
                }
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
                {
                    drawUnit.Draw(spriteBatch);
                }
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && !drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void ShaderDraw_AlphaBlend(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Front)
                {
                    drawUnit.Draw(spriteBatch);
                }
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Middle)
                {
                    drawUnit.Draw(spriteBatch);
                }
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.AlphaBlend && drawUnit.UseShader && drawUnit.DrawSortWithUnits == Enum.DrawSortWithUnits.Behind)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
        }
        public virtual void ShaderDraw__Additive(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.Additive && drawUnit.UseShader)
                {
                    switch (drawUnit.DrawSortWithUnits)
                    {
                        case Enum.DrawSortWithUnits.Front:
                            drawUnit.Draw(spriteBatch);
                            break;
                        case Enum.DrawSortWithUnits.Middle:
                            drawUnit.Draw(spriteBatch);
                            break;
                        case Enum.DrawSortWithUnits.Behind:
                            drawUnit.Draw(spriteBatch);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public virtual void ShaderDraw_NonPremultiplied(SpriteBatch spriteBatch)
        {
            if (drawUnits == null)
                return;
            foreach (var drawUnit in drawUnits)
            {
                if (drawUnit.ModifyBlendState == Enum.ModifyBlendState.NonPremultiplied && drawUnit.UseShader)
                {
                    drawUnit.Draw(spriteBatch);
                }
            }
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

        public Animator()
        {
            Initialize();
        }
    }
}
