using Microsoft.Xna.Framework;
using MysteriousAlchemy.Content.UI.UIElements.AlchemyEntityUIElements;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Core.Abstract;
using MysteriousAlchemy.Core.Loader;
using MysteriousAlchemy.Core.Systems;
using MysteriousAlchemy.Utils;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI
{
    public class UI_AlchemyEditor : BetterUIState
    {
        public override int UILayer(List<GameInterfaceLayer> layers) => layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
        public static bool visable = false;
        public override bool Visable => visable;

        AlchemyUnicode Selectunicode;

        AEEditionPanel editionPanel;
        AESearchPanel searchPanel;
        AEOpreatePanel opreatePanel;
        AEViewNodePanel viewNodePanel;


        // 定位用的
        UIElement Base;
        bool IsLinking;

        Vector2 PanelSize = new Vector2(1330, 830);

        public void SetVisable(bool value)
        {
            if (value)
                CreateEditPanel();
            visable = value;
        }
        private void MakeExitButton(UIElement outerContainer)
        {
            UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true)
            {
                Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
                Height = StyleDimension.FromPixels(50f),
                VAlign = 1f,
                HAlign = 0.5f,
                Top = StyleDimension.FromPixels(-0)
            };

            uITextPanel.OnMouseOver += (evt, listenUI) =>
            {
                SoundEngine.PlaySound(MASoundID.MenuTick);
                ((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
                ((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
            };
            uITextPanel.OnMouseOut += (evt, listenUI) =>
            {
                ((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
                ((UIPanel)evt.Target).BorderColor = Color.Black;
            };

            uITextPanel.OnLeftMouseDown += Click_GoBack;
            uITextPanel.SetSnapPoint("ExitButton", 0);
            outerContainer.Append(uITextPanel);
        }

        private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(MASoundID.MenuClose);
            if (Main.gameMenu)
                Main.menuMode = 0;
            else
                IngameFancyUI.Close();
        }
        private void CreateEditPanel()
        {
            PanelSize = new Vector2(1330, 830 + 75);
            RemoveAllChildren();
            Base = new UIElement();
            AddElement(Base, -PanelSize.X / 2f, 0.5f, -PanelSize.Y / 2f + 100, 0.5f, PanelSize.X, 0, PanelSize.Y, 0);
            AddSubPanel();
            MakeExitButton(Base);
        }
        public override void Update(GameTime gameTime)
        {
            DebugUtils.NewText(Selectunicode.value);
            //CheckCanVisable();
            base.Update(gameTime);
        }
        private void CheckCanVisable()
        {
            visable = Main.inFancyUI;
        }
        public void RefreshAllPanel()
        {
            //searchPanel
            //viewpanel
            opreatePanel.SetUnicode(Selectunicode);
        }
        public void SetUnicode(AlchemyUnicode unicode)
        {
            if (IsLinking && Selectunicode != null)
            {
                AlchemySystem.etherGraph.AddLink(Selectunicode, unicode, null);
                IsLinking = false;
            }
            else
            {
                Selectunicode = unicode;
                RefreshAllPanel();
            }

        }
        public void SetIsLinking(bool value)
        {
            IsLinking = value;
        }

        private void AddSubPanel()
        {

            AddOpreatePanel();
            AddViewNodePanel();
            //AddSearchPanel();
        }
        private void AddOpreatePanel()
        {
            float OpWidth = 660;
            opreatePanel = new AEOpreatePanel();
            opreatePanel.Top.Set(0, 0);
            opreatePanel.Left.Set(665, 0);
            opreatePanel.Width.Set(OpWidth, 0);
            opreatePanel.Height.Set(-75, 1f);
            Base.Append(opreatePanel);
        }
        private void AddSearchPanel()
        {
            float SpWidth = 170;
            searchPanel = new AESearchPanel(170);
            searchPanel.Top.Set(0, 0);
            searchPanel.Left.Set(viewNodePanel.Left.Pixels + 3f, 0);
            searchPanel.Width.Set(SpWidth, 0);
            searchPanel.Height.Set(-75, 1f);
            Base.Append(searchPanel);


        }
        private void AddViewNodePanel()
        {
            viewNodePanel = new AEViewNodePanel();
            viewNodePanel.Top.Set(0, 0);
            viewNodePanel.Left.Set(0, 0);

            viewNodePanel.Height.Set(-75, 1);
            viewNodePanel.Width.Set(660, 0);
            Base.Append(viewNodePanel);
            viewNodePanel.NodeViewer.OnMircoIconClicked += OnIconClicked;
        }
        public void RefreshAllPanel(AlchemyUnicode unicode)
        {

        }

        private void OnIconClicked(AlchemyUnicode unicode)
        {
            UIloader.GetUIState<UI_AlchemyEditor>().SetUnicode(unicode);
        }
    }
}