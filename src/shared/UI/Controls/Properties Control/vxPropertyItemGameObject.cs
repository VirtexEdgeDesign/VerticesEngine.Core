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
using VerticesEngine.Graphics;
using VerticesEngine.ContentManagement;

namespace VerticesEngine.UI.Controls
{
    /// <summary>
    /// Property item group.
    /// </summary>
    public class vxPropertyControlGameObject : vxTextbox
    {
        vxPropertyItemGameObject Property;

        public vxToggleImageButton ChooseObjectBtn;

        public Texture2D IconImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.UI.Controls.vxPropertyTextbox"/> class.
        /// </summary>
        /// <param name="Property">Property.</param>
        /// <param name="Text">Text.</param>
		public vxPropertyControlGameObject(vxPropertyItemGameObject Property, string Text) :
        base(Text, Vector2.Zero)
        {
            Padding = new Vector2(0, 1);
            this.Property = Property;
            //Set Text
            this.Text = Text;

            this.IsEnabled = Property.IsReadOnly;

            Font = Property.Font;

            Height = Property.GetHeight();
            Width = 3000;

            
            var inactiveTexture = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/check_box_uncheck");
            var activeTexture = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/check_box_mixed");
            //CheckedTexture = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/misc/check_box");
            IconImage = vxContentManager.Instance.Load<Texture2D>("vxengine/textures/sandbox/rbn/entities/entity_add_16");

            ChooseObjectBtn = new vxToggleImageButton(activeTexture, inactiveTexture, Vector2.Zero, 16, 16);
            ChooseObjectBtn.ToggleState = false;

            ChooseObjectBtn.Clicked += ChooseObjectBtn_Clicked;

            Theme.Text = new vxColourTheme(Color.White * 0.75f, Color.White, Color.Black);
        }

        private void ChooseObjectBtn_Clicked(object sender, vxUIControlClickEventArgs e)
        {
            Console.WriteLine("Toggle State " + ChooseObjectBtn.ToggleState);
            if (ChooseObjectBtn.ToggleState)
            {
                if (vxEngine.Instance.CurrentScene is vxGameplayScene3D)
                {
                    var scene = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>();

                    // make sure that we're telling the game that we want to get the object when selected
                    scene.MouseClickState = MouseClickState.ReturnItemToInspector;
                    scene.ItemSelectedForInspector += OnItemSelectedCallback;
                }
            }
            else
            {
                ClearToggleState();
            }
        }

        private void OnItemSelectedCallback(object sender, vxSandboxItemSelectedForInspectorEventArgs e)
        {
            Console.WriteLine(e.SelectedGameObject.Id);
            this.Property.SetValue(e.SelectedGameObject);
            ClearToggleState();
        }

        void ClearToggleState()
        {
            if (ChooseObjectBtn.ToggleState)
            {
                ChooseObjectBtn.ToggleState = false;
                if (vxEngine.Instance.CurrentScene is vxGameplayScene3D)
                {
                    var scene = vxEngine.Instance.GetCurrentScene<vxGameplayScene3D>();

                    // reset the selection mode and remove the event handler
                    scene.MouseClickState = MouseClickState.SelectItem;
                    scene.ItemSelectedForInspector -= OnItemSelectedCallback;
                }
            }
        }

        public override SpriteFont GetDefaultFont()
        {
            return vxUITheme.Fonts.Size12;
        }

        protected internal override void Update()
        {
            IsUpdateable = false;

            base.Update();

            ChooseObjectBtn.Position = vxLayout.GetVector2(Bounds.Location + Bounds.Size - new Point(16, 18));
            ChooseObjectBtn.Update();

            if (IsSelected)
            {
                // handle exiting object selection
                if (vxInput.IsNewKeyPress(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    Console.WriteLine("ESCAPED! Toggle State: " + ChooseObjectBtn.ToggleState);
                    ClearToggleState();
                }
            }
            else
            {
                if (ChooseObjectBtn.ToggleState)
                {
                    ClearToggleState();
                }
            }
        }



        // Don't draw the background
        public override void Draw()
        {
            //base.DrawText();
            if (ChooseObjectBtn.ToggleState)
            {
                vxGraphics.SpriteBatch.DrawString(Font, "Select Game Object", Position + Vector2.UnitX * 18,
                                          (GetStateColour(Theme.Text)) * TransitionAlpha, vxPropertiesControl.Scale);
            }
            else {
                vxGraphics.SpriteBatch.DrawString(Font, DisplayText, Position + Vector2.UnitX * 18,
                                        (GetStateColour(Theme.Text)) * TransitionAlpha, vxPropertiesControl.Scale);
        }
        // Draw Object Icon
        vxGraphics.SpriteBatch.Draw(IconImage, vxLayout.GetRect(Position, 16), Color.White);

            // Draw Button
            ChooseObjectBtn.Position = vxLayout.GetVector2(Bounds.Location + Bounds.Size - new Point(16, 18));
            ChooseObjectBtn.Draw();
        }
    }


    public class vxPropertyItemGameObject : vxPropertyItemBaseClass
    {

        public string IdName
        {
            get { return _idName; }
            set { _idName = value; }
        }
        private string _idName = "";


        public vxPropertyItemGameObject(vxPropertyGroup propertyGroup, PropertyInfo PropertyInfo, List<object> TargetObjects) :
        base(propertyGroup, PropertyInfo, TargetObjects)
        {
            // Create temp collection
            List<object> slctnst = new List<object>();
            slctnst.Add(this);

            PropertyBox.IsEnabled = false;

            SetToolTip(GetPropertyValueAsString());

            GetPropertyValue();
        }

        public override void SetValue(object newValue)
        {
            base.SetValue(newValue);
        }

        public override vxUIControl CreatePropertyInputControl()
        {
            return new vxPropertyControlGameObject(this, Value.ToString());
        }

        public override string GetPropertyValueAsString()
        {
            object result = base.GetPropertyValue();

            if (result is vxGameObject)
            {
                _idName = ((vxGameObject)result).Id;
                return _idName;
            }

            return "NULL";
        }

        public override void RefreshValue()
        {
            base.RefreshValue();
        }

        public override object GetPropertyValue()
        {
            object result = base.GetPropertyValue();

            if (result is vxGameObject)
            {
                Value = ((vxGameObject)result).Id;
            }
            else if (result is PropertyResponse)
            {                
                Value = GetPropertyResponseString((PropertyResponse)result);
            }

            return result;
        }

        public string GetPropertyResponseString(PropertyResponse response)
        {
            string responseString = "N/A";

            switch(response)
            {
                case PropertyResponse.Varies:
                    responseString = VARIES_TEXT;
                    break;
                case PropertyResponse.NULL:
                    responseString = NULL_TEXT;
                    break;
                case PropertyResponse.Error:
                    responseString = ERROR_TEXT;
                    break;
            }
            return responseString;
        }


        public override bool OnSelectionSetPropertyCompairson(object currentObject, object previousObject)
        {
            return ((vxGameObject)currentObject == (vxGameObject)previousObject);
        }
    }
}
