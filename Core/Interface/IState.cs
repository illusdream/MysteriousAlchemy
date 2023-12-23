using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    /// <summary>
    /// 状态机状态接口
    /// </summary>
    public interface IState
    {
        //进入该状态
        public void EntryState(IStateMachine stateMachine);
        //在该状态里该做的事
        public void OnState(IStateMachine stateMachine);
        //退出该状态
        public void ExitState(IStateMachine stateMachine);

        /// <summary>
        /// 修改索引名，返回true表示修改>
        /// </summary>
        /// <param name="name">修改后的值</param>
        /// <returns></returns>
        public bool ModifyName(out string name);
    }
}
