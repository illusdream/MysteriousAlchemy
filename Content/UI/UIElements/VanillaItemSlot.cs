using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.VanillaJSONFronting;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace MysteriousAlchemy.UI.UIElements
{
    internal class VanillaItemSlotWrapper : UIElement
    {
        internal Item Item;
        private readonly int _context;
        private readonly float _scale;
        private readonly Vector2 _size;
        internal Func<Item, bool> ValidItemFunc;

        internal event Action<int> OnEmptyMouseover;

        private int timer = 0;

        internal VanillaItemSlotWrapper(int context = ItemSlot.Context.BankItem, float scale = 1f)
        {
            _context = context;
            _scale = scale;
            Item = new Item();
            Item.SetDefaults(0);

            var inventoryBack9 = TextureAssets.InventoryBack9;
            _size = new Vector2(inventoryBack9.Width() * scale, inventoryBack9.Width() * scale);
            Width.Set(inventoryBack9.Width() * scale, 0f);
            Height.Set(inventoryBack9.Height() * scale, 0f);
        }

        internal VanillaItemSlotWrapper(int context = ItemSlot.Context.BankItem, int width = 0)
        {
            _context = context;
            _size = new Vector2(width, width);
            Item = new Item();
            Item.SetDefaults(0);
            _scale = width / TextureAssets.InventoryBack9.Width();
            var inventoryBack9 = TextureAssets.InventoryBack9;
            Width.Set(_size.X, 0f);
            Height.Set(_size.Y, 0f);
        }

        /// <summary>
        /// Returns true if this item can be placed into the slot (either empty or a pet item)
        /// </summary>
        internal bool Valid(Item item)
        {
            return ValidItemFunc(item);
        }

        internal void HandleMouseItem()
        {
            if (ValidItemFunc == null || Valid(Main.mouseItem))
            {
                //Handles all the click and hover actions based on the context
                ItemSlot.Handle(ref Item, _context);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = _scale;
            Rectangle rectangle = GetDimensions().ToRectangle();

            bool contains = ContainsPoint(Main.MouseScreen);

            if (contains && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
                HandleMouseItem();
            }
            ItemSlot.Draw(spriteBatch, ref Item, _context, rectangle.TopLeft());

            if (contains && Item.IsAir)
            {
                timer++;
                OnEmptyMouseover?.Invoke(timer);
            }
            else if (!contains)
            {
                timer = 0;
            }

            Main.inventoryScale = oldScale;
        }

        public bool IsAir()
        {
            if (Item.IsAir)
            {
                return true;
            }
            return false;
        }

        public bool IsFull()
        {
            return Item.stack == Item.maxStack;
        }

        public bool AddItem(int ItemID, int stack)
        {
            Item item = new Item(ItemID, stack);

            if (IsAir())
            {
                Item = item;
                return true;
            }
            if (!IsAir() && CompareItem(ItemID))
            {
                if (Item.stack + stack > Item.maxStack)
                {
                    return true;
                }
                Item.stack += stack;
                return false;
            }

            return true;
        }

        public bool ComsumItem(int stack)
        {
            if (Item.stack - stack < 0)
            {
                return false;
            }
            if (Item.stack - stack == 0)
            {
                Item.SetDefaults();
                return true;
            }
            Item.stack -= stack;
            return true;
        }

        public bool CompareItem(int OutItemID)
        {
            if (Item.type == OutItemID)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    internal class VanillaItemSlotGroup : UIElement
    {

    }
}