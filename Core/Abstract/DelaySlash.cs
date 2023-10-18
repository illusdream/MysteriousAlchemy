using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Core.Abstract
{
    //延迟刀光，
    public class DelaySlash
    {
        public bool active;

        public Vector2 postion;

        public Vector2[] vertexTop;

        public Vector2[] vertexBottom;

        public Color color = Color.White;

        public int Timer;
        public int TotalTime;

        public string maskTexture;
        public string colorTexture;
        public string distortTexture;
        public string colorShapeTex;
        public float distortPower;
        public float distortBlend;
        public float blendColor;
        public float distortUVOffestSpeed;
        public float alphaBlend;
        public float alphaFlip;
        public float lightScaleOfCT;
        public Action BloomAction;
        public virtual void OnSpown()
        {

        }
        public virtual void Update()
        {
            Timer++;
            alphaFlip = Timer / (float)TotalTime;
            if (Timer >= TotalTime)
                active = false;
        }
        public virtual void Draw()
        {
            DebugUtils.NewText(postion);
            DrawUtils.DrawDefaultSlash(vertexTop, vertexBottom, postion, color,
                AssetUtils.GetTexture2D(maskTexture),
                AssetUtils.GetTexture2D(colorTexture),
                AssetUtils.GetTexture2D(distortTexture),
                AssetUtils.GetTexture2D(colorShapeTex),
                distortPower,
                distortBlend,
                blendColor,
                distortUVOffestSpeed,
                alphaBlend,
                lightScaleOfCT
                );
            if (BloomAction is not null)
                VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.BloomAreaDraw, BloomAction);
        }

        public static int NewDelaySlash<T>(Vector2 position, Vector2[] vertexTop, Vector2[] vertexBottom, Color color, int totalTime, string maskTexture, string colorTexture, string distortTexture, string colorShapeTex, float distortPower, float distortBlend, float blendColor, float distortUVOffestSpeed, float alphaBlend, Action bloomAction, float lightScaleOfCT) where T : DelaySlash
        {
            int result = VisualEffectSystem.SwingEffect_DelaySlashCount - 1;
            DelaySlash[] delaySlashs = VisualEffectEntitySystem.delaySlashs;
            for (int i = 0; i < VisualEffectSystem.SwingEffect_DelaySlashCount; i++)
            {
                if (delaySlashs[i].active)
                    continue;
                delaySlashs[i] = new DelaySlash();
                delaySlashs[i].active = true;
                delaySlashs[i].postion = position;
                delaySlashs[i].vertexTop = vertexTop;
                delaySlashs[i].color = color;
                delaySlashs[i].vertexBottom = vertexBottom;
                delaySlashs[i].maskTexture = maskTexture;
                delaySlashs[i].colorTexture = colorTexture;
                delaySlashs[i].distortTexture = distortTexture;
                delaySlashs[i].colorShapeTex = colorShapeTex;
                delaySlashs[i].distortPower = distortPower;
                delaySlashs[i].alphaBlend = alphaBlend;
                delaySlashs[i].distortBlend = distortBlend;
                delaySlashs[i].blendColor = blendColor;
                delaySlashs[i].distortUVOffestSpeed = distortUVOffestSpeed;
                delaySlashs[i].BloomAction = bloomAction;
                delaySlashs[i].lightScaleOfCT = lightScaleOfCT;

                delaySlashs[i].TotalTime = totalTime;
                break;
            }
            return result;
        }
    }
}
