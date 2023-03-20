using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace VerticesEngine.Physics.Entities
{
    internal class vxPhysics2DFarseerBaseEntity : vxPhysics2DEntity
	{
        internal Body farseerCollider;

        public Vector2 Position 
        {
            get { return ConvertUnits.ToDisplayUnits(farseerCollider.Position); }
            set { farseerCollider.Position = ConvertUnits.ToSimUnits(value); } 
        }

        public float Mass
        {
            get { return farseerCollider.Mass; }
            set { farseerCollider.Mass = value; }
        }



        public float Rotation 
        {
            get { return farseerCollider.Rotation; }
            set { farseerCollider.Rotation = value; }
        }

        public bool IsFixedRotation 
        {
            get { return farseerCollider.FixedRotation; }
            set { farseerCollider.FixedRotation = value; }
        }

        public vxPhysicsBodyType PhysicsBodyType
        {
            get
            {
                vxPhysicsBodyType bodyType = vxPhysicsBodyType.Static;
                switch (farseerCollider.BodyType)
                {
                    case BodyType.Static:
                        bodyType= vxPhysicsBodyType.Static;
                        break;
                    case BodyType.Dynamic:
                        bodyType = vxPhysicsBodyType.Dynamic;
                        break;
                    default:
                        bodyType = vxPhysicsBodyType.Kinematic;
                        break;
                }
                return bodyType;
            }
            set
            {
                switch (value)
                {
                    case vxPhysicsBodyType.Static:
                        farseerCollider.BodyType = BodyType.Static;
                        break;
                    case vxPhysicsBodyType.Dynamic:
                        farseerCollider.BodyType = BodyType.Dynamic;
                        break;
                    default:
                        farseerCollider.BodyType = BodyType.Kinematic;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public vxEntity2D Entity { get { return _parent; } }
        vxEntity2D _parent;

        public void AttachEntity(vxEntity2D entity)
        {
            _parent = entity;
        }

        public vxPhysics2DFarseerBaseEntity(Body collider)
		{
            farseerCollider = collider;
            farseerCollider.UserData = this;
		}
	}





    internal class vxPhysics2DFarseerCircleCollider : vxPhysics2DFarseerBaseEntity, vxPhysics2DCircleCollider
    {
        public float Radius { get { return _radius; } }
        float _radius;

        public vxPhysics2DFarseerCircleCollider(World FarseerSim, Vector2 position, float radius, float density)
            : base(BodyFactory.CreateCircle(FarseerSim, 
                                            ConvertUnits.ToSimUnits(radius), 
                                                                     density, ConvertUnits.ToSimUnits(position)))
        {
            _radius = radius;
        }
    }






    internal class vxPhysics2DFarseerRectCollider : vxPhysics2DFarseerBaseEntity, vxPhysics2DRectCollider
    {
        public vxPhysics2DFarseerRectCollider(World FarseerSim, Vector2 position, float width, float height, float density)
            : base(BodyFactory.CreateRectangle(FarseerSim,
                                               ConvertUnits.ToSimUnits(width), ConvertUnits.ToSimUnits(height),
                                               density, ConvertUnits.ToSimUnits(position)))
        {
            
        }
    }
}
