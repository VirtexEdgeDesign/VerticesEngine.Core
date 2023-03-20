using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VerticesEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using VerticesEngine.UI.Themes;
using VerticesEngine.Utilities;
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Basic Button GUI Control.
    /// </summary>
    public class vxPanel : vxUIControl
    {
        /// <summary>
        /// Gets or sets the texture for this Menu Entry Background.
        /// </summary>
        /// <value>The texture.</value>
        public Texture2D BackgroundTexture;


		public List<vxUIControl> Items
        {
            get { return _items; }
        }
		private List<vxUIControl> _items = new List<vxUIControl>();

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxPanel"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		/// <param name="Width">Width.</param>
		/// <param name="Height">Height.</param>
		public vxPanel(Vector2 position, int Width, int Height)
			: this(new Rectangle((int)position.X, (int)position.Y, Width, Height))
		{
			
		}
		

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxPanel"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="Bounds">Bounds.</param>
		public vxPanel(Rectangle Bounds) : base(Bounds.Location.ToVector2())
		{
            //Set up Font
            this.Font = vxUITheme.Fonts.Size24;

            Text = "";

            this.Width = Bounds.Width;
            this.Height = Bounds.Height;

            BackgroundTexture = vxInternalAssets.Textures.Blank;

            PositionChanged += OnPositionChanged;

            Theme.Background = new vxColourTheme(Color.Gray);
		}

        protected override void OnDisposed()
        {
            base.OnDisposed();

            foreach (var item in _items)
                item.Dispose();

            _items.Clear();
        }

        /// <summary>
        /// Called on position changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        public virtual void OnPositionChanged(object sender, EventArgs e)
        {
			// update the item positions
			for (int c = 0; c < Items.Count; c++)
				Items[c].Position = Position + Items[c].OriginalPosition;
        }

        private void this_OnInitialHover(object sender, EventArgs e)
        {
			//If Previous Selection = False and Current is True, then Create Highlite Sound Instsance
#if !NO_DRIVER_OPENAL
			PlaySound(vxUITheme.SoundEffects.MenuHover, 0.3f);
#endif
		}

		void this_Clicked (object sender, VerticesEngine.UI.Events.vxUIControlClickEventArgs e)
		{
#if !NO_DRIVER_OPENAL
			PlaySound(vxUITheme.SoundEffects.MenuConfirm, 0.3f);
			#endif
		}


		public virtual void Add(vxUIControl control)
		{
			Items.Add(control);

			// Now Change the Position of this 
			control.Position = this.Position + control.OriginalPosition;
		}

		protected internal override void Update()
		{
			base.Update();

            UpdateItems();
		}

        public virtual void UpdateItems()
        {
			// Now Draw the Controls for this panel.
			for (int c = 0; c < Items.Count; c++)
				Items[c].Update();
        }
        
        /// <summary>
        /// Draws the GUI Item
        /// </summary>
		public override void Draw()
        {
            //vxConsole.WriteToScreen(this, hasUpdated);

			if(DoBorder)
				vxGraphics.SpriteBatch.Draw(BackgroundTexture, Bounds.GetBorder(BorderSize), BorderColour* Opacity);
			
            if(DrawBackground)
                vxGraphics.SpriteBatch.Draw(BackgroundTexture, Bounds, GetStateColour(Theme.Background) * Opacity);
		
        	DrawItems();
		}

        public bool DrawBackground = true;

		public virtual void DrawItems()
		{
			// Now Draw the Controls for this panel.
			for (int c = 0; c < Items.Count; c++)
				Items[c].Draw();
		}
    }
}
