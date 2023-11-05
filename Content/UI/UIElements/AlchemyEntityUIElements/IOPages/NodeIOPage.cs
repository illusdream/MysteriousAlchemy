using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Enum;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.IOPages
{

    public class NodeIOPage : IOPage
    {

        public AlchemyEntity entity
        {
            get
            {
                if (AlchemySystem.FindAlchemyEntitySafely<AlchemyEntity>(unicode, out var result))
                {
                    return result;
                }
                return null;
            }
        }
        public NodeIOPage(Pull_outButtom pull_OutButtom, int AnimationTime, Vector2 StaticEdgeRange, Vector2 ActiveEdgeRange, Vector2 PulloutVector, Vector2 bottomOriginOffest) : base(pull_OutButtom, AnimationTime, StaticEdgeRange, ActiveEdgeRange, PulloutVector, bottomOriginOffest)
        {


        }
        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }
        public override void OnSetUnicode(AlchemyUnicode unicode)
        {
            base.OnSetUnicode(unicode);
        }
    }

}
