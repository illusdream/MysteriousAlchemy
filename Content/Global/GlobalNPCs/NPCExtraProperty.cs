using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Global.GlobalNPCs
{
    public class NPCExtraProperty : GlobalNPC
    {
        public override bool InstancePerEntity => true;



        private int _ScytheHitCount;
        private int MaxScytheHitCount = 10;
        public int ScytheHitCount
        {
            get { return Math.Clamp(_ScytheHitCount, 0, MaxScytheHitCount); }
        }
        public float GetScytheHitPercent()
        { return ScytheHitCount / (float)MaxScytheHitCount; }
        public int GetScytheHitCount()
        { return ScytheHitCount; }
        public void SetMaxScytheHit(int value)
        {
            MaxScytheHitCount = value;
        }
        public void AddScytheHitCount(int value)
        {
            _ScytheHitCount += value;
        }


        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            base.OnSpawn(npc, source);
        }
    }
}