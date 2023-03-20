
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using VerticesEngine.Util;
using VerticesEngine.Graphics;
using VerticesEngine.UI.Controls;
using VerticesEngine.UI.Events;
using VerticesEngine.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using VerticesEngine.Plugins;
using VerticesEngine.Entities;
using VerticesEngine.Particles;

namespace VerticesEngine
{
    public partial class vxGameplayScene3D
    {
        /// <summary>
        /// This Collection stores all the items which are in the editor at start time, therefore
        /// any items which are added during the simulation (particles, entitie, etc...) can be caught when
        /// the stop method is run.
        /// </summary>
        private List<vxEntity> m_editorItems = new List<vxEntity>();

        /// <summary>
        /// This Dictionary contains a collection of all Registered items within the Sandbox.
        /// </summary>
        public Dictionary<string, vxSandboxEntityRegistrationInfo> RegisteredSandboxItemTypes
        {
            get { return m_registeredSandboxItemTypes; }
        }
        private Dictionary<string, vxSandboxEntityRegistrationInfo> m_registeredSandboxItemTypes = new Dictionary<string, vxSandboxEntityRegistrationInfo>();

        public static int SandboxItemButtonSize = 128;

        /// <summary>
        /// Creates a New Sandbox Item using the specified type and position
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public vxEntity3D InstantiateItem(Type type, Vector3 position)
        {
            vxConsole.WriteVerboseLine("InstantiateItem : {0}", type);
            
            // Let's try to get the most complex constructor
            System.Reflection.ConstructorInfo ctor = type.GetConstructor(new[] { typeof(Vector3) });
            if (ctor == null)
            {
                var newItem = Activator.CreateInstance(type);
                return (vxEntity3D) newItem;
                // ctor = type.GetConstructor(new[] { typeof(vxGameplayScene3D) });
                // if (ctor == null)
                // {
                //     var newItem = Activator.CreateInstance(type);
                //     return (vxEntity3D) newItem;
                // }
                // return (vxEntity3D)ctor.Invoke(new object[] { this });
            }
            else
            {
                return (vxEntity3D)ctor.Invoke(new object[] { position });
            }
        }



        /// <summary>
        /// Registers a new sandbox item.
        /// </summary>
        /// <returns>The new sandbox item.</returns>
        /// <param name="EntityDescription">Entity description.</param>
        private vxSandboxItemButton RegisterNewSandboxItem(vxSandboxEntityRegistrationInfo EntityDescription)
        {
            return RegisterNewSandboxItem(EntityDescription, Vector2.Zero, SandboxItemButtonSize, SandboxItemButtonSize);
        }


        /// <summary>
        /// Registers the new sandbox item.
        /// </summary>
        /// <returns>The new sandbox item.</returns>
        /// <param name="EntityDescription">Entity description.</param>
        /// <param name="ButtonPosition">Button position.</param>
        /// <param name="Width">Width.</param>
        /// <param name="Height">Height.</param>
        private vxSandboxItemButton RegisterNewSandboxItem(vxSandboxEntityRegistrationInfo EntityDescription, Vector2 ButtonPosition, int Width, int Height)
        {
            //First Ensure the Entity Description Is Loaded.
            try
            {
                if (EntityDescription.Icon == null)
                    EntityDescription.Icon = GenerateSandboxItemIcon(EntityDescription);
            }
            catch
            {
                vxConsole.WriteError(EntityDescription.Key);
                EntityDescription.Icon = vxInternalAssets.Textures.RandomValues;
            }

            if (m_registeredSandboxItemTypes.ContainsKey(EntityDescription.Key))
                return null;

            //Next Register the Entity with the Sandbox Registrar
            m_registeredSandboxItemTypes.Add(EntityDescription.Key, EntityDescription);


            vxConsole.WriteVerboseLine($"\tRegistering: \t'{EntityDescription.Key}' to Dictionary");

            if (EntityDescription.Description == null)
            {
                vxConsole.WriteLine(EntityDescription.Key + " Is null");
            }

            vxSandboxItemButton button = new vxSandboxItemButton(EntityDescription.Icon != null ? EntityDescription.Icon : vxInternalAssets.Textures.DefaultDiffuse,
                EntityDescription.Name,
                EntityDescription.Key,
                ButtonPosition, Width, Height);

            button.Clicked += AddItemButtonClicked;

            return button;
        }

        //protected vxSandboxItemButton RegisterNewSandboxImportedItem(string guid, Vector2 ButtonPosition, int Width, int Height)
        //{
        //    m_importedEntitiesTabPag
        //}


        protected override void LoadParticlePools()
        {
            foreach (var particleSet in vxEntityRegister.ParticleDefinitions.Values)
            {
                var particlePool = new vxParticlePool(particleSet.Type, particleSet.PoolSize);

                for (int i = 0; i < particleSet.PoolSize; i++)
                {
                    System.Reflection.ConstructorInfo ctor = particleSet.Type.GetConstructor(new[] { typeof(vxGameplayScene3D) });
                    var particle = (vxParticle3D)ctor.Invoke(new object[] { this });
                    particle.IsEnabled = false;
                    particlePool.Pool.Add(particle);
                }
                ParticleSystem.AddPool(particlePool);
            }

            base.LoadParticlePools();
        }


        private void AddItemButtonClicked(object sender, vxUIControlClickEventArgs e)
        {
            m_entitySlideTab.Close();
            OnNewItemAdded(((vxSandboxItemButton)e.GUIitem).Key);
        }

        protected void OnNewItemAdded(string key)
        {
            selcMode.ToggleState = false;
            SandboxEditMode = vxEnumSanboxEditMode.AddItem;

            //First Dispose of the Temp Part
            DisposeOfTempPart();

            //Tell the GUI it doesn't have focus.
            UIManager.HasFocus = false;

            TempPart = AddSandboxItem(key, Matrix.Identity);
        }


        /// <summary>
        /// Adds a sandbox item of type 'T'.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <returns></returns>
        public vxEntity3D AddSandboxItem<T>(Vector3 position) where T : vxEntity3D
        {
            return AddSandboxItem<T>(new vxTransform(position));
        }


        /// <summary>
        /// Adds a sandbox item of type 'T'.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="World"></param>
        /// <returns></returns>
        public vxEntity3D AddSandboxItem<T>(vxTransform World) where T : vxEntity3D
        {
            return AddSandboxItem(typeof(T).ToString(), World);
        }

        public virtual vxEntity3D AddSandboxItem(string key, Matrix matrix4x4Transform)
        {
            return AddSandboxItem(key, new vxTransform(matrix4x4Transform));
        }

        /// <summary>
        /// Adds the sandbox item. Returns the new items id.
        /// </summary>
        /// <returns>The sandbox item.</returns>
        /// <param name="key">Key.</param>
        /// <param name="World">World.</param>
        public virtual vxEntity3D AddSandboxItem(string key, vxTransform World)
        {
            //Set Currently Selected Key
            CurrentlySelectedKey = key;

            //Create the new Entity as per the Key and let the temp_part access it.
            var newEntity = GetNewEntity(key);

            if (newEntity != null)
            {
                // fire the 'OnAdded' function
                if (IsLoadingFile)
                {
                    newEntity.OnAdded();
                }

                newEntity.Transform = World;
            }

            return newEntity;
        }


        /// <summary>
        /// Returns a new instance based off of the returned key. This must be overridden by an inherited class.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual vxEntity3D GetNewEntity(string key)
        {
            // First Check if Registrar has the key
            if (m_registeredSandboxItemTypes.ContainsKey(key))
            {
                try
                {
                    var entity = this.InstantiateItem(m_registeredSandboxItemTypes[key].Type, Vector3.Zero);
                    entity.ItemKey = key;
                    return entity;
                }
                catch(Exception ex)
                {
                    vxConsole.WriteException(key, ex);
                    return null;
                }
            }
            // the passed in key may be a guid
            else if(importedFiles.ContainsKey(key))
            {
                var importedEntity = new vxImportedEntity3D(this);
                importedEntity.InitImportedEntity(key);
                return importedEntity;
            }
            else
            {
                // Older files have the namespace in the key so try and see if this has a period in it.
                int periodIndex = key.LastIndexOf(".");
                if (periodIndex > 0)
                {
                    string oldKey = key.Substring(periodIndex + 1);
                    if (m_registeredSandboxItemTypes.ContainsKey(oldKey))
                    {
                        try
                        {
                            var entity = this.InstantiateItem(m_registeredSandboxItemTypes[oldKey].Type, Vector3.Zero);
                            entity.ItemKey = key;

                            return entity;
                        }
                        catch (Exception ex)
                        {
                            vxConsole.WriteException(key, ex);
                            return null;
                        }
                    }
                    else if (key == typeof(vxImportedEntity3D).ToString())
                    {
                        return new vxImportedEntity3D(this);
                    }
                    else
                    {
                        vxConsole.WriteException(key, new Exception(string.Format("'{0}' Key Not Found!", key)));
                        return null;
                    }
                }
                else
                {
                    vxConsole.WriteException(key, new Exception(string.Format("'{0}' Key Not Found!", key)));
                    return null;
                }
            }
        }

        /// <summary>
        /// Disposes of the Currently Created Temp Part.
        /// </summary>
        public virtual void DisposeOfTempPart()
        {
            if (TempPart != null)
            {
                TempPart.Dispose();
            }

            if (Entities.Contains(TempPart))
                Entities.Remove(TempPart);

            TempPart = null;
        }


        /// <summary>
        /// Generates a sandbox icon for a given entity description
        /// </summary>
        /// <param name="EntityDescription"></param>
        /// <returns></returns>
        protected virtual Texture2D GenerateSandboxItemIcon(vxSandboxEntityRegistrationInfo EntityDescription)
        {
            var Icon = vxInternalAssets.Textures.Blank;

            //return;

            // TODO: Reinstate
            if (File.Exists(Path.Combine(vxEngine.Game.Content.RootDirectory, EntityDescription.FilePath + "_ICON.xnb")))
                Icon = vxEngine.Game.Content.Load<Texture2D>(EntityDescription.FilePath + "_ICON");
            else
            {
                Icon = vxInternalAssets.Textures.Blank;

                RenderTarget2D render = new RenderTarget2D(
                vxGraphics.GraphicsDevice,
                    vxGameplayScene3D.SandboxItemButtonSize, vxGameplayScene3D.SandboxItemButtonSize);

                // Create a new entity
                try
                {

                    System.Reflection.ConstructorInfo ctor = EntityDescription.Type.GetConstructor(new[] { typeof(vxGameplayScene3D), typeof(Vector3) });

                    vxEntity3D entity;

                    // if there isn't this constructor, then there should be one with just the scene
                    if (ctor == null)
                    {
                        ctor = EntityDescription.Type.GetConstructor(new[] { typeof(vxGameplayScene3D) });
                        entity = (vxEntity3D)ctor.Invoke(new object[] { this });
                    }
                    else
                    {
                        entity = (vxEntity3D)ctor.Invoke(new object[] { this, Vector3.Zero });
                    }
                    
                //Icon = RenderEntityIcon(entity, Path.Combine(vxIO.PathToTempFolder, guid, "imported"));
                    //vxEntity3D entity = NewEntityDelegate(Scene);

                    // Get the Bounds so that it'll fit to the screen.
                    float modelRadius = entity.BoundingShape.Radius * 2.0f;

                    if (modelRadius == float.PositiveInfinity)
                        modelRadius = 750;


                    vxGraphics.GraphicsDevice.SetRenderTarget(render);
                    vxGraphics.GraphicsDevice.Clear(Color.DimGray * 0.5f);
                    vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

                    var WorldMatrix = Matrix.CreateTranslation(new Vector3(0, 0, modelRadius));
                    WorldMatrix *= Matrix.CreateFromAxisAngle(Vector3.Right, -MathHelper.PiOver4 * 2 / 3) * Matrix.CreateFromAxisAngle(Vector3.Up, MathHelper.PiOver4);

                    var Projection = Matrix.CreateOrthographic(modelRadius, modelRadius, 0.001f, modelRadius * 2);
                    var View = Matrix.Invert(WorldMatrix);


                    DrawEntity(entity, Matrix.CreateTranslation(-entity.ModelCenter), View, Projection, WorldMatrix.Translation, vxRenderPipeline.Passes.OpaquePass);
                    DrawEntity(entity, Matrix.CreateTranslation(-entity.ModelCenter), View, Projection, WorldMatrix.Translation, vxRenderPipeline.Passes.TransparencyPass);


                    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/MetricIcons";
                    if (Directory.Exists(folderPath) == false)
                        Directory.CreateDirectory(folderPath);

                    string path = folderPath + "/" + EntityDescription.Type.Name + "_ICON.png";
                    Stream streampng = File.OpenWrite(path);
                    render.SaveAsPng(streampng, render.Width, render.Height);
                    streampng.Flush();
                    streampng.Close();
                    streampng.Dispose();
                    Icon = render;
                    vxGraphics.GraphicsDevice.SetRenderTarget(null);
                    entity.Dispose();
                    //render.Dispose();
                    //Thread.Sleep(10);
                    //FileStream filestream = new FileStream(path, FileMode.Open);
                    //this.Icon = Texture2D.FromStream(vxGraphics.GraphicsDevice, filestream);
                }
                catch
                   (Exception ex)
                {
                    vxConsole.WriteException(this, ex);
                }
            }
            return Icon;
        }


        protected virtual Texture2D GenerateImportedItemIcon(string guid)
        {
            var Icon = vxInternalAssets.Textures.DefaultDiffuse;

                // Create a new entity
                try
                {

                var entity = new vxImportedEntity3D(this);
                entity.InitImportedEntity(guid);

                Icon = GenerateEntityIcon(entity, vxIO.PathToImportFolder, guid);
                }
                catch
                   (Exception ex)
                {
                    vxConsole.WriteException(this, ex);
                }
            //}
            return Icon;
        }

        /// <summary>
        /// Generates an Entity Icon and Saves it to desk
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pathToSave"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private Texture2D GenerateEntityIcon(vxEntity3D entity, string pathToSave, string fileName)
        {
            Texture2D icon = vxInternalAssets.Textures.Blank;
            try
            {
                vxConsole.WriteIODebug($"Generating Icon for {entity}");

                RenderTarget2D render = new RenderTarget2D(vxGraphics.GraphicsDevice, SandboxItemButtonSize, SandboxItemButtonSize);

                // Get the Bounds so that it'll fit to the screen.
                float modelRadius = entity.BoundingShape.Radius * 2.0f;

                if (modelRadius == float.PositiveInfinity)
                    modelRadius = 750;


                vxGraphics.GraphicsDevice.SetRenderTarget(render);
                vxGraphics.GraphicsDevice.Clear(new Color(0.15f, 0.15f, 0.15f, 1f));
                vxGraphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

                var WorldMatrix = Matrix.CreateTranslation(new Vector3(0, 0, modelRadius));
                WorldMatrix *= Matrix.CreateFromAxisAngle(Vector3.Right, -MathHelper.PiOver4 * 2 / 3) * Matrix.CreateFromAxisAngle(Vector3.Up, MathHelper.PiOver4);

                var Projection = Matrix.CreateOrthographic(modelRadius, modelRadius, 0.001f, modelRadius * 2);
                var View = Matrix.Invert(WorldMatrix);


                DrawEntity(entity, Matrix.CreateTranslation(-entity.ModelCenter), View, Projection, WorldMatrix.Translation, vxRenderPipeline.Passes.OpaquePass);
                DrawEntity(entity, Matrix.CreateTranslation(-entity.ModelCenter), View, Projection, WorldMatrix.Translation, vxRenderPipeline.Passes.TransparencyPass);

                vxIO.EnsureDirExists(pathToSave);

                string imgFilePath = Path.Combine(pathToSave, fileName + "_icon.png");

                vxConsole.WriteIODebug($"    Saving Icon to {imgFilePath}");

                Stream streampng = File.OpenWrite(imgFilePath);
                render.SaveAsPng(streampng, render.Width, render.Height);
                streampng.Flush();
                streampng.Close();
                streampng.Dispose();
                icon = render;
                vxGraphics.GraphicsDevice.SetRenderTarget(null);
                entity.Dispose();
            }
            catch(Exception ex)
            {
                icon = vxInternalAssets.Textures.DefaultDiffuse;
                vxConsole.WriteException($"Saving Icon For {entity}", ex);
            }

            return icon;
        }

        protected void DrawEntity(vxEntity3D entity, Matrix world, Matrix view, Matrix projection, Vector3 TempCamPos, string renderpass)
        {
            var TempWVP = world * view * projection;
            var worldInvT = Matrix.Transpose(Matrix.Invert(world));

            entity.MeshRenderer.Draw(world, view, projection, TempCamPos, renderpass);

            for (int mi = 0; mi < entity.Model.Meshes.Count; mi++)
            {

                //TODO: Fix
                //if (renderpass == mesh.Material.MaterialRenderPass)
                //{
                //    mesh.Material.World = world;
                //    mesh.Material.WorldInverseTranspose = worldInvT;// Matrix.Transpose(Matrix.Invert(world));
                //    mesh.Material.WVP = TempWVP;// world * view * projection;
                //    mesh.Material.View = view;
                //    mesh.Material.Projection = projection;
                //    mesh.Material.CameraPosition = TempCamPos;
                //    mesh.Material.LightDirection = Vector3.One;
                //    mesh.Draw();
                //}
            }
        }
    }
}