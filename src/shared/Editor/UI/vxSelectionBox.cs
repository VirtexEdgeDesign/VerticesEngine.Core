using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.UI;

namespace VerticesEngine.Editor.UI
{
    public enum SelectioBoxState
    {
        None,
        Draggin,
        Finished
    }

    public class vxSelectionBox : vxUIControl
    {
        public static vxSelectionBox Instance
        {
            get { return m_instance; }
        }
        private static vxSelectionBox m_instance;

        public vxSelectionBox():base()
        {
            m_instance = this;
        }

        private Vector2 StartPoint = Vector2.One;


        public SelectioBoxState SelectioBoxState = SelectioBoxState.None;

        public void SetStartPoint(Vector2 point)
        {
            StartPoint = point;
            SelectioBoxState = SelectioBoxState.Draggin;
        }


        /// <summary>
        /// Called when the Selection box has lifted
        /// </summary>
        public event Action<Rectangle> OnSelection;

        protected internal override void Update()
        {
            base.Update();

            switch(SelectioBoxState)
            {
                case SelectioBoxState.None:
                    // nothing
                    break;

                case SelectioBoxState.Draggin:

                    if(vxInput.Cursor.X < StartPoint.X && StartPoint.Y < vxInput.Cursor.Y)
                    {
                        var x = vxInput.Cursor.X;
                        var y = StartPoint.Y;
                        var w = StartPoint.X - vxInput.Cursor.X;
                        var h = vxInput.Cursor.Y - StartPoint.Y;
                        selectionRectangle = vxLayout.GetRect(x, y, w, h);
                    }
                    else if (vxInput.Cursor.X > StartPoint.X && StartPoint.Y > vxInput.Cursor.Y)
                    {
                        var x = StartPoint.X;
                        var y = vxInput.Cursor.Y;
                        var w = vxInput.Cursor.X - StartPoint.X ;
                        var h = StartPoint.Y - vxInput.Cursor.Y;
                        selectionRectangle = vxLayout.GetRect(x, y, w, h);
                    }
                    else if (vxInput.Cursor.X < StartPoint.X && StartPoint.Y > vxInput.Cursor.Y)
                    {
                        selectionRectangle = vxLayout.GetRect(vxInput.Cursor, StartPoint- vxInput.Cursor);
                    }
                    else
                    { 
                        selectionRectangle = vxLayout.GetRect(StartPoint, vxInput.Cursor - StartPoint);
                    }
                    
                    if (vxInput.IsNewMainInputUp())
                    {
                        SelectioBoxState = SelectioBoxState.Finished;
                    }
                    break;
                case SelectioBoxState.Finished:
                    OnSelection?.Invoke(selectionRectangle);
                    SelectioBoxState = SelectioBoxState.None;
                    selectionRectangle = Rectangle.Empty;
                    break;

            }
        }

        private Rectangle selectionRectangle;

        public override void Draw()
        {
            base.Draw();

            if (SelectioBoxState == SelectioBoxState.Draggin)
            {
                vxGraphics.SpriteBatch.Draw(DefaultTexture, selectionRectangle, Color.DeepSkyBlue * 0.5f);
            }
        }
    }
}
