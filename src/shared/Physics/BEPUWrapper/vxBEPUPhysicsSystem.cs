using BEPUphysics;
using BEPUphysicsDrawer.Models;
using BEPUutilities.Threading;
using Microsoft.Xna.Framework;
using System;

namespace VerticesEngine.Physics.BEPU
{
    public class vxBEPUPhysicsSystem : vxIPhysicsLibraryWrapper, vxISubSystem
    {
        /// <summary>
        /// The BEPU wrapper is a scene subsystem.
        /// </summary>
        public SubSystemType Type
        {
            get
            {
                return SubSystemType.Scene;
            }
        }

        public bool IsEnabled { get => _isEnabled; set => _isEnabled = value; }
        private bool _isEnabled = true;

        internal Space PhyicsSimulation;

        private ParallelLooper BEPUParallelLooper;

        internal ModelDrawer PhysicsDebugViewer;


        public vxBEPUPhysicsSystem(bool isMultiThreaded=true)
        {
            int threadCount = 1;
            // Next Initialise the Physics Engine
            if (isMultiThreaded && Environment.ProcessorCount > 1)
            {
                //Starts BEPU with multiple Cores
                BEPUParallelLooper = new ParallelLooper();

                for (int i = 0; i < Environment.ProcessorCount; i++)
                {
                    BEPUParallelLooper.AddThread();
                }

                PhyicsSimulation = new Space(BEPUParallelLooper);
                threadCount = PhyicsSimulation.ParallelLooper.ThreadCount;
            }
            else
            {
                // Start BEPU without Multi Threading
                PhyicsSimulation = new Space();
            }

            PhyicsSimulation.Solver.IterationLimit = 100;
            PhyicsSimulation.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);

            vxDebug.LogEngine(new
            {
                //type = this.GetType() + ".InitialisePhysics()",
                isMultiThreaded = PhyicsSimulation.ForceUpdater.AllowMultithreading,
                threadCount = threadCount,
                iterationLimit = PhyicsSimulation.Solver.IterationLimit,
                gravity = PhyicsSimulation.ForceUpdater.Gravity,

            });

            PhysicsDebugViewer = new ModelDrawer(vxEngine.Game);
        }

        public void Dispose()
        {
            PhysicsDebugViewer.Clear();
            BEPUParallelLooper.Dispose();

            PhyicsSimulation = null;
            PhysicsDebugViewer = null;
        }

        public void Initialise()
        {

        }

        public void Update()
        {

        }

        public bool RayCast(Vector3 source, Vector3 direction)
        {
            return true;
        }
    }
}
