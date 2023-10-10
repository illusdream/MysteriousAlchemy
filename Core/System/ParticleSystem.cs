using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.System
{

    public class ParticleSystem : ModSystem
    {
        public static int ParticleCount = 1000;

        public static Particle[] particles;

        public ParticleSystem()
        {
            particles = new Particle[ParticleCount];
            for (int i = 0; i < ParticleCount; i++)
            {
                particles[i] = new Particle();
            }
        }
        public override void Load()
        {
            if (Main.dedServ)
                return;
            particles = new Particle[ParticleCount];
            for (int i = 0; i < ParticleCount; i++)
            {
                particles[i] = new Particle();
            }
            base.Load();
        }
        public override void Unload()
        {
            for (int i = 0; i < ParticleCount; i++)
            {
                particles[i] = null;
            }
            base.Unload();
        }
        public override void PostUpdateDusts()
        {
            base.PostUpdateDusts();

            if (Main.netMode == NetmodeID.Server)//不在服务器上运行
                return;
            if (Main.gameInactive)//不在游戏暂停时运行
                return;

            for (int i = 0; i < ParticleCount; i++)
            {
                if (particles[i].active == false)
                    continue;

                if (particles[i].ShouldUpdatePosition())
                    particles[i].position += particles[i].velocity;


                //防止存在时间过长
                if (particles[i].scale < 0.01f)
                    particles[i].active = false;
                if (particles[i].timer > 1000)
                    particles[i].active = false;
            }
        }
    }

}
