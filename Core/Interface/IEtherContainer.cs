using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Interface
{
    public interface IEtherContainer
    {
        public float Ether { get; set; }
        public float MaxEther { get; set; }

        public bool active { get; set; }
        public void Limit();

        public void ApplyEther(IEtherContainer etherContainer, float ether);

        public void ReciveEther(float ether);
    }
}
