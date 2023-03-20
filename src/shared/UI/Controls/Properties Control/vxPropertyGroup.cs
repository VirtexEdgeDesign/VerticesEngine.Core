using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using Microsoft.Xna.Framework.Audio;
using VerticesEngine.UI.Themes;
using System.Collections.Generic;
using VerticesEngine.ContentManagement;

namespace VerticesEngine.UI.Controls
{
	/// <summary>
	/// Property item group.
	/// </summary>
	public class vxPropertyGroup : vxUIControl
	{
		public bool IsExpanded
		{
			get { return ToggleButton.ToggleState; }
		}

		vxToggleImageButton ToggleButton;

		public List<vxPropertyItemBaseClass> Items = new List<vxPropertyItemBaseClass>();

		Vector2 TitleSize;

		Rectangle Container = new Rectangle();

		Point Borders = new Point(16);

        public vxPropertiesControl PropertyControl;

        Rectangle Splitter = new Rectangle();

		public vxPropertyGroup(vxPropertiesControl PropertyControl, string Text)
		{
			Padding = new Vector2(0, 1);

			this.PropertyControl = PropertyControl;

			//Set Text
			this.Text = Text;

			Bounds = new Rectangle(0, 0, 64, 64);

			ToggleButton = new vxToggleImageButton(vxInternalAssets.UI.PropertyBulletPlus, vxInternalAssets.UI.PropertyBulletMinus, Vector2.Zero);
			
			ToggleButton.Clicked += delegate {
				PropertyControl.ResetLayout();
			};
           // Console.WriteLine(vxInternalAssets.Fonts.ViewerFont.LineSpacing);
            //Font = vxInternalAssets.Fonts.ViewerFont;
            Font = PropertyControl.Font;
			TitleSize = Font.MeasureString(Text);

            Height = GetHeight();

            Borders = new Point(Height);

			Width = 3000;

            this.PositionChanged += delegate {
				ResetLayout();
            };
		}

		public void Add(vxPropertyItemBaseClass property)
		{
			Items.Add(property);
            ResetLayout();
		}

        protected override void OnDisposed()
        {
            base.OnDisposed();

            ToggleButton.Dispose();

            foreach (var item in Items)
                item.Dispose();

        }


        public virtual int GetHeight()
        {
            return vxPropertiesControl.GetScaledHeight(Math.Max(Font.LineSpacing * 16/13, 16));
        }

		public override void ResetLayout()
		{
			Vector2 RunningLength = Position + Borders.ToVector2();
			Container.Location = Position.ToPoint() + Borders;

			if (IsExpanded)
			{
				foreach (var property in Items)
				{
                    // Set position
                    property.Width = this.Width - Borders.X - 3;
					property.Position = RunningLength;
                    RunningLength += Vector2.UnitY * (property.Height + 1);
				}

				Height = (int)(RunningLength.Y - Position.Y);
			}
			else
			{
                Height = GetHeight();
			}
			Container.Width = this.Width - Borders.X - 1;
			Container.Height = this.Height - Borders.X;

            BorderBounds = Bounds.GetBorder(1);

            BorderBounds.Location -= new Point(0,1);

                Splitter = new Rectangle(
                (int)Position.X + Container.Width / 2,
                (int)Position.Y + GetHeight(),
                1, Container.Height);

			ToggleButton.Position = Position - new Vector2(1,0);
		}

		protected internal override void Update()
        {
			IsUpdateable = false;
            base.Update();

            foreach (var property in Items)
                property.Update();

            ToggleButton.Update();
        }

        public override void DrawBorder()
        {
            // Draw Left Border
            PropertyControl.LineBatch.DrawLine(
                Position, Position + Vector2.UnitY * Height);

            // Draw Right Border
            PropertyControl.LineBatch.DrawLine(
                Position + Vector2.UnitX * Width, Position + new Vector2(Width, Height));

            Vector2 spltrPos = Splitter.Location.ToVector2();
            // Draw Splitter
            PropertyControl.LineBatch.DrawLine(spltrPos, 
                                               spltrPos + Vector2.UnitY * Splitter.Height, Color.Black * 0.35f);

            if (IsExpanded)
            {
                foreach (var property in Items)
                    property.DrawBorder();
            }
        }

		public override void DrawText()
		{

            Vector2 textPos = Position + new Vector2(Padding.X, GetHeight() / 2 - Font.MeasureString(Text).Y / 2 + 2);
            SpriteBatch.DrawString(Font, Text, textPos.ToIntValue() + new Vector2(Borders.X, 0), Color.White, vxPropertiesControl.Scale);

			if (IsExpanded)
			{
				foreach (var property in Items)
					property.DrawText();
			}
		}
		public override void Draw()
		{
			//Now get the Art Provider to draw the scene

			base.Draw();

			SpriteBatch.Draw(DefaultTexture, Bounds, Color.Black * 0.75f);
			SpriteBatch.Draw(DefaultTexture, Container, Color.Gray * 0.15f);

			if (IsExpanded)
			{
				foreach (var property in Items)
					property.Draw();
			}

			ToggleButton.Draw();
		}
	}
}
