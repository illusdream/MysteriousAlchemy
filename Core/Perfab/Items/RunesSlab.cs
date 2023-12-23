using Microsoft.Xna.Framework;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Core.Perfab.Items
{
    public abstract class RunesSlab : ModItem
    {
        public override string Texture => AssetUtils.Items_AlchemySlab + "Item_2767";
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 15;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = MASoundID.Ding_Item4;
            Item.autoReuse = false;
            Item.maxStack = 9999;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 10;
            Item.useAnimation = 10;

        }
        public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {

            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "1", "1"));
            tooltips.Add(new TooltipLine(Mod, "1", "1"));
            tooltips.Add(new TooltipLine(Mod, "1", "1"));
            tooltips.Add(new TooltipLine(Mod, "1", "1"));
            base.ModifyTooltips(tooltips);
        }
    }
}