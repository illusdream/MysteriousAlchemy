using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

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
        public const string Ether = Items + "Ether/";

        public const string Projectiles = Asset + "Projectiles/";
        public const string WeaponProjectile = Projectiles + "WeaponProjectile/";

        public const string Tiles = Asset + "Tiles/";



        public const string Texture = Asset + "Texture/";

        public const string ColorBar = Texture + "ColorBar/";
        public const string Extra = Texture + "Extra/";
        public const string Flow = Texture + "Flow/";
        public const string Mask = Texture + "Mask/";
        public const string Glow = Texture + "Glow/";
        public const string UI = Texture + "UI/";

        public const string Effect = ModPath + "Effects/";

        public static Texture2D GetTexture2D(string path)
        {
            return ModContent.Request<Texture2D>(path).Value;
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
    }
}