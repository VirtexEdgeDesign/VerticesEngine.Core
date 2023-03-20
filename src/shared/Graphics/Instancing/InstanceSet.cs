//TODO: Add Instance Support.
#if VIRTICES_INST_MESH
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VerticesEngine;
using VerticesEngine.Graphics;

namespace VerticesEngine
{
    /// <summary>
    /// A Set which includes all the instance data for a specific model
    /// </summary>
    public class InstanceSet
    {
        public Matrix[] instancedModelBones;
        public DynamicVertexBuffer mInstanceDataStream;
        vxEngine Engine;

        private bool _doShadowMap = true;
        public bool DoShadowMap
        {
            get { return _doShadowMap; }
            set { _doShadowMap = value; }
        }

        /// <summary>
        /// The List of Entity Instances to Draw
        /// </summary>
        public List<vxEntity3D> instances = new List<vxEntity3D>();


        // To store instance transform matrices in a vertex buffer, we use this custom
        // vertex type which encodes 4x4 matrices as a set of four Vector4 values.
        public static VertexDeclaration mInstanceVertexDeclaration;

        public Matrix[] instanceTransforms;
        public vxModel InstancedModel
        {
            get { return instancedModel; }
            set
            {
                instancedModel = value;
                instancedModelBones = new Matrix[instancedModel.ModelMain.Bones.Count];
                instancedModel.ModelMain.CopyAbsoluteBoneTransformsTo(instancedModelBones);                
            }
        }
        vxModel instancedModel;

        GraphicsDevice mGraphicsDevice;

        public InstanceSet(vxEngine Engine)
        {
            this.Engine = Engine;

            mGraphicsDevice = vxGraphics.GraphicsDevice;

            mInstanceVertexDeclaration = new VertexDeclaration(new[]
            {
                new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 2),
                new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 3),
                new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 4),
                new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 5)
            });

        }

        public void Initialise(vxEngine Engine)
        {
            foreach (ModelMeshPart part in InstancedModel.ModelMain.Meshes.SelectMany(m => m.MeshParts))
            {
                part.Effect.Parameters["LightDirection"].SetValue(Vector3.Normalize(new Vector3(100, 130, 0)));

                part.Effect.Parameters["LightColor"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                part.Effect.Parameters["AmbientLightColor"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));


                part.Effect.Parameters["Shininess"].SetValue(0.01f);
                part.Effect.Parameters["SpecularIntensity"].SetValue(8.0f);

                part.Effect.Parameters["PoissonKernel"].SetValue(Engine.Renderer.PoissonKernel);
                part.Effect.Parameters["RandomTexture3D"].SetValue(Engine.Renderer.RandomTexture3D);
                part.Effect.Parameters["RandomTexture2D"].SetValue(Engine.Renderer.RandomTexture3D);
                part.Effect.CurrentTechnique = part.Effect.Techniques["Lambert"];
            }
            
            //foreach (var eff in InstancedModel.ModelShadow.Meshes.SelectMany(m => m.Effects))
            //{
            //    eff.CurrentTechnique = eff.Techniques["ShadowInstanced"];
            //}





            foreach (ModelMeshPart part in InstancedModel.ModelMain.Meshes.SelectMany(m => m.MeshParts))
            {
                part.Effect.Parameters["LightDirection"].SetValue(Vector3.Normalize(new Vector3(100, 130, 0)));

                part.Effect.Parameters["LightColor"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                part.Effect.Parameters["AmbientLightColor"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));


                part.Effect.Parameters["Shininess"].SetValue(0.01f);
                part.Effect.Parameters["SpecularIntensity"].SetValue(8.0f);

                part.Effect.Parameters["PoissonKernel"].SetValue(Engine.Renderer.PoissonKernel);
                part.Effect.Parameters["RandomTexture3D"].SetValue(Engine.Renderer.RandomTexture3D);
                part.Effect.Parameters["RandomTexture2D"].SetValue(Engine.Renderer.RandomTexture3D);
                part.Effect.CurrentTechnique = part.Effect.Techniques["Lambert"];
            }

            //foreach (var eff in InstancedModel.ModelShadow.Meshes.SelectMany(m => m.Effects))
            //{
            //    eff.CurrentTechnique = eff.Techniques["ShadowInstanced"];
            //}
        }

        /// <summary>
        /// Adds an entity too the instanced mesh collection.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(vxEntity3D entity)
        {
            instances.Add(entity);

            entity.InstanceSetParent = this;

            SetSize();
        }


        /// <summary>
        /// Removes the specified Entity from the collection.
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(vxEntity3D entity)
        {
            instances.Remove(entity);

            SetSize();
        }

        /// <summary>
        /// Set's the Size of the Instance Mesh Array.
        /// </summary>
        public void SetSize()
        {
            // Gather instance transform matrices into a single array.
            Array.Resize(ref instanceTransforms, instances.Count);
        }

        public void Update()
        {

            for (int i = 0; i < instances.Count; i++)
            {
                instanceTransforms[i] = instances[i].World;
            }

            if (instances.Count > 0)
            {
                // Make sure our instance data vertex buffer is big enough. (4x4 float matrix)
                int instanceDataSize = 16 * sizeof(float) * instances.Count;

                if ((mInstanceDataStream == null) ||
                    (mInstanceDataStream.VertexCount < instances.Count))
                {
                    if (mInstanceDataStream != null)
                        mInstanceDataStream.Dispose();

                    mInstanceDataStream = new DynamicVertexBuffer(vxGraphics.GraphicsDevice,
                        mInstanceVertexDeclaration, instances.Count, BufferUsage.WriteOnly);
                }

                // Upload transform matrices to the instance data vertex buffer.
                mInstanceDataStream.SetData(instanceTransforms, 0, instances.Count, SetDataOptions.Discard);
            }
        }

        /// <summary>
        /// Sets the instancing data.
        /// </summary>
        /// <param name="model2worldTransformations">Model2world transformations.</param>
        /// <param name="numInstances">Number instances.</param>
        public void SetInstancingData(Matrix[] model2worldTransformations, int numInstances)
        {
            if (numInstances > 0)
            {
                // Make sure our instance data vertex buffer is big enough. (4x4 float matrix)
                int instanceDataSize = 16 * sizeof(float) * numInstances;

                if ((mInstanceDataStream == null) ||
                    (mInstanceDataStream.VertexCount < numInstances))
                {
                    if (mInstanceDataStream != null)
                        mInstanceDataStream.Dispose();

                    mInstanceDataStream = new DynamicVertexBuffer(mGraphicsDevice, mInstanceVertexDeclaration, numInstances, BufferUsage.WriteOnly);
                }

                // Upload transform matrices to the instance data vertex buffer.
                mInstanceDataStream.SetData(model2worldTransformations, 0, numInstances, SetDataOptions.Discard);
            }
        }

        /// <summary>
        /// Renders the instanced shadow.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="numInstances">Number instances.</param>
        public virtual void RenderInstancedShadow(Model model, vxCamera3D camera, int numInstances)
        {            
            //Only XNA Supports Instance Drawing Currently
#if VRTC_PLTFRM_XNA
            for (int i = 0; i < Engine.Renderer.NumberOfShadowSplits; ++i)
            {
                {
                    int x = i % 2;
                    int y = i / 2;
                    var viewPort = new Viewport(
                        x * Engine.Renderer.ShadowMapSize, 
                        y * Engine.Renderer.ShadowMapSize,
                        Engine.Renderer.ShadowMapSize,
                        Engine.Renderer.ShadowMapSize);

                    mGraphicsDevice.Viewport = viewPort;
                }

                // now render the spheres
                if (numInstances > 0)
                {
                    Matrix[] transforms = new Matrix[model.Bones.Count];
                    model.CopyAbsoluteBoneTransformsTo(transforms);

                    foreach (ModelMesh mesh in model.Meshes)
                    {
                        foreach (var part in mesh.MeshParts)
                        {
                            part.Effect.CurrentTechnique = part.Effect.Techniques["ShadowInstanced"];
                            part.Effect.Parameters["ViewProjection_Sdw"].SetValue(Engine.Renderer.ShadowSplitProjections[i]);
                            part.Effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index]);
                            part.Effect.Parameters["DepthBias_Sdw"].SetValue(new Vector2(
                                Engine.Renderer.ShadowDepthBias[i, 0], Engine.Renderer.ShadowDepthBias[i, 1]));
                            part.Effect.CurrentTechnique.Passes[0].Apply();

                            // set vertex buffer
                            mGraphicsDevice.SetVertexBuffers(new[]
                            {
                                part.VertexBuffer,
                                new VertexBufferBinding(mInstanceDataStream, 0, 1 )
                            });

                            // set index buffer and draw
                            mGraphicsDevice.Indices = part.IndexBuffer;
                            mGraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount, numInstances);
                        }
                    }
                }
            }
#endif
        }

        /// <summary>
		/// Instanced rendering. Draws instancedModel numInstances times. This demo uses hardware
		/// instancing: a secondary vertex stream is created, where the transform matrices of the
		/// individual instances are passed down to the shader. Note that in order to be efficient,
		/// the model should contain as little meshes and meshparts as possible.
        /// </summary>
        /// <param name="model">Model.</param>
        /// <param name="camera">Camera.</param>
        /// <param name="numInstances">Number instances.</param>
        /// <param name="technique">Technique.</param>
        public virtual void RenderInstanced(Model model, vxCamera3D camera, int numInstances, string technique)
        {            
            //Only XNA Supports Instance Drawing Currently
#if VRTC_PLTFRM_XNA
            if (numInstances > 0)
            {
                // Draw the model. A model can have multiple meshes, so loop.
                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);

                // loop through meshes
                foreach (ModelMesh mesh in model.Meshes)
                {
                    /*
                    foreach (var shadowedEffect in mesh.Effects.Where(e => e.Parameters.Any(p => p.Name == "ShadowMap")))
                    {
                        shadowedEffect.Parameters["ShadowMap"].SetValue(RenderTargets.RT_ShadowMap);
                        shadowedEffect.Parameters["ShadowTransform"].SetValue(Engine.Renderer.ShadowSplitProjectionsWithTiling);
                        shadowedEffect.Parameters["TileBounds"].SetValue(Engine.Renderer.ShadowSplitTileBounds);
                    }
                    */
                    // get bone matrix
                    Matrix boneMatrix = transforms[mesh.ParentBone.Index];
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect.CurrentTechnique = part.Effect.Techniques[technique];
                        part.Effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index]);
                        part.Effect.Parameters["View"].SetValue(camera.View);
                        part.Effect.Parameters["Projection"].SetValue(camera.Projection);
                        part.Effect.Parameters["LightDirection"].SetValue(Vector3.Normalize(Engine.Renderer.lightPosition));

                        if (Engine.Settings.Graphics.ShadowQuality == Settings.vxEnumQuality.None)
                            DoShadowMap = false;
                        else
                            DoShadowMap = true;

                        //if (part.Effect.Parameters["DoShadow"] != null)
                            part.Effect.Parameters["DoShadow"].SetValue(DoShadowMap);

                        part.Effect.Parameters["ShadowMap"].SetValue(RenderTargets.RT_ShadowMap);
                        part.Effect.Parameters["ShadowTransform"].SetValue(Engine.Renderer.ShadowSplitProjectionsWithTiling);
                        part.Effect.Parameters["TileBounds"].SetValue(Engine.Renderer.ShadowSplitTileBounds);
                        part.Effect.Parameters["SplitColors"].SetValue(Engine.Renderer.ShadowSplitColors.Select(c => c.ToVector4()).ToArray());

                        if (part.Effect.Parameters["VX_CAMERA_POS"] != null)
                            part.Effect.Parameters["VX_CAMERA_POS"].SetValue(Engine.Current3DSceneBase.Camera.WorldMatrix.Translation);

                        if (part.Effect.Parameters["FogNear"] != null)
                            part.Effect.Parameters["FogNear"].SetValue(5);

                        if (part.Effect.Parameters["FogFar"] != null)
                            part.Effect.Parameters["FogFar"].SetValue(Engine.Current3DSceneBase.Camera.FarPlane / 4);
                        
                        if (part.Effect.Parameters["FogColor"] != null)
                            part.Effect.Parameters["FogColor"].SetValue(Vector4.One);

                        part.Effect.CurrentTechnique.Passes[0].Apply();

                        // set vertex buffer
                        mGraphicsDevice.SetVertexBuffers(new[]
                        {
                            part.VertexBuffer,
                            new VertexBufferBinding(mInstanceDataStream, 0, 1 )
                        });

                        // draw primitives 
                        mGraphicsDevice.Indices = part.IndexBuffer;
                        mGraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 
                            part.VertexOffset, 0, part.NumVertices, 
                            part.StartIndex, part.PrimitiveCount, numInstances);
                    }
                }
            }
#endif
        }
    }
}
#endif