using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerticesEngine.Graphics
{
    public interface vxIRenderPass
    {
        bool IsDisposed { get; }

        void OnGraphicsRefresh();

        void LoadContent();

        void RegisterRenderTargetsForDebug();

        void Prepare(vxCamera camera);

        void Apply(vxCamera camera);

        void Dispose();
    }
}
