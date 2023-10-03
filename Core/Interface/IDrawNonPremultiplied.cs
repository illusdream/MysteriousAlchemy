using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    public interface IDrawNonPremultiplied : ISpriteSortMode
    {
        /// <summary>
        /// NonPremultiplied绘制模式
        /// </summary>
        public void DrawNonPremultiplied(SpriteBatch spriteBatch);
    }
}
