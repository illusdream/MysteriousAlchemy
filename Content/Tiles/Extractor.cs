using Microsoft.Xna.Framework;
using MysteriousAlchemy.Items;
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
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ID.ContentSamples.CreativeHelper;

namespace MysteriousAlchemy.Tiles
{
    public class Extractor : ModTile
    {
        public override string Texture => AssetUtils.Tiles + Name;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[ModContent.TileType<Extractor>()] = true;
            Main.tileSolid[ModContent.TileType<Extractor>()] = false;
            Main.tileNoAttach[ModContent.TileType<Extractor>()] = true;
            Main.tileCut[ModContent.TileType<Extractor>()] = false;
            Main.tileMergeDirt[ModContent.TileType<Extractor>()] = false;
            Main.tileWaterDeath[ModContent.TileType<Extractor>()] = false;
            Main.tileLavaDeath[ModContent.TileType<Extractor>()] = false;
            GetItemDrops(ModContent.ItemType<Items.Extractor>(), 1);
            //��鱻�ھ�ʱ������
            HitSound = SoundID.Dig;
            //�ھ�ʱ����������
            DustType = DustID.Dirt;
            //��鱻�ھ�ʱ�ܵ����˺�����ϵ����Խ����Խ�����ƻ�
            MineResist = 1f;
            //�ܱ��ھ���Ҫ����С������Ĭ��0
            MinPick = 20;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.addTile(ModContent.TileType<Extractor>());
            base.SetStaticDefaults();
        }

        public override bool Slope(int i, int j)
        {
            return false;
        }

        public override bool RightClick(int i, int j)
        {
            UI.UI_Extractor.Visable = !UI.UI_Extractor.Visable;
            return true;
        }
    }

    public struct testJosn
    {
        public int ID;
        public int stack;
    }
}