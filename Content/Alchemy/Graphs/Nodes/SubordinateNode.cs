using MysteriousAlchemy.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Content.Alchemy.Graphs.Nodes
{
    public class SubordinateNode : Node
    {
        public static new readonly Func<TagCompound, SubordinateNode> DESERIALIZER = Load_EtherNode;
        public static SubordinateNode Load_EtherNode(TagCompound tag)
        {
            var instance = new SubordinateNode();
            instance.CustomDate = tag.GetCompound(nameof(CustomDate));
            instance.unicode = tag.Get<AlchemyUnicode>(nameof(entity));


            instance.LoadData(instance.CustomDate);
            return instance;
        }
    }
}
