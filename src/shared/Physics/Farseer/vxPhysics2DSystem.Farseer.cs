//using System;
//using Microsoft.Xna.Framework;
//using VerticesEngine.Physics.Entities;

//namespace VerticesEngine.Physics
//{
    
//    internal class vxPhysicsSimulation2DFarseer : vxPhysics2DSystem
//    {
//        // Interface Items
//        public Physics2DSolver PhysicsBackend { get { return Physics2DSolver.Farseer; } }


//        // Farseer Code
//        internal static FarseerPhysics.Dynamics.World FarseerSim;



//        public vxPhysicsSimulation2DFarseer(Vector2 gravity)
//        {
//            FarseerSim = new FarseerPhysics.Dynamics.World(gravity);
//        }

//        /// <summary>
//        /// Step the specified elapsedTime.
//        /// </summary>
//        /// <param name="elapsedTime">Elapsed time.</param>
//        public void Step(float elapsedTime)
//        {
//            FarseerSim.Step(elapsedTime);
//        }


//        /// <summary>
//        /// Creates the circle collider.
//        /// </summary>
//        /// <returns>The circle collider.</returns>
//        /// <param name="position">Position.</param>
//        /// <param name="radius">Radius.</param>
//        /// <param name="density">Density.</param>
//        public vxPhysics2DCircleCollider CreateCircleCollider(Vector2 position, float radius, float density)
//        {
//            return new vxPhysics2DFarseerCircleCollider(FarseerSim, position, radius, density);
//        }

//        public vxPhysics2DRectCollider CreateRectCollider(Vector2 position, float width, float height, float density)
//        {
//            return new vxPhysics2DFarseerRectCollider(FarseerSim, position, width, height, density);
//        }
//    }
//}
