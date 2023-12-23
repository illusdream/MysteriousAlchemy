using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Perfab.AEAnimator;
using System.Diagnostics.Metrics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Abstract
{
    public class BaseAlchemyEntityState : IState
    {
        public BaseAlchemyEntityState(AlchemyEntity entity)
        {
            Entity = entity;
        }

        protected AlchemyEntity Entity { get; set; }
        public virtual void EntryState(IStateMachine stateMachine)
        {

        }

        public virtual void ExitState(IStateMachine stateMachine)
        {

        }

        public virtual bool ModifyName(out string name)
        {
            name = GetType().ToString();
            return false;
        }

        public virtual void OnState(IStateMachine stateMachine)
        {

        }
    }

    public class AlchemyEntityState_Deactive : BaseAlchemyEntityState
    {
        int counter;
        public AlchemyEntityState_Deactive(AlchemyEntity entity) : base(entity)
        {
        }
        public override void EntryState(IStateMachine stateMachine)
        {
            Entity?.Animator?.SwitchState("Deactive");
            base.EntryState(stateMachine);
        }
        public override void OnState(IStateMachine stateMachine)
        {
            counter++;
            if (counter > 10)
            {
                counter = 0;
                Entity.SwitchState("EntryActive");
            }
            base.OnState(stateMachine);
        }
        public override bool ModifyName(out string name)
        {
            name = "Deactive";
            return true;
        }
    }
    public class AlchemyEntityState_EntryActive : BaseAlchemyEntityState
    {
        int counter;
        public AlchemyEntityState_EntryActive(AlchemyEntity entity) : base(entity)
        {
        }
        public override void EntryState(IStateMachine stateMachine)
        {
            Entity.Animator?.SwitchState("EntryActive");
            base.EntryState(stateMachine);
        }
        public override void OnState(IStateMachine stateMachine)
        {
            counter++;
            if (counter > 10)
            {
                counter = 0;
                Entity.SwitchState("Active");
            }
            base.OnState(stateMachine);
        }
        public override bool ModifyName(out string name)
        {
            name = "EntryActive";
            return true;
        }
    }
    public class AlchemyEntityState_Active : BaseAlchemyEntityState
    {
        int counter;
        public AlchemyEntityState_Active(AlchemyEntity entity) : base(entity)
        {
        }
        public override void EntryState(IStateMachine stateMachine)
        {
            Entity.Animator?.SwitchState("Active");
            base.EntryState(stateMachine);
        }
        public override void OnState(IStateMachine stateMachine)
        {
            counter++;
            if (counter > 10)
            {
                counter = 0;
                Entity.SwitchState("ExitActive");
            }
            base.OnState(stateMachine);
        }
        public override bool ModifyName(out string name)
        {
            name = "Active";
            return true;
        }
    }
    public class AlchemyEntityState_ExitActive : BaseAlchemyEntityState
    {
        int counter;
        public AlchemyEntityState_ExitActive(AlchemyEntity entity) : base(entity)
        {
        }
        public override void EntryState(IStateMachine stateMachine)
        {
            Entity.Animator?.SwitchState("ExitActive");
            base.EntryState(stateMachine);
        }
        public override void OnState(IStateMachine stateMachine)
        {
            counter++;
            if (counter > 10)
            {
                counter = 0;
                Entity.SwitchState("Deactive");
            }
            base.OnState(stateMachine);
        }
        public override bool ModifyName(out string name)
        {
            name = "ExitActive";
            return true;
        }
    }
}