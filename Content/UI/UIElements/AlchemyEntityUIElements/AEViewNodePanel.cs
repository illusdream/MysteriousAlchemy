using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Chat.Commands;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements
{
    public class AEViewNodePanel : UIPanel
    {
        public Vector2 ViewCenter = Main.LocalPlayer.Center;
        public Vector2 ViewRange = new Vector2(Main.screenWidth, Main.screenHeight);


        public bool IsToggle;
        public Vector2 ToggleVec = Vector2.Zero;
        public Vector2 StartPoint = Vector2.Zero;


        public List<AEMircoIcon> icons = new List<AEMircoIcon>();
        public ImageButtomWithText ToDefaultCenter;
        public ImageButtomWithText ToDefaultSize;
        public ImageButtomWithText ToDefaultPosition_Size;
        public AEViewNodePanel()
        {
            OverflowHidden = true;

            AddMircoIcon();


            ToDefaultCenter = new ImageButtomWithText(AssetUtils.UI + "ToDefaultPosition", "ToDefaultPosition");
            ToDefaultCenter.Width.Set(22, 0);
            ToDefaultCenter.Height.Set(22, 0);
            ToDefaultCenter.Top.Set(0, 0);
            ToDefaultCenter.Left.Set(-22, 1f);
            ToDefaultCenter.OnLeftClick += ToDefaultCenter_OnLeftClick;
            Append(ToDefaultCenter);

            ToDefaultSize = new ImageButtomWithText(AssetUtils.UI + "ToDefaultSize", "ToDefaultSize");
            ToDefaultSize.Width.Set(22, 0);
            ToDefaultSize.Height.Set(22, 0);
            ToDefaultSize.Top.Set(0, 0);
            ToDefaultSize.Left.Set(-55, 1f);
            ToDefaultSize.OnLeftClick += ToDefaultSize_OnLeftClick;
            Append(ToDefaultSize);

            ToDefaultPosition_Size = new ImageButtomWithText(AssetUtils.UI + "ToDefaultPosition_Size", "ToDefaultPosition&Size");
            ToDefaultPosition_Size.Width.Set(22, 0);
            ToDefaultPosition_Size.Height.Set(22, 0);
            ToDefaultPosition_Size.Top.Set(0, 0);
            ToDefaultPosition_Size.Left.Set(-88, 1f);
            ToDefaultPosition_Size.OnLeftClick += ToDefaultPosition_Size_OnLeftClick; ;
            Append(ToDefaultPosition_Size);
        }

        private void ToDefaultPosition_Size_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ViewRange = new Vector2(Main.screenWidth, Main.screenHeight);
            ViewCenter = Main.LocalPlayer.Center;
        }

        private void ToDefaultSize_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ViewRange = new Vector2(Main.screenWidth, Main.screenHeight);
        }

        private void ToDefaultCenter_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
        {
            ViewCenter = Main.LocalPlayer.Center;
        }

        private void AddMircoIcon()
        {
            AlchemySystem.subordinateGraph.InWorld.ForEach((o) =>
            {
                if (AlchemySystem.FindAlchemyEntitySafely(o, out AlchemyEntity result))
                {
                    var instance = new AEMircoIcon(o);
                    instance.OnLeftClick += AEIconClick;
                    icons.Add(instance);
                    Append(instance);
                }
            });
        }

        private void AEIconClick(UIMouseEvent evt, UIElement listeningElement)
        {
            DebugUtils.NewText(((AEMircoIcon)listeningElement).unicode.value);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsToggle)
            {
                ToggleVec = Main.MouseScreen - StartPoint;
            }
            if (IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            icons.ForEach((o) =>
            {
                o.RecalculateShowPostion(IsToggle ? ViewCenter - ToggleVec : ViewCenter, ViewRange);
            });
            base.Update(gameTime);
        }
        public override void LeftMouseDown(UIMouseEvent evt)
        {
            IsToggle = true;
            StartPoint = Main.MouseScreen;
            base.RightMouseDown(evt);
        }
        public override void LeftMouseUp(UIMouseEvent evt)
        {
            IsToggle = false;
            ViewCenter -= ToggleVec;
            ToggleVec = Vector2.Zero;
            base.RightMouseUp(evt);
        }
        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            ViewRange += new Vector2(evt.ScrollWheelValue);
            ViewRange = new Vector2(Math.Clamp(ViewRange.X, 0, Main.rightWorld), Math.Clamp(ViewRange.Y, 0, Main.bottomWorld));
            base.ScrollWheel(evt);
        }
    }
}
