﻿using MysteriousAlchemy.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.RGB;

namespace MysteriousAlchemy.Core.Abstract
{
    public abstract class StateMachine : IStateMachine
    {
        public IState CurrectState { get; set; }
        public Dictionary<string, IState> States { get; set; }
        /// <summary>
        /// 注册所有方法
        /// </summary>
        public virtual void Initialize()
        {

        }

        public virtual void RegisterState<T>(T state) where T : IState
        {
            if (States == null)
                States = new Dictionary<string, IState>();
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
            CurrectState.ExitState(this);
            if (!States.ContainsKey(Name)) throw new ArgumentException("该状态并不存在");
            States[Name].EntryState(this);
            SetState(Name);
        }

        public void AI()
        {
            CurrectState?.OnState(this);
        }

        public StateMachine()
        {
            Initialize();
        }
    }
}
