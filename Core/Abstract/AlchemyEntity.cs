using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Core.Perfab.AEAnimator;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Core.Abstract
{
    //炼金术相关基类,可保存
    public class AlchemyEntity : TagSerializable, IEtherContainer, IStateMachine
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
                [nameof(unicode)] = unicode,
                [nameof(Name)] = Name

            };
            return instance;
        }
        public static AlchemyEntity Load(TagCompound tag)
        {
            return InstanceLoad<AlchemyEntity>(tag);
        }
        public static T InstanceLoad<T>(TagCompound tag) where T : AlchemyEntity, new()
        {
            var instance = new T();
            instance.Ether = tag.GetFloat(nameof(Ether));
            instance.MaxEther = tag.GetFloat(nameof(MaxEther));
            instance.active = tag.GetBool(nameof(active));
            instance.TopLeft = tag.Get<Vector2>(nameof(TopLeft));
            instance.Size = tag.Get<Vector2>(nameof(Size));
            instance.unicode = tag.Get<AlchemyUnicode>(nameof(unicode));
            instance.Name = tag.GetString(nameof(Name));
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

        public string Name { get; set; } = "";
        public float Ether { get; set; }
        public float MaxEther { get; set; }
        //达到该值后触发效果
        public float EtherTriggerCount { get; set; }
        //这个EtherEntity是否活跃，如果在世界中且不活跃清除该EE，如果从背包中放置到世界中，则强制变为true
        public bool active { get; set; }
        //是否被激活，如果激活则更新等等需要主动的函数
        public bool activate { get; set; }

        public Vector2 TopLeft;

        public Vector2 Size = new Vector2(16, 16);

        public Vector2 Center
        {
            get
            {
                return TopLeft + Size / 2f;
            }
        }

        //1--9999的整数，用于在图中确定对应的实体 ,0为默认值，代表无
        public AlchemyUnicode unicode;


        public virtual Type AnimatorType => typeof(BaseAEAnimator);
        private Animator _animator;
        public Animator Animator
        {
            get
            {
                if (_animator != null)
                {
                    return _animator;
                }
                else
                {
                    Core.Abstract.Animator instance = AnimatorManager.Instance.FindAnimator("AE" + unicode.ToString());
                    return instance ?? AnimatorCreate();
                }
            }
            set
            {
                _animator = value;
            }
        }


        public virtual string Icon => AssetUtils.UI_Alchemy + "AEmciroIcon_0";

        public IState CurrectState { get; set; }
        public Dictionary<string, IState> States { get; set; }

        public AlchemyEntity()
        {
            unicode = new AlchemyUnicode();
            AnimatorCreate();
            Initialize();
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
        /// <summary>
        /// 重写该函数的时候最好把Base放前面，base内写了创建动画机部分，不写就自己实现
        /// </summary>
        public virtual Animator AnimatorCreate()
        {
            if (!AnimatorType.IsSubclassOf(typeof(BaseAEAnimator)) && AnimatorType != typeof(BaseAEAnimator))
            {
                throw new ArgumentException("AE动画机类型不对，检查AnimatorType");
            }
            Animator instance = (Animator)Activator.CreateInstance(AnimatorType, Center);

            instance.Name = "AE" + unicode.ToString();

            AnimatorManager.Instance.AddAnimator(instance);
            Animator = instance;
            return instance;
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
        public virtual void Trigger()
        {

        }

        /// <summary>
        /// 逻辑更新
        /// </summary>
        public void Update()
        {
            AI();
            OnUpdate();
        }
        public virtual void OnUpdate()
        {

        }
        /// <summary>
        /// 同步动画机与AE状态与位置
        /// </summary>
        public void AnimatorSyncing()
        {
            if (Animator != null)
            {
                Animator.Position = Center;
                AnimatorUpdate();
            }

        }
        public virtual void AnimatorUpdate()
        {

        }
        public void RemoveFromWorld()
        {
            RemoveAnimator();
        }
        public virtual void OnRemoveFromWorld()
        {

        }
        public void RemoveAnimator()
        {
            OnRemoveAnimator();
            AnimatorManager.Instance.RemoveAnimator(Animator);
        }
        public virtual void OnRemoveAnimator()
        {

        }
        /// <summary>
        /// 当该<see cref="AlchemyEntity"/>出现在世界中的绘制，特指在方块中存储或者在被实体化MagicCircle中存储
        /// </summary>


        public virtual bool ResetUnicode(HashSet<AlchemyUnicode> needExclude)
        {

            bool Repeat = false;
            while (true)
            {
                unicode = new AlchemyUnicode();
                if (!needExclude.Contains(unicode))
                    return true;
            }
        }
        #region 动画机相关
        public void RegisterState<T>(T state) where T : IState
        {
            States ??= new Dictionary<string, IState>();
            if (state.ModifyName(out string name))
            {
                if (States.ContainsKey(name))
                    throw new ArgumentException("已被注册");
                States.Add(name, state);
                return;
            }
            if (States.ContainsKey(typeof(T).ToString()))
                throw new ArgumentException("已被注册");
            States.Add(typeof(T).ToString(), state);
        }

        public void SetState(string Name)
        {
            if (!States.ContainsKey(Name)) throw new ArgumentException("该状态并不存在");
            if (States.ContainsKey(Name))
                CurrectState = States[Name];
        }

        public void SwitchState(string Name)
        {
            CurrectState.ExitState(this);
            if (!States.ContainsKey(Name)) throw new ArgumentException("该状态并不存在");
            States[Name].EntryState(this);
            SetState(Name);
        }

        public virtual void Initialize()
        {
            RegisterState<AlchemyEntityState_Deactive>(new AlchemyEntityState_Deactive(this));
            RegisterState<AlchemyEntityState_EntryActive>(new AlchemyEntityState_EntryActive(this));
            RegisterState<AlchemyEntityState_Active>(new AlchemyEntityState_Active(this));
            RegisterState<AlchemyEntityState_ExitActive>(new AlchemyEntityState_ExitActive(this));
            SetState("Deactive");
            //先执行一遍进入状态函数
            CurrectState.EntryState(this);
        }

        public virtual void AI()
        {
            CurrectState?.OnState(this);
        }
        #endregion
    }

    public struct AlchemyUnicode : TagSerializable
    {
        int _unicode;

        int worldID = 0;
        //用于检测是否在这个世界，-（世界ID）为不在这个世界但是正常执行更新函数，0为被玩家存储在背包中，正常执行更新内容，绘制执行玩家内绘制，正数为在这个世界中，正常更新并绘制
        public int value
        {
            get { return _unicode; }
            set
            {
                _unicode = Math.Clamp(value, 1, 9999);
            }
        }
        public int WorldID { get { return worldID; } set { worldID = value; } }
        public AlchemyUnicode()
        {
            _unicode = Main.rand.Next(1, 9999);
        }
        private AlchemyUnicode(int unicode)
        {
            _unicode = unicode;
        }

        public static AlchemyUnicode Zero = new AlchemyUnicode(0);

        public static bool operator ==(AlchemyUnicode u1, AlchemyUnicode u2)
        {
            return u1.value == u2.value;
        }
        public static bool operator !=(AlchemyUnicode u1, AlchemyUnicode u2)
        {
            return u1.value != u2.value;
        }
        public override string ToString()
        {
            return value.ToString();
        }





        public static readonly Func<TagCompound, AlchemyUnicode> DESERIALIZER = Load;
        public TagCompound SerializeData()
        {
            var instance = new TagCompound()
            {
                [nameof(_unicode)] = _unicode,
                [nameof(worldID)] = worldID
            };
            return instance;
        }
        public static AlchemyUnicode Load(TagCompound tag)
        {
            var instance = new AlchemyUnicode();
            instance._unicode = tag.GetInt(nameof(_unicode));
            instance.worldID = tag.GetInt(nameof(worldID));
            return instance;
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }
            return ((AlchemyUnicode)obj)._unicode == _unicode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_unicode);
        }
    }
}
