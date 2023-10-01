using Microsoft.Xna.Framework;
using MysteriousAlchemy.Items;
using MysteriousAlchemy.UI.UIElements;
using MysteriousAlchemy.VanillaJSONFronting;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MysteriousAlchemy.UI
{
    public class UI_Extractor : UIState
    {
        private static int BeforeLength = 3;
        private static int AfterLength = 3;
        public static bool Visable = false;
        VanillaItemSlotWrapper[] BeforeExtractorItemSlotGroup = new VanillaItemSlotWrapper[BeforeLength];
        VanillaItemSlotWrapper[] AfterExtractorItemSlotGroup = new VanillaItemSlotWrapper[AfterLength];
        public override void OnInitialize()
        {

            //实例化一个面板
            UIPanel panel = new UIPanel();
            //设置面板的宽度
            panel.Width.Set(488f, 0f);
            //设置面板的高度
            panel.Height.Set(568f, 0f);
            //设置面板距离屏幕最左边的距离
            panel.Left.Set(-244f, 0.5f);
            //设置面板距离屏幕最上端的距离
            panel.Top.Set(-284f, 0.5f);
            //将这个面板注册到UIState
            Append(panel);
            base.OnInitialize();

            for (int i = 0; i < BeforeLength; i++)
            {
                BeforeExtractorItemSlotGroup[i] = new VanillaItemSlotWrapper(4, 1f);
                //设置按钮距离所属ui部件的最左端的距离
                BeforeExtractorItemSlotGroup[i].Left.Set(-11f, 0.1f + 0.15f * i);
                //设置按钮距离所属ui部件的最顶端的距离
                BeforeExtractorItemSlotGroup[i].Top.Set(-11f, 0.1f);
                //将按钮注册入面板中，这个按钮的坐标将以面板的坐标为基础计算
                //panel.Append(BeforeExtractorItemSlotGroup[i]);
            }


            for (int i = 0; i < BeforeLength; i++)
            {
                AfterExtractorItemSlotGroup[i] = new VanillaItemSlotWrapper(4, 1f);
                //设置按钮距离所属ui部件的最左端的距离
                AfterExtractorItemSlotGroup[i].Left.Set(-11f, 0.1f + 0.15f * i);
                //设置按钮距离所属ui部件的最顶端的距离
                //AfterExtractorItemSlotGroup[i].Top.Set(-11f, 0.3f);


                //将按钮注册入面板中，这个按钮的坐标将以面板的坐标为基础计算
                //panel.Append(AfterExtractorItemSlotGroup[i]);
            }
            UIImageButton closeButton = new UIImageButton(TextureAssets.EmoteMenuButton);
            closeButton.Width.Set(TextureAssets.EmoteMenuButton.Width(), 0f);
            closeButton.Height.Set(TextureAssets.EmoteMenuButton.Height(), 0f);
            closeButton.Left.Set(-TextureAssets.EmoteMenuButton.Height() / 2f, 0.9f);
            closeButton.Top.Set(-TextureAssets.EmoteMenuButton.Height() / 2f, 0.1f);
            closeButton.OnLeftClick += CloseButton_OnLeftClick;
            panel.Append(closeButton);
        }
        private void CloseButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {

        }
        public override void Update(GameTime gameTime)
        {

            ExtarcteRecipeCheck();


            base.Update(gameTime);
        }

        public void ExtarcteRecipeCheck()
        {
            List<ExtarcteRecipe> extarcteRecipes;
            extarcteRecipes = JSON_VanillaReader.Instance.GetJsonList<ExtarcteRecipe>("VanillaJSONFronting/ExtracteRecipe");
            foreach (ExtarcteRecipe item in extarcteRecipes)
            {
                for (int i = 0; i < BeforeLength; i++)
                {
                    if (BeforeExtractorItemSlotGroup[i].CompareItem(item.GetIngredientItemID()))
                    {
                        for (int j = 0; j < AfterLength; j++)
                        {
                            if ((AfterExtractorItemSlotGroup[j].IsAir()) || (AfterExtractorItemSlotGroup[j].CompareItem(item.GetProductItemID()) && !AfterExtractorItemSlotGroup[j].IsFull()))
                            {
                                BeforeExtractorItemSlotGroup[i].ComsumItem(item.Ingredient.Stack);
                                AfterExtractorItemSlotGroup[j].AddItem(item.GetProductItemID(), item.Product.Stack);
                            }
                        }
                    }
                }
            }
        }

    }
}