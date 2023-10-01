using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Tiles;
using MysteriousAlchemy.UI.UIElements;
using MysteriousAlchemy.Utils;
using MysteriousAlchemy.VanillaJSONFronting;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace MysteriousAlchemy.UI
{
    public class UI_MysteriousAlterOld : UIState
    {
        private static int OuterSlotLength = 8;
        private static int MiddleSlotLength = 4;
        public static bool Visable = false;
        public static MysteriousAlterTileEntity MysteriousAlterTileEntity = null;
        VanillaItemSlotWrapper[] OuterSlotGroup = new VanillaItemSlotWrapper[OuterSlotLength];
        VanillaItemSlotWrapper[] MiddleSlotGroup = new VanillaItemSlotWrapper[MiddleSlotLength];

        RecipeItemShow[] DictionaryImages = new RecipeItemShow[OuterSlotLength];
        RecipeItemShow DictionaryProductImages;

        CustomPanel ComposePanel;
        CustomButton ComposePanelButton;

        CustomPanel DictionaryPanel;
        CustomButton RightButton;
        CustomButton LeftButton;
        int RecipeShowCount;
        string ComposePanelButtonString = LocalizationUtils.LocalLang("合成", "Compose");
        string ComposeStirng = LocalizationUtils.LocalLang("合成", "Compose");
        string DictionaryString = LocalizationUtils.LocalLang("合成表", "Dictionary");
        //false代表显示DictionaryString，true显示ComposeStirng
        bool ComposeButtonStringShow = false;

        public override void OnInitialize()
        {
            UIPanel MainPanel = new UIPanel();
            //设置面板的宽度
            MainPanel.Width.Set(800, 0f);
            //设置面板的高度
            MainPanel.Height.Set(480, 0f);
            //设置面板距离屏幕最左边的距离
            MainPanel.Left.Set(-360f, 1f);
            //设置面板距离屏幕最上端的距离
            MainPanel.Top.Set(-240f, 1f);
            //将这个面板注册到UIState
            Append(MainPanel);

            UIPanel Operator = new UIPanel();
            Operator.Width.Set(240, 0f);
            //设置面板的高度
            Operator.Height.Set(400, 0f);
            //设置面板距离屏幕最左边的距离
            Operator.Left.Set(-240f, 1f);
            //设置面板距离屏幕最上端的距离
            Operator.Top.Set(60f, 0f);
            MainPanel.Append(Operator);
            //实例化一个面板
            ComposePanel = new CustomPanel();
            //设置面板的宽度
            ComposePanel.Width.Set(400, 0f);
            //设置面板的高度
            ComposePanel.Height.Set(400, 0f);
            //设置面板距离屏幕最左边的距离
            ComposePanel.Left.Set(40, 0f);
            //设置面板距离屏幕最上端的距离
            ComposePanel.Top.Set(60, 0f);
            ComposePanel.active = true;
            //将这个面板注册到UIState
            MainPanel.Append(ComposePanel);

            for (int i = 0; i < OuterSlotLength; i++)
            {
                OuterSlotGroup[i] = new VanillaItemSlotWrapper(4, 1f);
                //设置按钮距离所属ui部件的最左端的距离
                OuterSlotGroup[i].Left.Set(-26f, 0.5f + 0.375f * MathF.Cos(MathF.PI * i / 8 * 2));
                //设置按钮距离所属ui部件的最顶端的距离
                OuterSlotGroup[i].Top.Set(-26f, 0.5f + 0.375f * MathF.Sin(MathF.PI * i / 8 * 2));
                //将按钮注册入面板中，这个按钮的坐标将以面板的坐标为基础计算
                ComposePanel.Append(OuterSlotGroup[i]);
            }

            for (int i = 0; i < MiddleSlotLength; i++)
            {
                MiddleSlotGroup[i] = new VanillaItemSlotWrapper(4, 1f);
                //设置按钮距离所属ui部件的最左端的距离
                MiddleSlotGroup[i].Left.Set(-26f, 0.5f + 0.375f * 0.375f * MathF.Cos(MathF.PI * i / 4 * 2));
                //设置按钮距离所属ui部件的最顶端的距离
                MiddleSlotGroup[i].Top.Set(-26f, 0.5f + 0.375f * 0.375f * MathF.Sin(MathF.PI * i / 4 * 2));
                //将按钮注册入面板中，这个按钮的坐标将以面板的坐标为基础计算
                ComposePanel.Append(MiddleSlotGroup[i]);
            }



            DictionaryPanel = new CustomPanel();
            DictionaryPanel.Width.Set(400, 0f);
            //设置面板的高度
            DictionaryPanel.Height.Set(400, 0f);
            //设置面板距离屏幕最左边的距离
            DictionaryPanel.Left.Set(40, 0f);
            //设置面板距离屏幕最上端的距离
            DictionaryPanel.Top.Set(60, 0f);
            MainPanel.Append(DictionaryPanel);
            for (int i = 0; i < OuterSlotLength; i++)
            {
                DictionaryImages[i] = new RecipeItemShow(TextureAssets.InventoryBack);
                //设置按钮距离所属ui部件的最左端的距离
                DictionaryImages[i].Left.Set(-26f, 0.5f + 0.375f * MathF.Cos(MathF.PI * i / 8 * 2));
                //设置按钮距离所属ui部件的最顶端的距离
                DictionaryImages[i].Top.Set(-26f, 0.5f + 0.375f * MathF.Sin(MathF.PI * i / 8 * 2));
                DictionaryImages[i].active = false;
                //将按钮注册入面板中，这个按钮的坐标将以面板的坐标为基础计算
                DictionaryPanel.Append(DictionaryImages[i]);
            }
            DictionaryProductImages = new RecipeItemShow(TextureAssets.InventoryBack);
            DictionaryProductImages.Left.Set(-26f, 0.5f);
            DictionaryProductImages.Top.Set(-26f, 0.5f);
            DictionaryProductImages.active = false;
            DictionaryPanel.Append(DictionaryProductImages);

            var buttonTex = ModContent.Request<Texture2D>(AssetUtils.UI + "CookPrep");
            CustomButton closeButton = new CustomButton(buttonTex, "奥术合成");
            closeButton.Width.Set(176, 0);
            closeButton.Height.Set(42, 0);
            closeButton.Left.Set(-88, 0.5f);
            closeButton.Top.Set(-21, 0.1f);
            closeButton.Clicked += CloseButton_OnLeftClick;
            Operator.Append(closeButton);

            CustomButton closeButton1 = new CustomButton(buttonTex, "铭文蚀刻");
            closeButton1.Width.Set(176, 0);
            closeButton1.Height.Set(42, 0);
            closeButton1.Left.Set(-88, 0.5f);
            closeButton1.Top.Set(-21, 0.2f);
            //Operator.Append(closeButton1);

            ComposePanelButton = new CustomButton(buttonTex, ComposePanelButtonString);
            ComposePanelButton.Width.Set(176, 0);
            ComposePanelButton.Height.Set(42, 0);
            ComposePanelButton.Left.Set(40 + 200 - 88, 0);
            ComposePanelButton.Top.Set(-21, 0.07f);
            ComposePanelButton.Clicked += ChangeComposeButtonString;
            MainPanel.Append(ComposePanelButton);

            LeftButton = new CustomButton(ModContent.Request<Texture2D>(AssetUtils.UI + "LeftButton"), null);
            LeftButton.Width.Set(22, 0);
            LeftButton.Height.Set(22, 0);
            LeftButton.Left.Set(40 + 100 - 11, 0);
            LeftButton.Top.Set(-11, 0.07f);
            LeftButton.active = false;
            LeftButton.Clicked += () => { RecipeShowCount--; UpdateRecipeImages(); };
            MainPanel.Append(LeftButton);


            RightButton = new CustomButton(ModContent.Request<Texture2D>(AssetUtils.UI + "RightButton"), null);
            RightButton.Width.Set(22, 0);
            RightButton.Height.Set(22, 0);
            RightButton.Left.Set(40 + 300 - 11, 0);
            RightButton.Top.Set(-11, 0.07f);
            RightButton.active = false;
            RightButton.Clicked += () => { RecipeShowCount++; UpdateRecipeImages(); };
            MainPanel.Append(RightButton);
        }


        private void CloseButton_OnLeftClick()
        {
            AlterCompose();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }



        private void AlterCompose()
        {
            //读取合成表
            List<MysteriousAlterRecipe> mysteriousAlterRecipes = new List<MysteriousAlterRecipe>();
            mysteriousAlterRecipes = JSON_VanillaReader.Instance.GetJsonList<MysteriousAlterRecipe>("VanillaJSONFronting/MysteriousAlterRecipe");
            foreach (var recipe in mysteriousAlterRecipes)
            {
                if (CheckContextCompared(OuterSlotGroup, recipe.OuterIngredient) && CheckContextCompared(MiddleSlotGroup, recipe.MiddleIngredient))
                {
                    for (int i = 0; i < OuterSlotLength; i++)
                    {
                        //将合成表中对应Tex发送给TileEntity
                        MysteriousAlterTileEntity.OuterIngredient[i] = OuterSlotGroup[i].Item.Clone();
                        //减少对应物品
                        OuterSlotGroup[i].ComsumItem(recipe.OuterIngredient[i].Stack);

                    }
                    for (int i = 0; i < MiddleSlotLength; i++)
                    {
                        //将合成表中对应Tex发送给TileEntity
                        MysteriousAlterTileEntity.MiddleIngredient[i] = MiddleSlotGroup[i].Item.Clone();
                        //减少对应物品
                        MiddleSlotGroup[i].ComsumItem(recipe.MiddleIngredient[i].Stack);

                    }
                    //将合成表中对应Tex发送给TileEntity
                    MysteriousAlterTileEntity.Product = GetItem(recipe.Product);
                    MysteriousAlterTileEntity.AltarComposeAnimation = true;
                    Visable = false;
                }
            }


        }
        /// <summary>
        /// 检查Context是否正确
        /// </summary>
        /// <param name="InSlot"></param>
        /// <param name="Contexts"></param>
        /// <returns></returns>
        private bool CheckContextCompared(VanillaItemSlotWrapper[] InSlot, ItemContext[] Contexts)
        {
            for (int i = 0; i < InSlot.Length; i++)
            {
                if (!(InSlot[i].CompareItem(Contexts[i].GetRealID())) && InSlot[i].Item.stack < Contexts[i].Stack)
                {
                    return false;
                }
            }
            return true;
        }

        private Item GetItem(ItemContext itemContext)
        {
            if (itemContext.ItemIDMode == ItemIDMode.TR)
            {
                return new Item(int.Parse(itemContext.ItemID));
            }
            else if (itemContext.ItemIDMode == ItemIDMode.MOD)
            {
                return new Item(int.Parse(itemContext.ItemID));
            }
            return null;
        }

        private void ChangeComposeButtonString()
        {
            ComposeButtonStringShow = !ComposeButtonStringShow;
            if (ComposeButtonStringShow)
            {
                ComposePanelButton._text = ComposeStirng;
                ComposePanel.active = true;
                DictionaryPanel.active = false;
                RightButton.active = false;
                LeftButton.active = false;
                for (int i = 0; i < OuterSlotLength; i++)
                {
                    DictionaryImages[i].active = false;
                }
                DictionaryProductImages.active = false;
            }
            else
            {
                ComposePanelButton._text = DictionaryString;
                ComposePanel.active = false;
                DictionaryPanel.active = true;
                RightButton.active = true;
                LeftButton.active = true;
                for (int i = 0; i < OuterSlotLength; i++)
                {
                    DictionaryImages[i].active = true;
                }
                DictionaryProductImages.active = true;
                UpdateRecipeImages();
            }
        }

        private List<Item> GetRecipeTextures(int recipeIndex)
        {
            //读取合成表
            List<MysteriousAlterRecipe> mysteriousAlterRecipes = new List<MysteriousAlterRecipe>();
            mysteriousAlterRecipes = JSON_VanillaReader.Instance.GetJsonList<MysteriousAlterRecipe>("VanillaJSONFronting/MysteriousAlterRecipe");
            int RecipesCounts = mysteriousAlterRecipes.Count;
            int FinalIndex = recipeIndex % RecipesCounts;
            List<Item> RecipeTextures = new List<Item>();
            for (int i = 0; i < OuterSlotLength; i++)
            {
                int ItemID = mysteriousAlterRecipes[FinalIndex].OuterIngredient[i].GetRealID();
                Item ItemTex = mysteriousAlterRecipes[FinalIndex].OuterIngredient[i].GetItem();
                RecipeTextures.Add(ItemTex);
            }
            Item ProductTex = mysteriousAlterRecipes[FinalIndex].Product.GetItem();
            RecipeTextures.Add(ProductTex);
            return RecipeTextures;
        }


        private void UpdateRecipeImages()
        {
            int[] stack = new int[OuterSlotLength + 1];
            List<Item> RecipeTex = GetRecipeTextures(RecipeShowCount);
            for (int i = 0; i < OuterSlotLength; i++)
            {
                DictionaryImages[i].item = RecipeTex[i];
            }
            DictionaryProductImages.item = RecipeTex[OuterSlotLength];
        }
    }
}