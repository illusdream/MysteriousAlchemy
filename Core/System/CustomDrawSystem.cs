using MonoMod.RuntimeDetour;
using MysteriousAlchemy.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hook = MysteriousAlchemy.Core.Interface.Hook;

namespace MysteriousAlchemy.Core.System
{
    //自定义的绘制系统
    //拖尾之类的暂时不做迁移——还没想好
    //目前功能 ：对话相关的绘制，部分场景，第一次拿起武器的文字描述
    //           自定义粒子系统的绘制
    //           彩色字体可能单独拿出来绘制——一份特效，一份UI 如果需要做到部分字体被拖尾遮住可能需要，但是加个条件判断不就好了吗
    //           Default和Immediate分开绘制，防止多次end，begin
    public class CustomDrawSystem : Hook
    {

        public int LoaderIndex => 1;

        public void Load()
        {

        }

        public void Unload()
        {

        }
    }
}
