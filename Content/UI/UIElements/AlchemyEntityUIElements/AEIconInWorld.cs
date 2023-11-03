using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    public class AEIconInWorld : UIElement
    {
        public AlchemyUnicode unicode;

        public AEIconInWorld(AlchemyUnicode unicode)
        {
            this.unicode = unicode;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            CalculatedStyle Dimension = GetDimensions();
            Rectangle target = Dimension.ToRectangle();
            if (AlchemySystem.FindAlchemyEntitySafely(unicode, out AlchemyEntity entity))
            {
                target.Width = (int)(entity.Size.X * Main.UIScale);
                target.Height = (int)(entity.Size.Y * Main.UIScale);
            }
            spriteBatch.Draw(AssetUtils.GetTexture2D(AssetUtils.UI_Alchemy + "AEmciroIcon_0"), target, Color.White);
        }
        public override void Update(GameTime gameTime)
        {
            if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (AlchemySystem.FindAlchemyEntitySafely(unicode, out AlchemyEntity entity))
            {
                Left.Set(entity.TopLeft.ToScreenPosition().X, 0);
                Top.Set(entity.TopLeft.ToScreenPosition().Y, 0);

                Width.Set(entity.Size.X * Main.UIScale, 0);
                Height.Set(entity.Size.Y * Main.UIScale, 0);

                Recalculate();
            }

            base.Update(gameTime);
        }
    }
}