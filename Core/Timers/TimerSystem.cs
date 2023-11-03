using MysteriousAlchemy.Core.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Timers
{
    public class TimerSystem : ModSystem
    {
        public static List<Timer> timers = new List<Timer>();
        public override void Load()
        {
            base.Load();
        }
        public override void Unload()
        {
            timers = null;
            base.Unload();
        }
        public override void PreUpdateTime()
        {

            base.PreUpdateTime();
        }
        public override void PreUpdateEntities()
        {
            for (int i = 0; i < timers.Count; i++)
            {
                if (!timers[i].active)
                {
                    timers.RemoveAt(i);
                    continue;
                }

                timers[i].update();
            }
            base.PreUpdateEntities();
        }
        public static T RegisterTimer<T>(float timeScale, float triggerTime) where T : Timer, new()
        {
            var instance = new T();
            instance.active = true;
            instance.timeScale = timeScale;
            instance.triggerTime = triggerTime;
            timers.Add(instance);
            return instance;
        }

    }
}
