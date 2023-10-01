using Microsoft.Xna.Framework;
using MysteriousAlchemy.Core.Mapping;
using MysteriousAlchemy.Items;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.VanillaJSONFronting
{
    public struct ExtarcteRecipe
    {
        public ExtarcteRecipe(ItemContext _Ingredient, ItemContext _Product)
        {
            Ingredient = _Ingredient;
            Product = _Product;
        }
        public ItemContext Ingredient;

        public ItemContext Product;

        public int GetIngredientItemID()
        {
            if (Ingredient.ItemIDMode == ItemIDMode.TR)
            {
                return int.Parse(Ingredient.ItemID);
            }
            else if (Ingredient.ItemIDMode == ItemIDMode.MOD)
            {
                return ModItemIDMap.GetModItemID(Product.ItemID);
            }
            return 0;
        }

        public int GetProductItemID()
        {
            if (Product.ItemIDMode == ItemIDMode.TR)
            {
                return int.Parse(Product.ItemID);
            }
            else if (Product.ItemIDMode == ItemIDMode.MOD)
            {
                return ModItemIDMap.GetModItemID(Product.ItemID);
            }
            return 0;
        }
    }

    public struct MysteriousAlterRecipe
    {
        public ItemContext[] OuterIngredient = new ItemContext[8];
        public ItemContext[] MiddleIngredient = new ItemContext[4];
        public ItemContext Product = new ItemContext();

        public MysteriousAlterRecipe()
        {

        }
    }
    public struct ItemContext
    {
        public ItemContext(ItemIDMode _ItemIDMode, string _ItemID, int _Stack)
        {
            ItemIDMode = _ItemIDMode;
            ItemID = _ItemID;
            Stack = _Stack;
        }

        public ItemIDMode ItemIDMode;
        public string ItemID;
        public int Stack;

        public int GetRealID()
        {
            if (ItemIDMode == ItemIDMode.TR)
            {
                return int.Parse(ItemID);
            }
            else if (ItemIDMode == ItemIDMode.MOD)
            {
                return ModItemIDMap.GetModItemID(ItemID);
            }
            return 0;
        }
        public Item GetItem()
        {
            return new Item(GetRealID());
        }
    }
    public enum ItemIDMode
    {
        TR, MOD
    }
}