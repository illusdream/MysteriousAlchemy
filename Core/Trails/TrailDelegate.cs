using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Trails
{
    public delegate float TrailWidthModify(float factorAlongTrail);

    public delegate Color TrailColorModify(Vector2 textureCoordinates);
}
