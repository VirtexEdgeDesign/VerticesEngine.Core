using System;
using Microsoft.Xna.Framework.Graphics;
using VerticesEngine.Utilities;

namespace VerticesEngine.Graphics
{
    /// <summary>
    /// A shader is a class which is used to draw an object. These essentially consuming and extend the previous 'Effect' class from
    /// XNA and MonoGame
    /// </summary>
	public class vxShader : Effect
	{
        
		public vxShader(Effect effect) : base(effect)
		{
                
		}


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public static void PrintParameterValues(Effect effect)
		{
            vxConsole.WriteToScreen(effect.Name, "---------------------------------------------------------------------------------");
			foreach (EffectParameter para in effect.Parameters)
                vxConsole.WriteToScreen(effect.Name, para.Name + ": "+ para.ParameterType + ": " +  GetParameterValue(para));
            vxConsole.WriteToScreen(effect.Name, "---------------------------------------------------------------------------------");
		}

		public static object GetParameterValue(EffectParameter Parameter)
		{
            object toReturn = null;

			// First, Parse which Parameter Class it is
			switch (Parameter.ParameterClass)
			{
				// Some form of Vector (2D, 3D or 4D) or Vector Array.
				case EffectParameterClass.Vector:

					// Parse out which type of vector it is and whether it's an array or not
					switch (Parameter.ColumnCount)
					{
						// Vector2D
						case 2:
							if (Parameter.Elements.Count == 0)
                                toReturn = Parameter.GetValueVector2();
							//else
							//	return Parameter.GetValueVector2Array(Parameter.Elements.Count);
							break;


						// Vector3D
						case 3:
							if (Parameter.Elements.Count == 0)
                                toReturn = Parameter.GetValueVector3();
							//else
							//	return Parameter.GetValueVector3Array(Parameter.Elements.Count);
							break;

						// Vector4D
						case 4:
							if (Parameter.Elements.Count == 0)
                                toReturn = Parameter.GetValueVector4();
							//else
							//	return Parameter.GetValueVector4Array(Parameter.Elements.Count);
							break;
					}

					break;

				// Get a Matrix
				case EffectParameterClass.Matrix:
					if (Parameter.Elements.Count == 0)
                        toReturn = Parameter.GetValueMatrix();
					else
                        toReturn = Parameter.GetValueMatrixArray(Parameter.Elements.Count);
					break;

				// Get the Scalar/Float value.
				case EffectParameterClass.Scalar:

					switch (Parameter.ParameterType)
					{
						case EffectParameterType.Single:

					if (Parameter.Elements.Count == 0)
                                toReturn = Parameter.GetValueSingle();
                            //else
                              //  toReturn = Parameter.GetValueSingle();
                            //return Parameter.GetValueSingleArray(Parameter.Elements.Count);
                            break;

						case EffectParameterType.Bool:

                            toReturn = Parameter.GetValueBoolean();

							break;



			}
					break;

				case EffectParameterClass.Object:
					switch (Parameter.ParameterType)
                    {
                        //case EffectParameterType.Texture:
                        //    TextureCube tc = Parameter.GetValueTextureCube();
                        //    if (tc == null)
                        //        return "NULL";
                        //    return tc;
                        //    break;
                        case EffectParameterType.Texture2D:
                            toReturn = Parameter.GetValueTexture2D();
							break;
					}
					break;

				default:
                    toReturn = "<Parameter TYPE Not Implemented>";
					break;
			}

            if(toReturn == null)
            toReturn = string.Format("<Parameter Class Not Implemented> Class: {0} Type: {1}", 
                Parameter.ParameterClass, Parameter.ParameterType);
            //vxConsole.WriteToInGameDebug(para.Name + " : " + para.ParameterClass + " : " + para.RowCount);
            return toReturn;


        }
	}
}
