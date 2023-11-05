using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.Alchemy.Graphs.Links;
using MysteriousAlchemy.Content.Alchemy.Graphs.Nodes;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.LinkShelfs
{
    public class EtherLinkShowShelf : LinkShowshelf
    {
        IconTextMoniter IconTextMoniter_End;
        IconTextMoniter IconTransferRate;
        CrossScrollBar crossScrollBar;
        EtherLink entity
        {
            get
            {
                AlchemySystem.etherGraph.FindLink(start, end, out var result);
                return result;
            }
        }
        public EtherLinkShowShelf(AlchemyUnicode start, AlchemyUnicode end) : base(start, end)
        {
            Height.Set(66, 0);


            IconTransferRate = new IconTextMoniter(AssetUtils.GetUIImageTextInfoValue("AlchemyUI.Link.Ether.TransferRate"));
            Append(IconTransferRate);
            IconTextMoniter_End = new IconTextMoniter(AssetUtils.GetUIImageTextInfoValue("AlchemyUI.Node.Ether.Ether_now"));
            IconTextMoniter_End.Left.Set(0, 0.5f);
            Append(IconTextMoniter_End);
            crossScrollBar = new CrossScrollBar();
            crossScrollBar.Width.Set(-24, 1);
            crossScrollBar.Left.Set(12, 0);
            crossScrollBar.Top.Set(40, 0);
            Append(crossScrollBar);
            crossScrollBar.SetRange(entity.MinCount, entity.MaxCount);
            crossScrollBar.ViewPosition = (entity.EtherCountPerFrame / (entity.MaxCount - entity.MinCount));
            crossScrollBar.OnDragging += ResetEther;

            AddDividingLine();
        }
        public void AddDividingLine()
        {
            DividingLine dividingLine_List_Opreator = new DividingLine(new Terraria.UI.StyleDimension(0, 1f));
            dividingLine_List_Opreator.Top.Set(0, 1f);
            Append(dividingLine_List_Opreator);
        }
        public override void Update(GameTime gameTime)
        {
            IconTextMoniter_End.SetText(end.value.ToString());
            IconTransferRate.SetText(entity.EtherCountPerFrame.ToString());
            base.Update(gameTime);
        }
        private void ResetEther(float value)
        {
            entity.SetEtherCountPerFrame(value);
        }
    }
}
