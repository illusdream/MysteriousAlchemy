using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.ExtraEffectEntity
{
    //特殊效果的基类
    public class ExtraEffectEntity
    {
        //世界坐标
        public Vector2 Position;
        //屏幕坐标
        public Vector2 ScreenPosition
        {
            get { return Position - Main.screenPosition; }
        }
        //速度
        public Vector2 Velocity;
        //是否用速度更新位置 
        public bool CanUpdatePosition = true;
        //额外更新次数
        public int extraUpdates;

        public virtual void AI()
        {

        }
        public virtual void Draw()
        {

        }
    }
}