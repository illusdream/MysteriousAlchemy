using Microsoft.Xna.Framework;
using System.ComponentModel;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace MysteriousAlchemy.Core.System
{
    public class VisualEffectSystem
    {
        public static bool HitEffect_ScreenShaking = true;
        public static void AddScreenShake_OnHit(Vector2 startPosition, Vector2 direction, float strength, float vibrationCyclesPerSecond = 6, int frames = 6, float distanceFalloff = 10f, string uniqueIdentity = null)
        {
            if (HitEffect_ScreenShaking)
            {
                PunchCameraModifier punchCameraModifier = new PunchCameraModifier(startPosition, direction, strength, vibrationCyclesPerSecond, frames, distanceFalloff, uniqueIdentity);
                Main.instance.CameraModifiers.Add(punchCameraModifier);
            }

        }
    }

    public class VisualEffectConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("HitEffect")]
        [SeparatePage]

        [DefaultValue(true)]
        public bool HitEffect_ScreenShaking;


        public override void OnChanged()
        {
            SetValues();
        }
        public override void OnLoaded()
        {
            SetValues();
        }
        public void SetValues()
        {
            VisualEffectSystem.HitEffect_ScreenShaking = HitEffect_ScreenShaking;
        }
    }
}