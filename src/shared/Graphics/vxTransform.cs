using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace VerticesEngine
{
    /// <summary>
    /// Data needed for this transform for the render pass. This usually holds calculated matrices such as WVP, INV_W etc... so that they're
    /// only calculated once per-entity per render loop
    /// </summary>
    public class RenderPassTransformData
    {
        public Matrix World = Matrix.Identity;
        public Matrix WVP = Matrix.Identity;
        public Matrix WorldInvT = Matrix.Identity;
        public Vector3 CameraPos = Vector3.Zero;
        public bool IsShadowCaster = true;
        public Color IndexColour = Color.Transparent;
    }

    public static class vxExtentions
    {
        public static vxTransform ToTransform(this Matrix matrix)
        {
            return new vxTransform(matrix);
        }

        /// <summary>
        /// Converts a given quaternion to Eurler angles, in Degrees
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public static Vector3 ToEulerAngles(this Quaternion q)
        {
            Matrix matrix = Matrix.CreateFromQuaternion(q);

            float yaw = (float)System.Math.Atan2(matrix.M13, matrix.M33);
            float pitch = (float)System.Math.Asin(-matrix.M23);
            float roll = (float)System.Math.Atan2(matrix.M21, matrix.M22);

            return -1 * new Vector3(MathHelper.ToDegrees((float)roll), MathHelper.ToDegrees((float)yaw), MathHelper.ToDegrees((float)pitch));
        }
    }


    /// <summary>
    /// This is the main transform class which houses an entities scale, position and orientations
    /// </summary>
    public class vxTransform
    {
        /// <summary>
        /// The scale to apply to this transform
        /// </summary>
        public Vector3 Scale
        {
            get { return m_scale; }
            set
            {
                m_scale = value;
                CalcWorldTransform();
            }
        }
        private Vector3 m_scale = Vector3.One;


        /// <summary>
        /// The position of this transform in 3D space
        /// </summary>
        public Vector3 Position
        {
            get { return m_position; }
            set
            {
                m_position = value;
                CalcWorldTransform();
            }
        }
        private Vector3 m_position = Vector3.Zero;

        #region -- Rotation --

        /// <summary>
        /// The Rotation Quaternion
        /// </summary>
        public Quaternion Rotation
        {
            get { return m_rotation; }
            set
            {
                m_rotation = value;
                CalcWorldTransform();
            }
        }
        private Quaternion m_rotation = Quaternion.Identity;

        public Vector3 Forward
        {
            get {
                var vec = m_4x4Matrix.Forward;
                vec.Normalize();
                return vec; 
            }
        }
        public Vector3 Backward
        {
            get
            {
                var vec = m_4x4Matrix.Backward;
                vec.Normalize();
                return vec;
            }
        }

        public Vector3 Up
        {
            get
            {
                var vec = m_4x4Matrix.Up;
                vec.Normalize();
                return vec;
            }
        }
        public Vector3 Down
        {
            get
            {
                var vec = m_4x4Matrix.Down;
                vec.Normalize();
                return vec;
            }
        }

        public Vector3 Right
        {
            get
            {
                var vec = m_4x4Matrix.Right;
                vec.Normalize();
                return vec;
            }
        }

        public Vector3 Left
        {
            get
            {
                var vec = m_4x4Matrix.Left;
                vec.Normalize();
                return vec;
            }
        }


        #endregion

        public vxTransform() : this(Vector3.Zero, Quaternion.Identity) { }
        public vxTransform(Vector3 position) : this(position, Quaternion.Identity) { }
        public vxTransform(Vector3 position, Quaternion rotation) : this(position, rotation, Vector3.One) { }
        public vxTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            m_position = position;
            m_rotation = rotation;
            m_scale = scale;
        }

        public vxTransform(Matrix matrix4x4Transform)
        {
            m_position = matrix4x4Transform.Translation;

            var m = matrix4x4Transform;
            var sclx = Math.Sqrt(m[0] * m[0] + m[1] * m[1] + m[2] * m[2]);
            var scly = Math.Sqrt(m[4] * m[4] + m[5] * m[5] + m[6] * m[6]);
            var sclz = Math.Sqrt(m[8] * m[8] + m[9] * m[9] + m[10] * m[10]);
            m_scale = new Vector3((float)sclx, (float)scly, (float)sclz);

            // ensure scale is okay
            if (m_scale == Vector3.Zero)
                m_scale = Vector3.One;

            // now remove the scale from the rotation
            matrix4x4Transform[0] /= m_scale.X;
            matrix4x4Transform[1] /= m_scale.X;
            matrix4x4Transform[2] /= m_scale.X;

            matrix4x4Transform[4] /= m_scale.Y;
            matrix4x4Transform[5] /= m_scale.Y;
            matrix4x4Transform[6] /= m_scale.Y;

            matrix4x4Transform[8] /= m_scale.Z;
            matrix4x4Transform[9] /= m_scale.Z;
            matrix4x4Transform[10] /= m_scale.Z;

            m_rotation = Quaternion.CreateFromRotationMatrix(matrix4x4Transform);

        }

        /// <summary>
        /// The 4x4 world matrix which makes up the scale, rotation and position as a single matrix. This is 
        /// usually passed through to the shaders as the model or world matrix
        /// </summary>
        public Matrix Matrix4x4Transform
        {
            get
            {
                if (m_isTransformDirty)
                    CalcWorldTransform();

                return m_4x4Matrix;
            }
        }
        private Matrix m_4x4Matrix = Matrix.Identity;


        public RenderPassTransformData RenderPassData
        {
            get { return m_renderPassTransformData; }
        }
        private RenderPassTransformData m_renderPassTransformData = new RenderPassTransformData();

        /// <summary>
        /// Has a dependant variable such as position or scale been changed that requires the world transform be recalculated?
        /// </summary>
        private bool m_isTransformDirty = true;

        /// <summary>
        /// calucaltes the world transform matrix
        /// </summary>
        private void CalcWorldTransform()
        {
            m_4x4Matrix = 
                Matrix.CreateScale(m_scale) * 
                Matrix.CreateFromQuaternion(m_rotation) * 
                Matrix.CreateTranslation(m_position);

            if (OnTransformUpdated != null)
            {
                OnTransformUpdated();
            }
            m_isTransformDirty = false;
        }

        /// <summary>
        /// Sets the position and rotation without firing off a transform updated event for each
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public void SetTransform(Vector3 position, Quaternion rotation)
        {
            this.m_position = position;
            this.m_rotation = rotation;
            CalcWorldTransform();
        }

        public Action OnTransformUpdated = () => { };

        /// <summary>
        /// Rotates the transform by a given euler angle (in degrees) about x, y and z
        /// </summary>
        /// <param name="x">The amount of roll</param>
        /// <param name="y">The amount of yaw</param>
        /// <param name="z">The amount of pitch</param>
        public void Rotate(float x, float y, float z)
        {            
            Rotation *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(y), MathHelper.ToRadians(z), MathHelper.ToRadians(x));
        }

        /// <summary>
        /// Rotates the transform by a given euler angle (in degrees) about x, y and z
        /// </summary>
        /// <param name="eulerAngles">X is roll, Y is yaw and Z it pitch</param>
        public void Rotate(Vector3 eulerAngles)
        {
            Rotation *= Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(eulerAngles.Y), MathHelper.ToRadians(eulerAngles.Z), MathHelper.ToRadians(eulerAngles.X));
        }

        /// <summary>
        /// Rotates the transform about the given axis by the specified number of degrees
        /// </summary>
        /// <param name="axis">The axis to rotate about</param>
        /// <param name="angle">The number of Degrees to rotate by</param>
        public void Rotate(Vector3 axis, float angle)
        {
            Rotation *= Quaternion.CreateFromAxisAngle(axis, MathHelper.ToRadians(angle));
        }

        public vxTransform ToCopy()
        {
            return new vxTransform(m_position, m_rotation, m_scale);
        }

        /// <summary>
        /// Returns back a transform as the identity matrix
        /// </summary>
        public static vxTransform Identity
        {
            get
            {
                return m_identity;
            }
        }
        private static vxTransform m_identity = new vxTransform()
        {
            m_scale = Vector3.One,
            m_rotation = Quaternion.Identity,
            m_position = Vector3.Zero
        }; 
    }
}
