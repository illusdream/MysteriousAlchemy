using Basic.Reference.Assemblies;
using Humanizer;
using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Alchemy.AlchemyEntities;
using MysteriousAlchemy.Content.Animators;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System.Diagnostics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace MysteriousAlchemy.Content.Items.TestItem
{
    public class TestEtherProducerItem : ModItem
    {
        public override string Texture => AssetUtils.Items + Name;
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
            Item.useTime = 1;
            Item.useAnimation = 1;
            Item.useStyle = ItemUseStyleID.Swing;
        }
        public override bool? UseItem(Player player)
        {
            for (int i = 0; i < 1; i++)
            {

                if (AlchemySystem.TryRegisterAlchemyEntity(typeof(TestEtherProducer), out var result))
                {

                    result.TopLeft = Main.MouseWorld + Main.rand.NextVector2Circular(200, 200);
                    result.Animator.Position = result.TopLeft;
                }
                else
                {
                    DebugUtils.NewText("false");
                }
                //AnimatorManager.Instance.Register<BaseAEAnimator>().Position = 
            }


            return base.UseItem(player);
        }
    }
}