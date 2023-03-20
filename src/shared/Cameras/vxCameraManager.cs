using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace VerticesEngine
{
    public class vxCameraManager : vxGameObject, IEnumerable<vxCamera>
    {
        public List<vxCamera> Cameras { get; private set; }

        public vxCameraManager(int capacity = 4) : base()
        {
            Cameras = new List<vxCamera>(capacity);
        }



        public override void OnGraphicsRefresh()
        {
            base.OnGraphicsRefresh();

            foreach (var camera in Cameras)
            {
                camera.OnGraphicsRefresh();
            }
        }

        public void Add(vxCamera camera)
        {
            Cameras.Add(camera);
        }

        /// <summary>
        /// Renders all cameras
        /// </summary>
        public void RenderAllCameras()
        {
            foreach(var camera in Cameras)
            {
                camera.Render();
            }
        }

        public IEnumerator<vxCamera> GetEnumerator()
        {
            return Cameras.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
