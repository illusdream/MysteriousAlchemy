using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace MysteriousAlchemy.Utils
{
    public class NPCUtils
    {
        public static NPC GetNPCCanTrack(Vector2 position, float radium)
        {
            NPC result = null;
            foreach (var npc in Main.npc)
            {
                if (!npc.friendly && npc.active && (npc.Center - position).Length() < radium && npc.type != NPCID.TargetDummy)
                {
                    if (result == null)
                    {
                        result = npc;
                    }
                    if ((result.Center - position).Length() > (npc.Center - position).Length())
                    {
                        result = npc;
                    }
                }
            }
            return result;
        }
        public static Vector2 GetVector2ToCanTrackNPC(Vector2 position, float radium)
        {
            NPC npc = GetNPCCanTrack(position, radium);
            if (npc is not null)
                return (npc.Center - position);
            return Vector2.Zero;
        }
    }
}
