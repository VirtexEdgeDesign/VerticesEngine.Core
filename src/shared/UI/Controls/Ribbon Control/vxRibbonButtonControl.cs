using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{

	/// <summary>
	/// Ribbon control.
	/// </summary>
	public class vxRibbonButtonControl : vxButtonControl
	{

		public vxRibbonButtonControl(vxRibbonControlGroup RibbonControlGroup, string Text, Texture2D texture) :
		this(RibbonControlGroup, Text, texture, vxEnumButtonSize.Small)
		{ }

		public vxRibbonButtonControl(vxRibbonControlGroup RibbonControlGroup, string Text, Texture2D texture, vxEnumButtonSize ButtonSize):
		base(Text, Vector2.Zero)
		{
			this.Icon = texture;

			this.ButtonSize = ButtonSize;

            Height = 24;
            Width = 72;

            Font = vxInternalAssets.Fonts.ViewerFont;

            Position = new Vector2(vxScreen.Width, vxScreen.Height);

			if (ButtonSize == vxEnumButtonSize.Big)
			{
				Height = (int)( Padding.Y + 32 + Padding.Y + Font.LineSpacing + Padding.Y);
			}
            else
            {
                Width = Math.Max((int)Font.MeasureString(Text).X + 8, 72);
            }

			RibbonControlGroup.Add(this);


            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.Transparent, Color.DeepSkyBlue, Color.DarkOrange),
                new vxColourTheme(Color.Black * 0.75f, Color.Black, Color.Black));
		}




        public override void Draw()
        {
            Font = vxInternalAssets.Fonts.ViewerFont;

            // The default is the Centered text
            Vector2 TextPosition = Bounds.Location.ToVector2() + GetCenteredTextPosition(Font, Text).ToPoint().ToVector2();
            SpriteBatch.Draw(DefaultTexture, BorderBounds.GetBorder(1), GetStateColour(Theme.Background));
                
            SpriteBatch.Draw(DefaultTexture, BorderBounds, (HasFocus ? Color.WhiteSmoke * 0.9f : ToggleState ? Color.WhiteSmoke * 0.75f : Color.Transparent));

            // If there's no Icon, then draw the button with centered text
            if (Icon == null)
            {
                SpriteBatch.DrawString(Font,
                                       Text,
                                       TextPosition,
                                       GetStateColour(Theme.Background));
            }
            // If there is an Icon, then Position the Icon Respectively
            else
            {
                // If there is an Icon, then draw the icon to the left of the text
                if (ButtonSize == vxEnumButtonSize.Small)
                {
                    SpriteBatch.Draw(Icon,
                                     new Rectangle(Bounds.X + (int)Padding.X,
                                                   Bounds.Y + Height / 2 - 8,
                                                   16, 16), Color.White);


                    SpriteBatch.DrawString(Font,
                                           Text,
                                           Bounds.Location.ToVector2() + new Vector2(16 + Padding.X * 3, Padding.Y),
                                       GetStateColour(Theme.Text));
                }
                else if (ButtonSize == vxEnumButtonSize.Big)
                {
                    int strtx = Bounds.X + Width / 2 - 16;
                    SpriteBatch.Draw(Icon,
                                     new Rectangle(strtx,
                                                   Bounds.Y + (int)Padding.Y,
                                                   32, 32), Color.White);

                    SpriteBatch.DrawString(Font,
                                       Text,
                                           new Vector2(
                                               (int)(Bounds.X + GetCenteredTextPosition(Font, Text).X),
                                               (int)(Bounds.Y + (int)Padding.Y + 32 + (int)Padding.Y)),
                                           GetStateColour(Theme.Text));
                }
            }
            /*
            if (DoBorder && HasFocus)
            {
                BorderBounds = new Rectangle(Bounds.X - BorderSize, Bounds.Y - BorderSize, Bounds.Width + BorderSize * 2, Bounds.Height + BorderSize * 2);
                SpriteBatch.Draw(DefaultTexture, BorderBounds, GetStateColour(ItemTheme.Background) * 1.25f);
            }

            float factor = 1;//0.9f;
            if (HasFocus)
                factor = 1.5f;
            

            Color BackgroundColour = Color.Transparent;
            BackgroundColour.A = 255;
            Color outterColour = Color.WhiteSmoke * 0.9f * factor;
            outterColour.A = 255;

            // Draw the Main Background
            SpriteBatch.Draw(DefaultTexture, Bounds, HasFocus ? outterColour : BackgroundColour);



            // Draw a Selection Boundary
            if (HasFocus && DoSelectionBorder)
            {
                // Draw the border
                BorderBounds = new Rectangle(Bounds.X + 1, Bounds.Y + 1,
                                             Bounds.Width - 2, Bounds.Height - 2);

                SpriteBatch.Draw(DefaultTexture, BorderBounds, BackgroundColour);
            }


            // The default is the Centered text
            Vector2 TextPosition = Bounds.Location.ToVector2() + GetCenteredTextPosition(Font, Text).ToPoint().ToVector2();

            // Set Text Position Based on Text Justification
            switch (TextHorizontalJustification)
            {
                case vxEnumTextHorizontalJustification.Left:
                    TextPosition = Bounds.Location.ToVector2() + Padding;
                    break;
            }


            // If there's no Icon, then draw the button with centered text
            if (Icon == null)
            {
                SpriteBatch.DrawString(Font,
                                       Text,
                                       TextPosition,
                                       GetStateColour(ItemTheme.Background));
            }







            // If there is an Icon, then Position the Icon Respectively
            else
            {
                // If there is an Icon, then draw the icon to the left of the text
                if (ButtonSize == vxEnumButtonSize.Small)
                {
                    SpriteBatch.Draw(Icon,
                                     new Rectangle(Bounds.X + (int)Padding.X,
                                                   Bounds.Y + Height / 2 - 8,
                                                   16, 16), Color.White);


                    SpriteBatch.DrawString(Font,
                                           Text,
                                           Bounds.Location.ToVector2() + new Vector2(16 + Padding.X * 3, Padding.Y),
                                       GetStateColour(ItemTheme.Text));
                }
                else if (ButtonSize == vxEnumButtonSize.Big)
                {
                    int strtx = Bounds.X + Width / 2 - 16;
                    SpriteBatch.Draw(Icon,
                                     new Rectangle(strtx,
                                                   Bounds.Y + (int)Padding.Y,
                                                   32, 32), Color.White);

                    SpriteBatch.DrawString(Font,
                                       Text,
                                           new Vector2(
                                               (int)(Bounds.X + GetCenteredTextPosition(Font, Text).X),
                                               (int)(Bounds.Y + (int)Padding.Y + 32 + (int)Padding.Y)),
                                           GetStateColour(ItemTheme.Text));
                }

            }
            */
        }
	}
}
