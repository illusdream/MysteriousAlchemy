using Microsoft.Xna.Framework;
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
        public static List<Timer> UITimers = new List<Timer>();
        public override void Load()
        {
            base.Load();
        }
        public override void Unload()
        {
            timers = null;
            UITimers = null;
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
        public override void UpdateUI(GameTime gameTime)
        {
            for (int i = 0; i < UITimers.Count; i++)
            {
                if (!UITimers[i].active)
                {
                    UITimers.RemoveAt(i);
                    continue;
                }

                UITimers[i].update();
            }
            base.UpdateUI(gameTime);
        }
        public static T RegisterTimer<T>(float timeScale, float triggerTime, bool IsUI_Timer) where T : Timer, new()
        {
            var instance = new T();
            instance.active = true;
            instance.timeScale = timeScale;
            instance.triggerTime = triggerTime;

            if (IsUI_Timer)
            {
                UITimers.Add(instance);
            }
            else
            {
                timers.Add(instance);
            }
            return instance;
        }

    }
}
