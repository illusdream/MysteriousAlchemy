using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.Alchemy.Graphs.Links;
using MysteriousAlchemy.Content.Alchemy.Graphs.Nodes;
using MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.IOPages.LinkPages;
using MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.LinkShelfs;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Loader;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements.AdjLinkShelfs
{
    public class AdjEtherLinkShowShelf : LinkShowshelf
    {
        IconTextMoniter IconTextMoniter_End;
        IconTextMoniter IconTransferRate;
        CrossScrollBar crossScrollBar;
        ImageButtom CancelLinkButtom;
        EtherLink entity
        {
            get
            {
                AlchemySystem.etherGraph.FindLink(start, end, out var result);
                return result;
            }
        }
        public AdjEtherLinkShowShelf(AlchemyUnicode start, AlchemyUnicode end) : base(start, end)
        {
            Height.Set(88, 0);


            IconTransferRate = new IconTextMoniter(AssetUtils.GetUIImageTextInfoValue("AlchemyUI.Link.Ether.TransferRate"), 30);
            IconTransferRate.Height.Set(30, 0);
            IconTransferRate.Top.Set(54, 0);
            Append(IconTransferRate);

            IconTextMoniter_End = new IconTextMoniter(AssetUtils.GetUIImageTextInfoValue("AlchemyUI.Node.Ether.Ether_now"), 30);
            IconTextMoniter_End.Height.Set(30, 0);
            IconTextMoniter_End.Top.Set(0, 0);
            Append(IconTextMoniter_End);

            crossScrollBar = new CrossScrollBar();
            crossScrollBar.Width.Set(-24, 1);
            crossScrollBar.Left.Set(12, 0);
            crossScrollBar.Top.Set(30, 0);
            Append(crossScrollBar);
            crossScrollBar.SetRange(entity.MinCount, entity.MaxCount);
            crossScrollBar.ViewPosition = (entity.EtherCountPerFrame / (entity.MaxCount - entity.MinCount));
            crossScrollBar.OnDragging += ResetEther;

            CancelLinkButtom = new ImageButtom(AssetUtils.UI + "Cancel");
            CancelLinkButtom.Height.Set(30, 0);
            CancelLinkButtom.Width.Set(30, 0);
            CancelLinkButtom.Left.Set(-36, 1);
            CancelLinkButtom.OnLeftClick += CancelLinkButtom_OnLeftClick;
            Append(CancelLinkButtom);


            AddDividingLine();
        }

        private void CancelLinkButtom_OnLeftClick(Terraria.UI.UIMouseEvent evt, Terraria.UI.UIElement listeningElement)
        {
            AlchemySystem.etherGraph.RemoveLink(start, end);
            UIloader.GetUIState<UI_AlchemyEditor>().viewNodePanel.ResetViewPanel();
            removeSelf?.Invoke(this);
        }

        public void AddDividingLine()
        {
            DividingLine dividingLine_List_Opreator = new DividingLine(new Terraria.UI.StyleDimension(0, 1f));
            dividingLine_List_Opreator.Top.Set(0, 1f);
            Append(dividingLine_List_Opreator);
        }
        public override void Update(GameTime gameTime)
        {
            if (entity != null)
            {
                //传输对象的名字，如果没有名字就是unicode
                IconTextMoniter_End.SetText(entity?.startName);

                //传输速率
                IconTransferRate.SetText("被" + Math.Round((entity.EtherCountPerFrame * 60), 2).ToString() + " E/s");
            }

            base.Update(gameTime);
        }
        private void ResetEther(float value)
        {
            entity.SetEtherCountPerFrame(value);
        }

    }
}
