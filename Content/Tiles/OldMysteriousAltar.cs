using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour;
using MysteriousAlchemy.Content.Animators;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Dusts;
using MysteriousAlchemy.Items;
using MysteriousAlchemy.Modsystem;
using MysteriousAlchemy.UI;
using MysteriousAlchemy.Utils;
using MysteriousAlchemy.VanillaJSONFronting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Liquid;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using static Terraria.ID.ContentSamples.CreativeHelper;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace MysteriousAlchemy.Tiles
{
    public class OldMysteriousAltar : ModTile
    {
        public override string Texture => AssetUtils.Tiles + Name;
        public bool StartCompose = false;
        bool GiveItem = true;
        private int OuterCircleLength = 250;
        private float InnerCircleLength { get { return OuterCircleLength * 0.7f; } }

        private string EmeraldTablet = "Verum, sine mendacio, certum et verissimum:\r\nQuod est inferius est sicut quod est superius, et quod est superius est sicut quod est inferius, ad perpetranda miracula rei unius.\r\nEt sicut res omnes fuerunt ab uno, meditatione unius, sic omnes res natae ab hac una re, adaptatione.\r\nPater eius est Sol. Mater eius est Luna, portavit illud Ventus in ventre suo, nutrix eius terra est.\r\nPater omnis telesmi totius mundi est hic.\r\nVirtus eius integra est si versa fuerit in terram.\r\nSeparabis terram ab igne, subtile ab spisso, suaviter, magno cum ingenio.\r\nIdeo fugiet a te omnis obscuritas.\r\nHaec est totius fortitudinis fortitudo fortis, quia vincet omnem rem subtilem, omnemque solidam penetrabit.\r\nSic mundus creatus est.\r\nHinc erunt adaptationes mirabiles, quarum modus est hic.\r\nItaque vocatus sum Hermes Trismegistus, habens tres partes philosophiae totius mundi.\r\nCompletum est quod dixi de operatione Solis.";

        private string[] TipStringArray = new string[48];
        private float[] TipStringArrayRandomWaveScale = new float[48];

        private Vector2[] FinalStageCircleHV = new Vector2[3];
        public const int FrameWidth = 18 * 5;
        public const int FrameHeight = 18 * 4;

        public int entityID;

        private Player Player => Main.LocalPlayer;
        private float ActiveEtherCount = 0;
        private float MaxActiveEtherCount = 100;
        private float ActiveEtherPercent
        {
            get { return ActiveEtherCount / MaxActiveEtherCount; }
        }

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileCut[Type] = false;
            Main.tileMergeDirt[Type] = false;
            Main.tileWaterDeath[Type] = false;
            Main.tileLavaDeath[Type] = false;
            GetItemDrops(ModContent.ItemType<Items.MysteriousAltar>(), 1);
            //物块被挖掘时的声音
            HitSound = SoundID.Dig;
            //挖掘时产生的粒子
            DustType = DustID.Dirt;
            //物块被挖掘时受到“伤害”的系数，越大则越难以破坏
            MineResist = 1f;
            //能被挖掘需要的最小镐力，默认0
            MinPick = 20;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);

            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<OldMysteriousAlterTileEntity>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
            base.SetStaticDefaults();
        }
        public override bool Slope(int i, int j)
        {
            return false;
        }
        OldMysteriousAlterTileEntity entity = null;
        public override bool RightClick(int i, int j)
        {
            UI_MysteriousAlterOld.Visable = !UI_MysteriousAlterOld.Visable;
            UI_MysteriousAlterOld.MysteriousAlterTileEntity = entity;
            return true;
        }
        public override void MouseOver(int i, int j)
        {
            LeftClick();
            base.MouseOver(i, j);
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            // Only required If you decide to make your tile utilize different styles through Item.placeStyle

            // This preserves its original frameX/Y which is required for determining the correct texture floating on the pedestal, but makes it draw properly
            tileFrameX %= FrameWidth; // Clamps the frameX
            tileFrameY %= FrameHeight; // Clamps the frameY (two horizontally aligned place styles, hence * 2)
        }

        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            DebugUtils.NewText(AnimatorManager.Instance.Animators.Count);
            // Since this tile does not have the hovering part on its sheet, we have to animate it ourselves
            // Therefore we register the top-left of the tile as a "special point"
            // This allows us to draw things in SpecialDraw


            Main.instance.TilesRenderer.AddSpecialLegacyPoint(TileUtils.GetTopLeftTileInMultitile(i, j).ToPoint());

        }
        int SpecialDrawFrameCount = 0;
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            SpecialDrawFrameCount++;
            int MultitileSize = 5 * 4;
            if (SpecialDrawFrameCount % (MultitileSize) == 0)
            {
                if (entity.AltarComposeAnimation)
                {
                    AltarComposeAnim(entity.Stage, spriteBatch, i, j);
                }

            }
            AltarMagicCircleDraw(i, j, spriteBatch);

        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int index = 0;
            if (ModContent.GetInstance<OldMysteriousAlterTileEntity>().Find(i, j) != -1)
            {
                index = ModContent.GetInstance<OldMysteriousAlterTileEntity>().Find(i, j);

            }
            if (index == 0)
                return base.PreDraw(i, j, spriteBatch);
            entity = (OldMysteriousAlterTileEntity)TileEntity.ByID[index];
            return base.PreDraw(i, j, spriteBatch);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            base.PostDraw(i, j, spriteBatch);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<OldMysteriousAlterTileEntity>().Kill(i, j);
            base.KillMultiTile(i, j, frameX, frameY);
        }

        bool Stringbuilder = true;
        public void AltarMagicCircleDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Texture2D AltarCircleTex = AssetUtils.GetTexture2D(AssetUtils.Glow + "AltarGlow");
            Vector2 TileButtomCenter = TileUtils.GetTopLeftTileInMultitile(i, j).ToVector2() * 16 + new Vector2(40, 64 + 2) + zero;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            Effect AltarMagicCircleEffect = AssetUtils.GetEffect("AltarLightEffect");
            AltarMagicCircleEffect.Parameters["textureSize"].SetValue(AltarCircleTex.Size());
            AltarMagicCircleEffect.Parameters["glareLength"].SetValue(48);
            AltarMagicCircleEffect.Parameters["timer"].SetValue((float)Main.time / 150f);
            AltarMagicCircleEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(AltarCircleTex, TileButtomCenter - Main.screenPosition, null, Color.White, 0, new Vector2(AltarCircleTex.Width / 2f, AltarCircleTex.Height), 1, SpriteEffects.None, 0);
            spriteBatch.End();
            spriteBatch.Begin();
        }
        public void AltarComposeAnim(int stage, SpriteBatch spriteBatch, int i, int j)
        {

            //这样做的原因是为了适应使用不同照明模式时发生的图形坐标偏移 
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            if (Stringbuilder)
            {
                StringBuilder();
                Stringbuilder = false;
            }
            switch (stage)
            {
                case 0:
                    Stage0MagicCircleStartDraw(spriteBatch, i, j, new Vector2(0, -150), entity.AnimStage1_Timer);
                    IngredientDraw(spriteBatch, TileUtils.GetCenterMultitile(i, j), entity.AnimStage1_Timer, 170, 0.7f, 15, 0.7f);
                    GiveItem = true;
                    break;
                case 1:

                    MagicCircleDraw(spriteBatch, i, j, new Vector2(0, -150), entity.AnimStage2_Timer);
                    //OuterStringDraw(spriteBatch, TileUtils.GetCenterMultitile(i, j) - Main.screenPosition, 100);
                    TransformDustEffect(i, j, 60);
                    if (entity.AnimStage2_Timer % 25 == 0)
                    {
                        StringBuilder();
                    }
                    OuterStringDraw(spriteBatch, TileUtils.GetCenterMultitile(i, j), 75, 20, entity.AnimStage2_Timer, 0.1f);
                    RandomSparkleSpown(TileUtils.GetCenterMultitile(i, j) + new Vector2(0, -150), 120, 5, 3, 30, 50);
                    break;
                case 2:
                    Stage2MagicCircleDisappearDraw(spriteBatch, i, j, new Vector2(0, -150), entity.AnimStage3_Timer);
                    if (entity.AnimStage3_Timer > 60 && GiveItem)
                    {
                        Stage2ItemSpown(spriteBatch, TileUtils.GetCenterMultitile(i, j) + new Vector2(0, -150), entity.AnimStage3_Timer, 60);
                        GiveItem = false;
                    }
                    break;
                case 3:

                    break;
                default:
                    break;
            }
        }

        public void MagicCircleDraw(SpriteBatch spriteBatch, int i, int j, Vector2 offest, int Timer)
        {
            float TotalTime = 240f;
            float ComposeInter = Timer / TotalTime;
            Texture2D CircleTex = AssetUtils.GetTexture2D(AssetUtils.Texture + "Projectile_490");

            Vector2 TileDrawRevise = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 20f, OuterCircleLength / CircleTex.Size().Length(), (float)MathHelper.TwoPi * Timer / 120f, -MathHelper.PiOver4 - (float)MathHelper.TwoPi * Timer / 120f, () =>
            {
                MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1);
            });
            DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 15f, OuterCircleLength / 0.7f / CircleTex.Size().Length(), -(float)MathHelper.TwoPi * Timer / 200f, MathHelper.PiOver2 / 3, () =>
            {
                MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1);
            });
            DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 30f, InnerCircleLength / CircleTex.Size().Length(), 0, MathHelper.PiOver2, () =>
            {
                MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1);
            });
            DrawUtils.DrawMagicLighting(spriteBatch, (float)Main.time, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, OuterCircleLength / CircleTex.Size().Length(), (float)MathHelper.TwoPi * Timer / 120f, MathHelper.PiOver4 + (float)MathHelper.TwoPi * Timer / 120f, 0.07f, 30);
            DrawUtils.DrawMagicLighting(spriteBatch, (float)Main.time, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, OuterCircleLength / 0.7f / CircleTex.Size().Length(), -(float)MathHelper.TwoPi * Timer / 200f, MathHelper.PiOver2 / 3, 0.07f, 50);
            DrawUtils.DrawMagicLighting(spriteBatch, (float)Main.time, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, InnerCircleLength / CircleTex.Size().Length(), 0, MathHelper.PiOver2, 0.07f, 20);

            FinalStageCircleHV[0] = new Vector2((float)MathHelper.TwoPi * Timer / 120f, -MathHelper.PiOver4 - (float)MathHelper.TwoPi * Timer / 120f);
            FinalStageCircleHV[1] = new Vector2(-(float)MathHelper.TwoPi * Timer / 200f, MathHelper.PiOver2 / 3);
            FinalStageCircleHV[2] = new Vector2(0, MathHelper.PiOver2);
        }

        public void Stage0MagicCircleStartDraw(SpriteBatch spriteBatch, int i, int j, Vector2 offest, int Timer)
        {
            int StartTime = 120;
            int beginTime = 60;
            float StartStageInter = Timer / (float)StartTime;
            Texture2D CircleTex = AssetUtils.GetTexture2D(AssetUtils.Texture + "Projectile_490");
            Vector2 TileDrawRevise = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 StartPoint = new Vector2(0, -150);
            //先从无生成三个环
            #region
            float SpownInter = Timer / (float)StartTime;
            float BeginInter = (Timer - StartTime) / (float)beginTime;
            Vector2 SpownHV = new Vector2(-MathHelper.PiOver4, MathHelper.PiOver2);
            Vector2 Circel_1_ReadlyHV = new Vector2(0, MathHelper.PiOver4);
            Vector2 Circel_2_ReadlyHV = new Vector2(0, MathHelper.PiOver2 / 3);
            Vector2 Circel_3_ReadlyHV = new Vector2(0, MathHelper.PiOver2);
            if (Timer < StartTime)
            {
                DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + StartPoint * SpownInter - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 25f, OuterCircleLength / CircleTex.Size().Length() * SpownInter, -MathHelper.PiOver4, MathHelper.PiOver2, () =>
                {
                    MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                    MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1);
                });
                DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + StartPoint * SpownInter - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 15f, OuterCircleLength / 0.7f / CircleTex.Size().Length() * SpownInter, -MathHelper.PiOver4, MathHelper.PiOver2, () =>
                {
                    MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                    MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1);
                });
                DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + StartPoint * SpownInter - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 30f, InnerCircleLength / CircleTex.Size().Length() * SpownInter, -MathHelper.PiOver4, MathHelper.PiOver2, () =>
                {
                    MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                    MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1);
                });
            }
            else if (Timer > StartTime && Timer < StartTime + beginTime)
            {

                Vector2 Circel_1_NoReadlyHV = Vector2.Lerp(SpownHV, Circel_1_ReadlyHV, BeginInter);
                Vector2 Circel_2_NoReadlyHV = Vector2.Lerp(SpownHV, Circel_2_ReadlyHV, BeginInter);
                Vector2 Circel_3_NoReadlyHV = Vector2.Lerp(SpownHV, Circel_3_ReadlyHV, BeginInter);
                DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + StartPoint - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 25f, OuterCircleLength / CircleTex.Size().Length(), Circel_1_NoReadlyHV.X, Circel_1_NoReadlyHV.Y, () =>
                {
                    MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                    MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1);
                });
                DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + StartPoint - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 15f, OuterCircleLength / 0.7f / CircleTex.Size().Length(), Circel_2_NoReadlyHV.X, Circel_2_NoReadlyHV.Y, () =>
                {
                    MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                    MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1);
                });
                DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + StartPoint - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 30f, InnerCircleLength / CircleTex.Size().Length(), Circel_3_NoReadlyHV.X, Circel_3_NoReadlyHV.Y, () =>
                {
                    MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                    MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1);
                });
            }
            #endregion



            //MainUtil.Draw(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 25f, OuterCircleLength / CircleTex.Size().Length(), (float)MathHelper.TwoPi * Timer / 120f, MathHelper.PiOver4 + (float)MathHelper.TwoPi * Timer / 120f, () =>
            //{
            //    MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
            //});
            //MainUtil.Draw(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 15f, OuterCircleLength / 0.7f / CircleTex.Size().Length(), -(float)MathHelper.TwoPi * Timer / 200f, MathHelper.PiOver2 / 3, () =>
            //{
            //    MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
            //});
            //MainUtil.Draw(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 30f, InnerCircleLength / CircleTex.Size().Length(), 0, MathHelper.PiOver2, () =>
            //{
            //    MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
            //});
            //MainUtil.DrawMagicLighting(spriteBatch, (float)Main.time, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, OuterCircleLength / CircleTex.Size().Length(), (float)MathHelper.TwoPi * Timer / 120f, MathHelper.PiOver4 + (float)MathHelper.TwoPi * Timer / 120f, 0.07f, 300);
            //MainUtil.DrawMagicLighting(spriteBatch, (float)Main.time, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, OuterCircleLength / 0.7f / CircleTex.Size().Length(), -(float)MathHelper.TwoPi * Timer / 200f, MathHelper.PiOver2 / 3, 0.07f, 30);
            //MainUtil.DrawMagicLighting(spriteBatch, (float)Main.time, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, InnerCircleLength / CircleTex.Size().Length(), 0, MathHelper.PiOver2, 0.07f, 300);
        }

        public void Stage2MagicCircleDisappearDraw(SpriteBatch spriteBatch, int i, int j, Vector2 offest, int Timer)
        {
            float FinalTime = 120;
            float FinalInter = Timer / FinalTime;
            Texture2D CircleTex = AssetUtils.GetTexture2D(AssetUtils.Texture + "Projectile_490");

            Vector2 TileDrawRevise = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 20f, OuterCircleLength / CircleTex.Size().Length(), FinalStageCircleHV[0].X + (float)MathHelper.TwoPi * Timer / 120f, FinalStageCircleHV[0].Y - (float)MathHelper.TwoPi * Timer / 120f, () =>
            {
                MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1 - FinalInter);
            });
            DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 15f, OuterCircleLength / 0.7f / CircleTex.Size().Length(), FinalStageCircleHV[1].X - (float)MathHelper.TwoPi * Timer / 200f, MathHelper.PiOver2 / 3, () =>
            {
                MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1 - FinalInter);
            });
            DrawUtils.DrawTile(spriteBatch, CircleTex, TileUtils.GetCenterMultitile(i, j) + TileDrawRevise + offest - Main.screenPosition, MysteriousAlchemy.AltarTransform, Timer / 30f, InnerCircleLength / CircleTex.Size().Length(), 0, MathHelper.PiOver2, () =>
            {
                MysteriousAlchemy.AltarTransform.Parameters["timer"].SetValue((float)Main.time / 60f);
                MysteriousAlchemy.AltarTransform.Parameters["alpha"].SetValue(1 - FinalInter);
            });
        }

        public void Stage2ItemSpown(SpriteBatch spriteBatch, Vector2 SpownPos, int Timer, int SpownTime)
        {
            PrettySparkleSpown(20, SpownPos, TextureAssets.Item[entity.Product.type].Width(), TextureAssets.Item[entity.Product.type].Height());

            if (Timer > SpownTime && GiveItem)
            {
                Item.NewItem(new EntitySource_TileEntity(entity), SpownPos, 10, 10, entity.Product.type, entity.Product.stack);
                GiveItem = false;
            }

        }

        public void StringBuilder()
        {
            var rand = new Random();
            for (int i = 0; i < TipStringArray.Length; i++)
            {
                if (entity.OuterIngredient[i % 4].type < ItemID.Count)
                {
                    int RandNum = rand.Next(TipStringArray.Length - 7);
                    TipStringArray[i] = EmeraldTablet.Substring(RandNum, rand.Next(7));
                }
                else
                {
                    int RandNum = rand.Next(entity.OuterIngredient[i % 4].Name.Length - 5);
                    TipStringArray[i] = entity.OuterIngredient[i % 4].Name.Substring(RandNum, rand.Next(5));
                }
            }
            for (int i = 0; i < TipStringArrayRandomWaveScale.Length; i++)
            {
                TipStringArrayRandomWaveScale[i] = rand.NextSingle();
            }
        }
        public void OuterStringDraw(SpriteBatch spriteBatch, Vector2 CenterPos, int Distance, float MaxWaveScale, float Timer, float WaveRate)
        {
            Vector2 TileDrawRevise = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            for (int i = 0; i < TipStringArray.Length; i++)
            {
                //Terraria.Utils.DrawBorderStringFourWay(spriteBatch, (ReLogic.Graphics.DynamicSpriteFont)FontAssets.MouseText, TipStringArray[i], CenterPos.X + Distance * MathF.Cos(MathF.PI * 2 * i / TipStringArray.Length) + TileDrawRevise.X, CenterPos.Y + Distance * MathF.Sin(MathF.PI * 2 * i / TipStringArray.Length) + TileDrawRevise.Y, Color.White, Color.White, new Vector2(0.5f, 0.5f));
                //spriteBatch.DrawString((ReLogic.Graphics.DynamicSpriteFont)FontAssets.MouseText, TipStringArray[i], CenterPos + TileDrawRevise + new Vector2(0, -150) + new Vector2(MathF.Cos(MathF.PI * 2 * i / TipStringArray.Length), MathF.Sin(MathF.PI * 2 * i / TipStringArray.Length)) * (Distance + MathF.Sin((Timer) * WaveRate + i) * TipStringArrayRandomWaveScale[i] * MaxWaveScale), Color.White, MathF.PI * 2 * i / TipStringArray.Length, new Vector2(0, 0.5f), 0.5f, SpriteEffects.FlipHorizontally, 0);
                ColorfulStringSystem.AddColorfulString(spriteBatch, (ReLogic.Graphics.DynamicSpriteFont)FontAssets.MouseText.Value, TipStringArray[i], Color.White, CenterPos + new Vector2(0, -150) + new Vector2(MathF.Cos(MathF.PI * 2 * i / TipStringArray.Length), MathF.Sin(MathF.PI * 2 * i / TipStringArray.Length)) * (Distance + MathF.Sin((Timer) * WaveRate + i) * TipStringArrayRandomWaveScale[i] * MaxWaveScale), MathF.PI * 2 * i / TipStringArray.Length, new Vector2(0, 0.5f), 0.5f, SpriteEffects.FlipHorizontally, 0);

            }
        }

        public void IngredientDraw(SpriteBatch spriteBatch, Vector2 CenterPos, int Timer, int OutDistance, float Outscale, int InnerDistance, float Innerscale)
        {
            Vector2 StartPoint = new Vector2(0, -150);
            Vector2 TileDrawRevise = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null);
            Effect IngredientStroke = ModContent.Request<Effect>("MysteriousAlchemy/Effects/IngredientStroke").Value;

            IngredientStroke.CurrentTechnique.Passes[0].Apply();
            //升起阶段
            if (Timer < 120)
            {
                IngredientStroke.Parameters["timer"].SetValue(1);
                for (int k = 0; k < entity.OuterIngredient.Length; k++)
                {
                    if (entity.OuterIngredient[k] != null)
                    {
                        spriteBatch.Draw((Texture2D)TextureAssets.Item[entity.OuterIngredient[k].type], CenterPos + new Vector2((float)Math.Cos(Math.PI * 2 * k / 8), (float)Math.Sin(Math.PI * 2 * k / 8)) * OutDistance + TileDrawRevise + StartPoint * Timer / 120f - Main.screenPosition, new Rectangle(0, 0, TextureAssets.Item[entity.OuterIngredient[k].type].Width(), TextureAssets.Item[entity.OuterIngredient[k].type].Height()), Color.White, 0, TextureAssets.Item[entity.OuterIngredient[k].type].Size() / 2f, Outscale * Timer / 120f, SpriteEffects.None, 0);
                    }
                }
                for (int k = 0; k < entity.MiddleIngredient.Length; k++)
                {
                    if (entity.MiddleIngredient[k] != null)
                    {
                        spriteBatch.Draw((Texture2D)TextureAssets.Item[entity.MiddleIngredient[k].type], CenterPos + new Vector2((float)Math.Cos(Math.PI * 2 * k / 8), (float)Math.Sin(Math.PI * 2 * k / 8)) * InnerDistance + TileDrawRevise + StartPoint * Timer / 120f - Main.screenPosition, new Rectangle(0, 0, TextureAssets.Item[entity.MiddleIngredient[k].type].Width(), TextureAssets.Item[entity.MiddleIngredient[k].type].Height()), Color.White, 0, TextureAssets.Item[entity.MiddleIngredient[k].type].Size() / 2f, Innerscale * Timer / 120f, SpriteEffects.None, 0);
                    }

                }

            }
            else
            {
                IngredientStroke.Parameters["timer"].SetValue((Timer - 120) / 60f);
                for (int k = 0; k < entity.OuterIngredient.Length; k++)
                {
                    if (entity.OuterIngredient[k] != null)
                    {

                        spriteBatch.Draw((Texture2D)TextureAssets.Item[entity.OuterIngredient[k].type], CenterPos + new Vector2((float)Math.Cos(Math.PI * 2 * k / 8), (float)Math.Sin(Math.PI * 2 * k / 8)) * OutDistance + TileDrawRevise + StartPoint - Main.screenPosition, new Rectangle(0, 0, TextureAssets.Item[entity.OuterIngredient[k].type].Width(), TextureAssets.Item[entity.OuterIngredient[k].type].Height()), Color.White, 0, TextureAssets.Item[entity.OuterIngredient[k].type].Size() / 2f, Outscale, SpriteEffects.None, 0);
                        if (entity.OuterIngredient[k].type != 0)
                        {
                            PrettySparkleSpown(3, CenterPos + new Vector2((float)Math.Cos(Math.PI * 2 * k / 8), (float)Math.Sin(Math.PI * 2 * k / 8)) * OutDistance + StartPoint, 30, 40);
                        }
                    }
                }
                for (int k = 0; k < entity.MiddleIngredient.Length; k++)
                {
                    if (entity.MiddleIngredient[k] != null)
                    {
                        spriteBatch.Draw((Texture2D)TextureAssets.Item[entity.MiddleIngredient[k].type], CenterPos + new Vector2((float)Math.Cos(Math.PI * 2 * k / 8), (float)Math.Sin(Math.PI * 2 * k / 8)) * InnerDistance + TileDrawRevise + StartPoint - Main.screenPosition, new Rectangle(0, 0, TextureAssets.Item[entity.MiddleIngredient[k].type].Width(), TextureAssets.Item[entity.MiddleIngredient[k].type].Height()), Color.White, 0, TextureAssets.Item[entity.MiddleIngredient[k].type].Size() / 2f, Innerscale, SpriteEffects.None, 0);
                    }
                }

            }

            spriteBatch.End();
            spriteBatch.Begin();
        }

        public void TransformDustEffect(int i, int j, int number)
        {
            Texture2D CircleTex = AssetUtils.GetTexture2D(AssetUtils.Texture + "Projectile_490");
            for (int k = 0; k < number; k++)
            {
                Vector2 ToCenter = DrawUtils.GetDrawMartixParameter(CircleTex, Vector2.Zero, MathHelper.TwoPi * k / (float)number, InnerCircleLength * 0.8f / CircleTex.Size().Length(), 0, MathHelper.PiOver2).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2) * -1;
                Dust.NewDustPerfect(DrawUtils.GetDrawMartixParameter(CircleTex, TileUtils.GetCenterMultitile(i, j) + new Vector2(0, -150), MathHelper.TwoPi * k / (float)number, InnerCircleLength * 0.8f / CircleTex.Size().Length(), 0, MathHelper.PiOver2), ModContent.DustType<AltarMagicVortex>(), ToCenter);
            }
        }

        public void PrettySparkleSpown(int strength, Vector2 position, float width, float height)
        {
            Rectangle rectangle = Terraria.Utils.CenteredRectangle(position, new Vector2(width, height));
            for (float num = 0; num < strength; num += 1)
            {
                PrettySparkleParticle prettySparkleParticle = new PrettySparkleParticle();
                int num2 = Main.rand.Next(20, 40);
                prettySparkleParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, 0);
                prettySparkleParticle.LocalPosition = Main.rand.NextVector2FromRectangle(rectangle);
                prettySparkleParticle.Rotation = (float)Math.PI / 2f;
                prettySparkleParticle.Scale = new Vector2(1f + Main.rand.NextFloat() * 2f, 0.7f + Main.rand.NextFloat() * 0.7f);
                prettySparkleParticle.Velocity = new Vector2(0f, -1f);
                prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
                prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
                prettySparkleParticle.TimeToLive = num2;
                prettySparkleParticle.FadeOutEnd = num2;
                prettySparkleParticle.FadeInEnd = num2 / 2;
                prettySparkleParticle.FadeOutStart = num2 / 2;
                prettySparkleParticle.AdditiveAmount = 0.35f;
                prettySparkleParticle.DrawVerticalAxis = false;
                Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
                PrettySparkleParticle prettySparkleParticle2 = new PrettySparkleParticle();
                prettySparkleParticle2.ColorTint = new Color(255, 255, 255, 0);
                prettySparkleParticle2.LocalPosition = Main.rand.NextVector2FromRectangle(rectangle);
                prettySparkleParticle2.Rotation = (float)Math.PI / 2f;
                prettySparkleParticle2.Scale = prettySparkleParticle.Scale * 0.5f;
                prettySparkleParticle2.Velocity = new Vector2(0f, -1f);
                prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
                prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
                prettySparkleParticle2.TimeToLive = num2;
                prettySparkleParticle2.FadeOutEnd = num2;
                prettySparkleParticle2.FadeInEnd = num2 / 2;
                prettySparkleParticle2.FadeOutStart = num2 / 2;
                prettySparkleParticle2.AdditiveAmount = 1f;
                prettySparkleParticle2.DrawVerticalAxis = false;
                Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
            }
        }

        public void RandomSparkleSpown(Vector2 position, float Radius, int count, int strength, float width, float height)
        {
            for (int i = 0; i < count; i++)
            {
                PrettySparkleSpown(strength, position + Main.rand.NextVector2Circular(Radius, Radius), width * Main.rand.NextFloat(0.5f, 0.9f), height * Main.rand.NextFloat(0.5f, 0.9f));
            }
        }

        private void LeftClick()
        {
            //判断手持物品是否是嬗变水晶且左键点击
            if (Player.HeldItem.type == ModContent.ItemType<ActiveEther>() && Main.mouseLeft)
            {
                AddEther(40);
            }
        }
        private void AddEther(float value)
        {
            ActiveEtherCount += value;
            ActiveEtherCount = Math.Clamp(ActiveEtherCount, 0, MaxActiveEtherCount);
        }
    }


    public class OldMysteriousAlterTileEntity : ModTileEntity
    {
        public bool AltarComposeAnimation = false;
        //读取材料 3秒
        public int AnimStage1_Timer = 0;
        private int AnimStage1_Lngeth = 180;
        //合成过程 4秒
        public int AnimStage2_Timer = 0;
        private int AnimStage2_Lngeth = 240;
        //合成完成 出物品 2秒
        public int AnimStage3_Timer = 0;
        private int AnimStage3_Lngeth = 120;
        //合成完成
        public int AnimStage4_Timer = 0;
        private int AnimStage4_Lngeth = 600;

        //合成的图片
        public Item[] OuterIngredient = new Item[8];
        public Item[] MiddleIngredient = new Item[4];
        public Item Product = null;

        public int Stage = 0;

        public AltarAnimator altarAnimator;
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            //The MyTile class is shown later
            return tile.HasTile && tile.TileType == ModContent.TileType<OldMysteriousAltar>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Sync the entire multitile's area.  Modify "width" and "height" to the size of your multitile in tiles
                int width = 5;
                int height = 4;
                NetMessage.SendTileSquare(Main.myPlayer, i, j, width, height);
                // Sync the placement of the tile entity with other clients
                // The "type" parameter refers to the tile type which placed the tile entity, so "Type" (the type of the tile entity) needs to be used here instead
                NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
            }

            // ModTileEntity.Place() handles checking if the entity can be placed, then places it for you
            // Set "tileOrigin" to the same value you set TileObjectData.newTile.Origin to in the ModTile
            Point16 tileOrigin = new Point16(0, 0);
            int placedEntity = Place(i, j);
            (TileEntity.ByID[placedEntity] as OldMysteriousAlterTileEntity).altarAnimator = AnimatorManager.Instance.Register<AltarAnimator>();
            (TileEntity.ByID[placedEntity] as OldMysteriousAlterTileEntity).altarAnimator.Position = TileUtils.GetCenterMultitile(i, j) + new Vector2(0, -150);
            return placedEntity;
        }
        public override void OnKill()
        {
            DebugUtils.NewText(altarAnimator);
            altarAnimator.Kill();
            base.OnKill();
        }

        public override void OnNetPlace()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
            }
        }

        public override void Update()
        {
            if (AltarComposeAnimation)
            {
                AltarComposeAnimControll();
            }
            if (altarAnimator == null)
            {
                // altarAnimator = AnimatorManager.Instance.Register<AltarAnimator>();
                altarAnimator.Position = TileUtils.GetCenterMultitile(Position.X, Position.Y) + new Vector2(0, -150);
            }
            base.Update();
        }
        public int AltarComposeAnimControll()
        {
            switch (Stage)
            {
                case 0:
                    AnimStage1_Timer++;
                    if (AnimStage1_Timer > AnimStage1_Lngeth)
                    {
                        Stage++;
                    }
                    break;
                case 1:
                    AnimStage2_Timer++;
                    if (AnimStage2_Timer > AnimStage2_Lngeth)
                    {
                        Stage++;
                    }
                    break;
                case 2:
                    AnimStage3_Timer++;
                    if (AnimStage3_Timer > AnimStage3_Lngeth)
                    {
                        //重置动画机状态
                        Stage = 0;
                        AltarComposeAnimation = false;
                        AnimStage1_Timer = 0;
                        AnimStage2_Timer = 0;
                        AnimStage3_Timer = 0;
                    }
                    break;
                default:
                    break;
            }
            return Stage;
        }
    }

}