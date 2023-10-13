using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.TileEntitys;
using MysteriousAlchemy.Core.Interface;
using MysteriousAlchemy.Items;
using MysteriousAlchemy.Tiles;
using MysteriousAlchemy.UI;
using MysteriousAlchemy.Utils;
using MysteriousAlchemy.VanillaJSONFronting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ID.ContentSamples.CreativeHelper;

namespace MysteriousAlchemy.Content.Tiles
{
    public class MysteriousAltarTile : ModTile
    {
        public override string Texture => AssetUtils.Tiles + Name;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[ModContent.TileType<MysteriousAltarTile>()] = true;
            Main.tileSolid[ModContent.TileType<MysteriousAltarTile>()] = false;
            Main.tileNoAttach[ModContent.TileType<MysteriousAltarTile>()] = true;
            Main.tileCut[ModContent.TileType<MysteriousAltarTile>()] = false;
            Main.tileMergeDirt[ModContent.TileType<MysteriousAltarTile>()] = false;
            Main.tileWaterDeath[ModContent.TileType<MysteriousAltarTile>()] = false;
            Main.tileLavaDeath[ModContent.TileType<MysteriousAltarTile>()] = false;
            GetItemDrops(ModContent.ItemType<MysteriousAltar>(), 1);
            //物块被挖掘时的声音
            HitSound = SoundID.Dig;
            //挖掘时产生的粒子
            DustType = DustID.Dirt;
            //物块被挖掘时受到“伤害”的系数，越大则越难以破坏
            MineResist = 1f;
            //能被挖掘需要的最小镐力，默认0
            MinPick = 20;

            TileObjectData.newTile.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.SolidTile | Terraria.Enums.AnchorType.SolidWithTop | Terraria.Enums.AnchorType.SolidSide, 5, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.Width = 5;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[3] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(2, 2);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<MysteriousAltarTileEntity>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);
            base.SetStaticDefaults();
        }

        public override bool Slope(int i, int j)
        {
            return false;
        }

        public override bool RightClick(int i, int j)
        {
            MysteriousAltarTileEntity mysteriousAltarTileEntity;
            TileUtils.TryGetTileEntityAs<MysteriousAltarTileEntity>(i, j, out mysteriousAltarTileEntity);
            UI_AltarCompose.MysteriousAlterTileEntity = mysteriousAltarTileEntity;
            for (int k = 0; k < mysteriousAltarTileEntity.Ingredient.Count; k++)
            {
                DebugUtils.NewText(k);
                UI_AltarCompose.ItemSlots[k].Item = mysteriousAltarTileEntity.Ingredient[k];
            }
            mysteriousAltarTileEntity.altarAnimator.SwitchUI_AltarComposeVisable();
            return true;
        }
        public override void MouseOver(int i, int j)
        {
            base.MouseOver(i, j);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<MysteriousAltarTileEntity>().Kill(i, j);
            base.KillMultiTile(i, j, frameX, frameY);
        }
    }
}