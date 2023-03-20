using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using VerticesEngine.Graphics;

namespace VerticesEngine.Diagnostics
{
    /// <summary>
    /// This control handles all of the internal engine stop watches.
    /// </summary>
    [vxDebugControl]
	public class vxProfilerGraphDebugControl : vxDebugUIControlBaseClass
    {
        /// <summary>
        /// How often to update the average
        /// </summary>
        public int SampleSpan = 50;

        /// <summary>
        /// Basic Effect to render the Vertices
        /// </summary>
        BasicEffect basicEffect;



        /// <summary>
        /// Is the Debug Timer Control Active?
        /// </summary>
        public new bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                foreach (var timer in vxProfiler.TimerCollection.Values)
                {
                    timer.IsVisible = value;
                }
            }
        }
        bool _isVisible = false;

        Rectangle Backing;
        Rectangle DisplayBacking;
        Rectangle TextDisplayBacking;
        int buffer = 4;

        public override string GetCommand()
        {
            return "tr";
        }

        public override string GetDescription()
        {
            return "Toggles the performance graphs";
        }

        public vxProfilerGraphDebugControl() : base ("Profiler Graph")
        {
            basicEffect = new BasicEffect(vxGraphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;


            Backing = new Rectangle(15, vxGraphics.GraphicsDevice.Viewport.Height - 75, 300, 60);

            DisplayBacking = new Rectangle(Backing.X - buffer, Backing.Y - buffer,
                Backing.Width + 2 * buffer + 25, Backing.Height + 2 * buffer);

            TextDisplayBacking = new Rectangle(DisplayBacking.Right + buffer, DisplayBacking.Y,
                150, DisplayBacking.Height);
        }
        public override void CommandExecute(IDebugCommandHost host, string command, IList<string> args)
        {
            base.CommandExecute(host, command, args);

            this.IsVisible = !this.IsVisible;
        }

        ///// <summary>
        ///// Add a New Timer
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="timer"></param>
        //public void AddTimer(object key, vxDebugTimer timer)
        //{
        //    timer.Backing = Backing;
        //    timer.SampleSizeForAvg = SampleSpan;
        //    if (TimerCollection.ContainsKey(key) == false)
        //        TimerCollection.Add(key, timer);
        //}


        protected internal override void Draw()
        {
			if (IsVisible)
			{

                SpriteFont font = vxInternalAssets.Fonts.DebugFont;

                // First draw the background
                vxGraphics.SpriteBatch.Begin("Debug - Profiler");
                
            Backing = new Rectangle(15, vxGraphics.GraphicsDevice.Viewport.Height - 75, 300, 60);
                DisplayBacking = new Rectangle(Backing.X - buffer, Backing.Y - buffer,
      Backing.Width + 2 * buffer + 25, Backing.Height + 2 * buffer);

                TextDisplayBacking = new Rectangle(DisplayBacking.Right + buffer, DisplayBacking.Y,
                    150, DisplayBacking.Height);
                
				vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, DisplayBacking, Color.Black * 0.75f);
				vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, TextDisplayBacking, Color.Black * 0.75f);

				Rectangle rc = new Rectangle(Backing.X, Backing.Y, 1, Backing.Height);

				//Draw Sample Span
				for (float t = 0; t < Backing.Width; t += SampleSpan)
				{
					rc.X = (int)(Backing.X + t);
					vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, rc, Color.Gray * 0.75f);
				}

				//Now draw Start, Middle and End
				for (float t = 0; t < Backing.Width + 1; t += Backing.Width / 2)
				{
					rc.X = (int)(Backing.X + t);
					vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, rc, Color.White);
				}

				int heightInc = 0;

				// The Colour label sqaure size
				int sq = 16;

				// Now draw the labels
				foreach (var item in vxProfiler.TimerCollection.Values)
				{
                    item.Backing = Backing;
					// Shorten the Time to a certain accuracy
					string avg = item.Average.ToString();
					if (avg.Length > 6)
						avg = avg.Substring(0, 6);

					Rectangle sqRec = new Rectangle(
						TextDisplayBacking.X + buffer / 2,
						TextDisplayBacking.Y + buffer + heightInc,
						sq / 3, sq - buffer);

					// Draw the Colour Label
					vxGraphics.SpriteBatch.Draw(vxInternalAssets.Textures.Blank, sqRec, item.Color);

					// Draw Title
					vxGraphics.SpriteBatch.DrawString(font, item.Name + ": ",
												  new Vector2(
													  (int)(TextDisplayBacking.X + buffer + sqRec.Width),
													  (int)(TextDisplayBacking.Y + heightInc + buffer / 2)), Color.White);

					// Now Draw the timer Value
					vxGraphics.SpriteBatch.DrawString(font, avg + " ms",
						new Vector2(
													  (int)(TextDisplayBacking.X + buffer / 2 + 60 + sqRec.Width),
													  (int)(TextDisplayBacking.Y + heightInc + buffer / 2)), Color.White);

					//Draw Graph Limit Numbers
					vxGraphics.SpriteBatch.DrawString(font, "50",
						new Vector2(Backing.Right + buffer, Backing.Top - 2), Color.White);

					vxGraphics.SpriteBatch.DrawString(font, "0",
						new Vector2(Backing.Right + buffer, Backing.Bottom - font.LineSpacing), Color.White);


					// Incremement the height to draw
					heightInc += font.LineSpacing;
				}
				vxGraphics.SpriteBatch.End();

                DrawGraph();
			}
		}
        /// <summary>
        /// Draw all of the timer Debug info
        /// </summary>
        public void DrawGraph()
        {
            if (IsVisible)
            {
                basicEffect.Projection = Matrix.CreateOrthographicOffCenter
        (0, vxGraphics.GraphicsDevice.Viewport.Width,     // left, right
            vxGraphics.GraphicsDevice.Viewport.Height, 0,    // bottom, top
            0, 1);                                         // near, far plane



                basicEffect.CurrentTechnique.Passes[0].Apply();

                foreach (var item in vxProfiler.TimerCollection.Values)
                {
                    if (item.TimeQueue.Count > 4)
                    {
                        vxGraphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip,
                        item.Vertices, 0, item.TimeQueue.Count - 1);
                    }
                }

            }
        }
    }




    /// <summary>
    /// This is an internal Engine stop watch control to measure performance.
    /// </summary>
    public class vxDebugTimerGraphSet
    {
        Stopwatch Timer = new Stopwatch();

        public string Name = "";

        const int NUMOFVERTS = 500;

        float StartTime = 0;
        float EndTime = 0;

        public Color Color;

        // The Current Presented Average
        public float Average = 0;

        //The runnign average to be used in calcualting the overall average.
        float runningAvg = 0;

        //The number of samples that should be used in each average
        public int SampleSizeForAvg = 50;

        //The current sample increment
        int CurrentSample = 0;


        /// <summary>
        /// The Vertices Queue as an Array
        /// </summary>
        public VertexPositionColor[] Vertices;

        public Queue<float> TimeQueue;

        public float Difference = 0;
        public float PreviousDifference = 0;

        public Rectangle Backing;

        public bool IsVisible = false;

        public vxDebugTimerGraphSet(string name, Color color)
        {
            Name = name;

            Color = color;

            TimeQueue = new Queue<float>();

            Vertices = new VertexPositionColor[NUMOFVERTS];

            Timer.Start();
        }

        /// <summary>
        /// Starts the Timer
        /// </summary>
        public void Start()
        {
            if (IsVisible && vxEngine.BuildType == vxBuildType.Debug)
            {
                vxProfiler.IsEnabled = true;
                StartTime = (float)Timer.Elapsed.TotalMilliseconds;
                EndTime = 0;
            }
        }

        /// <summary>
        /// Get the amount of time now.
        /// </summary>
        public void Stop()
        {
            if (IsVisible && vxEngine.BuildType == vxBuildType.Debug)
            {
                // Get the Finalised End Time
                EndTime = (float)Timer.Elapsed.TotalMilliseconds;

                // Now get the difference
                Difference = EndTime - StartTime;

                // Now deal with the running average
                CurrentSample++;

                //if it's less than the max, then add the current difference into the average
                if (CurrentSample < SampleSizeForAvg)
                {
                    runningAvg += Difference;
                }
                //If the current sample is pased the sample size, then set the new average and reset everything
                else
                {
                    Average = runningAvg / (float)SampleSizeForAvg;

                    runningAvg = 0;

                    CurrentSample = 0;
                }

                TimeQueue.Enqueue(EndTime - StartTime);

                if (TimeQueue.Count > NUMOFVERTS)
                {
                    TimeQueue.Dequeue();
                }


                float[] queueArray = TimeQueue.ToArray();

                for (int ind = 0; ind < queueArray.Length; ind++)
                {
                    if (ind < Vertices.Length)
                    {
                        //First get the Height Offset
                        float h = Backing.Bottom - Math.Min(queueArray[ind], Backing.Height);

                        //Now Set the Position
                        Vertices[ind].Position = new Vector3(Backing.Left - (ind - queueArray.Length) * Backing.Width / NUMOFVERTS, h, 0);

                        //Now Finally Set the Colour
                        Vertices[ind].Color = Color;
                    }
                }
            }
        }
    }
}
