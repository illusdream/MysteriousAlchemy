using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Timers
{
    /// <summary>
    /// 计时器，主要用于动画
    /// </summary>
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
        /// <summary>
        /// 线性插值
        /// </summary>
        /// <returns></returns>
        public virtual float GetLinearInter()
        {
            return time / triggerTime;
        }
        /// <summary>
        /// 缓动插值，推荐用来做UI元素的开启，或者移动 慢—快—慢
        /// </summary>
        /// <returns></returns>
        public virtual float GetEaseInter()
        {
            return -(MathF.Cos(MathF.PI * GetLinearInter()) - 1) / 2;
        }
        /// <summary>
        /// 缓动插值，或许可以做弹簧之类的效果？ 慢—快—慢,但有略微溢出，会在开始与末尾稍微超过预定值
        /// </summary>
        /// <returns></returns>
        public float GetEaseInterLittleOut()
        {
            const float c1 = 1.70158f;
            const float c2 = c1 * 1.525f;

            return GetLinearInter() < 0.5
          ? (float)(Math.Pow(2 * GetLinearInter(), 2) * ((c2 + 1) * 2 * GetLinearInter() - c2)) / 2
          : (float)(Math.Pow(2 * GetLinearInter() - 2, 2) * ((c2 + 1) * (GetLinearInter() * 2 - 2) + c2) + 2) / 2;
        }


        public virtual void ResetTimer()
        {
            time = 0;
        }
    }
}
