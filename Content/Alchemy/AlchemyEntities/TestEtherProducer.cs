using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Perfab.AEAnimator;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;
using static MysteriousAlchemy.Core.Perfab.AEAnimator.BaseAEAnimator;

namespace MysteriousAlchemy.Content.Alchemy.AlchemyEntities
{
    public class TestEtherProducer : AlchemyEntity
    {
        public override Type AnimatorType => typeof(BaseAEAnimator);
        public static new readonly Func<TagCompound, TestEtherProducer> DESERIALIZER = Load;
        int counter;
        int counter2;
        public static new TestEtherProducer Load(TagCompound tag)
        {
            return InstanceLoad<TestEtherProducer>(tag);
        }
        public override void SetDefault()
        {
            MaxEther = 100;
            base.SetDefault();
        }
        public override void OnUpdate()
        {
            //counter++;
            //if (counter == 1 && Animator?.CurrectState?.GetType() == typeof(Test1))
            //{
            //    Animator.SwitchState(typeof(Test2).ToString());
            //    counter2++;
            //    counter = 0;
            //}
            //else if (counter == 30 && Animator?.CurrectState?.GetType() == typeof(Test2) && counter2 < 10)
            //{
            //    Animator.SwitchState(typeof(Test1).ToString());
            //    counter = 0;
            //}

            Ether += 0.1f;
            Limit();
            base.OnUpdate();
        }
    }
}
