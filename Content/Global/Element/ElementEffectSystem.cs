using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Buffs;
using MysteriousAlchemy.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Global.Element
{
    public class ElementEffectSystem : ModSystem
    {
        List<NPC> FreezeNPCS = new List<NPC>();
        List<Vector2> FreezeNPColdPosition = new List<Vector2>();
        public override void PreUpdateNPCs()
        {
            foreach (var npc in Main.npc)
            {
                if (npc.active)
                {
                    if (npc.HasBuff<NPCFreezeEffect>())
                    {
                        FreezeNPCS.Add(npc);
                        FreezeNPColdPosition.Add(npc.position);
                    }
                }

            }
            base.PreUpdateNPCs();
        }
        public override void PostUpdateNPCs()
        {
            if (FreezeNPCS.Count > 0)
            {
                for (int i = 0; i < FreezeNPCS.Count; i++)
                {
                    if (FreezeNPCS[i].active)
                    {
                        FreezeNPCS[i].position = FreezeNPColdPosition[i];
                    }
                }
            }
            FreezeNPCS.Clear();
            FreezeNPColdPosition.Clear();
            base.PostUpdateNPCs();
        }
    }

    internal class NPCElementDrawEffect : GlobalNPC
    {
        public override void DrawEffects(NPC npc, ref Color drawColor)
        {

            base.DrawEffects(npc, ref drawColor);
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.active && npc.HasBuff<NPCFreezeEffect>())
            {
                Texture2D FreezeTex = AssetUtils.GetTexture2D(AssetUtils.Texture + "EffectTexture/Frozen");
                spriteBatch.Draw(FreezeTex, npc.position - Main.screenPosition, null, new Color(255, 255, 255, 0.1f), 0, Vector2.Zero, npc.Hitbox.Size() / FreezeTex.Size(), SpriteEffects.None, 0);
            }
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }
    }
}