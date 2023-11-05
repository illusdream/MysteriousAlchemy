using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    public class NodeIOPulloutPages : UIElement
    {
        public AlchemyUnicode unicode;


        private float closeHeight;
        private float openHeight;

        public bool InPageAnimation;

        public Pull_outButtom NodeOpreationPageButtom;
        public PulloutInnerPanel NodeOpreationPage;
        private bool _NodeOpreateOpen;
        public bool NodeOpreateOpen { get { return _NodeOpreateOpen; } }

        public Pull_outButtom LinkOpreationPageButtom;
        public PulloutInnerPanel LinkOpreationPage;
        private bool _LinkOpreateOpen;
        public bool LinkOpreateOpen { get { return _LinkOpreateOpen; } }

        public Pull_outButtom AdjacencyNodeOpreationPageButtom;
        public PulloutInnerPanel AdjacencyNodeOpreationPage;
        private bool _AdjacencyNodeOpreateOpen;
        public bool AdjacencyNodeOpreateOpen { get { return _AdjacencyNodeOpreateOpen; } }











        private void AddPulloutPages()
        {

        }
    }
}
