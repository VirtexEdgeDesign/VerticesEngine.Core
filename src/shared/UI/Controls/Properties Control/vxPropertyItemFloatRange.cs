using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;
using VerticesEngine;
using VerticesEngine.UI.Events;
using Microsoft.Xna.Framework.Audio;
using VerticesEngine.UI.Themes;
using System.Collections.Generic;
using System.Reflection;
using VerticesEngine.Input;
using Microsoft.Xna.Framework.Input;

namespace VerticesEngine.UI.Controls
{
    class vxSliderMarker : vxUIControl
    {
        public vxSliderMarker() 
        {
            Width = 14;
            Height = Width;
        }

        protected internal override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            SpriteBatch.Draw(DefaultTexture, Bounds.GetBorder(1), Color.Black);
            SpriteBatch.Draw(DefaultTexture, Bounds, this.HasFocus ? Color.DeepSkyBlue: Color.DarkOrange);
        }
    }


    public class vxPropertyControlFloatRange : vxUIControl
    {
        public float Min = 0;

        public float Max = 1;

        /// <summary>
        /// The Value of the Slider.
        /// </summary>
        public float Value = 0.5f;

        /// <summary>
        /// The previous value to check if anything has changed.
        /// </summary>
        float PreviousValue = 0.5f;

        /// <summary>
        /// The Incremental value that the Slider can be set to.
        /// </summary>
        public float Tick = 0.01f;

        private vxSliderMarker m_marker;

        vxPropertyItemFloatRange m_property;

        Rectangle m_trackBounds = new Rectangle(0, 0, 1, 1);

        private bool m_canTakeInput = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="VerticesEngine.UI.Controls.vxPropertyControlFloatRange"/> class.
        /// </summary>
        /// <param name="Engine">The Vertices Engine Reference.</param>
        /// <param name="text">This GUI Items Text.</param>
        /// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public vxPropertyControlFloatRange(vxPropertyItemFloatRange Property, float initval, float min, float max)
        {
            ////Colour_Text = Engine.GUITheme.vxLabelColorNormal;
            this.m_property = Property;
            this.Font = vxInternalAssets.Fonts.ViewerFont;
            //System.IO.FileInfo fi;// = System.IO.File.

            m_marker = new vxSliderMarker();

            Text = "test";

            Min = min;
            Max = max;
        }

        protected virtual void OnValueChange(float newValue, float previousValue)
        {
            //vxConsole.WriteLine(newValue);
        }

        protected internal override void Update()
        {
            base.Update();
            m_marker.Update();


            //if (m_canTakeInput == true && vxInput.MouseState.LeftButton == ButtonState.Released)
            //    m_canTakeInput = false;

            if (m_marker.HasFocus && vxInput.IsNewMouseButtonPress(MouseButtons.LeftButton))
                m_canTakeInput = true;



            if (m_canTakeInput)
            {
                // First get the new X component based off of the mouse movement.
                //float newX = m_marker.Position.X + vxInput.Cursor.X - vxInput.PreviousCursor.X;
                float newX = vxInput.Cursor.X - m_marker.Width/2;

                // Next clamp it between the track X bounds.
                newX = MathHelper.Clamp(newX, m_trackBounds.Left, m_trackBounds.Right);

                float mvmntPercentage = (newX - m_trackBounds.Left) / (m_trackBounds.Width);

                Value = Min + (Max - Min) * mvmntPercentage;

                Value = vxMathHelper.RoundToNearestSpecifiedNumber(Value, Tick);


                // if we have brought the mouse up this frame then let's apply the value difference
                if(vxInput.IsNewMainInputUp())
                {
                    vxPropertyItemBaseClass.IsCommandAdded = true;
                    m_canTakeInput = false;
                    if (PreviousValue != Value)
                    {
                        m_property.SetValue(Value);
                        OnValueChange(Value, PreviousValue);
                    }

                    PreviousValue = Value;
                }
                else // we'll assum that the input is down
                {
                    vxPropertyItemBaseClass.IsCommandAdded = false;
                    m_property.SetValue(Value);
                }
            }
        }

        public void InitValues(float value)
        {
            Value = value;
            PreviousValue = value;

            m_marker.Position = new Vector2(m_trackBounds.Width * Value / (Max - Min), m_marker.Position.Y);
        }


        public override void Draw()
        {
            base.Draw();

            var trackY = this.Position.Y;
            var trackHeight = 4;
            var trackWidth = this.Width - trackHeight - m_marker.Width;
            m_trackBounds = vxLayout.GetRect(this.Position.X, trackY + trackHeight, trackWidth, trackHeight);

            // first draw the track
            SpriteBatch.Draw(DefaultTexture, m_trackBounds.GetBorder(1), Color.Black);
            SpriteBatch.Draw(DefaultTexture, m_trackBounds, Color.Gray);

            // get marker location
            m_marker.Position = new Vector2(this.Position.X + (Value - Min) / (Max - Min) * trackWidth, trackY - 1);
            m_marker.Draw();

            string format = "0.000";

            if(Max-Min > 10)
                format = "##0";

            string valueString = $" [{Value.ToString(format)}]";
            SpriteBatch.DrawString(Font, valueString, this.Position - Vector2.UnitX * (Font.MeasureString(valueString).X+2), Color.White);
        }
    }

    public class vxPropertyItemFloatRange : vxPropertyItemBaseClass
    {
        vxPropertyControlFloatRange m_range;


        public vxPropertyItemFloatRange(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects, float min, float max, float tick) :
        base(propertyGroup, PropertyInfo, TargetObjects)
        {
            string val = GetPropertyValue().ToString();

            m_range.Min = min;
            m_range.Max = max;
            m_range.Tick = tick;

            float result = 0;
            if (float.TryParse(val, out result) && m_range != null)
            {
                m_range.InitValues(result);
            }
        }

        public override bool OnSelectionSetPropertyCompairson(object currentObject, object previousObject)
        {
            return ((float)currentObject == (float)previousObject);
        }

        public override vxUIControl CreatePropertyInputControl()
        {
            m_range = new vxPropertyControlFloatRange(this, 0.5f, 0, 1);
            return m_range;
        }

        public override void OnValueChange()
        {
            base.OnValueChange();

            string val = GetPropertyValue().ToString();

            float result = 0;
            if(float.TryParse(val, out result) && m_range != null)
            {
                m_range.InitValues(result);
            }
        }
    }
}
