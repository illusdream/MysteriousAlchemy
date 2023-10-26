using MysteriousAlchemy.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Core.Abstract
{
    public class Graph<T> : TagSerializable where T : Edge, new()
    {
        Dictionary<AlchemyUnicode, Node<T>> Nodes;

        public bool AddConnection(AlchemyEntity start, AlchemyEntity end, EdgeCallback<T> edgeCallback)
        {
            if (Nodes is null)
                return false;
            if (!Nodes.ContainsKey(start.unicode) || !Nodes.ContainsKey(end.unicode))
                return false;
            if (Nodes[start.unicode].ConstainConnection(end.unicode))
                return false;

            Nodes[start.unicode].AddConnection(end, edgeCallback);
            return true;
        }
        public bool AddConnection(AlchemyUnicode start, AlchemyUnicode end, EdgeCallback<T> edgeCallback)
        {
            if (Nodes is null)
                return false;
            if (!Nodes.ContainsKey(start) || !Nodes.ContainsKey(end))
                return false;
            if (Nodes[start].ConstainConnection(end))
                return false;
            var endInstance = Nodes[end].GetEntityInstance();
            Nodes[start].AddConnection(endInstance, edgeCallback);
            return true;
        }
        public bool RemoveConnection(AlchemyEntity start, AlchemyEntity end)
        {
            if (Nodes is null)
                return false;
            if (!Nodes.ContainsKey(start.unicode) || !Nodes.ContainsKey(end.unicode))
                return false;
            if (!Nodes[start.unicode].ConstainConnection(end.unicode))
                return false;

            Nodes[start.unicode].RemoveConnection(end);
            return true;
        }
        public bool RemoveConnection(AlchemyUnicode start, AlchemyUnicode end)
        {
            if (Nodes is null)
                return false;
            if (!Nodes.ContainsKey(start) || !Nodes.ContainsKey(end))
                return false;
            if (!Nodes[start].ConstainConnection(end))
                return false;
            var endInstance = Nodes[end].GetEntityInstance();
            Nodes[start].RemoveConnection(endInstance);
            return true;
        }
        public void ForEach(NodesCollectionCallBack<T> callBack)
        {
            foreach (var _keyValuePair in Nodes)
            {
                callBack(_keyValuePair);
            }
        }
        public bool FindNode(AlchemyEntity entity, out Node<T> target)
        {
            target = null;
            if (!Nodes.ContainsKey(entity.unicode))
                return false;
            target = Nodes[entity.unicode];
            return true;
        }
        public bool FindNode(AlchemyUnicode unicode, out Node<T> target)
        {
            target = null;
            if (!Nodes.ContainsKey(unicode))
                return false;
            target = Nodes[unicode];
            return true;
        }
        public bool AddNode(AlchemyEntity entity)
        {
            if (Nodes is null)
                return false;
            if (Nodes.ContainsKey(entity.unicode))
                return false;
            var instance = new Node<T>(entity);
            Nodes.Add(entity.unicode, instance);
            return true;
        }

        public bool RemoveNode(AlchemyUnicode unicode)
        {
            if (Nodes is null)
                return false;
            if (Nodes.ContainsKey(unicode))
                return false;
            //需要删除自己的同时将其他节点与此节点的链接删除
            foreach (var _node in Nodes)
            {
                var node = _node.Value;
                node.RemoveConnection(unicode);
            }
            Nodes.Remove(unicode);
            return true;
        }
        public bool RemoveNode(AlchemyEntity alchemyEntity)
        {
            return RemoveNode(alchemyEntity.unicode);
        }
        #region //数据保存
        TagCompound CustomDate;

        public static readonly Func<TagCompound, Graph<T>> DESERIALIZER = Load;
        public TagCompound SerializeData()
        {
            var list = new List<TagCompound>();
            foreach (var data in Nodes)
            {
                list.Add(new TagCompound()
                {
                    ["key"] = data.Key,
                    ["value"] = data.Value
                });
            }

            CustomDate = new TagCompound();
            SaveData(CustomDate);


            var instance = new TagCompound()
            {
                [nameof(CustomDate)] = CustomDate
            };
            return instance;
        }
        public static Graph<T> Load(TagCompound tag)
        {
            var instance = new Graph<T>();


            instance.Nodes ??= new Dictionary<AlchemyUnicode, Node<T>>();
            var list = tag.GetList<TagCompound>(nameof(Nodes));
            foreach (var data in list)
            {
                AlchemyUnicode unicode = data.Get<AlchemyUnicode>("key");
                Node<T> edge = data.Get<Node<T>>("value");
                instance.Nodes[unicode] = edge;
            }





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

    //图的节点
    public class Node<T> : TagSerializable where T : Edge, new()
    {
        //这两个变量不希望公开，图内部的元素应该保持相对的稳定性，用Node自身提供的函数以及Graph提供的函数保持内部的封装
        //实例成员
        AlchemyEntity entity;
        //邻接表
        Dictionary<AlchemyUnicode, T> Adjacency;

        public Node()
        {

        }
        public Node(AlchemyEntity alchemyEntity)
        {
            entity = alchemyEntity;
            Adjacency = new Dictionary<AlchemyUnicode, T>();
        }
        public AlchemyEntity GetEntityInstance()
        {
            return entity;
        }
        /// <summary>
        ///创建两个节点之间的链接，如果创建成功返回<see langword="true"/>，创建失败返回<see langword="false"/><br/>
        ///创建失败的原因：已存在该链接，无法添加一个重复的链接<br/>
        /// </summary>
        /// <param name="Endentity">链接实例的另外一个节点</param>
        /// <param name="edgeCallback">回调函数</param>
        /// <returns></returns>
        public bool AddConnection(AlchemyEntity Endentity, EdgeCallback<T> edgeCallback)
        {
            if (Adjacency.ContainsKey(Endentity.unicode))
                return false;

            var edgeInstance = new T();
            edgeInstance.start = this.entity.unicode;
            edgeInstance.end = Endentity.unicode;
            edgeCallback(edgeInstance);
            Adjacency.Add(Endentity.unicode, edgeInstance);
            return true;
        }

        /// <summary>
        ///删除两个节点之间的链接，如果创建成功返回<see langword="true"/>，创建失败返回<see langword="false"/><br/>
        ///删除失败的原因：不存在该链接<br/>
        /// </summary>
        /// <param name="Endentity">链接实例的另外一个节点</param>
        /// <returns></returns>
        public bool RemoveConnection(AlchemyEntity Endentity)
        {
            if (!Adjacency.ContainsKey(Endentity.unicode))
                return false;
            Adjacency.Remove(Endentity.unicode);
            return true;
        }
        public bool RemoveConnection(AlchemyUnicode unicode)
        {
            if (!Adjacency.ContainsKey(unicode))
                return false;
            Adjacency.Remove(unicode);
            return true;
        }
        /// <summary>
        /// 遍历邻接表
        /// </summary>
        /// <param name="HowToDo">你要干什么</param>
        public void ForEachAdjacency(EdgeCollectionCallback<T> HowToDo)
        {
            foreach (var connection in Adjacency)
            {
                HowToDo(connection);
            }
        }
        public bool Find(AlchemyUnicode alchemyUnicode, out T target)
        {
            target = null;
            if (!Adjacency.ContainsKey(alchemyUnicode))
                return false;
            target = Adjacency[alchemyUnicode];
            return true;
        }
        public bool ConstainConnection(AlchemyUnicode unicode)
        {
            return Adjacency.ContainsKey(unicode);
        }
        public bool HasConnection()
        {
            return Adjacency.Count > 0;
        }
        #region //数据保存
        TagCompound CustomDate;

        public static readonly Func<TagCompound, Node<T>> DESERIALIZER = Load;
        public TagCompound SerializeData()
        {
            var list = new List<TagCompound>();
            foreach (var data in Adjacency)
            {
                list.Add(new TagCompound()
                {
                    ["key"] = data.Key,
                    ["value"] = data.Value
                });
            }

            CustomDate = new TagCompound();
            SaveData(CustomDate);

            var instance = new TagCompound()
            {
                [nameof(CustomDate)] = CustomDate,

                [nameof(entity)] = entity,
                [nameof(Adjacency)] = list

            };
            return instance;
        }
        public static Node<T> Load(TagCompound tag)
        {
            var instance = new Node<T>();
            instance.CustomDate = tag.GetCompound(nameof(CustomDate));
            instance.entity = tag.Get<AlchemyEntity>(nameof(entity));
            instance.Adjacency ??= new Dictionary<AlchemyUnicode, T>();
            var list = tag.GetList<TagCompound>(nameof(Adjacency));
            foreach (var data in list)
            {
                AlchemyUnicode unicode = data.Get<AlchemyUnicode>("key");
                T edge = data.Get<T>("value");
                instance.Adjacency[unicode] = edge;
            }


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

    public class Edge : TagSerializable
    {
        //关系的起点
        public AlchemyUnicode start;
        //关系的终点
        public AlchemyUnicode end;

        public int weight;

        public Edge()
        {
        }
        public Edge(AlchemyUnicode start, AlchemyUnicode end)
        {
            this.start = start;
            this.end = end;

        }
        public Edge(AlchemyUnicode start, AlchemyUnicode end, int weight)
        {
            this.start = start;
            this.end = end;
            this.weight = weight;
        }

        public bool GetStartInstance<T>(out T target) where T : AlchemyEntity
        {
            target = null;
            if (!AlchemySystem.alchemyEntities.ContainsKey(start))
                return false;
            target = (T)AlchemySystem.alchemyEntities[start];
            return true;
        }
        public bool GetEndInstance<T>(out T target) where T : AlchemyEntity
        {
            target = null;
            if (!AlchemySystem.alchemyEntities.ContainsKey(end))
                return false;
            target = (T)AlchemySystem.alchemyEntities[end];
            return true;
        }
        #region //数据保存
        TagCompound CustomDate;
        public static readonly Func<TagCompound, Edge> DESERIALIZER = Load;
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
        public static Edge Load(TagCompound tag)
        {
            var instance = new Edge();
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

    public delegate void EdgeCallback<T>(T EgdeInstance) where T : Edge;
    public delegate void EdgeCollectionCallback<T>(KeyValuePair<AlchemyUnicode, T> EgdeInstance) where T : Edge;
    public delegate void NodesCollectionCallBack<T>(KeyValuePair<AlchemyUnicode, Node<T>> keyValuePair) where T : Edge, new();
}
