using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    //特效实体的基类，扭曲的风，残留刀光等等
    public class IVisualEffectEntity
    {
        public bool active { get; set; }

        public Vector2 position { get; set; }
    }
}
