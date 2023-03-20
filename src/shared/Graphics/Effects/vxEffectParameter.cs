using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// Effect parameter.
    /// </summary>
    public class vxEffectParameter
    {
        EffectParameter Parameter;

        /// <summary>
        /// The max.
        /// </summary>
        public float Max;

        /// <summary>
        /// The minimum.
        /// </summary>
        public float Min;


		public vxEffectParameter(EffectParameter Parameter):this(Parameter, 0, 1)
		{
			
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Graphics.vxEffectParameter"/> class.
        /// </summary>
        /// <param name="Parameter">Parameter.</param>
        /// <param name="Min">Minimum.</param>
        /// <param name="Max">Max.</param>
        public vxEffectParameter(EffectParameter Parameter, float Min = 0, float Max = 1)
        {
            this.Parameter = Parameter;
            this.Min = Min;
            this.Max = Max;
        }

        //
        // Properties
        //
        public EffectAnnotationCollection Annotations
        {
            get
            {
                return Parameter.Annotations;
            }
        }

        public int ColumnCount
        {
            get
            {
                return Parameter.ColumnCount;
            }
        }

        public EffectParameterCollection Elements
        {
            get
            {
                return Parameter.Elements;
            }
        }

        public string Name
        {
            get
            {
                return Parameter.Name;
            }
        }

        public EffectParameterClass ParameterClass
        {
            get
            {
                return Parameter.ParameterClass;
            }
        }

        public EffectParameterType ParameterType
        {
            get
            {
                return Parameter.ParameterType;
            }
        }

        public int RowCount
        {
            get
            {
                return Parameter.RowCount;
            }
        }

        public string Semantic
        {
            get
            {
                return Parameter.Semantic;
            }
        }


        public EffectParameterCollection StructureMembers
        {
            get
            {
                return Parameter.StructureMembers;
            }
        }

        //
        // Methods
        //
        public bool GetValueBoolean()
        {
            return Parameter.GetValueBoolean();
        }

        public int GetValueInt32()
        {
            return Parameter.GetValueInt32();
        }

        public Matrix GetValueMatrix()
        {
            return Parameter.GetValueMatrix();
        }

        public Matrix[] GetValueMatrixArray(int count)
        {
            return Parameter.GetValueMatrixArray(count);
        }

        public Quaternion GetValueQuaternion()
        {
            return Parameter.GetValueQuaternion();
        }

        public float GetValueSingle()
        {
            return Parameter.GetValueSingle();
        }

        public float[] GetValueSingleArray()
        {
            return Parameter.GetValueSingleArray();
        }

        public string GetValueString()
        {
            return Parameter.GetValueString();
        }

        public Texture2D GetValueTexture2D()
        {
            return Parameter.GetValueTexture2D();
        }

        //public Texture3D GetValueTexture3D()
        //{
        //    return Parameter.GetValueTexture3D();
        //}

        public TextureCube GetValueTextureCube()
        {
            return Parameter.GetValueTextureCube();
        }

        public Vector2 GetValueVector2()
        {
            return Parameter.GetValueVector2();
        }

        public Vector2[] GetValueVector2Array()
        {
            return Parameter.GetValueVector2Array();
        }

        public Vector3 GetValueVector3()
        {
            return Parameter.GetValueVector3();
        }

        public Vector3[] GetValueVector3Array()
        {
            return Parameter.GetValueVector3Array();
        }

        public Vector4 GetValueVector4()
        {
            return Parameter.GetValueVector4();
        }

        public Vector4[] GetValueVector4Array()
        {
            return Parameter.GetValueVector4Array();
        }

        public void SetValue(Vector3[] value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(Vector3 value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(Vector2[] value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(Vector2 value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(Texture value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(float[] value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(Vector4[] value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(Quaternion value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(Matrix[] value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(Vector4 value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(Matrix value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(int value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(bool value)
        {
            Parameter.SetValue(value);
        }

        public void SetValue(float value)
        {
            Parameter.SetValue(value);
        }

        public void SetValueTranspose(Matrix value)
        {
            Parameter.SetValueTranspose(value);
        }
    }
}
