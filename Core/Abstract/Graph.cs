using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Core.Abstract
{
    public class Graph<TLink, TNode> : TagSerializable where TLink : Link, new() where TNode : Node, new()
    {
        protected Dictionary<AlchemyUnicode, AdjacencyList<TNode, TLink>> Dic_AdjacencyList;

        public Graph()
        {
            Dic_AdjacencyList = new Dictionary<AlchemyUnicode, AdjacencyList<TNode, TLink>>();
        }
        public bool AddLink(AlchemyEntity start, AlchemyEntity end, LinkCallback<TLink> edgeCallback)
        {
            if (Dic_AdjacencyList is null)
                return false;
            if (!Dic_AdjacencyList.ContainsKey(start.unicode) || !Dic_AdjacencyList.ContainsKey(end.unicode))
                return false;
            if (Dic_AdjacencyList[start.unicode].ConstainLink(end.unicode))
                return false;

            Dic_AdjacencyList[start.unicode].AddLink(end, edgeCallback);
            Dic_AdjacencyList[end.unicode].AdjacencyNodes.Add(start.unicode);
            return true;
        }
        public bool AddLink(AlchemyUnicode start, AlchemyUnicode end, LinkCallback<TLink> edgeCallback)
        {
            if (Dic_AdjacencyList is null)
                return false;
            if (!Dic_AdjacencyList.ContainsKey(start) || !Dic_AdjacencyList.ContainsKey(end))
                return false;
            if (Dic_AdjacencyList[start].ConstainLink(end))
                return false;
            var endInstance = Dic_AdjacencyList[end].Node.GetEntityInstance();
            Dic_AdjacencyList[start].AddLink(endInstance, edgeCallback);
            Dic_AdjacencyList[end].AdjacencyNodes.Add(start);
            return true;
        }
        public bool RemoveLink(AlchemyEntity start, AlchemyEntity end)
        {
            if (Dic_AdjacencyList is null)
                return false;
            if (!Dic_AdjacencyList.ContainsKey(start.unicode) || !Dic_AdjacencyList.ContainsKey(end.unicode))
                return false;
            if (!Dic_AdjacencyList[start.unicode].ConstainLink(end.unicode))
                return false;

            Dic_AdjacencyList[start.unicode].RemoveLink(end);
            return true;
        }
        public bool RemoveLink(AlchemyUnicode start, AlchemyUnicode end)
        {
            if (Dic_AdjacencyList is null)
                return false;
            if (!Dic_AdjacencyList.ContainsKey(start) || !Dic_AdjacencyList.ContainsKey(end))
                return false;
            if (!Dic_AdjacencyList[start].ConstainLink(end))
                return false;
            var endInstance = Dic_AdjacencyList[end].Node.GetEntityInstance();
            Dic_AdjacencyList[start].RemoveLink(endInstance);
            return true;
        }
        public bool TryGetConnectionRangeInNode(AlchemyUnicode unicode, ref List<TLink> result)
        {
            if (!Dic_AdjacencyList.ContainsKey(unicode))
                return false;
            result = Dic_AdjacencyList[unicode].AdjacencyLinkToList_Value();
            return true;

        }
        public bool TryGetConnectionRangeInNode(AlchemyEntity entity, ref List<TLink> result)
        {
            if (!Dic_AdjacencyList.ContainsKey(entity.unicode))
                return false;
            result = Dic_AdjacencyList[entity.unicode].AdjacencyLinkToList_Value();
            return true;

        }
        public void ForEach(NodesCollectionCallBack<TNode, TLink> callBack)
        {
            if (Dic_AdjacencyList is null)
                return;
            foreach (var _keyValuePair in Dic_AdjacencyList)
            {
                AdjacencyList<TNode, TLink> adjacencyList = _keyValuePair.Value;
                callBack?.Invoke(adjacencyList);
            }
        }
        public bool FindNode(AlchemyEntity entity, out TNode target)
        {
            target = null;
            if (!Dic_AdjacencyList.ContainsKey(entity.unicode))
                return false;
            target = Dic_AdjacencyList[entity.unicode].Node;
            return true;
        }
        public bool FindNode(AlchemyUnicode unicode, out TNode target)
        {
            target = null;
            if (!Dic_AdjacencyList.ContainsKey(unicode))
                return false;
            target = Dic_AdjacencyList[unicode].Node;
            return true;
        }
        public bool AddNode(AlchemyEntity entity)
        {
            if (Dic_AdjacencyList is null)
                return false;
            if (Dic_AdjacencyList.ContainsKey(entity.unicode))
                return false;
            var instance = new TNode();
            instance.SetEntity(entity);
            Dic_AdjacencyList.Add(entity.unicode, new AdjacencyList<TNode, TLink>(instance));
            return true;
        }

        public virtual bool RemoveNode(AlchemyUnicode unicode)
        {
            if (Dic_AdjacencyList is null)
                return false;
            if (!Dic_AdjacencyList.ContainsKey(unicode))
                return false;
            //需要删除自己的同时将其他节点与此节点的链接删除
            foreach (var AjN in Dic_AdjacencyList[unicode].AdjacencyNodes)
            {
                Dic_AdjacencyList[AjN].RemoveLink(unicode);
            }
            Dic_AdjacencyList.Remove(unicode);
            return true;
        }
        public bool RemoveNode(AlchemyEntity alchemyEntity)
        {
            return RemoveNode(alchemyEntity.unicode);
        }

        public bool IsEmpty()
        {
            return Dic_AdjacencyList.Count == 0;
        }
        public virtual void Clear()
        {
            Dic_AdjacencyList.Clear();
        }
        public int GetNodeCounts()
        {
            if (Dic_AdjacencyList is null)
                return -1;
            return Dic_AdjacencyList.Count;
        }
        public bool Contain(AlchemyUnicode unicode)
        {
            return Dic_AdjacencyList.ContainsKey(unicode);
        }
        #region //数据保存
        protected TagCompound CustomDate;

        public static Func<TagCompound, Graph<TLink, TNode>> DESERIALIZER = Load;
        public TagCompound SerializeData()
        {

            CustomDate = new TagCompound();
            SaveData(CustomDate);


            var instance = new TagCompound()
            {
                [nameof(CustomDate)] = CustomDate,
            };
            AssetUtils.SaveData_Dictionary(ref instance, ref Dic_AdjacencyList, nameof(Dic_AdjacencyList));
            return instance;
        }
        public static Graph<TLink, TNode> Load(TagCompound tag)
        {
            var instance = new Graph<TLink, TNode>();

            AssetUtils.LoadData_Dictionary(tag, nameof(Dic_AdjacencyList), ref instance.Dic_AdjacencyList);

            instance.CustomDate = tag.GetCompound(nameof(CustomDate));
            instance.LoadData(instance.CustomDate);
            return instance;
        }
        public virtual void SaveData(TagCompound tag)
        {

        }
        public virtual void LoadData(TagCompound tag)
        {

        }
        #endregion
    }
    //将node和以该node开头的link存储在一起，方便后面操作,将原本node中的大部分操作转移到这里
    public class AdjacencyList<TNode, TLink> : TagSerializable where TNode : Node, new() where TLink : Link, new()
    {
        public Dictionary<AlchemyUnicode, TLink> AdjacencyLinks;
        public List<AlchemyUnicode> AdjacencyNodes;
        public TNode Node;

        public bool UnicodeCurrect(AlchemyUnicode unicode_Compared)
        {
            return Node.GetUnicode() == unicode_Compared;
        }
        public AlchemyUnicode GetUnicode()
        {
            return Node.GetUnicode();
        }
        public AdjacencyList()
        {
            AdjacencyLinks = new Dictionary<AlchemyUnicode, TLink>();
            AdjacencyNodes = new List<AlchemyUnicode>();
        }
        public AdjacencyList(TNode node)
        {
            Node = node;
            AdjacencyLinks = new Dictionary<AlchemyUnicode, TLink>();
            AdjacencyNodes = new List<AlchemyUnicode>();
        }
        /// <summary>
        ///创建两个节点之间的链接，如果创建成功返回<see langword="true"/>，创建失败返回<see langword="false"/><br/>
        ///创建失败的原因：已存在该链接，无法添加一个重复的链接<br/>
        /// </summary>
        /// <param name="Endentity">链接实例的另外一个节点</param>
        /// <param name="edgeCallback">回调函数</param>
        /// <returns></returns>
        public bool AddLink(AlchemyEntity Endentity, LinkCallback<TLink> edgeCallback)
        {
            if (AdjacencyLinks.ContainsKey(Endentity.unicode))
                return false;
            var edgeInstance = new TLink();
            edgeInstance.start = Node.GetUnicode();
            edgeInstance.end = Endentity.unicode;
            edgeCallback?.Invoke(edgeInstance);
            AdjacencyLinks.Add(Endentity.unicode, edgeInstance);

            return true;
        }
        /// <summary>
        ///删除两个节点之间的链接，如果创建成功返回<see langword="true"/>，创建失败返回<see langword="false"/><br/>
        ///删除失败的原因：不存在该链接<br/>
        /// </summary>
        /// <param name="Endentity">链接实例的另外一个节点</param>
        /// <returns></returns>
        public bool RemoveLink(AlchemyEntity Endentity)
        {
            if (!AdjacencyLinks.ContainsKey(Endentity.unicode))
                return false;
            AdjacencyLinks.Remove(Endentity.unicode);
            return true;
        }

        public bool RemoveLink(AlchemyUnicode unicode)
        {
            if (!AdjacencyLinks.ContainsKey(unicode))
                return false;
            AdjacencyLinks.Remove(unicode);
            return true;
        }

        /// <summary>
        /// 遍历邻接表
        /// </summary>
        /// <param name="HowToDo">你要干什么</param>
        public void ForEachAdjacency(LinkCollectionCallback<TLink> HowToDo)
        {
            foreach (var connection in AdjacencyLinks)
            {
                HowToDo(connection);
            }
        }
        public bool FindLink(AlchemyUnicode alchemyUnicode, out TLink target)
        {
            target = null;
            if (!AdjacencyLinks.ContainsKey(alchemyUnicode))
                return false;
            target = AdjacencyLinks[alchemyUnicode];
            return true;
        }

        public bool ConstainLink(AlchemyUnicode unicode)
        {
            return AdjacencyLinks.ContainsKey(unicode);
        }
        public bool HasConnection()
        {
            return AdjacencyLinks.Count > 0;
        }
        public List<TLink> AdjacencyLinkToList_Value()
        {
            List<TLink> list = new List<TLink>();
            ForEachAdjacency((o) =>
            {
                list.Add(o.Value);
            });
            return list;
        }
        public List<AlchemyUnicode> AdjacencyToList_Key()
        {
            List<AlchemyUnicode> list = new List<AlchemyUnicode>();
            ForEachAdjacency((o) =>
            {
                list.Add(o.Key);
            });
            return list;
        }
        #region 数据存储
        public readonly static Func<TagCompound, AdjacencyList<TNode, TLink>> DESERIALIZER = Load;
        public TagCompound SerializeData()
        {
            var instance = new TagCompound()
            {
                [nameof(Node)] = Node,
                [nameof(AdjacencyNodes)] = AdjacencyNodes
            };
            AssetUtils.SaveData_Dictionary(ref instance, ref AdjacencyLinks, nameof(AdjacencyLinks));
            return instance;
        }
        public static AdjacencyList<TNode, TLink> Load(TagCompound tag)
        {
            var instance = new AdjacencyList<TNode, TLink>();
            instance.Node = tag.Get<TNode>(nameof(Node));
            instance.AdjacencyNodes = tag.Get<List<AlchemyUnicode>>(nameof(AdjacencyNodes));
            AssetUtils.LoadData_Dictionary(tag, nameof(AdjacencyLinks), ref instance.AdjacencyLinks);
            return instance;
        }
        #endregion
    }
    //图的节点,不存储邻接表
    public class Node : TagSerializable
    {
        protected AlchemyUnicode unicode;
        //实例成员
        protected AlchemyEntity entity { get { AlchemySystem.FindAlchemyEntitySafely<AlchemyEntity>(unicode, out var result); return result; } }

        public Node()
        {

        }
        public Node(AlchemyEntity alchemyEntity)
        {
            unicode = alchemyEntity.unicode;

        }
        public void SetEntity(AlchemyEntity entity)
        {
            this.unicode = entity.unicode;
        }
        public AlchemyEntity GetEntityInstance()
        {
            return entity;
        }
        public AlchemyUnicode GetUnicode()
        {
            return entity.unicode;
        }

        #region //数据保存
        protected TagCompound CustomDate;

        public readonly static Func<TagCompound, Node> DESERIALIZER = Load;
        public TagCompound SerializeData()
        {

            CustomDate = new TagCompound();
            SaveData(CustomDate);

            var instance = new TagCompound()
            {
                [nameof(CustomDate)] = CustomDate,

                [nameof(entity)] = entity.unicode,
            };
            return instance;
        }
        public static Node Load(TagCompound tag)
        {
            var instance = new Node();
            instance.CustomDate = tag.GetCompound(nameof(CustomDate));
            instance.unicode = tag.Get<AlchemyUnicode>(nameof(entity));


            instance.LoadData(instance.CustomDate);
            return instance;
        }
        public virtual void SaveData(TagCompound tag)
        {

        }
        public virtual void LoadData(TagCompound tag)
        {

        }
        #endregion
    }

    public class Link : TagSerializable
    {
        //关系的起点
        public AlchemyUnicode start;
        //关系的终点
        public AlchemyUnicode end;

        public int weight;

        public Link()
        {
        }
        public Link(AlchemyUnicode start, AlchemyUnicode end)
        {
            this.start = start;
            this.end = end;

        }
        public Link(AlchemyUnicode start, AlchemyUnicode end, int weight)
        {
            this.start = start;
            this.end = end;
            this.weight = weight;
        }

        public bool GetStartInstance<T>(out T target) where T : AlchemyEntity
        {
            return AlchemySystem.FindAlchemyEntitySafely<T>(start, out target);
        }
        public bool GetEndInstance<T>(out T target) where T : AlchemyEntity
        {
            return AlchemySystem.FindAlchemyEntitySafely<T>(end, out target);
        }
        #region //数据保存
        protected TagCompound CustomDate;
        public static readonly Func<TagCompound, Link> DESERIALIZER = Load;
        public TagCompound SerializeData()
        {
            CustomDate = new TagCompound();
            SaveDate(CustomDate);

            var instance = new TagCompound()
            {
                [nameof(start)] = start,
                [nameof(end)] = end,
                [nameof(weight)] = weight,
                [nameof(CustomDate)] = CustomDate

            };
            return instance;
        }
        public static Link Load(TagCompound tag)
        {
            var instance = new Link();
            instance.start = tag.Get<AlchemyUnicode>(nameof(start));
            instance.end = tag.Get<AlchemyUnicode>(nameof(end));
            instance.weight = tag.GetInt(nameof(weight));
            instance.CustomDate = tag.GetCompound(nameof(CustomDate));

            instance.LoadDate(instance.CustomDate);
            return instance;
        }
        public virtual void SaveDate(TagCompound tag)
        {

        }
        public virtual void LoadDate(TagCompound tag)
        {

        }
        #endregion
    }

    public delegate void LinkCallback<T>(T EgdeInstance) where T : Link;
    public delegate void LinkCollectionCallback<T>(KeyValuePair<AlchemyUnicode, T> EgdeInstance) where T : Link;
    public delegate void NodesCollectionCallBack<TNode, TLink>(AdjacencyList<TNode, TLink> keyValuePair) where TNode : Node, new() where TLink : Link, new();
}
