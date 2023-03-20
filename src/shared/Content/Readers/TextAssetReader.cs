using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.Utilities;

using TRead = VerticesEngine.ContentManagement.TextAsset;

namespace VerticesEngine.ContentManagement
{
    public class TextAssetReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {            
            var txtType = input.ReadString();
            var version = input.ReadString();
            var assetText = input.ReadString();
            TRead asset = Newtonsoft.Json.JsonConvert.DeserializeObject<TRead>(assetText);
            return asset;
        }
    }
}
