using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Utils
{
    public class ProjectileUtils
    {
        /// <summary>
        /// 获取对应类型的弹幕
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modProjectile"></param>
        /// <returns></returns>
        public static T GetProjectileAs<T>(ModProjectile modProjectile) where T : ModProjectile
        {
            if (modProjectile is T)
                return modProjectile as T;
            return null;
        }
    }
}
