using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Global.Element
{
    public class NPCElement : GlobalNPC
    {

        public FreezeElement freezeElement = new FreezeElement();
        public BurnElement burnElement = new BurnElement();
        public RadiantElement radiantElement = new RadiantElement();
        public NecroticElement necroticElement = new NecroticElement();
        public AnnihilateElement annihilateElement = new AnnihilateElement();
        public EvolutionElement evolutionElement = new EvolutionElement();
        private BaseElement[] Elements = null;
        public override bool InstancePerEntity => true;
        public bool CanAI;
        public override void SpawnNPC(int npc, int tileX, int tileY)
        {

            base.SpawnNPC(npc, tileX, tileY);
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            base.OnSpawn(npc, source);
        }
        public override void Load()
        {
            Elements = new BaseElement[6] { freezeElement, burnElement, radiantElement, necroticElement, annihilateElement, evolutionElement };
            base.Load();
        }
        public override void ModifyHitByItem(NPC npc, Player player, Terraria.Item item, ref NPC.HitModifiers modifiers)
        {
            if (Elements == null)
            {
                Elements = new BaseElement[6] { freezeElement, burnElement, radiantElement, necroticElement, annihilateElement, evolutionElement };
            }
            for (int i = 0; i < Elements.Length; i++)
            {
                if (Elements[i].EnhanceHitCount > 0)
                {
                    Elements[i].ItemEffectModifyHit(npc, player, item, modifiers);
                    //将修改数据传给hitModifiers
                    modifiers.SourceDamage = modifiers.SourceDamage.CombineWith(Elements[i].StatModifier);
                    //重置元素的modifiers 
                    Elements[i].StatModifier = new StatModifier();
                }
            }
            base.ModifyHitByItem(npc, player, item, ref modifiers);
        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (Elements == null)
            {
                Elements = new BaseElement[6] { freezeElement, burnElement, radiantElement, necroticElement, annihilateElement, evolutionElement };
            }
            for (int i = 0; i < Elements.Length; i++)
            {
                if (Elements[i].EnhanceHitCount > 0)
                {
                    Elements[i].ProjectileModifyHit(npc, projectile, modifiers);
                    //将修改数据传给hitModifiers
                    modifiers.SourceDamage = modifiers.SourceDamage.CombineWith(Elements[i].StatModifier);
                    //重置元素的modifiers 
                    Elements[i].StatModifier = new StatModifier();
                }
            }
            base.ModifyHitByProjectile(npc, projectile, ref modifiers);

        }
        //用于执行每帧执行的元素效果
        public override bool PreAI(NPC npc)
        {
            if (base.PreAI(npc))
            {
                if (Elements == null)
                {
                    Elements = new BaseElement[6] { freezeElement, burnElement, radiantElement, necroticElement, annihilateElement, evolutionElement };
                }
                //每帧更新
                for (int i = 0; i < Elements.Length; i++)
                {
                    Elements[i].ExtraEffectPerFrame(npc);
                }
                //触发效果
                for (int i = 0; i < Elements.Length; i++)
                {
                    if (Elements[i].IsTrigger())
                    {
                        Elements[i].ExtraEffectTrigger(npc);
                    }
                }
            }

            return base.PreAI(npc);
        }
        public override void AI(NPC npc)
        {


            base.AI(npc);

        }
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Elements == null)
            {
                Elements = new BaseElement[6] { freezeElement, burnElement, radiantElement, necroticElement, annihilateElement, evolutionElement };
            }
            List<BaseElement> DrawIconList = new List<BaseElement>();
            for (int i = 0; i < Elements.Length; i++)
            {
                if (Elements[i].Stack > 0)
                {
                    DrawIconList.Add(Elements[i]);
                }
            }

            //绘制贴图
            for (int i = 0; i < DrawIconList.Count; i++)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                Effect ElementIcomShader = ModContent.Request<Effect>("MysteriousAlchemy/Effects/ElementIcon", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
                ElementIcomShader.Parameters["scale"].SetValue(DrawIconList[i].Stack / DrawIconList[i].MaxStack);
                ElementIcomShader.CurrentTechnique.Passes[0].Apply();
                Main.spriteBatch.Draw(AssetUtils.GetTexture2D(AssetUtils.Texture + "ElementTexture/" + DrawIconList[i].Name), npc.Center + new Vector2(0, -npc.Size.Y) * 1.5f + new Vector2(i - (DrawIconList.Count / 2f - 0.5f), 0) * 40 - Main.screenPosition, null, Color.White, 0, ModContent.Request<Texture2D>("MysteriousAlchemy/Texture/ElementTexture/" + DrawIconList[i].Name).Value.Size() / 2f, new Vector2(40, 40) / ModContent.Request<Texture2D>("MysteriousAlchemy/Texture/ElementTexture/" + DrawIconList[i].Name).Value.Size(), SpriteEffects.None, 0);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }

            base.DrawEffects(npc, ref drawColor);
        }
    }
    public class ProjectileElement : GlobalProjectile
    {
        public FreezeElement freezeElement = new FreezeElement();
        public BurnElement burnElement = new BurnElement();
        public RadiantElement radiantElement = new RadiantElement();
        public NecroticElement necroticElement = new NecroticElement();
        public AnnihilateElement annihilateElement = new AnnihilateElement();
        public EvolutionElement evolutionElement = new EvolutionElement();
        public override bool InstancePerEntity => true;
        //结算元素积累
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            target.GetGlobalNPC<NPCElement>().freezeElement.AddElement(freezeElement);
            target.GetGlobalNPC<NPCElement>().burnElement.AddElement(burnElement);
            target.GetGlobalNPC<NPCElement>().radiantElement.AddElement(radiantElement);
            target.GetGlobalNPC<NPCElement>().necroticElement.AddElement(necroticElement);
            target.GetGlobalNPC<NPCElement>().annihilateElement.AddElement(annihilateElement);
            target.GetGlobalNPC<NPCElement>().evolutionElement.AddElement(evolutionElement);
            base.ModifyHitNPC(projectile, target, ref modifiers);
        }
    }
    public class ItemElement : GlobalItem
    {
        public FreezeElement freezeElement = new FreezeElement();
        public BurnElement burnElement = new BurnElement();
        public RadiantElement radiantElement = new RadiantElement();
        public NecroticElement necroticElement = new NecroticElement();
        public AnnihilateElement annihilateElement = new AnnihilateElement();
        public EvolutionElement evolutionElement = new EvolutionElement();
        public override bool InstancePerEntity => true;
        public override void ModifyHitNPC(Terraria.Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            target.GetGlobalNPC<NPCElement>().freezeElement.AddElement(freezeElement);
            target.GetGlobalNPC<NPCElement>().burnElement.AddElement(burnElement);
            target.GetGlobalNPC<NPCElement>().radiantElement.AddElement(radiantElement);
            target.GetGlobalNPC<NPCElement>().necroticElement.AddElement(necroticElement);
            target.GetGlobalNPC<NPCElement>().annihilateElement.AddElement(annihilateElement);
            target.GetGlobalNPC<NPCElement>().evolutionElement.AddElement(evolutionElement);
            base.ModifyHitNPC(item, player, target, ref modifiers);
        }
    }

    public class BaseElement
    {
        private string name;
        //npc积累的元素
        public float Stack = 0;
        //积累到多少元素触发效果
        public float MaxStack = 100;
        //此NPC对该类元素积累的减免——应用在addtive
        public float Reduction = 1;
        //减免——应用在base
        public float Base = 0;
        //减免应用在最终阶段
        public float Flat = 0;
        //积累元素时的修改
        public StatModifier ElementAcculationModifier = new StatModifier();
        //对伤害的修改
        public StatModifier StatModifier = new StatModifier();
        //额外效果——每帧触发
        public Action<NPC> ExtraEffectPerFrame = null;
        //额外效果——阶段触发
        public Action<NPC> ExtraEffectTrigger = null;
        //Item额外效果——受击时触发
        public Action<NPC, Player, Terraria.Item, NPC.HitModifiers> ItemEffectModifyHit = null;
        //Projectile额外效果——受击时触发
        public Action<NPC, Projectile, NPC.HitModifiers> ProjectileModifyHit = null;

        public int EnhanceHitCount = 0;

        public virtual string Name { get => name; set => name = value; }

        public BaseElement()
        {
            ExtraEffectPerFrame += DefaultExtraEffectPerFrame;
            ExtraEffectTrigger += DefaultExtraEffectTrigger;
            ItemEffectModifyHit += DefaultItemEffectModifyHit;
            ProjectileModifyHit += DefaultProjectileModifyHit;
        }
        public void NPCSpownReset()
        {
            Stack = 0;
            MaxStack = 100;
            Reduction = 0;
        }
        //触发特殊效果
        public bool IsTrigger()
        {
            if (MaxStack <= Stack)
            {
                Stack -= MaxStack;
                return true;
            }

            return false;
        }
        public virtual void DefaultExtraEffectPerFrame(NPC npc)
        {

        }
        public virtual void DefaultExtraEffectTrigger(NPC npc)
        {

        }
        public virtual void DefaultItemEffectModifyHit(NPC npc, Player player, Terraria.Item item, NPC.HitModifiers modifiers)
        {

        }
        public virtual void DefaultProjectileModifyHit(NPC npc, Projectile projectile, NPC.HitModifiers modifiers)
        {

        }
        public void AddElement(BaseElement add)
        {
            Stack += ApplyTo(add);
            ExtraEffectPerFrame = add.ExtraEffectPerFrame;
            ExtraEffectTrigger = add.ExtraEffectTrigger;
            ItemEffectModifyHit = add.ItemEffectModifyHit;
            ProjectileModifyHit = add.ProjectileModifyHit;
        }
        public float ApplyTo(BaseElement @base)
        {
            return @base.ElementAcculationModifier.ApplyTo(@base.Stack - Base) * Reduction - Flat;
        }
    }

}