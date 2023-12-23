using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Utils;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Perfab.AEAnimator
{
    public class AEAState_Deactive : AnimationState<BaseAEAnimator>
    {
        public AEAState_Deactive(IStateMachine stateMachine) : base(stateMachine)
        {


        }
        public override void EntryState(IStateMachine animator)
        {
            DebugUtils.NewText(GetType().ToString());
            base.EntryState(animator);
        }
        public override bool ModifyName(out string name)
        {
            name = "Deactive";
            return true;
        }
    }
    public class AEASatet_EntryActive : AnimationState<BaseAEAnimator>
    {
        public AEASatet_EntryActive(IStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EntryState(IStateMachine animator)
        {
            DebugUtils.NewText(GetType().ToString());
            base.EntryState(animator);
        }
        public override bool ModifyName(out string name)
        {
            name = "EntryActive";
            return true;
        }
    }
    public class AEAState_Active : AnimationState<BaseAEAnimator>
    {
        public AEAState_Active(IStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EntryState(IStateMachine animator)
        {
            DebugUtils.NewText(GetType().ToString());
            base.EntryState(animator);
        }
        public override bool ModifyName(out string name)
        {
            name = "Active";
            return true;
        }
    }
    public class AEAState_ExitActive : AnimationState<BaseAEAnimator>
    {
        public AEAState_ExitActive(IStateMachine stateMachine) : base(stateMachine)
        {
        }
        public override void EntryState(IStateMachine animator)
        {
            DebugUtils.NewText(GetType().ToString());
            base.EntryState(animator);
        }
        public override bool ModifyName(out string name)
        {
            name = "ExitActive";
            return true;
        }
    }
}