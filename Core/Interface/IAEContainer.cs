using MysteriousAlchemy.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    /// <summary>
    /// 将<see cref="AlchemyEntity"/>附加到该实例类中
    /// </summary>
    internal interface IAEContainer
    {
        //最大容量
        public int Capacity { get; set; }
        /// <summary>
        /// 存储对应<see cref="IAEComponent"/>的<see cref="AlchemyUnicode"/>
        /// </summary>
        public List<AlchemyUnicode> Unicodes { get; set; }
        public bool CanInsert();

        public void Insert(AlchemyUnicode unicode);

        public void Remove(AlchemyUnicode unicode);
    }
}
