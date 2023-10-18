using MysteriousAlchemy.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Renderers;
using Terraria;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Systems
{
    public class VisualEffectEntitySystem : ModSystem
    {
        public static DistortWind[] distortWinds;

        public static DelaySlash[] delaySlashs;

        public VisualEffectEntitySystem()
        {
            distortWinds = new DistortWind[VisualEffectSystem.DistortEffect_DistortWindCount];

            delaySlashs = new DelaySlash[VisualEffectSystem.SwingEffect_DelaySlashCount];
        }
        public override void Load()
        {
            if (Main.dedServ)
                return;
            distortWinds = new DistortWind[VisualEffectSystem.DistortEffect_DistortWindCount];
            for (int i = 0; i < VisualEffectSystem.DistortEffect_DistortWindCount; i++)
            {
                distortWinds[i] = new DistortWind();
            }


            delaySlashs = new DelaySlash[VisualEffectSystem.SwingEffect_DelaySlashCount];
            for (int i = 0; i < VisualEffectSystem.SwingEffect_DelaySlashCount; i++)
            {
                delaySlashs[i] = new DelaySlash();
            }
            base.Load();
        }
        public override void Unload()
        {
            for (int i = 0; i < VisualEffectSystem.DistortEffect_DistortWindCount; i++)
            {
                distortWinds[i] = null;
            }


            for (int i = 0; i < VisualEffectSystem.SwingEffect_DelaySlashCount; i++)
            {
                delaySlashs[i] = null;
            }
            base.Unload();
        }
        public override void PostUpdateEverything()
        {
            for (int i = 0; i < VisualEffectSystem.DistortEffect_DistortWindCount; i++)
            {
                if (!distortWinds[i].active)
                    continue;
                distortWinds[i].update?.Invoke(distortWinds[i]);

            }


            for (int i = 0; i < VisualEffectSystem.SwingEffect_DelaySlashCount; i++)
            {
                if (!delaySlashs[i].active)
                    continue;
                delaySlashs[i].Update();

            }
            base.PostUpdateEverything();
        }
    }
}
