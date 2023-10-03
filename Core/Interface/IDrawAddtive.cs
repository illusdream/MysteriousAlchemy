using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    public interface IDrawAddtive : ISpriteSortMode
    {
        /// <summary>
        /// Addtive绘制模式
        /// </summary>
        public void DrawAddtive(SpriteBatch spriteBatch);
    }
}
