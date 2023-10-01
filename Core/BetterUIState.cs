using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace MysteriousAlchemy.Core
{
    public abstract class BetterUIState : UIState
    {
        /// <summary>
        /// 是否可见
        /// </summary>
        public virtual bool Visable { get; set; } = false;
        /// <summary>
        /// 用于描述UI显示在哪个图层上
        /// </summary>
        /// <param name="layers"></param>
        /// <returns></returns>
        public abstract int UILayer(List<GameInterfaceLayer> layers);
        /// <summary>
        /// 缩放
        /// </summary>
        public virtual InterfaceScaleType Scale { get; set; } = InterfaceScaleType.UI;

        public virtual void Unload() { }
    }
}
