using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using VerticesEngine.Graphics;
using VerticesEngine.UI.Themes;

namespace VerticesEngine.UI.Controls
{
    class vxAttributeCategory
    {
        public string name;
        public List<vxPropertyAttributePair> attributes = new List<vxPropertyAttributePair>();

        public vxAttributeCategory(object name)
        {
            this.name = name.ToString().SplitIntoSentance();
        }
    }


    class vxPropertyAttributePair
    {
        public PropertyInfo propertyInfo;
        public vxShowInInspectorAttribute attribute;
        public vxPropertyAttributePair(PropertyInfo propertyInfo, vxShowInInspectorAttribute attribute)
        {
            this.propertyInfo = propertyInfo;
            this.attribute = attribute;
        }
    }


    /// <summary>
    /// Vx properties control.
    /// </summary>
    public class vxPropertiesControl : vxPanel
    {
        vxPropertiesScrollPanelControl m_scrollPanel;

        public static bool ScaleByResolution = false;


        public static int GetScaledWidth(float i)
        {
            return ScaleByResolution ? vxLayout.GetScaledWidth(i) : (int)i;
        }
        public static int GetScaledHeight(float i)
        {
            return ScaleByResolution ? vxLayout.GetScaledHeight(i) : (int)i;
        }


        public static Vector2 Scale
        {
            get
            {
                return ScaleByResolution ? vxLayout.Scale : Vector2.One;
            }
        }

        /// <summary>
        /// The property type to extract.
        /// </summary>
        public Type PropertyTypeToExtract;

        public vxLabel SelectionName;

        public vxLabel TitleLabel;

        public vxLabel DescriptionLabel;

        public vxLineBatch LineBatch
        {
            get { return m_scrollPanel.LineBatch; }
        }

        //vxLabel DescriptionName;


        enum ExpandedStateUsage
        {
            Save,
            Load
        }

        private vxImage IconImage;

        private Dictionary<string, bool> ExpandedStateDictionary = new Dictionary<string, bool>();

        private SortedDictionary<string, vxAttributeCategory> categories = new SortedDictionary<string, vxAttributeCategory>();

        private float SavedScrollbarPos = 0;

        private int FooterSize = 75;

        private int iconSize = 72;

        private List<object> SelectionSet = new List<object>();

        public vxPropertiesControl(Vector2 Position, int Width, int Height, SpriteFont font = null)
            : base(Position, Width, Height)
        {
            // First Setup Font
            if (font == null)
                font = vxInternalAssets.Fonts.ViewerFont;
            this.Font = font;

            // Then the Footer Size
            FooterSize = Font.LineSpacing * 4;

            int iconSize = vxLayout.GetScaledSize(72);

            // Now The Panel Height
            int panelHeight = Height - (FooterSize + iconSize + 16);// Height;// - FooterSize;


            IconImage = new vxImage(DefaultTexture, Vector2.Zero, iconSize, iconSize);
            Items.Add(IconImage);

            SelectionName = new vxLabel("Title", new Vector2(IconImage.Width + font.LineSpacing * 0.25f, font.LineSpacing * 0.25f).ToIntValue());
            SelectionName.Font = font;
            Items.Add(SelectionName);


            m_scrollPanel = new vxPropertiesScrollPanelControl(Vector2.UnitY * (iconSize + 16), Width, panelHeight);
            Items.Add(m_scrollPanel);


            //TitleLabel = new vxLabel("Title", new Vector2(5, panelHeight + font.LineSpacing * 0.25f).ToIntValue());
            TitleLabel = new vxLabel("Title", new Vector2(5, m_scrollPanel.Bounds.Bottom + font.LineSpacing * 0.75f).ToIntValue());
            TitleLabel.Font = font;
            Items.Add(TitleLabel);

            DescriptionLabel = new vxLabel("Description", new Vector2(5, m_scrollPanel.Bounds.Bottom + font.LineSpacing * 2).ToIntValue());
            DescriptionLabel.Font = font;
            DescriptionLabel.Theme.Text =new vxColourTheme(Color.White * 0.65f);
            Items.Add(DescriptionLabel);

            DrawBackground = false;
        }


        public void GetPropertiesFromSelectionSet(List<vxEntity> entitySelectionSet)
        {
            if (entitySelectionSet.Count() > 0)
            {
                PropertyTypeToExtract = entitySelectionSet[0].GetType();

                SelectionName.Text = entitySelectionSet[0].GetTitle();
                IconImage.Texture = entitySelectionSet[0].GetIcon(iconSize, iconSize);

                // try to get the registration info
                var key = entitySelectionSet[0].GetType().Name;
                if (vxEntityRegister.EntityDefinitions.TryGetValue(key, out var val))
                {
                    if (val.Icon != null)
                        IconImage.Texture = val.Icon; 
                }
            }

            // Clear Current Selection Set
            SelectionSet.Clear();

            // Add and Process Selected Data List
            foreach(var entity in entitySelectionSet)
                SelectionSet.Add(entity);
            
            // Now loop through all items
            if(entitySelectionSet.Count() > 0)
            {
                AddPropertiesFromType(entitySelectionSet[0].GetType());
            }

            // Now refresh the expansion state
            LoopThroughCollection(ExpandedStateUsage.Load);
        }


        public void GetPropertiesFromObject(vxISelectable entitySelectionSet)
        {
            if (entitySelectionSet != null)
            {
                PropertyTypeToExtract = entitySelectionSet.GetType();

                SelectionName.Text = entitySelectionSet.GetTitle();
                IconImage.Texture = entitySelectionSet.GetIcon(iconSize, iconSize);
            }

            // Clear Current Selection Set
            SelectionSet.Clear();

            // Add and Process Selected Data List
            SelectionSet.Add(entitySelectionSet);

            // Now loop through all items
            if (entitySelectionSet != null)
                AddPropertiesFromType(entitySelectionSet.GetType());
            //entitySelectionSet.GetProperties(this);

            // Now refresh the expansion state
            LoopThroughCollection(ExpandedStateUsage.Load);
        }

        public void AddItem(vxUIControl item)
        {
            m_scrollPanel.AddItem(item);
        }


        public override void ResetLayout()
        {
           m_scrollPanel.ResetLayout();
        }

        void SavePropertyItemState(string preString, vxPropertyItemBaseClass item, ExpandedStateUsage usage)
        {
            string txt = preString + "." + item.Text.Replace(" ", string.Empty);
           // Console.WriteLine(txt  + "    Is Expanded: " + item.IsExpanded);

            if (usage == ExpandedStateUsage.Save)
            {
                SavedScrollbarPos = m_scrollPanel.ScrollBar.TravelPosition;
                // Does the Dictionary Contain the Key, if so, save it's expanded state
                if (ExpandedStateDictionary.ContainsKey(txt))
                    ExpandedStateDictionary[txt] = item.IsExpanded;
                // If the key is not in the dictionary, add it, and save the state
                else
                    ExpandedStateDictionary.Add(txt, item.IsExpanded);
            }
            else if(usage == ExpandedStateUsage.Load)
            {
                m_scrollPanel.ScrollBar.TravelPosition = SavedScrollbarPos;
                if (ExpandedStateDictionary.ContainsKey(txt))
                    item.IsExpanded = ExpandedStateDictionary[txt];
            }

            // Now recursively loop through
            foreach(var itm in item.Items)
            {
                if (itm is vxPropertyItemBaseClass)
                {
                    SavePropertyItemState(txt, (vxPropertyItemBaseClass)itm, usage);
                }
            }
        }

        void LoopThroughCollection(ExpandedStateUsage usage)
        {
            foreach (var grp in m_scrollPanel.Items)
            {
                if (grp is vxPropertyGroup)
                {
                    vxPropertyGroup pg = grp as vxPropertyGroup;
                    foreach (var item in pg.Items)
                        if (item is vxPropertyItemBaseClass)
                            SavePropertyItemState(pg.Text.Replace(" ", string.Empty), item, usage);
                }
            }
        }

        public void Clear()
        {
            LoopThroughCollection(ExpandedStateUsage.Save);
            m_scrollPanel.Clear();
        }



        /// <summary>
        /// Adds a properties group with all properties which have the specified attributes.
        /// </summary>
        /// <param name="pc">Pc.</param>
        /// <param name="GroupName">Group name.</param>
        /// <param name="AttributeType">Attribute type.</param>
        public void AddPropertiesFromType(Type EntityType)
        {
            List<PropertyInfo> propertiesInfo = EntityType.GetProperties().Where(
                p => p.GetCustomAttributes(typeof(vxShowInInspectorAttribute), true).Any()).ToList();

            categories.Clear();

            foreach (var property in propertiesInfo)
            {
                var attribute = property.GetCustomAttribute<vxShowInInspectorAttribute>();

                // first check if this category exists. if not, create it
                if (categories.ContainsKey(attribute.Category.ToString()) == false)
                    categories.Add(attribute.Category.ToString(), new vxAttributeCategory(attribute.Category));

                categories[attribute.Category.ToString()].attributes.Add(new vxPropertyAttributePair(property, attribute));
            }



            // now add the appropriate controls
            foreach (var category in categories.Values)
            {
                // create a new Category
                var propertiesGroup = new vxPropertyGroup(this, category.name);

                if (propertiesInfo.Count > 0)
                    this.AddItem(propertiesGroup);

                foreach (var property in category.attributes)
                {
                    propertiesGroup.Add(AddPropertyControl(propertiesGroup, property.propertyInfo, SelectionSet));
                }
            }
        }


        /// <summary>
        /// Adds the property to the property control.
        /// </summary>
        /// <returns>The property control.</returns>
        /// <param name="pg">Pg.</param>
        /// <param name="property">Property.</param>
        private vxPropertyItemBaseClass AddPropertyControl(vxPropertyGroup pg, PropertyInfo property, List<object> SelectionSet)
        {
            Type type = property.PropertyType;

            // check certain types first
            if (type == typeof(vxMesh))
                return new vxPropertyItemModel(pg, property, SelectionSet);

            if (type.IsSubclassOf(typeof(vxGameObject)))
                return new vxPropertyItemGameObject(pg, property, SelectionSet);

            if (type == typeof(vxItemList))
                return new vxPropertyItemList(pg, property, SelectionSet);

            if (type.IsEnum)
                return new vxPropertyItemChoices(pg, property, SelectionSet);

            if (type == typeof(List<string>))
                return new vxPropertyItemChoices(pg, property, SelectionSet);

            if (type == typeof(bool))
                return new vxPropertyItemBool(pg, property, SelectionSet);
            
            if(type == typeof(int))
                return new vxPropertyItemInt(pg, property, SelectionSet);

            if (type == typeof(float))
            {
                // check if it has a range property
                var hasRange = property.GetCustomAttribute<vxRangeAttribute>();
                if (hasRange != null)
                {
                    return new vxPropertyItemFloatRange(pg, property, SelectionSet, hasRange.Min, hasRange.Max, hasRange.Tick);
                }
                else
                {
                    return new vxPropertyItemFloat(pg, property, SelectionSet);
                }
            }
			////if (type == typeof(float[]))
            //    //return new vxPropertyItemFloatArray(pg, property, TargetObject);
            
            if (type == typeof(Vector2))
                return new vxPropertyItemVector2(pg, property, SelectionSet);
            
            if (type == typeof(Vector3))
                return new vxPropertyItemVector3(pg, property, SelectionSet);
                        
            //if (type == typeof(BoundingBox))
            //    return new vxPropertyItemBoundingBox(pg, property, TargetObject);

            //if (type == typeof(BoundingSphere))
            //    return new vxPropertyItemBoundingSphere(pg, property, TargetObject);

            if (type == typeof(Color))
                return new vxPropertyItemColour(pg, property, SelectionSet);

            if (type == typeof(Texture2D))
				return new vxPropertyItemTexture2D(pg, property, SelectionSet);
            
            return new vxPropertyItemBaseClass(pg, property, SelectionSet);
        }
    }
}
