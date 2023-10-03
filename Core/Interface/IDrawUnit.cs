using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    /// <summary>
    /// 动画机用绘制接口，除绘制模式外，可能只使用immediate绘制
    /// </summary>
    public interface IDrawUnit : IDrawAlphaBlend, IDrawAddtive, IDrawNonPremultiplied
    {
    }
}
