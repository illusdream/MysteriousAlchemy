using MysteriousAlchemy.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Content.Alchemy.Graphs.Links
{
    /// <summary>
    /// start为父节点，end为子节点
    /// </summary>
    public class SubordinateLink : Link
    {
        public static new readonly Func<TagCompound, SubordinateLink> DESERIALIZER = Load;
        public static new SubordinateLink Load(TagCompound tag)
        {
            var instance = new SubordinateLink();
            instance.start = tag.Get<AlchemyUnicode>(nameof(start));
            instance.end = tag.Get<AlchemyUnicode>(nameof(end));
            instance.weight = tag.GetInt(nameof(weight));
            instance.CustomDate = tag.GetCompound(nameof(CustomDate));

            instance.LoadDate(instance.CustomDate);
            return instance;
        }
    }
}
