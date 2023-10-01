using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using MysteriousAlchemy.Buffs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Global.Element
{
    public enum ElemenType
    {
        Freeze, Burn, Radiant, Necrotic, Annihilate, Evolution
    }
    public class FreezeElement : BaseElement
    {
        public override string Name => "FreezeElement";
        public override void DefaultExtraEffectPerFrame(NPC npc)
        {
            base.DefaultExtraEffectPerFrame(npc);
        }
        public override void DefaultExtraEffectTrigger(NPC npc)
        {
            //两秒控制不能再多
            npc.AddBuff(ModContent.BuffType<NPCFreezeEffect>(), 120);
            npc.GetGlobalNPC<NPCElement>().freezeElement.EnhanceHitCount++;
            base.DefaultExtraEffectTrigger(npc);
        }
        public override void DefaultItemEffectModifyHit(NPC npc, Player player, Terraria.Item item, NPC.HitModifiers modifiers)
        {
            if (npc.defense < modifiers.SourceDamage.ApplyTo(item.damage) / 2f)
            {
                npc.GetGlobalNPC<NPCElement>().freezeElement.StatModifier *= 2;
                npc.GetGlobalNPC<NPCElement>().freezeElement.EnhanceHitCount--;
            }
            base.DefaultItemEffectModifyHit(npc, player, item, modifiers);
        }
        public override void DefaultProjectileModifyHit(NPC npc, Projectile projectile, NPC.HitModifiers modifiers)
        {
            if (npc.defense < modifiers.SourceDamage.ApplyTo(projectile.damage) / 2f)
            {
                npc.GetGlobalNPC<NPCElement>().freezeElement.StatModifier *= 2;
                npc.GetGlobalNPC<NPCElement>().freezeElement.EnhanceHitCount--;
            }
            base.DefaultProjectileModifyHit(npc, projectile, modifiers);
        }

    }
    public class BurnElement : BaseElement
    {
        public override string Name => "BurnElement";
        public override void DefaultExtraEffectPerFrame(NPC npc)
        {
            base.DefaultExtraEffectPerFrame(npc);
        }
        public override void DefaultExtraEffectTrigger(NPC npc)
        {
            base.DefaultExtraEffectTrigger(npc);
        }
        public override void DefaultItemEffectModifyHit(NPC npc, Player player, Terraria.Item item, NPC.HitModifiers modifiers)
        {
            base.DefaultItemEffectModifyHit(npc, player, item, modifiers);
        }
        public override void DefaultProjectileModifyHit(NPC npc, Projectile projectile, NPC.HitModifiers modifiers)
        {
            base.DefaultProjectileModifyHit(npc, projectile, modifiers);
        }

    }
    public class RadiantElement : BaseElement
    {
        public override string Name => "RadiantElement";
        public override void DefaultExtraEffectPerFrame(NPC npc)
        {
            base.DefaultExtraEffectPerFrame(npc);
        }
        public override void DefaultExtraEffectTrigger(NPC npc)
        {
            base.DefaultExtraEffectTrigger(npc);
        }
        public override void DefaultItemEffectModifyHit(NPC npc, Player player, Terraria.Item item, NPC.HitModifiers modifiers)
        {
            base.DefaultItemEffectModifyHit(npc, player, item, modifiers);
        }
        public override void DefaultProjectileModifyHit(NPC npc, Projectile projectile, NPC.HitModifiers modifiers)
        {
            base.DefaultProjectileModifyHit(npc, projectile, modifiers);
        }

    }
    public class NecroticElement : BaseElement
    {
        public override string Name => "NecroticElement";
        public override void DefaultExtraEffectPerFrame(NPC npc)
        {
            base.DefaultExtraEffectPerFrame(npc);
        }
        public override void DefaultExtraEffectTrigger(NPC npc)
        {
            base.DefaultExtraEffectTrigger(npc);
        }
        public override void DefaultItemEffectModifyHit(NPC npc, Player player, Terraria.Item item, NPC.HitModifiers modifiers)
        {
            base.DefaultItemEffectModifyHit(npc, player, item, modifiers);
        }

        public override void DefaultProjectileModifyHit(NPC npc, Projectile projectile, NPC.HitModifiers modifiers)
        {
            base.DefaultProjectileModifyHit(npc, projectile, modifiers);
        }

    }
    public class AnnihilateElement : BaseElement
    {
        public override string Name => "AnnihilateElement";
        public override void DefaultExtraEffectPerFrame(NPC npc)
        {
            base.DefaultExtraEffectPerFrame(npc);
        }
        public override void DefaultExtraEffectTrigger(NPC npc)
        {
            base.DefaultExtraEffectTrigger(npc);
        }
        public override void DefaultItemEffectModifyHit(NPC npc, Player player, Terraria.Item item, NPC.HitModifiers modifiers)
        {
            base.DefaultItemEffectModifyHit(npc, player, item, modifiers);
        }
        public override void DefaultProjectileModifyHit(NPC npc, Projectile projectile, NPC.HitModifiers modifiers)
        {
            base.DefaultProjectileModifyHit(npc, projectile, modifiers);
        }

    }
    public class EvolutionElement : BaseElement
    {
        public override string Name => "EvolutionElement";
        public override void DefaultExtraEffectPerFrame(NPC npc)
        {
            base.DefaultExtraEffectPerFrame(npc);
        }
        public override void DefaultExtraEffectTrigger(NPC npc)
        {
            base.DefaultExtraEffectTrigger(npc);
        }
        public override void DefaultItemEffectModifyHit(NPC npc, Player player, Terraria.Item item, NPC.HitModifiers modifiers)
        {
            base.DefaultItemEffectModifyHit(npc, player, item, modifiers);
        }
        public override void DefaultProjectileModifyHit(NPC npc, Projectile projectile, NPC.HitModifiers modifiers)
        {
            base.DefaultProjectileModifyHit(npc, projectile, modifiers);
        }

    }
}