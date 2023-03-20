using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Input;
using VerticesEngine;

namespace VerticesEngine
{
    /// <summary>
    /// Handles Camera FPS Controls
    /// </summary>
    public class vxCameraChaseController : vxComponent
    {
        private vxCamera3D m_camera;

        #region Chase Camera Code

        #region Chased object properties (set externally each frame)

        /// <summary>
        /// Position of object being chased.
        /// </summary>
        public Vector3 ChasePosition
        {
            get { return m_chasePosition; }
            set { m_chasePosition = value; }
        }
        private Vector3 m_chasePosition;

        /// <summary>
        /// Direction the chased object is facing.
        /// </summary>
        public Vector3 ChaseDirection
        {
            get { return m_chaseDirection; }
            set { m_chaseDirection = value; }
        }
        private Vector3 m_chaseDirection;

        /// <summary>
        /// Chased object's Up vector.
        /// </summary>
        public Vector3 Up
        {
            get { return m_up; }
            set { m_up = value; }
        }
        private Vector3 m_up = Vector3.Up;

        #endregion

        #region Desired camera positioning (set when creating camera or changing view)

        /// <summary>
        /// Desired camera position in the chased object's coordinate system.
        /// </summary>
        public Vector3 DesiredPositionOffset
        {
            get { return m_desiredPositionOffset; }
            set { m_desiredPositionOffset = value; }
        }
        private Vector3 m_desiredPositionOffset = new Vector3(0, 2.0f, 2.0f);

        /// <summary>
        /// Desired camera position in world space.
        /// </summary>
        public Vector3 DesiredPosition
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return m_desiredPosition;
            }
        }
        private Vector3 m_desiredPosition;

        /// <summary>
        /// Look at point in the chased object's coordinate system.
        /// </summary>
        public Vector3 LookAtOffset
        {
            get { return m_lookAtOffset; }
            set { m_lookAtOffset = value; }
        }
        private Vector3 m_lookAtOffset = new Vector3(0, 2.8f, 0);

        /// <summary>
        /// Look at point in world space.
        /// </summary>
        public Vector3 LookAt
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return m_lookAt;
            }
        }
        private Vector3 m_lookAt;

        #endregion

        #region Camera physics (typically set when creating camera)

        /// <summary>
        /// Physics coefficient which controls the influence of the camera's position
        /// over the spring force. The stiffer the spring, the closer it will stay to
        /// the chased object.
        /// </summary>
        public float Stiffness
        {
            get { return m_stiffness; }
            set { m_stiffness = value; }
        }
        private float m_stiffness = 45000.0f;

        /// <summary>
        /// Physics coefficient which approximates internal friction of the spring.
        /// Sufficient damping will prevent the spring from oscillating infinitely.
        /// </summary>
        public float Damping
        {
            get { return m_damping; }
            set { m_damping = value; }
        }
        private float m_damping = 35000.0f;

        /// <summary>
        /// Mass of the camera body. Heaver objects require stiffer springs with less
        /// damping to move at the same rate as lighter objects.
        /// </summary>
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        private float mass = 1000.0f;

        #endregion

        #endregion


        protected override void Initialise()
        {
            base.Initialise();

            m_camera = (vxCamera3D)Entity;

            Reset();
        }


        protected internal override void Update()
        {
            if (vxEngine.Instance.CurrentScene != null && vxEngine.Instance.CurrentScene.IsActive)
            {
                UpdateWorldPositions();

                // Calculate spring force
                Vector3 stretch = m_camera.Position - m_desiredPosition;

                float maxD = 0.25f;
                if (stretch.Length() > maxD)
                {
                    stretch.Normalize();
                    m_camera.Position = m_desiredPosition + stretch * maxD;
                }
                stretch = m_camera.Position - m_desiredPosition;

                Vector3 force = -m_stiffness * stretch - m_damping * m_camera.Velocity;

                // Apply acceleration
                Vector3 acceleration = force / mass;

                m_camera.Velocity += acceleration * vxTime.DeltaTime;

                // Apply velocity
                m_camera.Position += m_camera.Velocity * vxTime.DeltaTime;

                //Camera.WorldMatrix = Matrix.CreateWorld(Camera.Position, Camera.View.Forward, Camera.View.Up);

                UpdateMatrices();

                m_camera.WorldMatrix = Matrix.Invert(m_camera.View);
            }
        }


        /// <summary>
        /// Forces camera to be at desired position and to stop moving. The is useful
        /// when the chased object is first created or after it has been teleported.
        /// Failing to call this after a large change to the chased object's position
        /// will result in the camera quickly flying across the world.
        /// </summary>
        public void Reset()
        {

            UpdateWorldPositions();

            // Stop motion
            m_camera.Velocity = Vector3.Zero;

            // Force desired position
            m_camera.Position = m_desiredPosition;

            UpdateMatrices();

            Update();
        }


        /// <summary>
        /// Rebuilds camera's view and projection matricies.
        /// </summary>
        private void UpdateMatrices()
        {
            try
            {
                m_camera.View = Matrix.CreateLookAt(m_camera.Position, LookAt, Up);
                m_camera.Projection = Matrix.CreatePerspectiveFieldOfView(m_camera.FieldOfView,
                        m_camera.AspectRatio, m_camera.NearPlane, m_camera.FarPlane);
            }
            catch { }

        }

        public Vector3 ExtraOffset = Vector3.Zero;

        /// <summary>
        /// Rebuilds object space values in world space. Invoke before publicly
        /// returning or privately accessing world space values.
        /// </summary>
        private void UpdateWorldPositions()
        {
            // Construct a matrix to transform from object space to worldspace
            Matrix transform = Matrix.Identity;
            transform.Forward = ChaseDirection;
            transform.Up = Up;
            transform.Right = Vector3.Cross(Up, ChaseDirection);

            // Calculate desired camera properties in world space
            m_desiredPosition = ChasePosition +
                Vector3.TransformNormal(DesiredPositionOffset, transform);
            m_lookAt = ChasePosition +
                Vector3.TransformNormal(LookAtOffset + ExtraOffset, transform);
        }

    }
}
