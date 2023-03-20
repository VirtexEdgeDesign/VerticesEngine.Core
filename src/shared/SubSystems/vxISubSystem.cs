using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    public enum SubSystemType
    {
        /// <summary>
        /// An engine sub system which lives the entire life of the engine instance
        /// </summary>
        Engine,

        /// <summary>
        /// A level sub system which is only managed and lives throughout the life of a specific level
        /// </summary>
        Scene

    }

    public interface vxISubSystem : IDisposable
    {
        SubSystemType Type { get; }

        /// <summary>
        /// Is this subsystem enabled
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// The system is initialised
        /// </summary>
        void Initialise();

        /// <summary>
        /// Updates this sub system
        /// </summary>
        void Update();
    }

    public interface vxISceneSubSystem : vxISubSystem
    {

    }

    public interface vxIEngineSubSystem : vxISubSystem
    {

    }
}
