using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Core.Abstract
{
    //炼金术相关基类,可保存
    public class AlchemyEntity : TagSerializable, IEtherContainer
    {

        #region //数据保存
        private TagCompound CustomData;
        public static readonly Func<TagCompound, AlchemyEntity> DESERIALIZER = Load;
        public TagCompound SerializeData()
        {
            //存储额外的数据
            CustomData = new TagCompound();
            SaveDate(CustomData);

            var instance = new TagCompound()
            {
                [nameof(Ether)] = Ether,
                [nameof(MaxEther)] = MaxEther,
                [nameof(active)] = active,
                [nameof(TopLeft)] = TopLeft,
                [nameof(Size)] = Size,
                [nameof(CustomData)] = CustomData,
                [nameof(unicode)] = unicode

            };
            return instance;
        }
        public static AlchemyEntity Load(TagCompound tag)
        {
            var instance = new AlchemyEntity();
            instance.Ether = tag.GetFloat(nameof(Ether));
            instance.MaxEther = tag.GetFloat(nameof(MaxEther));
            instance.active = tag.GetBool(nameof(active));
            instance.TopLeft = tag.Get<Vector2>(nameof(TopLeft));
            instance.Size = tag.Get<Vector2>(nameof(Size));
            instance.unicode = tag.Get<AlchemyUnicode>(nameof(unicode));
            instance.CustomData = tag.Get<TagCompound>(nameof(CustomData));

            //额外的数据加载
            instance.LoadDate(instance.CustomData);

            return instance;
        }
        public virtual void SaveDate(TagCompound tag)
        {
        }
        public virtual void LoadDate(TagCompound tag)
        {

        }
        #endregion


        public float Ether { get; set; }
        public float MaxEther { get; set; }
        //达到该值后触发效果
        public float EtherTriggerCount { get; set; }
        //这个EtherEntity是否活跃，如果在世界中且不活跃清除该EE，如果从背包中放置到世界中，则强制变为true
        public bool active { get; set; }
        //是否被激活，如果激活则更新等等需要主动的函数
        public bool activate { get; set; }

        public Vector2 TopLeft;

        public Vector2 Size;

        public Vector2 Center
        {
            get
            {
                return TopLeft + Size / 2f;
            }
        }
        //0--9999的整数，用于在图中确定对应的实体
        public AlchemyUnicode unicode;

        public AlchemyEntity()
        {
            unicode = new AlchemyUnicode();
        }
        public void Limit()
        {
            Ether = Math.Clamp(Ether, 0, MaxEther);
        }

        public void ApplyEther(IEtherContainer etherContainer, float ether)
        {
            if (!active || !etherContainer.active || (Ether - ether) < 0)
                return;

            Ether -= ether;
            Limit();

            etherContainer.ReciveEther(ether);
            etherContainer.Limit();
        }

        public void ReciveEther(float ether)
        {
            Ether += ether;
            Limit();
        }
        /// <summary>
        /// 设定初始值
        /// </summary>
        public virtual void SetDefault()
        {

        }


        public virtual bool EtherTrigger()
        {
            if (Ether >= EtherTriggerCount || active)
            {

                OnEtherTrigger();
                return true;
            }
            return false;
        }
        public virtual void OnEtherTrigger()
        {

        }


        /// <summary>
        /// 每帧更新
        /// </summary>
        public virtual void Update()
        {
            DebugUtils.NewText(unicode.value);
        }
        /// <summary>
        /// 当该<see cref="AlchemyEntity"/>出现在世界中的绘制，特指在方块中存储或者在被实体化MagicCircle中存储
        /// </summary>
        public virtual void DrawInWorld()
        {

        }
        /// <summary>
        /// 对背包部分的绘制
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="drawColor"></param>
        /// <param name="itemColor"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        public virtual void DrawInInventory(SpriteBatch spriteBatch, Vector2 position, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {

        }


    }

    public struct AlchemyUnicode : TagSerializable
    {
        int _unicode;

        public int value
        {
            get { return _unicode; }
            set
            {
                _unicode = Math.Clamp(value, 0, 9999);
            }
        }

        public AlchemyUnicode()
        {
            _unicode = Main.rand.Next(0, 10000);
        }
        public static bool operator ==(AlchemyUnicode u1, AlchemyUnicode u2)
        {
            return u1.value == u2.value;
        }
        public static bool operator !=(AlchemyUnicode u1, AlchemyUnicode u2)
        {
            return u1.value != u2.value;
        }

        public static readonly Func<TagCompound, AlchemyUnicode> DESERIALIZER = Load;
        public TagCompound SerializeData()
        {
            var instance = new TagCompound()
            {
                [nameof(_unicode)] = _unicode,
            };
            return instance;
        }
        public static AlchemyUnicode Load(TagCompound tag)
        {
            var instance = new AlchemyUnicode();
            instance._unicode = tag.GetInt(nameof(_unicode));
            return instance;
        }
    }
}
