using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Utils;
using ReLogic.Content;
using System.Drawing;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.UI.Chat;
using Terraria.GameInput;
using static Terraria.GameContent.Animations.On_Actions.Sprites;

namespace MysteriousAlchemy.UI.UIElements
{
    public class RecipeItemShow : CustomImage
    {
        private bool PlayerHasItem;
        public Item item;
        public RecipeItemShow(Asset<Texture2D> texture) : base(texture)
        {


        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (PlayerHasItem)
            {
                SetImage(TextureAssets.InventoryBack);
            }
            else
            {
                SetImage(TextureAssets.InventoryBack11);
            }

        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = Width.Pixels / TextureAssets.InventoryBack9.Width();
            bool contains = ContainsPoint(Main.MouseScreen);


            if (item != null)
            {
                Width.Set(TextureAssets.InventoryBack9.Width(), 0);
                Height.Set(TextureAssets.InventoryBack9.Height(), 0);
                ItemSlot.Draw(spriteBatch, ref item, ItemSlot.Context.BankItem, GetDimensions().ToRectangle().TopLeft());
                if (contains && !PlayerInput.IgnoreMouseInterface)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    ItemSlot.MouseHover(ref item);
                }
            }
            Main.inventoryScale = oldScale;
        }
        public void SetPlayerHasItem(bool value)
        {
            PlayerHasItem = value;
        }
    }
}