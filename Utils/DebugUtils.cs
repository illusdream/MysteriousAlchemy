using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;

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

        public static void InfoWithCombat(Vector2 position, params object[] objects)
        {
            DebugDraw += (sb) =>
            {
                List<object> list = new List<object>();
                list.AddRange(objects);
                for (int i = 0; i < list.Count; i++)
                {
                    Terraria.Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, list[i].ToString(), DrawUtils.ToScreenPosition(position).X, DrawUtils.ToScreenPosition(position).Y + 15 * i, Color.White, Color.Black, new Vector2(0, 0), 0.75f);
                }
            };

        }
        public static void InvokoDebugDraw(SpriteBatch spriteBatch)
        {
            DebugDraw?.Invoke(spriteBatch);
            ClearAction(ref DebugDraw);
        }
        private static void ClearAction(ref Action<SpriteBatch> action)
        {
            if (action is null)
                return;
            Delegate[] allAction = action.GetInvocationList();
            for (int i = 0; i < allAction.Length; i++)
            {
                action -= allAction[i] as Action<SpriteBatch>;
            }
        }
        public static Action<SpriteBatch> DebugDraw;
    }
}
