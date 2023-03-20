using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace VerticesEngine.Physics.Entities
{
    public enum vxPhysicsBodyType
    {
        Static,
        Dynamic,
        Kinematic
    }
	public interface vxPhysics2DEntity
    {
        vxEntity2D Entity { get; }

        void AttachEntity(vxEntity2D entity);

        Vector2 Position { get; set; }

        float Mass { get; set; }

        float Rotation { get; set; }

        bool IsFixedRotation { get; set; }

        vxPhysicsBodyType PhysicsBodyType { get; set; }
    }

    public interface vxPhysics2DCircleCollider
    {
        float Radius { get; }
    }

    public interface vxPhysics2DRectCollider
    {
        
    }

}
