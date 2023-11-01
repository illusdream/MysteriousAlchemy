using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Content.Alchemy.AlchemyEntities
{
    public class TestEtherProducer : AlchemyEntity
    {
        public static new readonly Func<TagCompound, TestEtherProducer> DESERIALIZER = Load;
        public static new TestEtherProducer Load(TagCompound tag)
        {
            return InstanceLoad<TestEtherProducer>(tag);
        }
        public override void SetDefault()
        {
            MaxEther = 100;
            base.SetDefault();
        }
        public override void Update()
        {
            Ether += 0.1f;
            Limit();
            base.Update();
        }
    }
}
