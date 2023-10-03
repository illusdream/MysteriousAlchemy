using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace MysteriousAlchemy.Core
{
    public abstract class BetterUIState : UIState
    {
        /// <summary>
        /// 是否可见
        /// </summary>
        public virtual bool Visable { get; set; } = false;
        /// <summary>
        /// 用于描述UI显示在哪个图层上
        /// </summary>
        /// <param name="layers"></param>
        /// <returns></returns>
        public abstract int UILayer(List<GameInterfaceLayer> layers);
        /// <summary>
        /// 缩放
        /// </summary>
        public virtual InterfaceScaleType Scale { get; set; } = InterfaceScaleType.UI;

        public virtual void Unload() { }


        /// <summary>
        /// Appends an element to this state with the given dimensions
        /// </summary>
        /// <param name="element">The element to append</param>
        /// <param name="x">The x position in pixels</param>
        /// <param name="y">The y position in pixels</param>
        /// <param name="width">The width in pixels</param>
        /// <param name="height">The height in pixels</param>
        internal void AddElement(UIElement element, int x, int y, int width, int height)
        {
            element.Left.Set(x, 0);
            element.Top.Set(y, 0);
            element.Width.Set(width, 0);
            element.Height.Set(height, 0);
            Append(element);
        }

        /// <summary>
        /// Appends an element to another element with the given dimensions
        /// </summary>
        /// <param name="element">The element to append</param>
        /// <param name="x">The x position in pixels</param>
        /// <param name="y">The y position in pixels</param>
        /// <param name="width">The width in pixels</param>
        /// <param name="height">The height in pixels</param>
        /// <param name="appendTo">The element to append to</param>
        internal void AddElement(UIElement element, int x, int y, int width, int height, UIElement appendTo)
        {
            element.Left.Set(x, 0);
            element.Top.Set(y, 0);
            element.Width.Set(width, 0);
            element.Height.Set(height, 0);
            appendTo.Append(element);
        }

        /// <summary>
        /// Appends an element to this state with the given dimensions
        /// </summary>
        /// <param name="element">The element to append</param>
        /// <param name="x">The x position in pixels</param>
        /// <param name="xPercent">The x position in percentage of the parents width</param>
        /// <param name="y">The y position in pixels</param>
        /// <param name="yPercent">The y position in percentage of the parents height</param>
        /// <param name="width">The width in pixels</param>
        /// <param name="widthPercent">The width in percentage of the parents width</param>
        /// <param name="height">The height in pixels</param>
        /// <param name="heightPercent">The height in percentage of the parents height</param>
        internal void AddElement(UIElement element, int x, float xPercent, int y, float yPercent, int width, float widthPercent, int height, float heightPercent)
        {
            element.Left.Set(x, xPercent);
            element.Top.Set(y, yPercent);
            element.Width.Set(width, widthPercent);
            element.Height.Set(height, heightPercent);
            Append(element);
        }

        /// <summary>
        /// Appends an element to this state with the given dimensions
        /// </summary>
        /// <param name="element">The element to append</param>
        /// <param name="x">The x position in pixels</param>
        /// <param name="xPercent">The x position in percentage of the parents width</param>
        /// <param name="y">The y position in pixels</param>
        /// <param name="yPercent">The y position in percentage of the parents height</param>
        /// <param name="width">The width in pixels</param>
        /// <param name="widthPercent">The width in percentage of the parents width</param>
        /// <param name="height">The height in pixels</param>
        /// <param name="heightPercent">The height in percentage of the parents height</param>
        /// <param name="appendTo">The element to append to</param>
        internal void AddElement(UIElement element, int x, float xPercent, int y, float yPercent, int width, float widthPercent, int height, float heightPercent, UIElement appendTo)
        {
            element.Left.Set(x, xPercent);
            element.Top.Set(y, yPercent);
            element.Width.Set(width, widthPercent);
            element.Height.Set(height, heightPercent);
            appendTo.Append(element);
        }
    }
}
