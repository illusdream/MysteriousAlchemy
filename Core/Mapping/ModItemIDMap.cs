using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Items;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Mapping
{
    public class ModItemIDMap : IOrderLoadable
    {
        private static Dictionary<string, int> ItemIDMap;

        //前置执行
        public int LoaderIndex => 4;
        public void Load()
        {
            ItemIDMap = new Dictionary<string, int>();
            //获取映射
            for (int i = 0; i < 2000; i++)
            {
                if (ItemLoader.GetItem(i + ItemID.Count)?.Mod?.Name == ModContent.GetModItem(ModContent.ItemType<ShiningRhizome>()).Mod.Name)
                {
                    if (!ItemIDMap.ContainsKey(ItemLoader.GetItem(i + ItemID.Count).Name))
                    {
                        ItemIDMap.Add(ItemLoader.GetItem(i + ItemID.Count).Name, ItemLoader.GetItem(i + ItemID.Count).Type);
                    }
                }
            }
        }

        public void Unload()
        {
            if (ItemIDMap != null)
                ItemIDMap = null;
        }
        public static Dictionary<string, int> GetModItemIDMap()
        {
            return ItemIDMap;
        }
        /// <summary>
        ///  获取moditemID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetModItemID(string name)
        {
            int id;
            foreach (var key in ItemIDMap)
            {
                if (key.Key == name)
                {
                    id = key.Value;
                    return id;
                }
            }
            return 0;
        }


    }
}