using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using Microsoft.Xna.Framework.Audio;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Dialogs
{
	/// <summary>
	/// File Chooser Dialog Item.
	/// </summary>
	public class vxScrollPanelItem : vxUIControl
	{
		/// <summary>
		/// The button image.
		/// </summary>
		public Texture2D ButtonImage;

		/// <summary>
		/// Gets or sets the art provider.
		/// </summary>
		/// <value>The art provider.</value>
		public vxScrollPanelItemArtProvider ArtProvider;


		/// <summary>
		/// Should this Scroll Panel Item Show the Icon.
		/// </summary>
		public bool ShowIcon = true;


		/// <summary>
		/// The is item selected.
		/// </summary>
		public bool IsItemSelected = false;

		/// <summary>
		/// Returns a Harcoded Type
		/// </summary>
		/// <returns></returns>
		public override Type GetBaseGuiType()
		{
			return typeof(vxScrollPanelItem);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Dialogs.vxScrollPanelItem"/> class.
		/// </summary>
		/// <param name="text">This GUI Items Text.</param>
		/// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		/// <param name="buttonImage">Button image.</param>
		/// <param name="ElementIndex">Element index.</param>
		public vxScrollPanelItem(string Text, Vector2 Position, Texture2D buttonImage, int ElementIndex=0) : base()
		{
			Padding = new Vector2(4);

			//Set Text
			this.Text = Text;

			//Set Position
			this.Position = Position;
			OriginalPosition = Position;

			//Set Index
			Index = ElementIndex;

			//Set Button Image
			ButtonImage = buttonImage;
			Bounds = new Rectangle(0, 0, 64, 64);

			Height = 64;
			Width = 3000;

			////Color_Normal = new Color(0.15f, 0.15f, 0.15f, 0.5f);
			//Color_Highlight = Color.DarkOrange;
			//Color_Selected = Color.DeepSkyBlue;
			////Colour_Text = Color.LightGray;

			this.OnInitialHover += this_OnInitialHover;
			this.Clicked += this_Clicked;

			//Have this button get a clone of the current Art Provider
			ArtProvider = (vxScrollPanelItemArtProvider)vxUITheme.ArtProviderForScrollPanelItem.Clone ();
		}

		private void this_OnInitialHover(object sender, EventArgs e)
		{
			//If Previous Selection = False and Current is True, then Create Highlite Sound Instsance
#if !NO_DRIVER_OPENAL
			//SoundEffectInstance MenuHighlight = vxUITheme.SoundEffects.SE_Menu_Hover.CreateInstance();
			//MenuHighlight.Volume = vxAudioManager.Double_SFX_Volume / 6;
			//MenuHighlight.Play();

#endif
		}

		void this_Clicked(object sender, VerticesEngine.UI.Events.vxUIControlClickEventArgs e)
		{
#if !NO_DRIVER_OPENAL
			//SoundEffectInstance equipInstance = vxUITheme.SoundEffects.SE_Menu_Confirm.CreateInstance();
			//equipInstance.Volume = vxAudioManager.Double_SFX_Volume / 3;
			//equipInstance.Play();
#endif
		}

		public virtual void UnSelect()
		{
			IsItemSelected = false;
		}
		public virtual void ThisSelect()
		{
			IsItemSelected = true;
		}

		public override void Draw()
		{
			//Now get the Art Provider to draw the scene
            this.ArtProvider.Draw(this);

			base.Draw();
		}
	}
}
