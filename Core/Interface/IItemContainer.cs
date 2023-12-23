using Microsoft.Xna.Framework;
using MonoMod.Core.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Interface
{
    //ֻ������Ʒ��صĽ�����IO���־���������д
    public interface IItemContainer
    {
        //�������
        public int Capacity { get; set; }

        //��ȡ���д洢��Itemʵ��
        public List<Item> GetItems();
        //��ȡ����ʵ��
        public Item GetItem(int index);
        public bool CanPushItem();
        //���ڴ�����Ʒ�����ȫ�������򷵻�0��һ��û����ȥ�򷵻�-1�����򷵻�δ�������Ŀ
        public int PushItem(Item item);
        //���ⵯ����Ʒ������ֵΪ��Ʒ
        public bool CanPopItem();
        public Item PopItem(int index);
        //������Ʒ
        public bool CanHandleItem();
        public bool HandleItem(int index, Item InputItem);
    }
}