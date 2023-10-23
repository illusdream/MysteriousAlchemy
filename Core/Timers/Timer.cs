using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Timers
{
    public class Timer
    {
        public float time;

        public float timeScale;

        public float triggerTime;

        public bool active;

        public Timer(float timeScale, float triggerTime)
        {
            this.timeScale = timeScale;
            this.triggerTime = triggerTime;
        }

        public virtual bool ConditionTrigger(bool Condition)
        {
            return Condition && time > triggerTime;
        }
        public virtual void update()
        {
            time += timeScale;
        }
        public virtual float GetTime()
        {
            return time;
        }
    }
}
