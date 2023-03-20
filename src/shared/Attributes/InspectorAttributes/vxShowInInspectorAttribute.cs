using System;

namespace VerticesEngine
{

    /// <summary>
    /// Properties with this Attribute will be shown in the Sandbox Properties Control
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class vxShowInInspectorAttribute : Attribute
    {
        public string Label { get; internal set; }

        public string Category { get; private set; }

        public string Description { get; private set; }

        public bool Debug = false;

        public bool IsReadOnly = false;


        /// <summary>
        /// Properties with this Attribute will be shown in the Sandbox Properties Control
        /// </summary>
        /// <param name="category">The Category for this Property</param>
        /// <param name="description">Description for this property</param>
        /// <param name="isDebugOnly">Should this Property only be added when the engine is in debug mode</param>
        public vxShowInInspectorAttribute(object category, string description = null, bool isDebugOnly = false)
        {
            // get the label during the 
            this.Label = string.Empty;
            this.Category = category.ToString();
            this.Description = description;
            this.Debug = isDebugOnly;
        }

        /// <summary>
        /// Properties with this Attribute will be shown in the Sandbox Properties Control
        /// </summary>
        /// <param name="label">Label for this Property</param>
        /// <param name="category">The Category for this Property</param>
        /// <param name="description">Description for this property</param>
        /// <param name="isReadOnly">Is this property readonly?</param>
        /// <param name="isDebugOnly">Should this Property only be added when the engine is in debug mode</param>
        public vxShowInInspectorAttribute(string label, object category, string description = null, bool isReadOnly = false, bool isDebugOnly = false)
        {
            this.Label = label;
            this.Category = category.ToString();
            this.Description = description;
            this.Debug = isDebugOnly;
            IsReadOnly = isReadOnly;
        }
    }
}
