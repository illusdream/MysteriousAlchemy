using MysteriousAlchemy.Content.Alchemy.Graphs.Links;
using MysteriousAlchemy.Content.Alchemy.Graphs.Nodes;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Content.Alchemy.Graphs
{
    public class EtherGraph : Graph<EtherLink, EtherNode>
    {

        public void UpdateAllEtherHandler()
        {
            ForEach((o) =>
            {
                o.ForEachAdjacency((item) =>
                {
                    item.Value.HandlerEther(1);
                });
                o?.Node?.GetEntityInstance()?.Limit();
            });
        }
        public new TagCompound SerializeData()
        {

            CustomDate = new TagCompound();
            SaveData(CustomDate);


            var instance = new TagCompound()
            {
                [nameof(CustomDate)] = CustomDate,
            };
            AssetUtils.SaveData_Dictionary(ref instance, ref Dic_AdjacencyList, "Dic_AdjacencyList");
            return instance;
        }
        public readonly static new Func<TagCompound, EtherGraph> DESERIALIZER = Load_EtherGraph;
        public static EtherGraph Load_EtherGraph(TagCompound tag)
        {
            var instance = new EtherGraph();
            AssetUtils.LoadData_Dictionary(tag, "Dic_AdjacencyList", ref instance.Dic_AdjacencyList);

            instance.CustomDate = tag.GetCompound(nameof(CustomDate));
            instance.LoadData(instance.CustomDate);

            return instance;
        }
    }
}
