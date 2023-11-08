using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Content.Alchemy.Graphs.Links
{
    public class EtherLink : Link
    {
        public float EtherCountPerFrame;
        public float MaxCount = 20 / 60f;
        public float MinCount = 0;
        public EtherLink()
        {
            EtherCountPerFrame = 0.01f;
        }
        public EtherLink(float etherCountPerFrame)
        {
            EtherCountPerFrame = MathHelper.Clamp(etherCountPerFrame, MinCount, MaxCount);
        }
        public void SetEtherCountPerFrame(float etherCountPerFrame)
        {
            EtherCountPerFrame = MathHelper.Clamp(etherCountPerFrame, MinCount, MaxCount);
        }
        public void HandlerEther(float statModify)
        {
            AlchemyEntity startInstance;
            AlchemyEntity endInstance;
            if (GetStartInstance(out startInstance) && GetEndInstance(out endInstance))
            {
                startInstance.ApplyEther(endInstance, EtherCountPerFrame);
                startInstance.Limit();
                endInstance.Limit();
            }

        }


        public static new readonly Func<TagCompound, EtherLink> DESERIALIZER = Load;
        public static new EtherLink Load(TagCompound tag)
        {
            var instance = new EtherLink();
            instance.start = tag.Get<AlchemyUnicode>(nameof(start));
            instance.end = tag.Get<AlchemyUnicode>(nameof(end));
            instance.weight = tag.GetInt(nameof(weight));
            instance.CustomDate = tag.GetCompound(nameof(CustomDate));

            instance.LoadDate(instance.CustomDate);
            return instance;
        }

        public override void SaveDate(TagCompound tag)
        {
            tag.Add(nameof(EtherCountPerFrame), EtherCountPerFrame);
            base.SaveDate(tag);
        }
        public override void LoadDate(TagCompound tag)
        {
            EtherCountPerFrame = tag.GetFloat(nameof(EtherCountPerFrame));
            base.LoadDate(tag);
        }
    }
}
