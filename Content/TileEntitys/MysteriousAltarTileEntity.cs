﻿using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Animators;
using MysteriousAlchemy.Content.Tiles;
using MysteriousAlchemy.Core.System;
using MysteriousAlchemy.Tiles;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MysteriousAlchemy.Content.TileEntitys
{
    public class MysteriousAltarTileEntity : ModTileEntity
    {
        public AltarAnimator altarAnimator;
        private int EtherCrytalCount = 0;
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            return tile.HasTile && tile.TileType == ModContent.TileType<MysteriousAltarTile>();
        }
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Sync the entire multitile's area.  Modify "width" and "height" to the size of your multitile in tiles
                int width = 5;
                int height = 3;
                NetMessage.SendTileSquare(Main.myPlayer, i, j, width, height);
                // Sync the placement of the tile entity with other clients
                // The "type" parameter refers to the tile type which placed the tile entity, so "Type" (the type of the tile entity) needs to be used here instead
                NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
            }

            // ModTileEntity.Place() handles checking if the entity can be placed, then places it for you
            // Set "tileOrigin" to the same value you set TileObjectData.newTile.Origin to in the ModTile
            Point16 tileOrigin = new Point16(2, 2);
            int placedEntity = Place(i, j);
            DebugUtils.NewText(new Vector2(i, j));
            (TileEntity.ByID[placedEntity] as MysteriousAltarTileEntity).altarAnimator = AnimatorManager.Instance.Register<AltarAnimator>();
            (TileEntity.ByID[placedEntity] as MysteriousAltarTileEntity).altarAnimator.Position = TileUtils.GetCenterMultitile(i, j) + new Vector2(0, -150);
            return placedEntity;
        }
        public override void OnKill()
        {
            altarAnimator?.Kill();
            base.OnKill();
        }
        public override void LoadData(TagCompound tag)
        {
            if (altarAnimator == null)
            {
                EtherCrytalCount = tag.GetInt(nameof(EtherCrytalCount));
                altarAnimator = AnimatorManager.Instance.Register<AltarAnimator>();
                altarAnimator.Position = TileUtils.GetCenterMultitile(Position.X, Position.Y) + new Vector2(0, -150);
                for (int i = 0; i < EtherCrytalCount; i++)
                {
                    altarAnimator?.AddEtherCrystal();
                }
            }
            base.LoadData(tag);
        }
        public override void SaveData(TagCompound tag)
        {
            tag.Add(nameof(EtherCrytalCount), altarAnimator.etherCrystalList_Ready.Count);
            altarAnimator.Kill();
            base.SaveData(tag);
        }

    }
}
