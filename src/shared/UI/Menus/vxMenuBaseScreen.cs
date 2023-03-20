#region File Description
//-----------------------------------------------------------------------------
// MenuScreen.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
using VerticesEngine.Input;
using VerticesEngine.Input.Events;
using VerticesEngine.UI.Controls;


#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VerticesEngine.UI.Themes;
using VerticesEngine.Graphics;
#endregion

namespace VerticesEngine.UI.Menus
{
    public enum ControllerNavFlow
    {
        Vertical,
        Horizontal
    }
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    public abstract class vxMenuBaseScreen : vxBaseScene
    {
        #region Properties

        protected ControllerNavFlow NavControllerFlow = ControllerNavFlow.Vertical;

        /// <summary>
        /// The default navigation controller flow for this 
        /// </summary>
        protected static ControllerNavFlow DefaultNavControllerFlow = ControllerNavFlow.Horizontal;

        public virtual bool IsMainMenu
        {
            get { return false; }
        }

        /// <summary>
        /// THe UI Manager for this menu
        /// </summary>
        public vxUIManager UIManager
        {
            get { return _uiManager; }
        }
        private readonly vxUIManager _uiManager = new vxUIManager();

        /// <summary>
        /// The Art provider for the Menu Screen.
        /// </summary>
        public vxMenuScreenArtProvider ArtProvider { get; internal set; }


        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        public IList<vxMenuEntry> MenuEntries
        {
            get { return m_menuEntries; }
        }

        readonly List<vxMenuEntry> m_menuEntries = new List<vxMenuEntry>();


        /// <summary>
        /// Menu Screen Title
        /// </summary>
        public string MenuTitle
        {
            get { return _menuTitle; }
            set { _menuTitle = value; }
        }
        string _menuTitle;


        int selectionIndex;
        int selectedEntry = 0;


        #endregion

        #region Initialization

        string _menuTitleLocalisationKey;

        public static float MenuTransitionOnTime = 0.5f;
        public static float MenuTransitionOffTime = 0.5f;

        protected List<vxUIControl> NavigatableUIItems
        {
            get { return m_navigatableUIItems; }
        }
        readonly List<vxUIControl> m_navigatableUIItems = new List<vxUIControl>();

        /// <summary>
        /// Create a new menu with the default navigation flow
        /// </summary>
        /// <param name="MenuTitleLocalisationKey"></param>
        public vxMenuBaseScreen(string MenuTitleLocalisationKey) : this(MenuTitleLocalisationKey, DefaultNavControllerFlow)
        { 
        
        }
        
        /// <summary>
        /// Create a new menu with a specified navigation flow
        /// </summary>
        /// <param name="MenuTitleLocalisationKey"></param>
        /// <param name="navFlow"></param>
        public vxMenuBaseScreen(string MenuTitleLocalisationKey, ControllerNavFlow navFlow)
        {
            this._menuTitleLocalisationKey = MenuTitleLocalisationKey;

            TransitionOnTime = TimeSpan.FromSeconds(MenuTransitionOnTime);
            TransitionOffTime = TimeSpan.FromSeconds(MenuTransitionOffTime);

            NavControllerFlow = navFlow;
        }

        #endregion

        #region Handle Input

        public override void LoadContent()
        {
            _menuTitle = vxLocalizer.GetText(_menuTitleLocalisationKey);

            base.LoadContent();

            //Initialise Art Providers.
            ArtProvider = vxUITheme.ArtProviderForMenuScreen;
        }

        protected virtual vxMenuEntry AddMenuItem(string lockey)
        {
            return AddMenuItem(new vxMenuEntry(this, lockey));
        }

        protected virtual vxMenuEntry AddMenuItem(vxMenuEntry menuEntry)
        {
            m_menuEntries.Add(menuEntry);

            AddUIItem(menuEntry, true);

            return menuEntry;
        }

        protected vxUIControl AddUIItem(vxUIControl uiItem, bool isMenuItem = false)
        {
            if (isMenuItem == false)
                UIManager.Add(uiItem);

            m_navigatableUIItems.Add(uiItem);
            return uiItem;
        }

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        protected internal override void HandleInput()
        {
            // Move to the previous menu entry?
            if (vxInput.IsMenuUp())
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = m_menuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (vxInput.IsMenuDown())
            {
                selectedEntry++;

                if (selectedEntry >= m_menuEntries.Count)
                    selectedEntry = 0;
            }

            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputHelper helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex = PlayerIndex.One;

            if (vxInput.IsMenuSelect())
            {
                OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (vxInput.IsMenuCancel())
            {
                OnCancel(playerIndex);
            }
        }


        protected internal override void OnLocalizationChanged()
        {
            base.OnLocalizationChanged();

            _menuTitle = vxLocalizer.GetText(_menuTitleLocalisationKey);

            // Update each nested vxMenuEntry object.
            for (int i = 0; i < m_menuEntries.Count; i++)
            {
                m_menuEntries[i].OnLocalizationChanged();
            }
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            if (selectedEntry != -1 && entryIndex >= 0 && entryIndex < m_menuEntries.Count)
            {
                m_menuEntries[entryIndex].OnSelectEntry(playerIndex);
            }
        }


        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
		public virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a vxMenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }


        #endregion

        #region Update and Draw

        public virtual void SetArtProvider(vxMenuScreenArtProvider NewArtProvider)
        {
            this.ArtProvider = NewArtProvider;
        }

        public int UINavIndex
        {
            get { return guiNavIndex; }
            set { guiNavIndex = value; }
        }
        
        int guiNavIndex = 0;

        public override string ToString()
        {
            return "MenuScreen {" + this.MenuTitle + "}";
        }

        protected override void OnFirstUpdate()
        {
            base.OnFirstUpdate();
        }
        private bool isOpened = false;
        /// <summary>
        /// Updates the menu.
        /// </summary>
        protected internal override void Update()
        {
            base.Update();

            // only update if it's not covered by anything
            if (!otherScreenHasFocus)
                UIManager.Update();

            foreach (vxUIControl item in UIManager.Items)
            {
                item.TransitionAlpha = TransitionAlpha;
            }

            if (vxInput.InputType == InputType.Controller)
            {
                // handle on first open
                if (!isOpened && TransitionAlpha == 1f)
                {
                    guiNavIndex = 0;
                    isOpened = true;
                    this.ArtProvider.OnNewMenuStart(this);
                    
                    if (guiNavIndex >= 0 && guiNavIndex < m_menuEntries.Count)
                        SetCursorPosition(m_menuEntries[guiNavIndex].Bounds.Center.ToVector2());
                }
                else if (TransitionAlpha < 1)
                {
                    isOpened = false;
                }
            }
            else
            {   
                // handle on first open
                if (!isOpened && TransitionAlpha == 1f)
                {
                    isOpened = true;
                    this.ArtProvider.OnNewMenuStart(this);
                }
                else if (TransitionAlpha < 1)
                {
                    isOpened = false;
                }
            }

            vxInput.IsCursorVisible = true;

            //
            //Set Menu Selection if Mouse is over
            //
            selectionIndex = 0;

            selectedEntry = -1;

            if (IsActive)
            {
                HandleMenuNavigation();

                UIManager.HandleInput();
            }
        }

        bool NavDown()
        {
            if (this.NavControllerFlow == ControllerNavFlow.Vertical)
            {
                return vxInput.IsNewMainInputDown();
            }
            else if (NavControllerFlow == ControllerNavFlow.Horizontal)
            {
                return vxInput.IsNewMainSlideRight();
            }
            else
            {
                return false;
            }
        }

        bool NavUp()
        {
            if (NavControllerFlow == ControllerNavFlow.Vertical)
            {
                return vxInput.IsNewMainInputUp();
            }
            else if (NavControllerFlow == ControllerNavFlow.Horizontal)
            {
                return vxInput.IsNewMainSlideLeft();
            }
            else
            {
                return false;
            }
        }

        public bool IsCursorSetByNavigation = true;

        void SetCursorPosition(Vector2 pos)
        {
            if(IsCursorSetByNavigation)
            vxInput.Cursor = pos;
        }

        protected virtual void HandleMenuNavigation()
        {
            if (NavDown() && m_menuEntries.Count > 0)
            {
                guiNavIndex = (guiNavIndex + 1) % m_menuEntries.Count;

                SetCursorPosition(m_menuEntries[guiNavIndex].Bounds.Center.ToVector2());
            }
            if (NavUp() && m_menuEntries.Count > 0)
            {
                guiNavIndex--;
                if (guiNavIndex < 0)
                    guiNavIndex = m_menuEntries.Count - 1;

                SetCursorPosition(m_menuEntries[guiNavIndex].Bounds.Center.ToVector2());
            }

            // Update each nested vxMenuEntry object.
            for (int i = 0; i < m_menuEntries.Count; i++)
            {

                //Always set this to false intially
                m_menuEntries[i].IsSelected = false;
                if (m_menuEntries[i].HasFocus == true)
                    selectedEntry = selectionIndex;

                selectionIndex++;

                m_menuEntries[i].Index = i;
                m_menuEntries[i].Update();
            }
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw()
        {
            vxSpriteBatch spriteBatch = vxGraphics.SpriteBatch;

            spriteBatch.Begin("UI.MenuScreen." + this.MenuTitle);

            if (TransitionAlpha > 0)
            {
                UIManager.Alpha = TransitionAlpha;
                UIManager.DrawByOwner();


                foreach (var item in UIManager.Items)
                {
                    if (item.HasFocus)
                        UIManager.FocusedItem = item;
                }

                if (UIManager.FocusedItem != null)
                {
                    UIManager.FocusedItem.DrawToolTip();
                    UIManager.FocusedItem = null;
                }
            }

            this.ArtProvider.Draw(this);

            if (m_menuEntries.Count > 0)
            {
                // Draw each menu entry in turn.
                for (int i = 0; i < m_menuEntries.Count; i++)
                {
                    vxMenuEntry vxMenuEntry = m_menuEntries[i];
                    vxMenuEntry.Draw();
                }
            }

            spriteBatch.End();
        }
        #endregion

        #region -- Utility Functions --


        #endregion
    }
}
