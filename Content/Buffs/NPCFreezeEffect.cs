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
            // 因为buff严格意义上不是一个TR里面自定义的数据类型，所以没有像buff.XXXX这样的设置属性方式了
            // 我们需要用另外一种方式设置属性

            // 这个属性决定buff在游戏退出再进来后会不会仍然持续，true就是不会，false就是会
            Main.buffNoSave[Type] = false;

            // 用来判定这个buff算不算一个debuff，如果设置为true会得到TR里对于debuff的限制，比如无法取消
            Main.debuff[Type] = false;

            // 决定这个buff能不能被被护士治疗给干掉，true是不可以，false则可以取消
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;

            // 决定这个buff是不是照明宠物的buff，以后讲宠物和召唤物的时候会用到的，现在先设为false
            Main.lightPet[Type] = false;

            // 决定这个buff会不会显示持续时间，false就是会显示，true就是不会显示，一般宠物buff都不会显示
            Main.buffNoTimeDisplay[Type] = false;

            // 决定这个buff在专家模式会不会持续时间加长，false是不会，true是会
            // 这个持续时间，专家翻倍，大师三倍
            //BuffID.Sets.LongerExpertDebuff[Type] = false;

            // 如果这个属性为true，pvp的时候就可以给对手加上这个debuff/buff
            Main.pvpBuff[Type] = false;

            // 死亡后是否不清除buff，true为不清除，false清除，默认清除
            Main.persistentBuff[Type] = false;

            // 决定这个buff是不是一个装饰性宠物，用来判定的，比如消除buff的时候不会消除它
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