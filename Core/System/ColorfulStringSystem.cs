using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Drawing;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;
using Color = Microsoft.Xna.Framework.Color;
using MonoMod.RuntimeDetour;
using MysteriousAlchemy.Dusts;
using MysteriousAlchemy.Items;
using MysteriousAlchemy.Modsystem;
using MysteriousAlchemy.UI;
using MysteriousAlchemy.Utils;
using MysteriousAlchemy.VanillaJSONFronting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Reflection;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;
using Terraria.ObjectData;
using Terraria.Utilities;
using static Terraria.ID.ContentSamples.CreativeHelper;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
namespace MysteriousAlchemy.Modsystem
{
    public class ColorfulStringSystem : ModSystem
    {
        static ColorfulStringInfo[] colorfulStringInfos = new ColorfulStringInfo[200];

        public static void AddColorfulString(SpriteBatch spriteBatch, DynamicSpriteFont spriteFont, string text, Color color, Vector2 PositionInScreen, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerdepth)
        {

            if (colorfulStringInfos == null)
            {
                colorfulStringInfos = new ColorfulStringInfo[200];
            }
            for (int i = 0; i < colorfulStringInfos.Length; i++)
            {
                if (colorfulStringInfos[i] != null)
                {
                    if (colorfulStringInfos[i].text == text)
                    {
                        colorfulStringInfos[i].spriteBatch = spriteBatch;
                        colorfulStringInfos[i].spriteFont = spriteFont;
                        colorfulStringInfos[i].text = text;
                        colorfulStringInfos[i].color = color;
                        colorfulStringInfos[i].PositionInScreen = PositionInScreen;
                        colorfulStringInfos[i].rotation = rotation;
                        colorfulStringInfos[i].origin = origin;
                        colorfulStringInfos[i].scale = scale;
                        colorfulStringInfos[i].effects = spriteEffects;
                        colorfulStringInfos[i].layerDepth = layerdepth;
                        colorfulStringInfos[i].existTime = 0;
                        return;
                    }
                    else if (colorfulStringInfos[i].text == null)
                    {
                        colorfulStringInfos[i].spriteBatch = spriteBatch;
                        colorfulStringInfos[i].spriteFont = spriteFont;
                        colorfulStringInfos[i].text = text;
                        colorfulStringInfos[i].color = color;
                        colorfulStringInfos[i].PositionInScreen = PositionInScreen;
                        colorfulStringInfos[i].rotation = rotation;
                        colorfulStringInfos[i].origin = origin;
                        colorfulStringInfos[i].scale = scale;
                        colorfulStringInfos[i].effects = spriteEffects;
                        colorfulStringInfos[i].layerDepth = layerdepth;
                        colorfulStringInfos[i].existTime = 0;
                        return;
                    }
                }
                else
                {
                    colorfulStringInfos[i] = new ColorfulStringInfo();
                    colorfulStringInfos[i].spriteBatch = spriteBatch;
                    colorfulStringInfos[i].spriteFont = spriteFont;
                    colorfulStringInfos[i].text = text;
                    colorfulStringInfos[i].color = color;
                    colorfulStringInfos[i].PositionInScreen = PositionInScreen;
                    colorfulStringInfos[i].rotation = rotation;
                    colorfulStringInfos[i].origin = origin;
                    colorfulStringInfos[i].scale = scale;
                    colorfulStringInfos[i].effects = spriteEffects;
                    colorfulStringInfos[i].layerDepth = layerdepth;
                    colorfulStringInfos[i].existTime = 0;
                    return;
                }
            }
        }
        public static void DrawAllColorfulString()
        {
            for (int i = 0; i < colorfulStringInfos.Length; i++)
            {
                if (colorfulStringInfos[i] != null)
                {
                    if (colorfulStringInfos[i].text != null)
                    {
                        colorfulStringInfos[i].spriteBatch.DrawString(colorfulStringInfos[i].spriteFont, colorfulStringInfos[i].text, colorfulStringInfos[i].PositionInScreen - Main.screenPosition, colorfulStringInfos[i].color, colorfulStringInfos[i].rotation, colorfulStringInfos[i].origin, colorfulStringInfos[i].scale, colorfulStringInfos[i].effects, colorfulStringInfos[i].layerDepth);
                        colorfulStringInfos[i].existTime++;
                    }
                }

            }
        }
        public override void PostUpdateEverything()
        {
            for (int i = 0; i < colorfulStringInfos.Length; i++)
            {
                if (colorfulStringInfos[i] != null)
                {
                    if (colorfulStringInfos[i].text != null)
                    {
                        if (colorfulStringInfos[i].existTime > 3)
                        {
                            colorfulStringInfos[i] = new ColorfulStringInfo();
                        }
                    }
                }
            }
        }
        public static void ClearAllColorfulString()
        {
            if (colorfulStringInfos == null)
            {
                colorfulStringInfos = new ColorfulStringInfo[200];
            }
            for (int i = 0; i < colorfulStringInfos.Length; i++)
            {
                if (colorfulStringInfos[i] == null)
                {
                    colorfulStringInfos[i] = new ColorfulStringInfo();
                }
            }
            for (int i = 0; i < colorfulStringInfos.Length; i++)
            {
                colorfulStringInfos[i] = new ColorfulStringInfo();
            }
        }
    }



    public class ColorfulStringInfo
    {
        public ColorfulStringInfo()
        {
            spriteBatch = null;


            spriteFont = null;
            text = null;
            color = Color.White;
            //Î»ÖÃ
            PositionInScreen = Vector2.Zero;
            rotation = 0;
            origin = Vector2.Zero;
            scale = 0;
            effects = SpriteEffects.None;
            layerDepth = 0;
            existTime = 0;
        }
        public SpriteBatch spriteBatch;
        public DynamicSpriteFont spriteFont;
        public string text;
        public Color color;
        //Î»ÖÃ
        public Vector2 PositionInScreen;
        public float rotation;
        public Vector2 origin;
        public float scale;
        public SpriteEffects effects;
        public float layerDepth;
        public int existTime;
    }
}