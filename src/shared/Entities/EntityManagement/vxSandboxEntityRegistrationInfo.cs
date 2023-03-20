
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Reflection;
using VerticesEngine.Graphics;
using VerticesEngine.Plugins;

namespace VerticesEngine
{
    /// <summary>
    /// This class holds all information to register an entity with the vxSandbox enviroment.
    /// </summary>
    public class vxSandboxEntityRegistrationInfo
    {
        //vxEngine Engine;

        public Type Type;

        /// <summary>
        /// The Icon for thie Item
        /// </summary>
        public Texture2D Icon;

        /// <summary>
        /// The Key for this item
        /// </summary>
        public readonly string Key;

        /// <summary>
        /// The Name of the Current Item
        /// </summary>
        public string Description;

        /// <summary>
        /// File Path to the main asset
        /// </summary>
        public string FilePath;

        public EntityType EntityType
        {
            get { return m_itemAttribute.EntityType; }
        }

        public string Name
        {
            get { return m_itemAttribute.Name.ToString(); }
        }

        public string Category
        {
            get { return m_itemAttribute.Category.ToString(); }
        }


        public string SubCategory
        {
            get { return m_itemAttribute.SubCategory.ToString(); }
        }

        public bool IsVisibleInSandboxList
        {
            get { return m_itemAttribute.IsVisibleInSandboxList; }
        }

        vxRegisterAsSandboxEntityAttribute m_itemAttribute;

        public vxEntitySpriteSheetDefinition SpriteSheetInfo;

        vxIPlugin contentPack;

        /// <summary>
        /// Gets the content pack key that this entity belongs to.
        /// </summary>
        /// <value>The content pack key.</value>
        public virtual vxPluginMetadata ContentPack
        {
            get { return _contentPackKey; }
        }
        vxPluginMetadata _contentPackKey = new vxPluginMetadata();


        /// <summary>
        /// Hide's this entity in the sandbox creation UIs
        /// </summary>
        public bool HideFromSandboxUI
        {
            get { return m_itemAttribute.HideFromSandboxUI; }
        }

        /// <summary>
        /// Creates a Sandbox Entity Registration using the type, item attributes and content pack
        /// </summary>
        /// <param name="type"></param>
        /// <param name="itemAttribute"></param>
        /// <param name="contentPack"></param>
        public vxSandboxEntityRegistrationInfo(Type type,  vxRegisterAsSandboxEntityAttribute itemAttribute, ref vxIPlugin contentPack)
        {
            // set item attribute
            this.m_itemAttribute = itemAttribute;

            // type
            this.Type = type;

            // key
            this.Key = type.Name;

            this.Description = "";

            // file path
            this.FilePath = itemAttribute.AssetPath;

            // content pack
            this.contentPack = contentPack;

            // sprite sheet info
            SpriteSheetInfo = new vxEntitySpriteSheetDefinition(type.FullName, contentPack, itemAttribute.SpritesheetLocation);

            
            vxSandboxEntityMetaAttribute meta = type.GetCustomAttribute<vxSandboxEntityMetaAttribute>();
            if (meta != null)
                SpriteSheetInfo.IconSource = meta.IconLocation;

            if(vxEntityRegister.EntitySpriteSheetRegister.ContainsKey(Key) == false)
                vxEntityRegister.EntitySpriteSheetRegister.Add(Key, SpriteSheetInfo);
            _contentPackKey = new vxPluginMetadata(contentPack);

        }
    }
}