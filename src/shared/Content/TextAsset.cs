using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.ContentManagement
{
    public enum TextAssetFileType
    { 
        Text,
        JSON,
        XML,
        YAML
    }

    public class TextAsset
    {
        public TextAssetFileType type = TextAssetFileType.JSON;
        public string text;
    }
}
