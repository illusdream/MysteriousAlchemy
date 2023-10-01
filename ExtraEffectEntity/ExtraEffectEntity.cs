using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.ExtraEffectEntity
{
    //����Ч���Ļ���
    public class ExtraEffectEntity
    {
        //��������
        public Vector2 Position;
        //��Ļ����
        public Vector2 ScreenPosition
        {
            get { return Position - Main.screenPosition; }
        }
        //�ٶ�
        public Vector2 Velocity;
        //�Ƿ����ٶȸ���λ�� 
        public bool CanUpdatePosition = true;
        //������´���
        public int extraUpdates;

        public virtual void AI()
        {

        }
        public virtual void Draw()
        {

        }
    }
}