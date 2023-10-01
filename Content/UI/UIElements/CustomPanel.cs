using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.UI.UIElements
{
    public class CustomPanel : UIPanel
    {
        public bool active;
        public CustomPanel() : base()
        {

        }
        public override void Update(GameTime gameTime)
        {
            if (active)
            {
                base.Update(gameTime);
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                base.Draw(spriteBatch);
            }

        }
    }
}