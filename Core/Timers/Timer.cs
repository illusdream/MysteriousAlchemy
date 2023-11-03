using MysteriousAlchemy.Utils;
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

        public bool pause;

        public Timer()
        {

        }
        public Timer(float timeScale, float triggerTime)
        {
            this.timeScale = timeScale;
            this.triggerTime = triggerTime;
        }

        public virtual bool ConditionTrigger(bool Condition)
        {
            if (Condition && time > triggerTime)
            {
                time = 0;
                return true;
            }
            return false;
        }
        public virtual void ConditionTrigger(bool condition, Action HowToDo)
        {
            if (ConditionTrigger(condition))
                HowToDo?.Invoke();
        }
        public virtual void update()
        {
            if (!pause)
            {
                time += timeScale;
            }
        }
        public virtual float GetTime()
        {
            return time;
        }
        public virtual float GetLinearInter()
        {
            return time / triggerTime;
        }
        public virtual float GetEaseInter()
        {
            return -(MathF.Cos(MathF.PI * GetLinearInter()) - 1) / 2;
        }
        public virtual void ResetTimer()
        {
            time = 0;
        }
    }
}
