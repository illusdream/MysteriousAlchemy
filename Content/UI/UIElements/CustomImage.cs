using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Utils;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;

namespace MysteriousAlchemy.UI.UIElements
{
    public class CustomImage : UIImage
    {
        public bool active;
        private static Texture2D TransparentTex = AssetUtils.GetTexture2D(AssetUtils.UI + "Transparent");
        public CustomImage(Asset<Texture2D> texture) : base(texture)
        {

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