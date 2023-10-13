using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.Animators;
using MysteriousAlchemy.Content.TileEntitys;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Tiles;
using MysteriousAlchemy.UI.UIElements;
using MysteriousAlchemy.Utils;
using MysteriousAlchemy.VanillaJSONFronting;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.UI;

namespace MysteriousAlchemy.UI
{
    public class UI_AltarCompose : BetterUIState
    {
        public static bool visable = false;
        public override int UILayer(List<GameInterfaceLayer> layers) => layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        public override bool Visable => visable;

        public static MysteriousAltarTileEntity MysteriousAlterTileEntity = null;
        public static AltarAnimator AltarAnimator = null;

        public static List<VanillaItemSlotWrapper> ItemSlots = new List<VanillaItemSlotWrapper>();
        public override void OnInitialize()
        {
            base.OnInitialize();
            for (int i = 0; i < 8; i++)
            {
                VanillaItemSlotWrapper vanillaItemSlotWrapper = new VanillaItemSlotWrapper(4, 0.55f);
                ItemSlots.Add(vanillaItemSlotWrapper);
                Append(vanillaItemSlotWrapper);
            }

        }
        public UI_AltarCompose() : base()
        {

        }
        public override void OnActivate()
        {
            base.OnActivate();

        }

        public override void Update(GameTime gameTime)
        {
            Recalculate();
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (AltarAnimator != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    ItemSlots[i].Left.Set(AltarAnimator.Position.ToScreenPosition().X + MathUtils.GetVector2InCircle(MathHelper.TwoPi / 8f * i, 125f).X - ItemSlots[i].GetDimensions().Width / 2f, 0);
                    ItemSlots[i].Top.Set(AltarAnimator.Position.ToScreenPosition().Y + MathUtils.GetVector2InCircle(MathHelper.TwoPi / 8f * i, 125f).Y - ItemSlots[i].GetDimensions().Height / 2f, 0);
                    ItemSlots[i].Recalculate();
                }
                Recalculate();
            }
            for (int i = 0; i < 8; i++)
            {
                if (MysteriousAlterTileEntity != null)
                {
                    MysteriousAlterTileEntity.Ingredient[i] = ItemSlots[i].Item;
                }
            }

            base.Draw(spriteBatch);
        }
        public static bool CheckContextCompared(List<VanillaItemSlotWrapper> InSlot, ItemContext[] Contexts)
        {
            for (int i = 0; i < Contexts.Length; i++)
            {
                if (!(InSlot[i].Item.type == Contexts[i].GetRealID()) || InSlot[i].Item.stack < Contexts[i].Stack)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool CheckCanCompose()
        {
            List<MysteriousAlterRecipe> mysteriousAlterRecipes = new List<MysteriousAlterRecipe>();
            mysteriousAlterRecipes = JSON_VanillaReader.Instance.GetJsonList<MysteriousAlterRecipe>("VanillaJSONFronting/MysteriousAlterRecipe");
            foreach (var recipe in mysteriousAlterRecipes)
            {
                if (CheckContextCompared(ItemSlots, recipe.OuterIngredient))
                {
                    return true;
                }

            }
            return false;
        }
        public static Item ComposeItem()
        {
            int cout = 0;
            List<MysteriousAlterRecipe> mysteriousAlterRecipes = new List<MysteriousAlterRecipe>();
            mysteriousAlterRecipes = JSON_VanillaReader.Instance.GetJsonList<MysteriousAlterRecipe>("VanillaJSONFronting/MysteriousAlterRecipe");
            for (int i = 0; i < mysteriousAlterRecipes.Count; i++)
            {
                cout++;
                if (CheckContextCompared(ItemSlots, mysteriousAlterRecipes[i].OuterIngredient))
                {
                    for (int k = 0; k < 8; k++)
                    {
                        ItemSlots[k].ComsumItem(mysteriousAlterRecipes[i].OuterIngredient[k].Stack);
                    }
                    var instance = new Item(mysteriousAlterRecipes[i].Product.GetRealID());
                    instance.stack = mysteriousAlterRecipes[i].Product.Stack;
                    return instance;
                }
            }
            return null;
        }


    }
}