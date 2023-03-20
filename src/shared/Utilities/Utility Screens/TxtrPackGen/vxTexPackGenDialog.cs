using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Threading;
using VerticesEngine.Graphics;
using VerticesEngine.Input;
using VerticesEngine.UI.Events;

namespace VerticesEngine.UI.Dialogs.Utilities
{

    public enum vxEnumTextureQuality
    {
        Ultra,
        High,
        Medium,
        Low
    }


    /// <summary>
    /// The graphic settings dialog.
    /// </summary>
    public class vxTexPackGenDialog : vxDialogBase
	{
		/// <summary>
		/// Has the Generate Button been clicked yet.
		/// </summary>
		bool hasFired = false;

		/// <summary>
		/// The current texture being scaled down.
		/// </summary>
		Texture2D currentTexture;

		/// <summary>
		/// The index of the current scale down.
		/// </summary>
		int scaleIndex = 0;

		/// <summary>
		/// An array of the currently scaled down textures.
		/// </summary>
		Texture2D[] textrs = new Texture2D[4];



		/// <summary>
		/// The texture pack count.
		/// </summary>
		public const int TexturePackCount = 4;

		/// <summary>
		/// The divisors.
		/// </summary>
		int[] divisors = { 1, 2, 4, 8, 16 };

		/// <summary>
		/// The minimum size of any of the textures.
		/// </summary>
		public const int MIN_SIZE = 32;




		// Each loop a different texture will be created. Although this is slower,
		// it allows for debuging.
		int catchIndex = 0;

		//int index = 0;
		string modelName = "";
		string meshName = "";
		string modelPath = "";
		string texturePath = "";

		enum TextureToShrink
		{
			DiffuseMap,
			NormalMap,
			SpecularMap,
			DistortionMap,
			ReflectionMap,
			None,
		};

		TextureToShrink textureToShrink = TextureToShrink.DiffuseMap;
		string RootPathToSave;

		/// <summary>
		/// The Graphics Settings Dialog
		/// </summary>
		public vxTexPackGenDialog()
			: base("TexPack Gen. v 0.2 - Vertices Engine Texture Pack Generator", vxEnumButtonTypes.Ok)
		{
			currentTexture = vxInternalAssets.Textures.Blank;
		}

		StreamWriter writer;

		/// <summary>
		/// Load graphics content for the screen.
		/// </summary>
		public override void LoadContent()
		{
			base.LoadContent();

			RootPathToSave = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + 
			                            "/vxTexPackGen/" + vxEngine.Game.Name +"/";



			//Btn_Apply.Clicked += Btn_Apply_Clicked;
			//OKButton.Clicked += OnOKButtonClickedCancelButtonancel.Clicked += OnCancelButtonClicked;
		}

		protected internal override void Update()
		{
			base.Update();

			if (vxInput.IsNewKeyPress(Keys.Enter) && hasFired == false)
			{
				hasFired = true;
				catchIndex = 0;
				writer = new StreamWriter(RootPathToSave+"Content.txt");
				writer.WriteLine("#---------------------------------- Vertices Content ---------------------------------#");
			}
		}


        /// <inheritdoc/>
        protected override void OnOKButtonClicked(object sender, vxUIControlClickEventArgs e)
		{
			base.OnOKButtonClicked(sender, e);

			if(writer!= null)
				writer.Close();
		}


		string currentSave = "";

		public override void Draw()
		{
			if (hasFired)
			{
                GeneratePack();
			}

			base.Draw();



            // TODO: Move to it's own executable
			string title = "TexPackGen - v 0.2 - Vertices Engine Texture Pack Generator";
			pos = ArtProvider.GUIBounds.Location.ToVector2();// new Vector2(2);

			vxGraphics.SpriteBatch.Begin("Tex Pack Gen");

			// Draw Text
			DrawLine(title);
			DrawLine("========================================================================");
			DrawLine("Click Enter to Generate Textures");
            DrawLine("");


			if (hasFired)
			{
				DrawLine("Saving files to: "+RootPathToSave);
				
				DrawLine("Current Index = " + catchIndex);
				DrawLine("Model Path: " + modelPath);
				DrawLine("Model Name: " + modelName);
				DrawLine("Mesh Name: " + meshName);
				DrawLine("Relative Texture Path: ./" + texturePath);

				if (currentTexture != null)
				{
					pos.X += 5;
					DrawLine("");
					DrawLine("Texture Name: " + currentTexture.Name);
					DrawLine("-----------------------------------------");
					DrawLine(string.Format("Size:X  {0} x {1}", currentTexture.Width, currentTexture.Height));
					DrawLine(string.Format("Format: {0}", currentTexture.Format));
					DrawLine(string.Format("Tag: {0}", currentTexture.Tag));
					//DrawLine("");
                    DrawLine(string.Format("Saving: {0}", currentSave));
                    DrawLine("");
					vxGraphics.SpriteBatch.Draw(currentTexture,
																new Rectangle((int)pos.X, (int)pos.Y,
																			  256 * currentTexture.Width / currentTexture.Height, 256), Color.White);

					for (int i = 0; i < 4; i++)
					{
						if (textrs[i] != null)
						{
							int offset = 0;
							for (int j = 0; j < i; j++)
							{
								offset += 256 / divisors[j] + 4;
							}


							string size = string.Format("{0}x{1}", textrs[i].Width, textrs[i].Height);
							vxGraphics.SpriteBatch.DrawString(
											vxInternalAssets.Fonts.ViewerFont, size,
															pos + new Vector2(offset, -vxInternalAssets.Fonts.ViewerFont.LineSpacing),
											Color.LightGray);

							vxGraphics.SpriteBatch.Draw(
									textrs[i],
								new Rectangle(
									(int)pos.X + offset,
									(int)pos.Y,
									256 / divisors[i],
									256 / divisors[i]), Color.White);
						}
					}

					if (scaleIndex == 0)
						textureToShrink++;
				}
			}
			vxGraphics.SpriteBatch.End();


		}


		Vector2 pos = new Vector2(2, 2);
		void DrawLine(string text)
		{
			vxGraphics.SpriteBatch.DrawString(
				vxInternalAssets.Fonts.ViewerFont,
				text,
				pos,
				Color.LightGray);
			pos.Y += vxInternalAssets.Fonts.ViewerFont.LineSpacing;
		}

		public void WriteToContentFile(string fileName)
		{
			writer.WriteLine("");
			writer.WriteLine("#begin "+fileName);
			writer.WriteLine("/importer:TextureImporter");
			writer.WriteLine("/processor:TextureProcessor");
			writer.WriteLine("/processorParam:ColorKeyColor=255,0,255,255");
			writer.WriteLine("/processorParam:ColorKeyEnabled=True");
			writer.WriteLine("/processorParam:GenerateMipmaps=True");
			writer.WriteLine("/processorParam:PremultiplyAlpha=True");
			writer.WriteLine("/processorParam:ResizeToPowerOfTwo=False");
			writer.WriteLine("/processorParam:MakeSquare=False");
			writer.WriteLine("/processorParam:TextureFormat=Color");
			writer.WriteLine("/build:"+fileName);
		}

		/// <summary>
		/// Gets the textures from all models loaded.
		/// </summary>
		void GeneratePack()
		{
			//currentTexture = null;
			//index = 0;
			//index = 0;
			hasFired = false;
			//foreach (vxMesh model in Engine.ContentManager.LoadedModels)
			//{
			//	foreach (vxModelMesh mesh in model.Meshes)
			//	{
			//		if (catchIndex == index)
			//		{
			//			if (scaleIndex == 0)
			//			{
			//				// Set Model Info
			//				modelName = model.Name;//.Substring(model.Name.LastIndexOf('/')+1);
			//				modelPath = model.ModelPath.GetParentPathFromFilePath();
			//				meshName = mesh.Name;
			//				texturePath = model.TexturePath;

			//				// Now get the current texture
			//				switch (textureToShrink)
			//				{
			//					case TextureToShrink.DiffuseMap:
			//						currentTexture = mesh.GetTexture(MeshTextureType.Diffuse);
			//						break;
			//					case TextureToShrink.NormalMap:
			//						currentTexture = mesh.GetTexture(MeshTextureType.NormalMap);
			//						break;
			//					case TextureToShrink.SpecularMap:
			//						currentTexture = mesh.GetTexture(MeshTextureType.RMAMap);
			//						break;

			//					default:
			//						currentTexture = null;
			//						textureToShrink = TextureToShrink.DiffuseMap;
			//						catchIndex++;
			//						break;
			//				}
			//			}
			//			//Thread.Sleep(500);

			//			if (currentTexture != null)
			//			{
			//				//string RootPathToSave = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			//				// Basic Picture Path
			//				// PathToModel.GetParentPathFromFilePath() + "/" + TexturePath + mesh.Name + "_dds";

			//				// Save a new version of the texture for each level of quality
			//				//foreach (vxEnumTextureQuality type in Enum.GetValues(typeof(vxEnumTextureQuality)))
			//				{
			//					vxEnumTextureQuality qualityType = (vxEnumTextureQuality)scaleIndex;
			//					Texture2D newText = ScaleDownTexture(currentTexture, divisors[(int)qualityType]);
			//					textrs[(int)qualityType] = newText;
			//					Thread.Sleep(10);

			//					string subscript = "_null";
			//					switch (textureToShrink)
			//					{
			//						case TextureToShrink.DiffuseMap:
			//							subscript = "_dds";
			//							break;
			//						case TextureToShrink.NormalMap:
			//							subscript = "_nm";
			//							break;
			//						case TextureToShrink.SpecularMap:
			//							subscript = "_sm";
			//							break;
			//					}

			//					string path = RootPathToSave + modelPath + "/" + texturePath + qualityType;


			//					if (Directory.Exists(path) == false)
			//						Directory.CreateDirectory(path);

			//					string filename = meshName + subscript + ".png";
			//					currentSave = path + "/" + filename;
			//					SaveTexture(currentSave, newText);

			//					string relFilePath = modelPath + "/" + texturePath + qualityType +"/" + filename;
			//					WriteToContentFile(relFilePath);

			//					scaleIndex++;
			//					scaleIndex = scaleIndex % 4;
			//				}
			//			}

			//			hasFired = true;
			//		}
			//		index++;
			//	}
			//}
		}


		/// <summary>
		/// Scales down texture the texture by the specified divisor.
		/// </summary>
		/// <returns>The down texture.</returns>
		/// <param name="texture">Texture.</param>
		/// <param name="divisor">Divisor.</param>
		public Texture2D ScaleDownTexture(Texture2D texture, int divisor)
		{
			int oldwidth = texture.Width;
			int oldheight = texture.Height;

			int newWidth = Math.Max(oldwidth / Math.Max(divisor, 1), MIN_SIZE);
			int newHeight = Math.Max(oldheight / Math.Max(divisor, 1), MIN_SIZE);

			//Now resize the texture
			return texture.Resize(newWidth, newHeight);
		}


		/// <summary>
		/// Saves the texture.
		/// </summary>
		/// <param name="filename">Filename.</param>
		/// <param name="texture">Texture.</param>
		public void SaveTexture(string filename, Texture2D texture)
		{
			Stream streampng = File.OpenWrite(filename);
			texture.SaveAsPng(streampng, texture.Width, texture.Height);
			streampng.Flush();
			streampng.Close();
			streampng.Dispose();
			Thread.Sleep(10);
		}
	}
}
