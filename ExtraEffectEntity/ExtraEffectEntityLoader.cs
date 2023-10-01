using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.ExtraEffectEntity
{
    public class ExtraEffectEntityLoader : ModSystem
    {
        List<ExtraEffectEntity> effectEntities;

        public override void PostUpdateEverything()
        {
            base.PostUpdateEverything();
        }

        public override void OnWorldLoad()
        {

            base.OnWorldLoad();
        }
    }
}