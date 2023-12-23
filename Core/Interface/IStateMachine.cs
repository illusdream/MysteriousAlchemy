using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    /// <summary>
    /// 状态机接口
    /// </summary>
    public interface IStateMachine
    {
        /// <summary>
        /// 状态机当前状态
        /// </summary>
        public IState CurrectState { get; set; }
        /// <summary>
        /// 全部状态,默认key为State类名
        /// </summary>
        public Dictionary<string, IState> States { get; set; }
        /// <summary>
        /// 切换状态，顺序：BeforeState.ExitState ->NextState.EntryState->SetState
        /// </summary>
        public void SwitchState(string Name);
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state"></param>
        public void SetState(string Name);

        public void RegisterState<T>(T state) where T : IState;

        /// <summary>
        /// 初始化状态机
        /// </summary>
        public void Initialize();
        /// <summary>
        /// 执行当前状态的OnState
        /// </summary>
        public void AI();
    }
}
