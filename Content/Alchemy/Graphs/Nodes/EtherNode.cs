using MysteriousAlchemy.Content.Alchemy.Graphs.Links;
using MysteriousAlchemy.Core.Abstract;
using System.Security.AccessControl;
using System;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Content.Alchemy.Graphs.Nodes
{

    public class EtherNode : Node
    {
        public void UpdateEtherLinks(float statModify)
        {

        }


        public static new readonly Func<TagCompound, EtherNode> DESERIALIZER = Load_EtherNode;
        public static EtherNode Load_EtherNode(TagCompound tag)
        {
            var instance = new EtherNode();
            instance.CustomDate = tag.GetCompound(nameof(CustomDate));
            instance.unicode = tag.Get<AlchemyUnicode>(nameof(entity));


            instance.LoadData(instance.CustomDate);
            return instance;
        }
    }
}
