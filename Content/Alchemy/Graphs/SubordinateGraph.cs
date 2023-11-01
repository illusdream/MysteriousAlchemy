using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Alchemy.Graphs.Links;
using MysteriousAlchemy.Content.Alchemy.Graphs.Nodes;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Content.Alchemy.Graphs
{
    public class SubordinateGraph : Graph<SubordinateLink, SubordinateNode>
    {
        public List<AlchemyUnicode> InWorld;

        public SubordinateGraph()
        {
            InWorld = new List<AlchemyUnicode>();
        }
        public void Update()
        {
            ForEach((o) =>
            {
                int count = 0;
                foreach (var node in o.AdjacencyLinks)
                {
                    if (AlchemySystem.FindAlchemyEntitySafely<AlchemyEntity>(node.Key, out var entity))
                    {
                        entity.TopLeft = o.Node.GetEntityInstance().TopLeft + MathUtils.GetVector2InCircle(MathHelper.TwoPi * count / o.AdjacencyLinks.Count, 200);
                        count++;
                    };
                }
            });
        }
        public void AddSubordinateLink(AlchemyEntity instance, AlchemyUnicode? SubTarget)
        {
            if (SubTarget is null)
            {
                InWorld.Add(instance.unicode);
            }
            else
            {
                if (FindNode((AlchemyUnicode)SubTarget, out var target))
                {
                    AddLink(target.GetEntityInstance(), instance, null);
                }
            }
        }
        public override bool RemoveNode(AlchemyUnicode unicode)
        {
            bool result = base.RemoveNode(unicode);
            if (InWorld.Contains(unicode) && result)
                InWorld.Remove(unicode);
            return result;
        }
        public override void Clear()
        {
            InWorld.Clear();
            base.Clear();
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add(nameof(InWorld), InWorld);
            base.SaveData(tag);
        }
        public override void LoadData(TagCompound tag)
        {
            InWorld = tag.Get<List<AlchemyUnicode>>(nameof(InWorld));
            base.LoadData(tag);
        }
        public readonly static new Func<TagCompound, SubordinateGraph> DESERIALIZER = Load;
        public static new SubordinateGraph Load(TagCompound tag)
        {
            var instance = new SubordinateGraph();
            AssetUtils.LoadData_Dictionary(tag, "Dic_AdjacencyList", ref instance.Dic_AdjacencyList);

            instance.CustomDate = tag.GetCompound(nameof(CustomDate));
            instance.LoadData(instance.CustomDate);

            return instance;
        }
    }
}
