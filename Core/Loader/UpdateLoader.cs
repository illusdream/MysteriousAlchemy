using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Loader
{
    public class UpdateLoader : Hook
    {
        public int LoaderIndex => 2;
        public static List<IUpdate> Updates;

        public static UpdateLoader Instance;
        public UpdateLoader()
        {
            Instance = this;
        }

        public void Load()
        {
            if (Main.dedServ)
            {
                return;
            }
            Updates = new List<IUpdate>();
            Mod Mod = MysteriousAlchemy.GetInstance();

            foreach (var type in Mod.Code.GetTypes())
            {
                if (!type.IsAbstract && type.GetInterfaces().Contains(typeof(IUpdate)))
                {
                    //创建实例
                    var instance = Activator.CreateInstance(type);
                    Updates.Add(instance as IUpdate);
                }
                //按加载顺序排序
                Updates.Sort((n, t) => n.UpdateIndex.CompareTo(t.UpdateIndex));
            }
            var updateMehtod = typeof(Main).GetMethod("DoUpdateInWorld", BindingFlags.Instance | BindingFlags.NonPublic);
            //初始化UpdateUnits并通过钩子挂载到原版DoUpdateInWorld后面（暂时）如果有问题我再搬其他地方去
            foreach (var Iupdate in Updates)
            {
                Iupdate.Load();

            }
            MonoModHooks.Add(updateMehtod, Update);
        }


        public void Update(On_Main.orig_DoUpdateInWorld orig, Main self, global::System.Diagnostics.Stopwatch sw)
        {
            orig(self, sw);
            foreach (var Iupdate in Updates)
            {
                Iupdate.Update();
            }
        }
        public void Unload()
        {
            if (Updates != null)
                Updates = null;
        }
    }


}
