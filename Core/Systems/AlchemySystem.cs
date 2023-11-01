using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.Alchemy.Graphs;
using MysteriousAlchemy.Content.Alchemy.Graphs.Links;
using MysteriousAlchemy.Content.Alchemy.Graphs.Nodes;
using MysteriousAlchemy.Content.UI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Loader;
using MysteriousAlchemy.Core.Timers;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using XPT.Core.Audio.MP3Sharp.Decoding;

namespace MysteriousAlchemy.Core.Systems
{
    public class AlchemySystem : ModSystem
    {
        ///只用于储存所有<see cref="AlchemyEntity"/>，并提供快速查询
        static Dictionary<AlchemyUnicode, AlchemyEntity> entities = new Dictionary<AlchemyUnicode, AlchemyEntity>();
        //暂时测试用

        public static EtherGraph etherGraph = new EtherGraph();
        public static SubordinateGraph subordinateGraph = new SubordinateGraph();

        public static bool TestVisable;

        public static AlchemyUnicode SelectUnicode;
        public static List<AlchemyEntity> HasBeenSelect = new List<AlchemyEntity>();
        public static Timer SelectTimer = TimerSystem.RegisterTimer<Timer>(1, 60);
        public static int WorldID { get { return Main.worldID; } }

        public override void Load()
        {
            base.Load();
        }
        public override void Unload()
        {
            entities = null;
            etherGraph = null;
            SelectTimer = null;
            HasBeenSelect = null;
            SelectTimer = null;
            base.Unload();
        }
        //先于其他实体更新
        public override void PreUpdateEntities()
        {
            foreach (var _instance in entities)
            {
                _instance.Value.Update();
            }
            etherGraph.UpdateAllEtherHandler();
            subordinateGraph.Update();



            SelectTimer ??= TimerSystem.RegisterTimer<Timer>(1, 60);
            if (SelectTimer.ConditionTrigger(true))
            {
                HasBeenSelect.Clear();

            }
            base.PreUpdateEntities();
        }
        public static void TestDraw(SpriteBatch spriteBatch)
        {
            if (TestVisable)
            {
                foreach (var _instance in entities)
                {
                    var instance = _instance.Value;
                    Rectangle targetRect = new Rectangle((int)(instance.TopLeft.X - Main.screenPosition.X), (int)(instance.TopLeft.Y - Main.screenPosition.Y), (int)instance.Size.X, (int)instance.Size.Y);
                    spriteBatch.Draw(AssetUtils.GetTexture2D(AssetUtils.Texture + "polartest"), targetRect, Color.White);
                }
                etherGraph?.ForEach((o) =>
                {
                    AlchemyEntity AE = o.Node.GetEntityInstance();
                    if (AE != null)
                    {
                        string adjnodes = new string("");
                        o.AdjacencyNodes.ForEach((d) =>
                        {
                            adjnodes += (d.value.ToString() + "  |  ");
                        });
                        DebugUtils.InfoWithCombat(AE.Center,
                                "unicode:" + AE.unicode.value,
                                "Center:" + AE.Center,
                                "Size:" + AE.Size,
                                "Active:" + AE.active,
                                "Ether:" + AE.Ether,
                                "MaxEther:" + AE.MaxEther,
                                "EtherLinkCount:" + o.AdjacencyLinks.Count,
                                "Type:" + o.Node.GetEntityInstance().GetType().ToString().Replace("MysteriousAlchemy.Content.Alchemy.AlchemyEntities.", " "),
                                "AdjNodes:" + adjnodes

    );
                    }

                });
            }
        }

        public static bool TryGetEtherEntity<T>(Vector2 target, out T result) where T : AlchemyEntity
        {
            SelectTimer ??= TimerSystem.RegisterTimer<Timer>(1, 60);
            result = null;
            foreach (var _entity in entities)
            {
                var entity = _entity.Value;
                if (MathUtils.Contain(entity.TopLeft, entity.Size, target) && !HasBeenSelect.Contains(entity))
                {
                    if (entity is T && entity.unicode.WorldID == WorldID)
                    {
                        result = (T)entity;
                        HasBeenSelect.Add(entity);
                    }

                    return true;
                }
            }
            HasBeenSelect.Clear();
            return false;
        }
        /// <summary>
        /// 将不在世界中的<see cref="AlchemyEntity"/>转换到世界中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">需要转换的实例</param>
        /// <param name="postion">转换后的左上角位置</param>
        /// <returns></returns>
        public static void RegisterAlchemyEntity<T>(Vector2 postion, AlchemyUnicode? SubTarget = null) where T : AlchemyEntity, new()
        {
            bool cancelRigister = false;
            var instance = new T();
            if (entities.ContainsKey(instance.unicode))
                cancelRigister = instance.ResetUnicode(entities.Keys.ToList());


            if (cancelRigister)
                return;

            instance.SetDefault();
            instance.active = true;
            instance.TopLeft = postion;
            instance.unicode.WorldID = WorldID;


            entities.Add(instance.unicode, instance);

            etherGraph.AddNode(instance);

            subordinateGraph.AddNode(instance);
            subordinateGraph.AddSubordinateLink(instance, SubTarget);

        }
        public static void RemoveAlchemyEntity(AlchemyUnicode unicode)
        {
            if (entities.ContainsKey(unicode))
            {
                entities.Remove(unicode);
                etherGraph.RemoveNode(unicode);
                subordinateGraph.RemoveNode(unicode);
            }

        }
        /// <summary>
        /// 将世界中的<see cref="AlchemyEntity"/> 转为可以被物品存储的形式(貌似现在没用了，因为所有<see cref="AlchemyEntity"/>都是保存在世界中的，存储被修改过的使用一个图来保存对应关系)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T TransEntityFormWorld<T>(Vector2 target) where T : AlchemyEntity
        {

            return null;
        }
        /// <summary>
        /// 找到对应的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="unicode"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool FindAlchemyEntitySafely<T>(AlchemyUnicode unicode, out T target) where T : AlchemyEntity
        {
            target = null;
            if (!entities.ContainsKey(unicode))
                return false;
            target = (T)entities[unicode];
            return true;
        }
        public static void ClearAllGraphAndEntity()
        {
            entities.Clear();
            etherGraph.Clear();
            subordinateGraph.Clear();
        }
        #region //数据加载
        public override void LoadWorldData(TagCompound tag)
        {
            AssetUtils.LoadData_Dictionary(tag, nameof(entities), ref entities);
            etherGraph = tag.Get<EtherGraph>(nameof(etherGraph));
            subordinateGraph = tag.Get<SubordinateGraph>(nameof(subordinateGraph));
            base.LoadWorldData(tag);
        }
        public override void SaveWorldData(TagCompound tag)
        {

            AssetUtils.SaveData_Dictionary(ref tag, ref entities, nameof(entities));
            tag.Add(nameof(etherGraph), etherGraph);
            tag.Add(nameof(subordinateGraph), subordinateGraph);
            base.SaveWorldData(tag);
        }
        #endregion
    }

    public class AlchemyTestConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("AlchemyTest")]
        [SeparatePage]

        [DefaultValue(true)]
        public bool TestVisable;

        public override void OnChanged()
        {
            SetValues();
        }
        public override void OnLoaded()
        {
            SetValues();
        }
        public void SetValues()
        {
            AlchemySystem.TestVisable = TestVisable;
        }
    }
}
