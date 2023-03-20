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
using VerticesEngine.Graphics;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Basic Button GUI Control.
	/// </summary>
	public class vxTreeNode : vxButtonControl
	{
		public vxTreeControl TreeControl;

		public List<vxTreeNode> Items = new List<vxTreeNode>();

        public new Texture2D Icon
        {
            get
            {
                return _icon;
            }
            set{
                _icon = value;
                LabelOffset = new Vector2(20, 0);
                Width = (int)(vxInternalAssets.Fonts.ViewerFont.MeasureString(Text).X + Padding.X * 2) + 16;
            }
        }
        Texture2D _icon;

		public bool IsExpanded
		{
            get { return _isExpanded; }
			set
			{
				_isExpanded = value;
				SetStatus();
			}
		}
		bool _isExpanded = false;

		vxButtonImageControl ArrowButton;

		Vector2 LabelOffset = new Vector2(4, 0);

		Rectangle IconBounds = new Rectangle(0, 0, 16, 16);

        public vxGameObject Entity;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxTreeNode"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="text">This GUI Items Text.</param>
		public vxTreeNode(vxTreeControl TreeControl, string text, Texture2D Icon = null) : base(text, Vector2.Zero)
		{
			this.TreeControl = TreeControl;

			ArrowButton = new vxButtonImageControl(vxInternalAssets.Textures.Texture_ArrowRight,
											vxInternalAssets.Textures.Texture_ArrowRight, Vector2.Zero);

			IsTogglable = true;
			//OnInitialHover += this_OnInitialHover;
			//
			ArrowButton.Clicked += this_Clicked;

			LabelOffset = new Vector2(Padding.X, 1);

			Height = 18;
			Width = (int)(vxInternalAssets.Fonts.ViewerFont.MeasureString(Text).X + Padding.X * 2);

            if(Icon != null)
			    this.Icon = Icon;
			// Increase the Width and Label Offset if there's an Icon
			//if (Icon != null)
			//{
			//	LabelOffset += new Vector2(16, 0);
			//	Width += 16;
			//}

            this.Clicked += OnSelected;
            IsTogglable = true;
		}


        void OnSelected(object sender, Events.vxUIControlClickEventArgs e)
        {
            if (Entity != null)
                Console.WriteLine(Entity);

            TreeControl.SelectItem(this);
            
            //Console.WriteLine(ToggleState);
        }

		void this_Clicked(object sender, VerticesEngine.UI.Events.vxUIControlClickEventArgs e)
		{
            PlaySound(vxUITheme.SoundEffects.MenuConfirm, 0.33f);
			IsExpanded = !IsExpanded;
		}

		void SetStatus()
		{

			if (Items.Count > 0)
			{
				if (IsExpanded)
				{
					ArrowButton.ButtonImage = vxInternalAssets.Textures.Texture_ArrowDown;
					ArrowButton.HoverButtonImage = vxInternalAssets.Textures.Texture_ArrowDown;
				}
				else
				{
					ArrowButton.ButtonImage = vxInternalAssets.Textures.Texture_ArrowRight;
					ArrowButton.HoverButtonImage = vxInternalAssets.Textures.Texture_ArrowRight;
				}
			}
			else
			{
				ArrowButton.ButtonImage = null;
				ArrowButton.HoverButtonImage = null;
			}
		}

		public virtual void Add(vxTreeNode node)
		{
			Items.Add(node);

			node.TreeControl = this.TreeControl;

			// Now Change the Position of this 
			node.Position = this.Position + node.OriginalPosition;
			SetStatus();
		}

		public virtual void Clear()
		{
			Items.Clear();
			SetStatus();
		}


		Vector2 PositionOffset = new Vector2(16, 0);


		protected internal override void Update()
		{
			base.Update();

			ArrowButton.Position = this.Position - PositionOffset;
			ArrowButton.Update();
			IconBounds.Location = (Position + new Vector2(Padding.X / 2, 1)).ToPoint();

			// Now Draw the Controls for this panel.

            if(IsExpanded)
			    foreach (vxTreeNode control in Items)
				    control.Update();
		}

		/// <summary>
		/// Draws the GUI Item
		/// </summary>
		public override void Draw()
		{
			//Now get the Art Provider to draw the scene
			Font = vxUITheme.Fonts.Size24;
            //Console.WriteLine(Bounds.Height);

			// Always draw the background
            if(HasFocus || ToggleState)
			vxGraphics.SpriteBatch.Draw(DefaultTexture, Bounds, 
                                    this.ToggleState ? Color.DeepSkyBlue : (HasFocus ? Color.DarkOrange : Color.DarkSlateGray * 0.75f));



			// Draw the Label
            //if(HasFocus == false)
			//vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont, Text, Position + LabelOffset+new Vector2(1), (HasFocus ? Color.Black * 0.5f : Color.Black));

            vxGraphics.SpriteBatch.DrawString(vxInternalAssets.Fonts.ViewerFont, Text, Position + LabelOffset, this.ToggleState ? Color.Black : (HasFocus ? Color.Black : Color.White));

			// Finally, draw the Icon if it's present
			if(Icon != null)
				vxGraphics.SpriteBatch.Draw(Icon, IconBounds, Color.White);
				

            //TreeControl.TreeText = Text;
            //TreeControl.incTabs();
			DrawItems();
            //TreeControl.decTabs();
		}

		public virtual void DrawItems()
		{
            if(Items.Count > 0) 
			    ArrowButton.Draw();

			// Tab over the tree control
			TreeControl.RunningX += 16;


			// Now Draw the Controls for this panel.
			foreach (vxTreeNode control in Items)
			{
				if (IsExpanded)
				{
					TreeControl.RunningY += control.Height;

					control.Position = TreeControl.Position + new Vector2(TreeControl.RunningX, TreeControl.RunningY) + PositionOffset;

					control.Draw();
				}
				else
				{
					control.Position = TreeControl.Position + new Vector2(TreeControl.RunningX, TreeControl.RunningY) + PositionOffset;
				}
			}

			// Now decrement the tab over
			TreeControl.RunningX -= 16;
		}
	}
}
