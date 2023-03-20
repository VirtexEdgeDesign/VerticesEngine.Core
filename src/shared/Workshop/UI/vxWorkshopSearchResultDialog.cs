using Microsoft.Xna.Framework;
using System;
using System.IO;
using VerticesEngine.Graphics;
using VerticesEngine.UI;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Dialogs;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;

namespace VerticesEngine.Workshop.UI
{
    /// <summary>
    /// Steam workshop search result dialog.
    /// </summary>
    public class vxWorkshopSearchResultDialog : vxDialogBase
    {
        #region Fields


        private vxLabel searchResultText;

        private vxScrollPanel scrollPanel;

        private vxWorkshopSearchResults results;

        private int pageNumber;

        private vxButtonImageControl NextGroup;
        private vxButtonImageControl PrevGroup;

        #endregion

        const int RESULTS_PER_PAGE = 10;

        #region Initialization



        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:VerticesEngine.Community.Dialogs.vxWorkshopSearchResultDialog"/> class.
        /// </summary>
        /// <param name="Engine">Engine.</param>
        /// <param name="pageNumber">Page number.</param>
        public vxWorkshopSearchResultDialog(vxWorkshopSearchResults results, int pageNumber)
            : base("Workshop Results", vxEnumButtonTypes.OkCancel)
        {
            this.results = results;
            this.pageNumber = pageNumber;
        }

        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            // Top Image
            searchResultText = new vxLabel("Searching...", new Vector2(
                ArtProvider.GUIBounds.X + ArtProvider.Padding.X * 2,
                ArtProvider.GUIBounds.Y + ArtProvider.Padding.Y * 2));
            InternalGUIManager.Add(searchResultText);
            searchResultText.Theme.Text = new vxColourTheme(Color.Black);
            //searchResultText.DoShadow = true;


            // Scroll Panel
            scrollPanel = new vxScrollPanel(new Vector2(
                ArtProvider.GUIBounds.X,
                ArtProvider.GUIBounds.Y + ArtProvider.Padding.Y * 3 + searchResultText.Height),
                                            (ArtProvider.GUIBounds.Width),
                                            ArtProvider.GUIBounds.Height - OKButton.Bounds.Height - (int)ArtProvider.Padding.Y * 6);


            InternalGUIManager.Add(scrollPanel);


            // Arrows
            int ArrowButtonSize = vxLayout.GetScaledSize(108);

            //var backgroundBounds = ArtProvider.GUIBounds;
            var bounds = scrollPanel.Bounds;

            NextGroup = new vxButtonImageControl(vxUITheme.SpriteSheetLoc.ArrowBtnFwd, new Vector2(bounds.Right + ArtProvider.Padding.X,
                                                                     bounds.Center.Y - ArrowButtonSize / 2))
            {
                DrawHoverBackground = false,
                Alpha = 0,
                Width = ArrowButtonSize,
                Height = ArrowButtonSize,
                IsShadowVisible = true
            };

            NextGroup.Clicked += delegate
            {
                vxSceneManager.RemoveScene(this);
                vxSceneManager.AddScene(new vxWorkshopSearchResultDialog(results, pageNumber + 1));

            };

            InternalGUIManager.Add(NextGroup);



            PrevGroup = new vxButtonImageControl(vxUITheme.SpriteSheetLoc.ArrowBtnBack, new Vector2(bounds.Left - ArrowButtonSize - ArtProvider.Padding.X,
                                                                     bounds.Center.Y - ArrowButtonSize / 2))
            {
                DrawHoverBackground = false,
                Alpha = 0,
                Width = ArrowButtonSize,
                Height = ArrowButtonSize,
                IsShadowVisible = true
            };
            PrevGroup.Clicked += delegate
            {
                vxSceneManager.RemoveScene(this);
                vxSceneManager.AddScene(new vxWorkshopSearchResultDialog(results, pageNumber - 1));

            };

            InternalGUIManager.Add(PrevGroup);

            NextGroup.IsEnabled = false;
            PrevGroup.IsEnabled = false;
            OKButton.IsEnabled = false;




            int start = pageNumber * RESULTS_PER_PAGE;

            int max = Math.Min(pageNumber * RESULTS_PER_PAGE + RESULTS_PER_PAGE, results.ItemResults.Count);

            for (int ind = start; ind < max; ind++)
            {
                AddScrollItem(ind);
            }
            scrollPanel.ResetLayout();
            searchResultText.Text = "Displaying " + (start + 1) + " - " + max + " of " + results.ItemResults.Count + " Items Found.";
            ProcessItemsAsync(0);


            if (pageNumber * RESULTS_PER_PAGE + RESULTS_PER_PAGE < results.ItemResults.Count)
                NextGroup.IsEnabled = true;

            if (pageNumber != 0)
                PrevGroup.IsEnabled = true;
        }


        void AddScrollItem(int index)
        {
            // Get a new File Dialog Item
            var fileDialogButton = new vxWorkshopDialogItem(results.ItemResults[index]);

            // Hookup Click Events
            fileDialogButton.Clicked += OnItemClicked;
            fileDialogButton.DoubleClicked += delegate
            {
                OKButton.Select();
            };

            //Set Button Width
            fileDialogButton.Width = vxGraphics.GraphicsDevice.Viewport.Width - (4 * (int)this.ArtProvider.Padding.X);
            if (fileDialogButton.Text != "")
                scrollPanel.AddItem(fileDialogButton);

            //ScrollPanel.ResetLayout();
            scrollPanel.ScrollBar.TravelPosition = 0;
        }

        private vxWorkshopDialogItem selectedDialogItem;

        public void OnItemClicked(object sender, vxUIControlClickEventArgs e)
        {
            OKButton.IsEnabled = true;

            foreach (var item in scrollPanel.Items)
                item.ToggleState = false;

            e.GUIitem.ToggleState = true;

            if (e.GUIitem is vxWorkshopDialogItem)
            {
                // Get the GUI Item
                selectedDialogItem = ((vxWorkshopDialogItem)e.GUIitem);
            }
        }



        public void ProcessItemsAsync(int index)
        {
            if (scrollPanel.Items.Count > 0 && index < scrollPanel.Items.Count)
            {
                var item = scrollPanel.Items[index];
                if (item is vxWorkshopDialogItem)
                {
                    ((vxWorkshopDialogItem)item).Process(this, index);
                }
            }
        }


        #endregion

        #region Handle Input


        /// <inheritdoc/>
        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            if (selectedDialogItem != null)
           {
                if (selectedDialogItem.Item.ItemType == vxWorkshopItemType.SandboxFile)
                {
                    // is the file available, if not then lets download it
                    if (selectedDialogItem.Item.IsInstalled)
                    {
                        OpenFile(selectedDialogItem.Item);
                    }
                    else
                    {
                        vxWorkshop.Instance.Download(selectedDialogItem.Item, (isSuccess, path) =>
                        {
                            if(isSuccess)
                            {
                                OpenFile(selectedDialogItem.Item);
                            }
                            else
                            {
                                vxMessageBox.Show("Error", "Could not download Sandbox file");
                            }
                        });
                    }
                }
                else if (selectedDialogItem.Item.ItemType == vxWorkshopItemType.Mod)
                {
                    // TODO: Should this be handeled?
                }
            }
        }

        public void OpenFile(vxIWorkshopItem workshopItem)
        {
            vxEngine.Game.OnWorkshopItemOpen(workshopItem);
            ExitScreen();
        }

        /// <inheritdoc/>
        protected override void OnCancelButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            ExitScreen();
        }


        #endregion


    }
}
