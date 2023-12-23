using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Utils;
using MysteriousAlchemy.Content.Animators;
using MysteriousAlchemy.Content.Dusts;
using MysteriousAlchemy.Content.Particles;
using MysteriousAlchemy.Content.Projectiles.Chilliness;
using MysteriousAlchemy.Content.UI;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Loader;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace MysteriousAlchemy.Content.Items.TestItem
{
    public class TestItem : ModItem
    {

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 15;
            Item.height = 15;
            Item.value = 100;
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.maxStack = 99;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.autoReuse = false;
        }

        public override bool? UseItem(Player player)
        {
            //if (UI_AlchemyEditor.visable)
            //{
            //    SoundEngine.PlaySound(MASoundID.MenuClose);
            //    if (Main.gameMenu)
            //        Main.menuMode = 0;
            //    else
            //        IngameFancyUI.Close();
            //    UIloader.GetUIState<UI_AlchemyEditor>().SetVisable(!UI_AlchemyEditor.visable);
            //}
            //else
            //{
            //    IngameFancyUI.OpenUIState(UIloader.GetUIState<UI_AlchemyEditor>());
            //    UIloader.GetUIState<UI_AlchemyEditor>().SetVisable(!UI_AlchemyEditor.visable);
            //}

            return base.UseItem(player);
        }
        public override void UpdateInventory(Player player)
        {

            base.UpdateInventory(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.BloomAreaDraw, () =>
            {
                spriteBatch.Draw(AssetUtils.GetTexture2D(AssetUtils.Texture + "White"), new Rectangle(0, 0, (int)(Main.screenWidth / 2f), (int)(Main.screenHeight / 2f)), Color.White);
            });

            base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            VisualPPSystem.AddAction(VisualPPSystem.VisualPPActionType.BloomAreaDraw, () =>
            {
                spriteBatch.Draw(AssetUtils.GetTexture2D(AssetUtils.Texture + "White"), new Rectangle((int)(Main.screenWidth / 2f), (int)(Main.screenHeight / 2f), 200, 200), Color.Red);
            });
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}