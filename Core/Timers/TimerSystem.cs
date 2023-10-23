using MysteriousAlchemy.Core.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Timers
{
    public class TimerSystem : ModSystem
    {
        public static List<Timer> timers;
        public override void Load()
        {
            timers = new List<Timer>();
            base.Load();
        }
        public override void PreUpdateTime()
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

            base.PreUpdateTime();
        }
        public static T RegisterTimer<T>(float timeScale, float triggerTime) where T : Timer, new()
        {
            var instance = new T();
            instance.timeScale = timeScale;
            instance.triggerTime = triggerTime;
            return instance;
        }

    }
}
