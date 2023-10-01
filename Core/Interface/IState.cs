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
        public void EntryState();
        //在该状态里该做的事
        public void OnState();
        //退出该状态
        public void ExitState();
    }
}
