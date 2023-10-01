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

            //ʵ����һ�����
            UIPanel panel = new UIPanel();
            //�������Ŀ��
            panel.Width.Set(488f, 0f);
            //�������ĸ߶�
            panel.Height.Set(568f, 0f);
            //������������Ļ����ߵľ���
            panel.Left.Set(-244f, 0.5f);
            //������������Ļ���϶˵ľ���
            panel.Top.Set(-284f, 0.5f);
            //��������ע�ᵽUIState
            Append(panel);
            base.OnInitialize();

            for (int i = 0; i < BeforeLength; i++)
            {
                BeforeExtractorItemSlotGroup[i] = new VanillaItemSlotWrapper(4, 1f);
                //���ð�ť��������ui����������˵ľ���
                BeforeExtractorItemSlotGroup[i].Left.Set(-11f, 0.1f + 0.15f * i);
                //���ð�ť��������ui��������˵ľ���
                BeforeExtractorItemSlotGroup[i].Top.Set(-11f, 0.1f);
                //����ťע��������У������ť�����꽫����������Ϊ��������
                //panel.Append(BeforeExtractorItemSlotGroup[i]);
            }


            for (int i = 0; i < BeforeLength; i++)
            {
                AfterExtractorItemSlotGroup[i] = new VanillaItemSlotWrapper(4, 1f);
                //���ð�ť��������ui����������˵ľ���
                AfterExtractorItemSlotGroup[i].Left.Set(-11f, 0.1f + 0.15f * i);
                //���ð�ť��������ui��������˵ľ���
                //AfterExtractorItemSlotGroup[i].Top.Set(-11f, 0.3f);


                //����ťע��������У������ť�����꽫����������Ϊ��������
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