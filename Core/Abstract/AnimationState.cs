using MysteriousAlchemy.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Abstract
{
    /// <summary>
    /// 动画状态，
    /// </summary>
    /// <typeparam name="T">对应动画机类型</typeparam>
    public abstract class AnimationState<T> : IState where T : Animator
    {
        /// <summary>
        /// 该状态持续时间，
        /// </summary>
        public int Timer { get; set; }

        public bool IgnoreTimer { get; set; }

        public T Animator;
        public AnimationState(IStateMachine stateMachine)
        {
            Animator = stateMachine as T;
        }



        public virtual void EntryState(IStateMachine animator)
        {

        }

        public virtual void ExitState(IStateMachine animator)
        {

        }

        public virtual void OnState(IStateMachine animator)
        {

            if (!IgnoreTimer)
            {
                if (Animator.Timer > Timer)
                {
                    AutoSwitchState(animator);
                }
            }
        }
        public virtual void AutoSwitchState(IStateMachine animator)
        {


        }

        public virtual bool ModifyName(out string name)
        {
            name = GetType().ToString();
            return false;
        }
    }
}
