using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rail;
using ReLogic.Content;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Utils
{
    /// <summary>
    /// 工具类：用于获取对应资源
    /// </summary>
    public static class AssetUtils
    {
        public const string ModPath = "MysteriousAlchemy/";

        public const string Content = ModPath + "Content/";
        public const string Asset = ModPath + "Asset/";

        public const string Buffs = Asset + "Buffs/";

        public const string Dusts = Asset + "Dusts/";

        public const string Items = Asset + "Items/";
        public const string Weapons = Items + "Weapons/";
        public const string Item_Chilliness = Items + "Chilliness/";
        public const string Ether = Items + "Ether/";
        public const string Items_MagicComponents = Items + "MagicComponents/";
        public const string Items_AlchemySlab = Items + "AlchemySlab/";

        public const string Projectiles = Asset + "Projectiles/";
        public const string WeaponProjectile = Projectiles + "WeaponProjectile/";
        public const string Proj_Chilliness = Projectiles + "Chilliness/";

        public const string Tiles = Asset + "Tiles/";
        public const string Tiles_MagicComponents = Tiles + "MagicComponents/";

        public const string UI = Asset + "UI/";
        public const string UI_Alchemy = UI + "Alchemy/";

        public const string Texture = Asset + "Texture/";
        public const string Runes = Asset + "Runes/";

        public const string ColorBar = Texture + "ColorBar/";
        public const string Extra = Texture + "Extra/";
        public const string Flow = Texture + "Flow/";
        public const string Mask = Texture + "Mask/";
        public const string Glow = Texture + "Glow/";
        public const string Localization = Texture + "Localization/";
        public const string Texture_UI = Texture + "UI/";

        public const string Effect = ModPath + "Effects/";

        public enum LoadStyle
        {
            Aysnc, ImmediateLoad, Static
        }
        #region 静态存储
        public static Dictionary<string, Texture2D> TextureDic = new Dictionary<string, Texture2D>() { };

        public static Texture2D TryGetTextureInStaticDictionary(string path)
        {
            TextureDic ??= new Dictionary<string, Texture2D>();
            if (TextureDic.ContainsKey(path))
            {
                return TextureDic[path];
            }
            else
            {
                Texture2D instance = ModContent.Request<Texture2D>(path, AssetRequestMode.ImmediateLoad).Value;
                TextureDic.Add(path, instance);
                return instance;
            }
        }
        #endregion
        public static Texture2D GetTexture2D(string path, LoadStyle loadStyle = LoadStyle.Aysnc)
        {
            switch (loadStyle)
            {
                case LoadStyle.Aysnc:
                    return ModContent.Request<Texture2D>(path).Value;

                case LoadStyle.ImmediateLoad:
                    return ModContent.Request<Texture2D>(path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                case LoadStyle.Static:
                    return TryGetTextureInStaticDictionary(path);
                default:
                    return ModContent.Request<Texture2D>(path).Value;

            }

        }
        public static Asset<Texture2D> GetTexture2DAsset(string path)
        {
            return ModContent.Request<Texture2D>(path);
        }
        public static Texture2D GetTexture2DImmediate(string path)
        {
            return ModContent.Request<Texture2D>(path, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }
        public static Texture2D GetColorBar(string name)
        {
            return ModContent.Request<Texture2D>(ColorBar + name).Value;
        }
        public static Texture2D GetFlow(string name)
        {
            return ModContent.Request<Texture2D>(Flow + name).Value;
        }
        public static Texture2D GetMask(string name)
        {
            return ModContent.Request<Texture2D>(Mask + name).Value;
        }
        public static Effect GetEffect(string name)
        {
            return ModContent.Request<Effect>(Effect + name, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }
        public static string GetRunesFullPath(string Char)
        {
            if (Regex.IsMatch(Char, "^[A-Z0-9]$|^10$"))
            {
                var path = Runes + Char;
                return path;
            }
            else
            {
                return Runes + 'A';
            }

        }
        public static Texture2D GetRunesTexture(string Char)
        {
            return ModContent.Request<Texture2D>(GetRunesFullPath(Char)).Value;
        }
        public static string GetRandomChar()
        {
            return ((char)('A' + Main.rand.Next(0, 26))).ToString();
        }
        /// <summary>
        /// 提前写好了Mods.MysteriousAlchemy.UIImageText.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetUIImageTextInfoValue(string path)
        {
            return Language.GetTextValue("Mods.MysteriousAlchemy.UIImageText." + path);
        }
        /// <summary>
        /// 提前写好了Mods.MysteriousAlchemy.UIImageText
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static LocalizedText GetUIImageTextInfo(string path)
        {
            return Language.GetText("Mods.MysteriousAlchemy.UIImageText." + path);
        }

        public static void SaveData_Dictionary<TKey, TValue>(ref TagCompound tag, ref Dictionary<TKey, TValue> Dic, string SaveName)
        {
            if (Dic == null)
                return;
            var list = new List<TagCompound>();
            foreach (var data in Dic)
            {
                list.Add(new TagCompound()
                {
                    ["Key"] = data.Key,
                    ["Value"] = data.Value
                });
            }
            tag[SaveName] = list;
        }
        public static void LoadData_Dictionary<TKey, TValue>(TagCompound tag, string SaveName, ref Dictionary<TKey, TValue> Dic)
        {
            if (Dic is null)
                Dic = new Dictionary<TKey, TValue>();
            var list = tag.GetList<TagCompound>(SaveName);
            foreach (var data in list)
            {
                var Key = data.Get<TKey>("Key");
                var Value = data.Get<TValue>("Value");
                Dic[Key] = Value;
            }
        }











        public static Texture2D WhitePic = GetTexture2D(Texture + "White");
    }


}