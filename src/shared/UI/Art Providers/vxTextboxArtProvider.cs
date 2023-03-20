using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;

namespace VerticesEngine.UI.Themes
{
    public class vxTextboxArtProvider : vxUIArtProvider<vxTextbox>
    {
        public bool DoShadow = false;

        //public string DisplayText = "";

        public Rectangle Bounds;

        public virtual string Caret
        {
            get { return "|"; }
        }

        /// <summary>
        /// Cursor character.
        /// </summary>
        //public string CursorCharacter = "|";

        public vxTextboxArtProvider() : base()
		{
			DefaultWidth = 150;
			DefaultHeight = 24;

			DoBorder = true;
			BorderWidth = 2;

            Theme = new vxUIControlTheme(
                new vxColourTheme(Color.DarkOrange, Color.DarkOrange * 1.2f, Color.DeepSkyBlue),
                new vxColourTheme(Color.Black),
            new vxColourTheme(Color.Black * 0.75f, Color.Black, Color.Black));

			//BackgroundImage = DefaultTexture; //Engine.InternalContentManager.Load<Texture2D>("Gui/DfltThm/vxUITheme/vxButton/Bckgrnd_Nrml");
            Padding = new Vector2(5);

            //Font = vxUITheme.Fonts.Size12;
        }

        public override SpriteFont Font => vxUITheme.Fonts.Size12;



        public object Clone()
		{
			return this.MemberwiseClone();
		}


        protected internal override void DrawUIControl(vxTextbox textbox)
        {
            //DisplayText = textbox.Text;
            //First Set the Bounding Rectangle for the Textbox
            Bounds = new Rectangle(
                (int)(textbox.Position.X - Padding.X),
                (int)(textbox.Position.Y - Padding.Y / 2),
                (int)(textbox.Width + Padding.X),
                (int)(textbox.Height + Padding.Y));

            Theme.SetState(textbox);

            // Now set the Height for a Multiline Box
            if (textbox.IsMultiLine)
                Bounds.Height = Math.Max((int)(Font.LineSpacing + Padding.Y / 2), textbox.Height);
            else
                Bounds.Height = (int)(Font.LineSpacing + Padding.Y);

            // Now set the Border Rectangle.
            Rectangle BackRectangle = Bounds.GetBorder(1);

            // Finally, set the Text position based on the mid of the text box.
            Vector2 TextPosition = Bounds.Location.ToVector2() + new Vector2(0, Bounds.Height / 2 - Font.LineSpacing / 2);

            // If it's multiline, then set it based on the position and padding.
            //if (textbox.IsMultiLine)
                TextPosition = textbox.Position + textbox.TextPositionOffset;


            if (DoShadow)
            {
                BackRectangle.Location.Add(new Point(3, 3));
                vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, BackRectangle, Color.Black * 0.5f);
                BackRectangle.Location.Subtract(new Point(3, 3));
            }

            //Draw the Text Box
            // Draw the Border

            vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, Bounds.GetBorder(1), 
                (textbox.HasFocus ? Color.Black : Color.Black * 0.75f) * textbox.TransitionAlpha);

            // Draw the Inside
            vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, Bounds, 
                (textbox.HasFocus ? Color.DarkOrange : Color.Gray) * textbox.TransitionAlpha);

            //Draw Text

            if (textbox.Text != "")
                TextPosition.X += Font.MeasureString(textbox.Text.Substring(0, textbox.DisplayTextStart + 1)).X;

            TextPosition = textbox.Position;

            string leftText = textbox.Text.Substring(0, textbox.CaretIndex);

            //string rightText;


            vxGraphics.SpriteBatch.DrawString(Font, leftText, TextPosition.ToPoint().ToVector2(),
            (textbox.HasFocus ? Color.Black : Color.Black * 0.75f) * textbox.TransitionAlpha);

            //vxGraphics.SpriteBatch.DrawString(Font, textbox.DisplayText, TextPosition.ToPoint().ToVector2(),
            //(textbox.HasFocus ? Color.Black : Color.Black * 0.75f) * textbox.TransitionAlpha);


            //if (textbox.IsMultiLine)
            //{
            //             if (textbox.Lines != null)
            //             {

            //                 vxGraphics.SpriteBatch.DrawString(Font, textbox.DisplayText, TextPosition.ToPoint().ToVector2(),
            //                 (textbox.HasFocus ? Color.Black : Color.Black * 0.75f) * textbox.TransitionAlpha);
            //             }
            //             /*
            //	if (textbox.Lines != null)
            //		for (int i = 0; i < textbox.Lines.Length; i++)
            //		{
            //			string line = textbox.Lines[i];

            //			Vector2 pos = TextPosition + new Vector2(0, Font.LineSpacing * i);


            //			// Is the line present in the bounds.
            //			if (Bounds.Contains(pos) && Bounds.Contains(TextPosition + new Vector2(0, Font.LineSpacing * (i + 1))))
            //			{
            //				vxGraphics.SpriteBatch.DrawString(Font, line, pos,
            //	(textbox.HasFocus ? Color.Black : Color.Black * 0.75f) * textbox.TransitionAlpha);
            //			}
            //		}
            //             */
            //         }
            //else
            //{
            //             if(textbox.Text != "")
            //	    TextPosition.X += Font.MeasureString(textbox.Text.Substring(0, textbox.DisplayTextStart + 1)).X;
            //             TextPosition = textbox.Position;
            //             vxGraphics.SpriteBatch.DrawString(Font, textbox.DisplayText, TextPosition.ToPoint().ToVector2(),
            //	(textbox.HasFocus ? Color.Black : Color.Black * 0.75f) * textbox.TransitionAlpha);
            //}
            //vxGraphics.SpriteBatch.DrawString(Font, DisplayText, TextPosition, 
            //(textbox.HasFocus ? Color.Black : Color.Black * 0.75f) * textbox.TransitionAlpha);

            //Draw Caret Seperately
            vxGraphics.SpriteBatch.DrawString(Font, Caret, textbox.CaretPosition, Color.Black * 0.75f * textbox.CaretAlpha);
        }
    }
}

