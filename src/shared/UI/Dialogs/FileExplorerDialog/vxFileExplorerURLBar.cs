using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using VerticesEngine.UI.Themes;
using VerticesEngine.UI.Controls;
using System.Collections.Generic;
using VerticesEngine.Graphics;
using VerticesEngine.ContentManagement;

namespace VerticesEngine.UI.Dialogs
{
	/// <summary>
	/// File Chooser Dialor Item.
	/// </summary>
	public class vxFileExplorerURLBar : vxPanel
	{
		/// <summary>
		/// The file path.
		/// </summary>
        public string FilePath {
            get { return _filePath; }
            set
            {
                _filePath = value;
                
                OnFilePathSet();
            }
        }
        string _filePath = "";
        List<vxFileExplorerURLBarButton> m_buttons = new List<vxFileExplorerURLBarButton>();


        public vxFileExplorerDialog FileExplorer;

        vxButtonImageControl BackButton;
        vxButtonImageControl ForwardButton;
        vxButtonImageControl DirUpButton;

        int backWidth = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Dialogs.vxFileDialogControlBar"/> class.
        /// </summary>
        /// <param name="FileExplorer">File explorer.</param>
        public vxFileExplorerURLBar(vxFileExplorerDialog FileExplorer, Vector2 Position, int width) : base(Position, 400,32)
		{
			Padding = new Vector2(4);
            backWidth = width;
            this.FileExplorer = FileExplorer;

            BackButton = new vxButtonImageControl(vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/fileexplorer/arrow_back"), 
               Position - Vector2.UnitX * 32*3);
            ForwardButton = new vxButtonImageControl(vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/fileexplorer/arrow_forward"),
                Position - Vector2.UnitX * 32 * 2);
            DirUpButton = new vxButtonImageControl(vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/fileexplorer/arrow_up"),
                Position - Vector2.UnitX * 32 * 1);
            Items.Add(BackButton);
            Items.Add(ForwardButton);
            Items.Add(DirUpButton);

            DirUpButton.Clicked += delegate {
                
                if(m_buttons.Count > 1)
                {
                    FileExplorer.Path = m_buttons[m_buttons.Count - 3].Path;
                }
                string[] urlParts = FilePath.Split('\\');
                string url = System.IO.Directory.GetDirectoryRoot(FilePath);

                // Build new path
                for (int i = 0; i < urlParts.Length-1; i++)
                    url = System.IO.Path.Combine(url, urlParts[i]);

                FileExplorer.Path = url;
            };


            BackButton.Clicked += delegate
            {
                if (navHistory.Count > 0)
                {
                    navFwdHistory.Push(FileExplorer.Path);
                    FileExplorer.Path = navHistory.Pop();
                }
            };

            ForwardButton.Clicked += delegate
            {
                //if(ForwardButton.)
            };


            DrawBackground = false;
		}

        private Stack<string> navHistory = new Stack<string>();
        private Stack<string> navFwdHistory = new Stack<string>();

        void OnFilePathSet()
        {
            m_buttons.Clear();
            int runningWidth = 0;
            string urlPath = System.IO.Directory.GetDirectoryRoot(FilePath);

            int i = 0;
            foreach (var dir in FilePath.Split('\\'))
            {
                Vector2 pos = Position + new Vector2(runningWidth, 0);
                if (dir != string.Empty)
                {
                    //urlPath = System.IO.Path.Combine(urlPath, dir);
                    if (i > 0)
                    {
                        urlPath += "\\" + dir;


                        if (dir != "")
                        {
                            var spltr = new vxFileExplorerURLBarSplitter(this, pos);
                            runningWidth += spltr.Width;
                            m_buttons.Add(spltr);
                        }
                    }
                    i++;

                    pos = Position + new Vector2(runningWidth, 0);
                    var button = new vxFileExplorerURLBarButton(this, dir, pos, urlPath);
                    if(dir != "")
                    {
                        button.Clicked += delegate
                        {
                            navHistory.Push(FileExplorer.Path);
                            FileExplorer.Path = button.Path;
                        };
                    }

                    runningWidth += button.Width;
                    m_buttons.Add(button);

                }
            }
        }

        protected internal override void Update()
        {
            base.Update();
            // Now Draw the Controls for this panel.
            for (int i = 0; i < m_buttons.Count; i++)
                m_buttons[i].Update();
        }

        public override void Draw()
        {
            base.Draw();

            // draw the background
            vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, vxLayout.GetRect(Position, backWidth, Height), Color.Black*0.5f);

            for (int i = 0; i < m_buttons.Count; i++)
                m_buttons[i].Draw();
        }
	}
}
