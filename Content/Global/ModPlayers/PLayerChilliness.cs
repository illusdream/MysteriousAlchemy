using MysteriousAlchemy.Content.Items.Chilliness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.Global.ModPlayers
{
    public class PLayerChilliness : ModPlayer
    {
        public bool EnhanceChilliness;
        private int frostEssenceCrystalCount;
        private int maxfrostEssenceCrystalCount = 3;
        public int MaxfrostEssenceCrystalCount { get { return maxfrostEssenceCrystalCount; } }

        public int FrostEssenceCrystalCount
        {
            get
            {
                return frostEssenceCrystalCount;
            }
            set
            {
                if (value < 0)
                    return;
                if (value > MaxfrostEssenceCrystalCount)
                    return;
                frostEssenceCrystalCount = value;
            }
        }
        public override void PreUpdate()
        {
            EnhanceChilliness = false;
            foreach (var item in Player.inventory)
            {
                if (item.type == ModContent.ItemType<FrostEssenceGrimoire>() && item.favorited)
                {
                    EnhanceChilliness = true;
                }
            }
            base.PreUpdate();
        }
    }

}
