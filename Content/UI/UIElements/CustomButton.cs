using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Drawing;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Color = Microsoft.Xna.Framework.Color;

namespace MysteriousAlchemy.UI.UIElements
{
    public class CustomButton : UIImageButton
    {
        public float scale;
        public string _text;
        public Action Clicked;
        public bool active;
        public CustomButton(Asset<Texture2D> texture, string Text) : base(texture)
        {
            _text = Text;
            active = true;
        }



        public override void MouseOver(UIMouseEvent evt)
        {
            Clicked();
            base.MouseOver(evt);
        }

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            Clicked();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            bool contains = ContainsPoint(Main.MouseScreen);

            if (active)
            {
                if (contains && !PlayerInput.IgnoreMouseInterface)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease && Clicked != null)
                    {
                        Clicked();
                    }
                }
                if (_text != null)
                {
                    Terraria.Utils.DrawBorderStringBig(spriteBatch, _text, GetDimensions().Center(), Color.White, 0.5f, 0.5f, 0.3f);
                    Terraria.Utils.DrawBorderStringFourWay(spriteBatch, (ReLogic.Graphics.DynamicSpriteFont)FontAssets.MouseText, _text, GetDimensions().ToRectangle().TopLeft().X, GetDimensions().ToRectangle().TopLeft().Y, Color.White, Color.Black, new Vector2(0.5f, 0.5f), scale);
                }
                base.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}