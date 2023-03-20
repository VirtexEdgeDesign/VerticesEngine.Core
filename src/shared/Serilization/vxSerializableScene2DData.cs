using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;

//Virtex vxEngine Declaration
using VerticesEngine;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Serilization
{

    /// <summary>
    /// This holds the Serializable data for a vxScene3D including all Entities, Terrains, as well as
    /// Level and Enviroment Data.
    /// </summary>
	public class vxSerializableScene2DData : vxSerializableSceneBaseData
    {
		
        [XmlElement("entity")]
		public List<vxSerializableEntityState2D> Entities;
        
		public vxSerializableScene2DData() : base()
        {
            Entities = new List<vxSerializableEntityState2D>();
        }

        // Clears the File to take a new file in.
        public override void Clear()
        {
            Entities.Clear();
        }
    }
}
