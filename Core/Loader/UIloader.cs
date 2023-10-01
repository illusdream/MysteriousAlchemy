using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MysteriousAlchemy.Core.Interface;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.UI;

namespace MysteriousAlchemy.Core.Loader
{
    internal class UIloader : IOrderLoadable
    {
        //UIState
        public static List<BetterUIState> BetterUIStates;
        //
        public static List<UserInterface> UserInterfaces;


        //靠后加载
        public int LoaderIndex => 0;

        public void Load()
        {
            if (Main.dedServ)
            {
                return;
            }
            BetterUIStates = new List<BetterUIState>();
            UserInterfaces = new List<UserInterface>();

            Mod mod = MysteriousAlchemy.GetInstance();

            foreach (var loadable in AssemblyManager.GetLoadableTypes(mod.Code))
            {
                if (loadable.IsSubclassOf(typeof(BetterUIState)))
                {
                    var instance = (BetterUIState)Activator.CreateInstance(loadable);
                    var userInterface = new UserInterface();
                    userInterface.SetState(instance);

                    BetterUIStates?.Add(instance);
                    UserInterfaces?.Add(userInterface);
                }
            }

        }

        public void Unload()
        {
            if (BetterUIStates != null)
            {
                BetterUIStates.ForEach((state) => state.Unload());
            }
            BetterUIStates = null;
            UserInterfaces = null;
        }

        public static void AddLayer(List<GameInterfaceLayer> layers, UserInterface userInterface, UIState state, int index, bool visible, InterfaceScaleType scale)
        {
            string name = state == null ? "Unknown" : state.ToString();
            layers.Insert(index, new LegacyGameInterfaceLayer("MysteriousAlchemy_" + name,
                delegate
                {
                    if (visible)
                    {
                        userInterface.Update(Main._drawInterfaceGameTime);
                        state.Draw(Main.spriteBatch);
                    }
                    return true;
                }, scale));
        }
        /// <summary>
        /// 用于获取UIState
        /// </summary>
        /// <typeparam name="T">UI状态</typeparam>
        /// <returns></returns>
        public static T GetUIState<T>() where T : BetterUIState => BetterUIStates.FirstOrDefault(n => n is T) as T;

        /// <summary>
        /// 用于获取UserInterface
        /// </summary>
        /// <typeparam name="T">UI状态</typeparam>
        /// <returns></returns>
        public static UserInterface GetUserInterface<T>() where T : BetterUIState => UserInterfaces.FirstOrDefault(n => n.CurrentState is T);
    }
    class UISystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            for (int k = 0; k < UIloader.BetterUIStates.Count; k++)
            {
                var state = UIloader.BetterUIStates[k];
                UIloader.AddLayer(layers, UIloader.UserInterfaces[k], state, state.UILayer(layers), state.Visable, state.Scale);
            }
        }
    }
}
