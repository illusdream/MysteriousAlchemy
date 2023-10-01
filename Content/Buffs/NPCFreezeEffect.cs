using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Global.Element;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Buffs
{
    public class NPCFreezeEffect : ModBuff
    {
        public override string Texture => AssetUtils.Buffs + Name;
        public override void SetStaticDefaults()
        {
            // ��Ϊbuff�ϸ������ϲ���һ��TR�����Զ�����������ͣ�����û����buff.XXXX�������������Է�ʽ��
            // ������Ҫ������һ�ַ�ʽ��������

            // ������Ծ���buff����Ϸ�˳��ٽ�����᲻����Ȼ������true���ǲ��ᣬfalse���ǻ�
            Main.buffNoSave[Type] = false;

            // �����ж����buff�㲻��һ��debuff���������Ϊtrue��õ�TR�����debuff�����ƣ������޷�ȡ��
            Main.debuff[Type] = false;

            // �������buff�ܲ��ܱ�����ʿ���Ƹ��ɵ���true�ǲ����ԣ�false�����ȡ��
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;

            // �������buff�ǲ������������buff���Ժ󽲳�����ٻ����ʱ����õ��ģ���������Ϊfalse
            Main.lightPet[Type] = false;

            // �������buff�᲻����ʾ����ʱ�䣬false���ǻ���ʾ��true���ǲ�����ʾ��һ�����buff��������ʾ
            Main.buffNoTimeDisplay[Type] = false;

            // �������buff��ר��ģʽ�᲻�����ʱ��ӳ���false�ǲ��ᣬtrue�ǻ�
            // �������ʱ�䣬ר�ҷ�������ʦ����
            //BuffID.Sets.LongerExpertDebuff[Type] = false;

            // ����������Ϊtrue��pvp��ʱ��Ϳ��Ը����ּ������debuff/buff
            Main.pvpBuff[Type] = false;

            // �������Ƿ����buff��trueΪ�������false�����Ĭ�����
            Main.persistentBuff[Type] = false;

            // �������buff�ǲ���һ��װ���Գ�������ж��ģ���������buff��ʱ�򲻻�������
            Main.vanityPet[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            base.Update(player, ref buffIndex);
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            dust = Main.dust[Terraria.Dust.NewDust(npc.Center - npc.Hitbox.Size() / 2f, npc.width, npc.height, DustID.Frost, 0, 0, 0, new Color(255, 255, 255), 1.0465117f)];
            dust.noGravity = true;
            dust.fadeIn = 0.69767445f;
            if (npc.GetGlobalNPC<NPCElement>().freezeElement.EnhanceHitCount == 0)
            {
                npc.DelBuff(buffIndex);
            }
            base.Update(npc, ref buffIndex);
        }
    }
}