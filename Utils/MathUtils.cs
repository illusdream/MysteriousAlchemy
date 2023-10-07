using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Utils
{
    public class MathUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="stepScale"></param>
        /// <returns></returns>
        public static Vector2 SteppingTrack(Vector2 origin, Vector2 target, float stepScale)
        {
            Vector2 distance = target - origin;
            Vector2 steppingVec = distance * stepScale;
            return origin + steppingVec;
        }
    }
}
