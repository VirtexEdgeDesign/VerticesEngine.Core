using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VerticesEngine;
using VerticesEngine.Graphics;
using VerticesEngine.Utilities;

public static partial class vxExtensions
{
    public enum ImageType
    {
        PNG,
        JPG
    }



    /// <summary>
    /// Saves the texture to disk.
    /// </summary>
    /// <param name="texture">Texture.</param>
    /// <param name="FileName">File name.</param>
    public static void SaveToDisk(this Texture2D texture, string FileName, ImageType ImageType = ImageType.PNG)
    {
        // Stream
        System.IO.Stream stream = System.IO.File.Create(FileName);

        // Save the Texture
        if (ImageType == ImageType.PNG)
            texture.SaveAsPng(stream, texture.Width, texture.Height);
        else if (ImageType == ImageType.JPG)
            texture.SaveAsJpeg(stream, texture.Width, texture.Height);


        stream.Flush();
        stream.Close();
        // Dispose
        stream.Dispose();
    }
    /// <summary>
    /// Converts a Texture2D to a byte array
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static byte[] ToByteArray(this Texture2D texture)
    {
        byte[] byteArray = new byte[texture.Width * texture.Height * 4];
        texture.GetData<byte>(byteArray);

        return byteArray;
    }



    /// <summary>
    /// Converts a Texture2D to a Color array.
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static Color[] ToColorArray(this Texture2D texture)
    {
        // the Colour Buffer of the previous Texture
        Color[] colourBuffer = new Color[texture.Width * texture.Height];

        if (texture.Format == SurfaceFormat.Vector4)
        {
            Vector4[] vec4Buffer = new Vector4[texture.Width * texture.Height];
            texture.GetData<Vector4>(vec4Buffer);

            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                {
                    int i = x + y * texture.Width;
                    colourBuffer[i] = vec4Buffer[i].ToColor();
                }
        }
        else
        {
            texture.GetData<Color>(colourBuffer);
        }
        // return the new colour buffer arra
        return colourBuffer;
    }




    /// <summary>
    /// Converts a Stream of bytes into a Texture2D with the spcecified Width and Height
    /// </summary>
    /// <param name="byteArray"></param>
    /// <param name="GraphicsDevice"></param>
    /// <param name="Width"></param>
    /// <param name="Height"></param>
    /// <returns></returns>
    public static Texture2D ToTexture2D(this byte[] byteArray, GraphicsDevice GraphicsDevice, int Width, int Height)
    {
        Texture2D texture = new Texture2D(GraphicsDevice, Math.Max(Width, 1), Math.Max(Height, 1));
        texture.SetData<byte>(byteArray);

        return texture;
    }


    /// <summary>
    /// Resize a Texture to a new Width and Height.
    /// </summary>
    /// <param name="texture">Texture to Resize.</param>
    /// <param name="Engine">Reference to the Engine to access the Graphics Device and SpriteBatch.</param>
    /// <param name="NewWidth">New Resized Width.</param>
    /// <param name="NewHeight">New Resized Height.</param>
    /// <returns>The new Resized texture.</returns>
    public static Texture2D Resize(this Texture2D texture, int NewWidth, int NewHeight)
    {
        //Create a New Render Target to take the resized texture.
        RenderTarget2D NewTexture = new RenderTarget2D(vxGraphics.GraphicsDevice, NewWidth, NewHeight);

        //Set the New Render Target.
        vxGraphics.GraphicsDevice.SetRenderTarget(NewTexture);
        vxGraphics.GraphicsDevice.Clear(Color.Transparent);

        //Draw the original texture resized.
        vxGraphics.SpriteBatch.Begin("Ext - Texture2D - Resize");
        vxGraphics.SpriteBatch.Draw(texture,
            new Rectangle(0, 0, NewWidth, NewHeight),
            new Rectangle(0, 0, texture.Width, texture.Height),
            Color.White);
        vxGraphics.SpriteBatch.End();

        //Reset the Rendertarget to null
        vxGraphics.GraphicsDevice.SetRenderTarget(null);


        return NewTexture;
    }


    /// <summary>
    /// Crops a Portion of a Texture and returns a New Texture.
    /// </summary>
    /// <param name="texture">Texture to Crop.</param>
    /// <param name="Engine">Reference to the Engine to access the Graphics Device and SpriteBatch.</param>
    /// <param name="CropRectangle">The Rectangle to Crop the Texture.</param>
    /// <returns>The New Cropped Texture.</returns>
    public static Texture2D Crop(this Texture2D texture, Rectangle CropRectangle)
    {
        //CropRectangle = new Rectangle(0, 0, texture.Width/2, texture.Height/2);
        //Create a New Render Target to take the resized texture.
        RenderTarget2D NewTexture = new RenderTarget2D(vxGraphics.GraphicsDevice, CropRectangle.Width, CropRectangle.Height);

        //Set the New Render Target.
        vxGraphics.GraphicsDevice.SetRenderTarget(NewTexture);
        vxGraphics.GraphicsDevice.Clear(Color.Transparent);

        //Draw the original texture resized.
        vxGraphics.SpriteBatch.Begin("Ext - Texture2D - Crop");
        vxGraphics.SpriteBatch.Draw(texture, Vector2.Zero, CropRectangle, Color.White);
        vxGraphics.SpriteBatch.End();

        //Reset the Rendertarget to null
        vxGraphics.GraphicsDevice.SetRenderTarget(null);

        return NewTexture;
    }

    /// <summary>
    /// Returns the aspect ratio of this Texture2D (Width / Height)
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static float GetAspectRatio(this Texture2D texture)
    {
        return (float)texture.Bounds.Width / (float)texture.Bounds.Height;
    }


    /*
    public static Texture2D LoadImage(this Texture2D texture, string path)
    {
#if __ANDROID__
        using (var assetStream = Activity.Assets.Open(path))
#else
        using (var fileStream = new FileStream(path, FileMode.Open))
#endif
        {
            texture = Texture2D.FromStream(vxGraphics.GraphicsDevice, fileStream);
        }
        return thumbnail;
    }
    */

    /// <summary>
    /// Returns a new Texture with the SurfaceFormat converted from Color to Vector4.
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="device">The GraphicsDevice.</param>
    /// <returns>A new Texture with the SurfaceFormat as Vector4.</returns>
    public static Texture2D ToVector4(this Texture2D texture, GraphicsDevice device)
        {
            // the Colour Buffer of the previous Texture
            Color[] colourBuffer = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colourBuffer);

            // the new Texture with the requested Surface Format
            Texture2D newtexture = new Texture2D(device, texture.Width, texture.Height, false, SurfaceFormat.Vector4);

            // The new Buffer to hold the Vector4's
            var newBuffer = new Vector4[texture.Width * texture.Height];

            // loop through the buffers
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                {
                    int i = x + y * texture.Width;
                    newBuffer[i] = colourBuffer[i].ToVector4();
                }

            // set the new Buffer data
            newtexture.SetData(newBuffer);

            // return the new texture
            return newtexture;
        }




        /// <summary>
        /// Loads the Height Data from this Texture
        /// </summary>
        public static float[,] ToHeightMapDataArray(this Texture2D heightMap, float HeightScaleFactor = 0.125f)
        {
            float[,] heightData;
            
            int terrainWidth = heightMap.Width;
            int terrainLength = heightMap.Height;

            Color[] heightMapColors = new Color[(terrainWidth) * (terrainLength)];
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth+1, terrainLength+1];
            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainLength; y++)
                {
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R * HeightScaleFactor;
                }

            for (int y = 0; y < terrainLength; y++)
            {
                heightData[terrainWidth, y] = heightData[terrainWidth-1, y];
                    }

            return heightData;

            //for (int x = 0; x < terrainWidth; x++)
            //    for (int y = 0; y < terrainLength; y++)
            //        heightData[x, y] = (heightData[x, y] - minimumHeight) / (maximumHeight - minimumHeight) * 30.0f;
        }
    }

