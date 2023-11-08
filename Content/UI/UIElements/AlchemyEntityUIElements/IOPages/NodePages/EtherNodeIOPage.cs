using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.IOPages.NodePages
{
    public class EtherNodeIOPage : NodeIOPage
    {
        IconTextMoniter etherNow;
        IconTextMoniter etherMax;
        public EtherNodeIOPage(Pull_outButtom pull_OutButtom, int AnimationTime, Vector2 StaticEdgeRange, Vector2 ActiveEdgeRange, Vector2 PulloutVector, Vector2 bottomOriginOffest) : base(pull_OutButtom, AnimationTime, StaticEdgeRange, ActiveEdgeRange, PulloutVector, bottomOriginOffest)
        {
            etherNow = new IconTextMoniter(AssetUtils.GetUIImageTextInfoValue("AlchemyUI.Node.Ether.Ether_now"), 40);
            Append(etherNow);
            etherMax = new IconTextMoniter(AssetUtils.GetUIImageTextInfoValue("AlchemyUI.Node.Ether.Ether_Max"), 40);
            Append(etherMax);
        }
        public override void Update(GameTime gameTime)
        {
            if (entity != null)
            {
                etherNow.SetText(Math.Round(entity.Ether, 2).ToString());
                etherMax.SetText(Math.Round(entity.MaxEther, 2).ToString());
            }
            base.Update(gameTime);
        }
    }
}
