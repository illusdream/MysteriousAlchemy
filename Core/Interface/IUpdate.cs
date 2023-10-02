using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    //需要自定义update的接口
    public interface IUpdate
    {
        /// <summary>
        /// 更新顺序
        /// </summary>
        public int UpdateIndex { get; }
        /// <summary>
        /// 更新函数
        /// </summary>
        public void Update();

        public void Load();

        public void Unload();

    }
}
