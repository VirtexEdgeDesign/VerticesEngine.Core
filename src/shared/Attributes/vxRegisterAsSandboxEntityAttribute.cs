using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Graphics;

namespace VerticesEngine
{
    public enum EntityAssetType
    {
        /// <summary>
        /// The main asset for this entity is a model
        /// </summary>
        Model,


        /// <summary>
        /// The main asset for this entity is a individual texture
        /// </summary>
        Texture,


        /// <summary>
        /// The main asset for this entity is an area on a sprite sheet
        /// </summary>
        SpriteSheet
    }

    /// <summary>
    /// Additional Sandbox Entity Meta Info such as Icon Location and description
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class vxSandboxEntityMetaAttribute : Attribute
    {
        public string Description { get; private set; }

        public string IconAssetPath { get; private set; }

        public bool HasIconSpriteSheet { get; private set; }

        public Rectangle IconLocation { get; set; }

        public vxSandboxEntityMetaAttribute(string description, int x, int y, int width, int height)
        {
            this.Description = description;
            this.IconLocation = new Rectangle(x, y, width, height);
            HasIconSpriteSheet = true;
            IconAssetPath = string.Empty;
        }

        public vxSandboxEntityMetaAttribute(string description, string iconPath)
        {
            this.Description = description;
            this.IconLocation = Rectangle.Empty;
            HasIconSpriteSheet = false;
            IconAssetPath = iconPath;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class vxRegisterAsSandboxEntityAttribute : Attribute
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Entity Category
        /// </summary>
        public object Category { get; private set; }

        /// <summary>
        /// Entity Sub Category
        /// </summary>
        public object SubCategory { get; private set; }


        /// <summary>
        /// The Asset Path
        /// </summary>
        public string AssetPath { get; private set; }


        /// <summary>
        /// Should it generate an icon every loop
        /// </summary>
        public bool GenerateIcon { get; private set; }

        /// <summary>
        /// If it has a sprite sheet, then a location must be specified. The sprite sheet used will be the
        /// content pack sprite sheet by default.
        /// </summary>
        public bool HasSpriteSheet { get; private set; }

        /// <summary>
        /// The sprite sheet location on the main sprite sheet.
        /// </summary>
        public Rectangle SpritesheetLocation { get; private set; }

        /// <summary>
        /// The icon location on the content packs main sprite sheet.
        /// </summary>
        public Rectangle IconLocation { get; private set; }


        /// <summary>
        /// Entity Type
        /// </summary>
        public EntityType EntityType { get; private set; }
        

        /// <summary>
        /// Asset Type
        /// </summary>
        public EntityAssetType EntityAssetType { get; private set; }


        /// <summary>
        /// Hide's this entity in the sandbox creation UIs
        /// </summary>
        public bool HideFromSandboxUI { get; private set; }

        /// <summary>
        /// Is this item visible in the sandbox list
        /// </summary>
        public bool IsVisibleInSandboxList { get; private set; }

        /// <summary>
        /// This tags a class as a sandbox item and allows you to explicitly set the asset path. This will generate an icon on launch
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryKey"></param>
        /// <param name="subCategory"></param>
        /// <param name="assetPath"></param>
        public vxRegisterAsSandboxEntityAttribute(string name, object category, object subCategory, string assetPath, EntityType EntityType = EntityType.BaseEntity, EntityAssetType EntityAssetType = EntityAssetType.Model, bool IsVisibleInSandboxList = true)
        {
            Name = name;
            Category = category;
            SubCategory = subCategory;
            AssetPath = assetPath;

            HasSpriteSheet = false;
            GenerateIcon = true;

            this.EntityType = EntityType;
            this.EntityAssetType = EntityAssetType;
            this.IsVisibleInSandboxList = IsVisibleInSandboxList;
            HideFromSandboxUI = false;
        }


        /// <summary>
        /// This tags a class as a sandbox item and allows you to explicitly set the asset path. This will generate an icon on launch
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="subCategory"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="EntityType"></param>
        public vxRegisterAsSandboxEntityAttribute(string name, object category, object subCategory, int x, int y, int width, int height, EntityType EntityType = EntityType.BaseEntity)
        {
            Name = name;
            Category = category;
            SubCategory = subCategory;

            this.EntityType = EntityType;
            this.EntityAssetType = EntityAssetType.SpriteSheet;

            this.IsVisibleInSandboxList = true;

            HasSpriteSheet = true;
            AssetPath = string.Empty;
            SpritesheetLocation = new Rectangle(x, y, width, height);
            IconLocation = new Rectangle(x, y, width, height);
            HideFromSandboxUI = false;
        }


        /// <summary>
        /// This tags a class as a sandbox item and allows you to explicitly set the asset path. This will generate an icon on launch
        /// </summary>
        /// <param name="name"></param>
        /// <param name="category"></param>
        /// <param name="subCategory"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hideFromSandboxUI"></param>
        public vxRegisterAsSandboxEntityAttribute(string name, object category, object subCategory, int x, int y, int width, int height, bool hideFromSandboxUI)
        {
            Name = name;
            Category = category;
            SubCategory = subCategory;

            this.EntityType = EntityType.BaseEntity;
            this.EntityAssetType = EntityAssetType.SpriteSheet;

            HasSpriteSheet = true;
            AssetPath = string.Empty;
            SpritesheetLocation = new Rectangle(x, y, width, height);
            IconLocation = new Rectangle(x, y, width, height);
            HideFromSandboxUI = hideFromSandboxUI;
        }
    }
}
