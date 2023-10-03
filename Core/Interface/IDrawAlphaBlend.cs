using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    /// <summary>
    /// 貌似没什么用，在系统默认Draw里就可以完成，暂时不删，需要用到的时候记得在CustomDrawSystem里补上相关代码
    /// </summary>
    public interface IDrawAlphaBlend : ISpriteSortMode
    {
        /// <summary>
        /// 默认绘制模式
        /// </summary>
        public void DrawAlphaBlend();
    }
}
