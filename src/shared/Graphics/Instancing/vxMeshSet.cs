using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace VerticesEngine.Graphics
{
    /// <summary>
    /// This is an experimental system which works with *obj models on their most
    /// basic levels allowing for Voxel engines and Pseudo "Instance Meshing to be possible
    /// within Monogame and on a cross platform level.
    /// </summary>
    /// <remarks>
    /// This holds a set of Vertice Data which corresponds too a set of vxEntities. The models
    /// don't need to be the same, as this is essentially just a collection of Vertices with
    /// corresponding Normals and UV Texture Coordinates.
    /// </remarks>
    public class vxMeshSet
    {
        /// <summary>
        /// Gets or sets the entity list.
        /// </summary>
        /// <value>The entity list.</value>
        public List<vxEntity3D> EntityList;

        /// <summary>
        /// Gets or sets the collection of Vertices.
        /// </summary>
        /// <value>The vertices.</value>
        public List<Vector3> Vertices;


        /// <summary>
        /// Gets or sets the collection of Normals.
        /// </summary>
        /// <value>The vertices.</value>
        public List<Vector3> Normals;

        /// <summary>
        /// Gets or sets the collection of texture UV coordinates.
        /// </summary>
        /// <value>The texture UV coordinate.</value>
        public List<Vector2> TextureUVCoordinate;


        public vxMeshSet()
        {
            EntityList = new List<vxEntity3D>();
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            TextureUVCoordinate = new List<Vector2>();
        }

        public void Add(vxEntity3D entity)
        {
            //TODO: Switch to vxModelVoxel

            ////First, this entity MUST have a non-null vxModel.
            //if (entity.vxModel != null) {
            //	//Next add it to the overall Model List
            //	EntityList.Add(entity);

            //	//Now Add the Vertices too the Global List
            //	foreach(Vector3 vec in entity.vxModel.Vertices)
            //		Vertices.Add(vec);

            //	//Now Normals
            //	foreach(Vector3 norm in entity.vxModel.Normals)
            //		Normals.Add(norm);

            //	//Get the Texture Coordinates
            //	foreach(Vector2 uvText in entity.vxModel.TextureUVCoordinate)
            //		TextureUVCoordinate.Add(uvText);

            //entity.MeshSet = this;
        }
    }
}