using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    public interface IDrawAlphaBlend : ISpriteSortMode
    {
        /// <summary>
        /// 默认绘制模式
        /// </summary>
        public void DrawAlphaBlend();
    }
}
