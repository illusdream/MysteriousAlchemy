using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Utils
{
    public static class MathUtils
    {
        public static Vector3 Vec3(this Vector2 vector) => new Vector3(vector.X, vector.Y, 0);
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
        public static float SteppingTrack(float origin, float target, float stepScale)
        {
            float distance = target - origin;
            float steppingValue = distance * stepScale;
            return origin + steppingValue;
        }
        public static T[] FastUnion<T>(this T[] front, T[] back)
        {
            T[] combined = new T[front.Length + back.Length];

            Array.Copy(front, combined, front.Length);
            Array.Copy(back, 0, combined, front.Length, back.Length);

            return combined;
        }
        public static float GetLerpValue(float from, float to, float t, bool clamped = false)
        {
            if (clamped)
            {
                if (from < to)
                {
                    if (t < from)
                    {
                        return 0f;
                    }

                    if (t > to)
                    {
                        return 1f;
                    }
                }
                else
                {
                    if (t < to)
                    {
                        return 1f;
                    }

                    if (t > from)
                    {
                        return 0f;
                    }
                }
            }

            return (t - from) / (to - from);
        }
        public static Vector2 GetVector2InCircle(float angle, float radium)
        {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * radium;
        }
        public static Vector2 RandomRing(Vector2 circleScale, float min, float max)
        {
            float randomIn2PI = Main.rand.NextFloat() * MathHelper.TwoPi;
            float randomInRing = Main.rand.NextFloat();
            Vector2 target = GetVector2InCircle(randomIn2PI, min + (max - min) * randomInRing);
            return target;
        }
        public static Vector2 RandomRingRange(float StartAngle, float EndAngle, float min, float max)
        {
            float randomInRange = Main.rand.NextFloat(StartAngle, EndAngle);
            float randomInRing = Main.rand.NextFloat();
            Vector2 target = GetVector2InCircle(randomInRange, min + (max - min) * randomInRing);
            return target;
        }
        public static float GetTime(float scale)
        {
            return (float)Main.time * scale;
        }
        public static Vector2 LerpVelocity(Vector2 origin, Vector2 target, float scale)
        {
            return (1 - scale) * origin + (target) * scale;
        }
    }
}
