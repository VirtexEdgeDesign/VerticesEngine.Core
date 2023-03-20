using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Physics
{
    public enum PhysicsEngineType
    {
        /// <summary>
        /// A third party physics library used for 2D Games, it supports debug visulalizations
        /// </summary>
        Farseer,

        /// <summary>
        /// A third party physics library used for 3D Games, it supports debug visualization and multithreading.
        /// </summary>
        BEPU,
    }

    

    /// <summary>
    /// This is the physics manager which holds settings and references to the different physics solver backends
    /// </summary>
    public class vxPhysicsSystem

    {
        #region -- Singleton Declaration --

        public static vxPhysicsSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new vxPhysicsSystem();
                }

                return _instance;
            }
        }

        private static vxPhysicsSystem _instance;

        #endregion



        private vxPhysicsSystem()
        {

        }

        /// <summary>
        /// The number of steps to take per frame
        /// </summary>
        [vxEngineSettingsAttribute("StepsPerFrame")]
        public static int StepsPerFrame = 3;

        #region -- Public Properties --

        /// <summary>
        /// Get's the current physics engine.
        /// </summary>
        public PhysicsEngineType PhysicsEngine
        {
            get { return _physicsEngine; }
        }
        private PhysicsEngineType _physicsEngine;

        /// <summary>
        /// Should the Physics Engine be multithreaded, if available.
        /// </summary>
        public bool IsMultiThreaded
        {
            get { return _isMultiThreaded; }
        }
        private bool _isMultiThreaded = true;

        /// <summary>
        /// Does this engine support multithreading
        /// </summary>
        public bool IsMultiThreadingSupported
        {
            get { return _isMultiThreadingSupported; }
        }
        private bool _isMultiThreadingSupported = true;

        #endregion

        /// <summary>
        /// Set's the current physics engine
        /// </summary>
        /// <param name="physicsEngine"></param>
        public void SetPhysicsEngine(PhysicsEngineType physicsEngine)
        {
            _physicsEngine = physicsEngine;

            switch (physicsEngine)
            {
                case PhysicsEngineType.Farseer:
                    _isMultiThreaded = false;
                    _isMultiThreadingSupported = false;
                    break;
                case PhysicsEngineType.BEPU:
                    _isMultiThreadingSupported = true;
                    _isMultiThreaded = true;
                    break;
            }
        }
    }
}
