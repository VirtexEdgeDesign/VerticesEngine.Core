using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VerticesEngine.Util;
//using VerticesEngine;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.MessageBoxs;
using VerticesEngine.Serilization;
using VerticesEngine.Utilities;

namespace VerticesEngine
{
    
    /// <summary>
    /// The save busy screen for 2D Scene.
    /// </summary>
    public class vxSaveBusyScreen2D : vxSaveBusyScreen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.vxSaveBusyScreen"/> class.
        /// </summary>
        public vxSaveBusyScreen2D(vxGameplayScene2D Scene)
            : base(Scene)
        {

        }


        //float percent = 0;

        public override void OnAsyncWriteSaveFile(object sender, DoWorkEventArgs e)
        {
            
        }
    }
}
