using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Utils
{
    internal class DebugUtils
    {
        /// <summary>
        /// 调试用输出Text，方便调试后删除
        /// </summary>
        /// <param name="o"></param>
        public static void NewText(object o)
        {
            Main.NewText(o);
        }
        public static void NewText_Vector2(object o)
        {
            Main.NewText("Vector2" + o);
        }

    }
}
