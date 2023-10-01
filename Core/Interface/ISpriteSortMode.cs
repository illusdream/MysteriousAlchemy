using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    public interface ISpriteSortMode
    {
        //只用default和immediate
        public SpriteSortMode Sort { get; set; }
    }
}
