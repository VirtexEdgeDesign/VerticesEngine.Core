using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine;

namespace VerticesEngine.Graphics.Rendering
{
    /// <summary>
    /// The scene sub system which handles all 
    /// </summary>
    internal class vxStaticMeshBatchRenderSystem : vxISceneSubSystem
    {
        public SubSystemType Type => SubSystemType.Scene;

        public bool IsEnabled { get => _isEnabled; set => _isEnabled = value; }
        private bool _isEnabled = false;


        public void Dispose()
        {
            
        }

        public void Initialise()
        {

        }

        
        public void Update()
        {
            
        }

        private Dictionary<Type, vxStaticMeshBatchEntity> collection = new Dictionary<Type, vxStaticMeshBatchEntity>();

        /// <summary>
        /// Registers a specific entity for a given archetype. NOTE that the <see cref="System.Type"/> for the <paramref name="entity"/> doesn't
        /// need to match the type of <typeparamref name="T"/>. For instance in Metric Racer there are multiple track piece Types, but they all
        /// are rendered together since they can share a given material.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public vxStaticMeshBatchRenderer RegisterEntityToArchetype<T>(vxEntity3D entity) where T : vxEntity3D
        {
            // get the archetype Type
            System.Type archeType = typeof(T);

            // if we don't have an archetype for this yet then let's make a new one
            if(!collection.ContainsKey(archeType))
            {
                var newStaticEntity = new vxStaticMeshBatchEntity();
                collection.Add(archeType, newStaticEntity);
            }

            // now we should add this entity to the Static Mesh Batcher entity
            var staticEntity = collection[archeType];
            staticEntity.AddEntity(entity);

            // finally let's grab the static mesh renderer for the associated entity
            var staticMeshBatchRenderer = staticEntity.GetComponent<vxStaticMeshBatchRenderer>();

            return staticMeshBatchRenderer;
        }
    }
}
