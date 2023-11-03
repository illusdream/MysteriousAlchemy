using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MysteriousAlchemy.Core;
using MysteriousAlchemy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.UI;

namespace MysteriousAlchemy.Content.UI.UIElements.BetterOriginalUI
{
    /// <summary>
    /// 对应的是原版的<see cref="UISearchBar"/>,原版的写的一坨，很多功能还得在外面额外添加<see cref="UIElement"/>才能实现
    /// </summary>
    public class InputTextBar : UIElement
    {
        public UIPanel BoxPanel;
        public UISearchBar _searchBar;

        public bool _didClickSearchBar;
        public bool _didClickOutInputBar;
        public string inputString;
        public InputTextBar(float widthPixel)
        {
            Height.Set(24f, 0);
            MinHeight.Set(24f, 0);
            MaxHeight.Set(24f, 0);

            Width.Set(widthPixel, 0);

            AddSearchBar();
        }
        private void AddSearchBar()
        {
            UIPanel uIPanel = (BoxPanel = new UIPanel
            {
                Left = new StyleDimension(0f + 3f, 0f),
                Width = new StyleDimension(Width.Pixels - 3f, 0f),
                Height = new StyleDimension(0f, 1f),
                VAlign = 0.5f
            });

            uIPanel.BackgroundColor = new Color(35, 40, 83);
            uIPanel.BorderColor = new Color(35, 40, 83);
            uIPanel.SetPadding(0f);
            Append(uIPanel);
            UISearchBar uISearchBar = (_searchBar = new UISearchBar(Language.GetText("UI.PlayerNameSlot"), 0.8f)
            {
                Width = new StyleDimension(0f, 1f),
                Height = new StyleDimension(0f, 1f),
                HAlign = 0f,
                VAlign = 0.5f,
                Left = new StyleDimension(0f, 0f),
                IgnoresMouseInteraction = true
            });

            uIPanel.OnLeftClick += Click_SearchArea;
            uISearchBar.OnContentsChanged += OnSearchContentsChanged;
            uIPanel.Append(uISearchBar);
            uISearchBar.OnStartTakingInput += OnStartTakingInput;
            uISearchBar.OnEndTakingInput += OnEndTakingInput;
            uISearchBar.OnNeedingVirtualKeyboard += OpenVirtualKeyboardWhenNeeded;
            UIImageButton uIImageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel"))
            {
                HAlign = 1f,
                VAlign = 0.5f,
                Left = new StyleDimension(-2f, 0f)
            };

            uIImageButton2.OnMouseOver += searchCancelButton_OnMouseOver;
            uIImageButton2.OnLeftClick += searchCancelButton_OnClick;
            uIPanel.Append(uIImageButton2);
        }
        private void Click_SearchArea(UIMouseEvent evt, UIElement listeningElement)
        {
            if (evt.Target.Parent != BoxPanel)
            {
                _searchBar.ToggleTakingText();
                _didClickSearchBar = true;
            }
        }
        private void OnSearchContentsChanged(string contents)
        {
            inputString = contents;
        }
        private void OnStartTakingInput()
        {
            BoxPanel.BorderColor = Main.OurFavoriteColor;
        }
        private void OnEndTakingInput()
        {
            BoxPanel.BorderColor = new Color(35, 40, 83);
        }
        private void OpenVirtualKeyboardWhenNeeded()
        {
            int maxInputLength = 40;
            UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Language.GetText("UI.PlayerNameSlot").Value, inputString, OnFinishedSettingName, GoBackHere, 0, allowEmpty: true);
            uIVirtualKeyboard.SetMaxInputLength(maxInputLength);
            UserInterface.ActiveInstance.SetState(uIVirtualKeyboard);
        }
        private void searchCancelButton_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            SoundEngine.PlaySound(MASoundID.MenuTick);
        }
        private void searchCancelButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (_searchBar.HasContents)
            {
                _searchBar.SetContents(null, forced: true);
                SoundEngine.PlaySound(MASoundID.MenuClose);
            }
            else
            {
                SoundEngine.PlaySound(MASoundID.MenuTick);
            }
        }
        private void OnFinishedSettingName(string name)
        {
            string contents = name.Trim();
            _searchBar.SetContents(contents);
            GoBackHere();
        }

        private void GoBackHere()
        {
            UserInterface.ActiveInstance.SetState(SearchUIStateInParent(this));
            _searchBar.ToggleTakingText();
        }
        private UIState SearchUIStateInParent(UIElement uIElement)
        {
            UIState result = null;
            if (uIElement != null)
            {
                if (uIElement.GetType().IsSubclassOf(typeof(UIState)))
                {
                    result = (UIState)uIElement;
                    return result;
                }
                else
                {
                    return SearchUIStateInParent(uIElement.Parent);
                }

            }
            return result;

        }
        private BetterUIState SearchBetterUIStateInParent(UIElement uIElement)
        {
            BetterUIState result = null;
            if (uIElement != null)
            {
                if (uIElement.GetType().IsSubclassOf(typeof(BetterUIState)))
                {
                    result = (BetterUIState)uIElement;
                    return result;
                }
                else
                {
                    return SearchBetterUIStateInParent(uIElement.Parent);
                }

            }
            return result;

        }
        public string GetInputText()
        {
            return inputString;
        }
        public override void Update(GameTime gameTime)
        {
            if (SearchBetterUIStateInParent(this).Visable && (Main.mouseLeft || Main.mouseRight) && !ContainsPoint(Main.MouseScreen))
                _didClickOutInputBar = true;
            if (_didClickOutInputBar && _searchBar.IsWritingText)
                _searchBar.ToggleTakingText();

            _didClickSearchBar = false;
            _didClickOutInputBar = false;
            base.Update(gameTime);
        }
    }
}
