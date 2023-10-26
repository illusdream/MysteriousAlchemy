using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Abstract;
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

namespace MysteriousAlchemy.Core.Systems
{
    public class AlchemySystem : ModSystem
    {
        ///存储在世界中的<see cref="AlchemyEntity"/>并更新与绘制
        public static List<AlchemyEntity> alchemyUnicodeEntities = new List<AlchemyEntity>();

        public static Dictionary<AlchemyUnicode, AlchemyEntity> alchemyEntities = new Dictionary<AlchemyUnicode, AlchemyEntity>();

        public static bool TestVisable;

        //先于其他实体更新
        public override void PreUpdateEntities()
        {
            for (int i = 0; i < alchemyUnicodeEntities.Count; i++)
            {
                alchemyUnicodeEntities[i].Update();
            }
            base.PreUpdateEntities();
        }
        public static void TestDraw(SpriteBatch spriteBatch)
        {
            if (TestVisable)
            {
                for (int i = 0; i < alchemyUnicodeEntities.Count; i++)
                {
                    AlchemyEntity instance = alchemyUnicodeEntities[i];
                    Rectangle targetRect = new Rectangle((int)(instance.TopLeft.X - Main.screenPosition.X), (int)(instance.TopLeft.Y - Main.screenPosition.Y), (int)instance.Size.X, (int)instance.Size.Y);
                    spriteBatch.Draw(AssetUtils.WhitePic, targetRect, Color.White);
                }
            }
        }

        public static bool TryGetEtherEntity<T>(Vector2 target, out T result) where T : AlchemyEntity
        {
            result = null;
            for (int i = 0; i < alchemyUnicodeEntities.Count; i++)
            {
                AlchemyEntity instance = alchemyUnicodeEntities[i];
                if (MathUtils.Contain(instance.TopLeft, instance.Size, target))
                {
                    result = (T)instance;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 将不在世界中的<see cref="AlchemyEntity"/>转换到世界中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">需要转换的实例</param>
        /// <param name="postion">转换后的左上角位置</param>
        /// <returns></returns>
        public static void RegisterEtherEntity<T>(T instance, Vector2 postion) where T : AlchemyEntity
        {
            alchemyUnicodeEntities.Add(instance);
            instance.TopLeft = postion;
        }
        /// <summary>
        /// 将世界中的<see cref="AlchemyEntity"/> 转为可以被物品存储的形式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T TransEntityFormWorld<T>(Vector2 target) where T : AlchemyEntity
        {
            if (TryGetEtherEntity<T>(target, out T result))
            {
                return result;
            }
            return null;
        }

        #region //数据加载
        public override void LoadWorldData(TagCompound tag)
        {
            alchemyUnicodeEntities = tag.Get<List<AlchemyEntity>>(nameof(alchemyUnicodeEntities));

            base.LoadWorldData(tag);
        }
        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(alchemyUnicodeEntities)] = alchemyUnicodeEntities;

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
