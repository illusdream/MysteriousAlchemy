using Microsoft.Xna.Framework;
using MonoMod.Core.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Interface
{
    //只负责物品相关的交互，IO部分具体类中再写
    public interface IItemContainer
    {
        //最大容量
        public int Capacity { get; set; }

        //获取所有存储的Item实例
        public List<Item> GetItems();
        //获取单个实例
        public Item GetItem(int index);
        public bool CanPushItem();
        //向内传输物品，如果全部传入则返回0，一个没传进去则返回-1，否则返回未传入的数目
        public int PushItem(Item item);
        //向外弹出物品，返回值为物品
        public bool CanPopItem();
        public Item PopItem(int index);
        //交换物品
        public bool CanHandleItem();
        public bool HandleItem(int index, Item InputItem);
    }
}