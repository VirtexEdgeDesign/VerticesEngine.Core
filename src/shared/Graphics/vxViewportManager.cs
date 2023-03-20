using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Viewport manager which really only reports different facts about Cameras and Viewports being used.
    /// It also draws the Split Screen borders, so if you want a different look, then override this class.
    /// </summary>
    public class vxViewportManager : IDisposable
	{
		/// <summary>
		/// The main viewport.
		/// </summary>
		public Viewport MainViewport;


		/// <summary>
		/// Gets the number of viewports.
		/// </summary>
		/// <value>The number of viewports.</value>
		public int NumberOfViewports
		{
            get { return Scene.Cameras.Count; }
		}


		/// <summary>
		/// The viewports.
		/// </summary>
		public readonly List<Viewport> Viewports;

        vxGameplaySceneBase Scene;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Graphics.vxViewportManager"/> class.
		/// </summary>
        /// <param name="Scene">Scene.</param>
		public vxViewportManager(vxGameplaySceneBase Scene)
		{
            this.Scene = Scene;

			MainViewport = vxGraphics.GraphicsDevice.Viewport;

			Viewports = new List<Viewport>();

            foreach (vxCamera camera in Scene.Cameras)
				Viewports.Add(camera.Viewport);
		}

        public void ResetMainViewport()
        {
            MainViewport = vxGraphics.GraphicsDevice.Viewport;
        }

		/// <summary>
		/// Resets the viewport to Fullscreen.
		/// </summary>
		public void ResetViewport()
		{
			// Reset the Viewport
			vxGraphics.GraphicsDevice.Viewport = MainViewport;
		}

        public void Dispose()
        {
            Scene = null;
            Viewports.Clear();
        }
    }
}
