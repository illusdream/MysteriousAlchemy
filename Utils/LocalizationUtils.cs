using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Utils
{
    /// <summary>
    /// 貌似没什么用了，发布之前删了吧
    /// </summary>
    public class LocalizationUtils
    {
        public static string LocalLang(string zh, string en)
        {
            return (Language.ActiveCulture.Name == "zh-Hans" ? zh : en);
        }
    }
}