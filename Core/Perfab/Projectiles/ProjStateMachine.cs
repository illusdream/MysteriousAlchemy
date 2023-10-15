using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Perfab.Projectiles
{
    public abstract class ProjStateMachine : ProjWithPlayer, IStateMachine
    {
        public override string Texture => AssetUtils.Projectiles + Name;
        public IState CurrectState { get; set; }
        public Dictionary<string, IState> States { get; set; }

        public virtual void Initialize()
        {

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
        public override void OnSpawn(IEntitySource source)
        {

            base.OnSpawn(source);
            Initialize();
        }
        public override void AI()
        {
            CurrectState?.OnState(this);
            base.AI();
        }
    }
    public class ProjState<T> : IState where T : ProjStateMachine
    {
        public T Projectile;
        public ProjState(ProjStateMachine projStateMachine)
        {
            Projectile = projStateMachine as T;
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