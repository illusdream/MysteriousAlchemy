using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteriousAlchemy.Core.Enum
{
    //存储各类枚举类

    public enum DrawMode
    {
        Default, Custom3D
    }
    public enum ModifySpriteEffect
    {
        None,
        FlipHorizontally,
        FlipVertically,
        FlipDiagonally
    }
    public enum ModifyBlendState
    {
        AlphaBlend, Additive, NonPremultiplied
    }
    public enum DrawSortWithPlayer
    {
        Front, Behind
    }
    public enum DrawSortWithUnits
    {
        Front, Middle, Behind
    }



    #region //炼金术相关

    public enum EtherEntityPivot
    {
        InWorld, InInventory
    }
    public enum AEGraphCategory
    {
        Ether, Subordinate
    }


    #endregion

}
