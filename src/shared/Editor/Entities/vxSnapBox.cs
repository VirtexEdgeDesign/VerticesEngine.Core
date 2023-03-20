
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework;
using VerticesEngine.Graphics;

namespace VerticesEngine.Util
{
    public interface ISnapbox
    {
        vxTransform Transform { get; set; }
    }

    public class vxSnapBox : vxEntity3D, ISnapbox
    {
		Entity PhysicsBody;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Entities.Util.vxSnapBox"/> class.
		/// </summary>
		/// <param name="Engine">Engine.</param>
		/// <param name="SnapBoxModel">Snap box model.</param>
		/// <param name="Width">Width.</param>
		/// <param name="Height">Height.</param>
		/// <param name="Length">Length.</param>
        public vxSnapBox(vxGameplayScene3D scene, vxMesh SnapBoxModel, int Width, int Height, int Length)
            : base(scene, SnapBoxModel, Vector3.Zero)
        {
            //EndLocalRotation = new Vector3(MathHelper.PiOver2, -MathHelper.PiOver4, MathHelper.PiOver4);
            //DoShadowMapping = false;
            PhysicsBody = new Box(Vector3.Zero, Width, Height, Length);
            //TODO: Update to use physics components

			Scene.PhyicsSimulation.Add(PhysicsBody);
			PhysicsBody.CollisionInformation.CollisionRules.Personal = BEPUphysics.CollisionRuleManagement.CollisionRule.NoSolver;
			PhysicsBody.CollisionInformation.Tag = this;

            RemoveSandboxOption(SandboxOptions.Save);
            RemoveSandboxOption(SandboxOptions.Export);
            RemoveSandboxOption(SandboxOptions.Delete);
        }



        protected override void OnWorldTransformChanged()
        {
            base.OnWorldTransformChanged();
            PhysicsBody.WorldTransform = Transform.Matrix4x4Transform;
        }
    }
}