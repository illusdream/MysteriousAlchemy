using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Perfab.Projectiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements
{
    public class UIStateMachine : UIElement, IStateMachine
    {
        public IState CurrectState { get; set; }
        public Dictionary<string, IState> States { get; set; }

        public void AI()
        {
            CurrectState.OnState(this);
        }

        public void RegisterState<T>(T state) where T : IState
        {
            States ??= new Dictionary<string, IState>();
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
    }
    public class UI_State<T> : IState where T : UIStateMachine
    {
        public T UIElment;
        public UI_State(UIStateMachine UI)
        {
            UIElment = UI as T;
        }

        public virtual void EntryState(IStateMachine stateMachine)
        {

        }

        public virtual void ExitState(IStateMachine stateMachine)
        {

        }

        public virtual void OnState(IStateMachine stateMachine)
        {

        }
    }
}