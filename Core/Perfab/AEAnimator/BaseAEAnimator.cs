using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Core.Perfab.AEAnimator
{
    public class BaseAEAnimator : Animator
    {
        public DrawUnit DrawUnit;
        public int count;
        public BaseAEAnimator() : base()
        {

        }
        public BaseAEAnimator(Vector2 Position) : base(Position)
        {

        }
        public override void Initialize()
        {
            base.Initialize();
            RegisterState<AEAState_Deactive>(new AEAState_Deactive(this));
            RegisterState<AEASatet_EntryActive>(new AEASatet_EntryActive(this));
            RegisterState<AEAState_Active>(new AEAState_Active(this));
            RegisterState<AEAState_ExitActive>(new AEAState_ExitActive(this));
            SetState("Deactive");

        }
        public override void AI()
        {
            base.AI();
        }
    }

}
