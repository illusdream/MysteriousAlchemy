using MysteriousAlchemy.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Abstract
{
    public abstract class StateMachine : IStateMachine
    {
        public IState CurrectState { get; set; }
        public Dictionary<string, IState> States { get; set; }
        /// <summary>
        /// 注册所有方法
        /// </summary>
        public void Initialize()
        {

        }

        public void RegisterState<T>(T state) where T : IState
        {
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
            CurrectState.ExitState();
            if (!States.ContainsKey(name)) throw new ArgumentException("该状态并不存在");
            States[name].EntryState();
            SetState<T>();
        }

        public void AI()
        {
            CurrectState?.OnState();
        }

        public StateMachine()
        {
            Initialize();
        }
    }
}
