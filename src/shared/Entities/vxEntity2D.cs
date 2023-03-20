using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VerticesEngine.Graphics;
using VerticesEngine.Graphics.Animation;
using VerticesEngine;

namespace VerticesEngine
{

    public enum ItemDirection
    {
        Left,
        Right
    }




	/// <summary>
	/// A Two Dimensional Entity which uses a Farseerer Body too set it's position, or vice versa.
	/// </summary>
	public class vxEntity2D : vxEntity
	{
        /// <summary>
        /// Gets the main sprite sheet used by this entity.
        /// </summary>
        /// <value>The main sprite sheet.</value>
        public Texture2D MainSpriteSheet { 
            
            get {
                if (_mainSpriteSheet == null)
                    return vxGraphics.MainSpriteSheet;
                else
                    return _mainSpriteSheet; 
            } 
            set {
                _mainSpriteSheet = value;
            }
        }
        Texture2D _mainSpriteSheet;

        /// <summary>
        /// Gets or sets the 2D Position of the entity.
        /// </summary>
        /// <value>The position.</value>
        [vxShowInInspector(vxInspectorCategory.EntityTransformProperties, "The Entities 2D Position")]
		public Vector2 Position
		{
			get
			{
				if (PhysicCollider != null)
					return ConvertUnits.ToDisplayUnits (PhysicCollider.Position);
				else
					return _position; 
			}
			set
			{
                try
                {
                    if (PhysicCollider != null)
                        PhysicCollider.Position = ConvertUnits.ToSimUnits(value);
                }
                catch
                {
                    vxConsole.WriteError("Could not set position for " + this.Id);
                }
                OnPositionUpdate(_position, value);

                _position = value;
			}
		}
		protected Vector2 _position = Vector2.Zero;

        protected virtual void OnPositionUpdate(Vector2 oldPos, Vector2 newPos)
        {
            
        }



        /// <summary>
        /// Gets or sets the float Rotation of the entity.
        /// </summary>
        /// <value>The position.</value>
        [vxShowInInspector(vxInspectorCategory.EntityTransformProperties, "The Entities Rotation in Radians")]
		public float Rotation
		{
			get
			{
				if (PhysicCollider != null)
					return PhysicCollider.Rotation;
				else
					return _rotation; 
			}
			set
            {
                try
                {
                    if (PhysicCollider != null)
					PhysicCollider.Rotation = value;
                }
                catch
                {
                    vxConsole.WriteError("Could not set rotation for " + this.Id);
                }

                _rotation = value;
			}
		}
		float _rotation = 0;

        public bool IgnoreGravity
        {
            get
            {
                if (PhysicCollider != null)
                    return PhysicCollider.IgnoreGravity;
                else
                    return _ignoreGravity;
            }
            set
            {
                if (PhysicCollider != null)
                    PhysicCollider.IgnoreGravity = value;

                _ignoreGravity = value;
            }
        }
        bool _ignoreGravity = false;


        protected bool ShouldIgnoreGravity = false;

        public virtual void ResetGravity()
        {
            IgnoreGravity = ShouldIgnoreGravity;
        }



        /// <summary>
        /// Gets the velocity.
        /// </summary>
        /// <value>The velocity.</value>
        public Vector2 Velocity
        {
            get
            {
                if (PhysicCollider != null)
                    _velocity = PhysicCollider.LinearVelocity;
                
                    return _velocity;
            }

            set
            {
                _preVelocity = _velocity;
                _velocity = value;

                if (PhysicCollider != null)
                    PhysicCollider.LinearVelocity = _velocity;
            }

        }
        Vector2 _velocity;


        /// <summary>
        /// The previous velocity.
        /// </summary>
        public Vector2 PreviousVelocity
        {
            get { return _preVelocity; }
        }
        Vector2 _preVelocity;


		/// <summary>
		/// The scale.
		/// </summary>
		public float Scale = 1f;

		/// <summary>
		/// The Farseer Physics Body (Note, it's not used in all instances of this class, so do a !=null check)
		/// </summary>
		public Body PhysicCollider
        {
            get { return _physicsBody; }
            set
            {
                //if(_physicsBody != null)
                //{
                //    //_physicsBody -= Onco
                //}
                _physicsBody = value;
                if(_physicsBody != null)
                    _physicsBody.UserData = this;


            }
        }

        public Body _physicsBody;



		/// <summary>
		/// The world.
		/// </summary>
		public  World PhysicsSimulation
        {
            get { return Scene.PhysicsSimulation; }
        }

		/// <summary>
		/// Gets or sets the texture of the Entity.
		/// </summary>
		/// <value>The texture.</value>
		public Texture2D Texture 
		{
			get{ return _texture; }
			set 
			{ 
				_texture = value; 
				if(_texture != null)
					Bounds = _texture.Bounds;
			}
		}
		private Texture2D _texture;


		/// <summary>
		/// The items sprite effect which dictates which direction it faces (left or right)
		/// </summary>
		public SpriteEffects SpriteEffect;

		/// <summary>
		/// Texture Origina.
		/// </summary>
		public Vector2 Origin = new Vector2(0);

		/// <summary>
		/// Entity Alpha value.
		/// </summary>
		public float Alpha = 1;

		/// <summary>
		/// Requested Entity Alpha value for smooth change.
		/// </summary>
		public float Alpha_Req = 1;

		/// <summary>
		/// The alpha chnage steps.
		/// </summary>
		public int AlphaChnageSteps = 4;

		/// <summary>
		/// Sets the Texture Layer Depth.
		/// </summary>
		public float LayerDepth = 0;

		/// <summary>
		/// The display color of the Entity.
		/// </summary>
		public Color DisplayColor = Color.White;

		/// <summary>
		/// The bounding rectangle.
		/// </summary>
		public Rectangle Bounds = new Rectangle();

		/// <summary>
		/// The highlite value.
		/// </summary>
		public float Highlite = 1;

        #region IO

        // User Defined Data for Saving
        public string UserDefinedData01 = "";
        public string UserDefinedData02 = "";
        public string UserDefinedData03 = "";
        public string UserDefinedData04 = "";
        public string UserDefinedData05 = "";

        /// <summary>
        /// Should the entity be saved? Some are owned or created by other entities, so there is not point in saving them as well.
        /// </summary>
        public bool IsSaveable = true;

        /// <summary>
        /// Preps for save.
        /// </summary>
        public virtual void PreSave() { }
        

        #endregion

        #region Animation Fields


        /// <summary>
        /// Gets or Sets the animation sprite sheet which is currently playing.
        /// </summary>
        public vxAnimationSprite2D AnimationSpriteSheet;

		/// <summary>
		/// Gets the index of the current frame in the animation.
		/// </summary>
		public int FrameIndex;

		/// <summary>
		/// The amount of time in seconds that the current frame has been shown for.
		/// </summary>
		private float time;

		/// <summary>
		/// Gets a texture origin at the bottom center of each frame.
		/// </summary>
		public Vector2 AnimationOrigin;


		/// <summary>
		/// Occurs when on animation begin.
		/// </summary>
		public event EventHandler<EventArgs> OnAnimationBegin;

		/// <summary>
		/// Occurs when on animation end.
		/// </summary>
		public event EventHandler<EventArgs> OnAnimationEnd;

        public vxGameplayScene2D Scene;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Entities.vxEntity2D"/> class.
		/// </summary>
        /// <param name="Scene">Scene.</param>
		/// <param name="sprite">Sprite.</param>
        public vxEntity2D(vxGameplayScene2D Scene, Texture2D sprite): base (Scene)
		{
            this.Scene = Scene;
			Texture = sprite;
			if(sprite != null)
				Origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
		}

        /// <summary>
        /// The main source rectangle.
        /// </summary>
        public Rectangle SpriteSourceRectangle;

        protected override vxEntityRenderer CreateRenderer()
        {
            return AddComponent<vxSpriteRenderer>();
        }

        /// <summary>
        /// Get's this entities Sprite Sheet. the Default is 'vxGraphics.MainSpriteSheet'. Override
        /// to use your own static sprite sheets, but you must batch your entities which use the same 
        /// sprite sheet together.
        /// </summary>
        /// <returns></returns>
        public virtual Texture2D GetSpriteSheet()
        {
            return vxGraphics.MainSpriteSheet;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Entities.vxEntity2D"/> class.
        /// </summary>
        /// <param name="Scene">Engine.</param>
        /// <param name="spriteSheetLocation">Sprite sheet location.</param>
        /// <param name="PhysicsSimulation">Physics simulation.</param>
        /// <param name="Position">Position.</param>
        public vxEntity2D(vxGameplayScene2D Scene, Rectangle spriteSheetLocation, Vector2 Position) : base(Scene)
        {
            this.Scene = Scene;
            SpriteSourceRectangle = spriteSheetLocation;

            Origin = new Vector2(SpriteSourceRectangle.Width / 2f, SpriteSourceRectangle.Height / 2f);

            //this.PhysicsSimulation = Scene.PhysicsSimulation;

            this.Position = Position;

            _usesSpriteSheet = true;

            MainSpriteSheet = GetSpriteSheet();

            Scene.Entities.Add(this);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Entities.vxEntity2D"/> class.
		/// </summary>
        /// <param name="Scene">Scene.</param>
		/// <param name="texture">Texture.</param>
		/// <param name="physicsSim">World.</param>
		/// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
        public vxEntity2D (vxGameplayScene2D Scene, Texture2D texture, Vector2 position)  
            : this (Scene, texture, position, new Vector2(0))
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:VerticesEngine.Entities.vxEntity2D"/> class.
		/// </summary>
        /// <param name="Scene">Scene.</param>
		/// <param name="texture">Texture.</param>
		/// <param name="physicsSim">Physics sim.</param>
		/// <param name="position">This Items Start Position. Note that the 'OriginalPosition' variable will be set to this value as well.</param>
		/// <param name="origin">Origin.</param>
        public vxEntity2D (vxGameplayScene2D Scene, Texture2D texture, Vector2 position, Vector2 origin)  : base (Scene)
        {
            this.Scene = Scene;
			Texture = texture;
			Origin = origin;
			Position = position;
			//PhysicsSimulation = Scene.PhysicsSimulation;
            Scene.Entities.Add (this);
		}


        /// <summary>
        /// Disposes the entity.
        /// </summary>
        protected override void OnDisposed()
        {
            Scene.Entities.Remove (this);

			base.OnDisposed ();
		}

        

        /*
        public virtual void UnloadEntity()
        {
            
        }
        */

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:VerticesEngine.Entities.vxEntity2D"/> uses sprite sheet.
        /// </summary>
        /// <value><c>true</c> if uses sprite sheet; otherwise, <c>false</c>.</value>
        public bool UsesSpriteSheet
        {
            get { return _usesSpriteSheet; }
            set { _usesSpriteSheet = value; }
        }
        bool _usesSpriteSheet = false;


        public virtual void DrawDistoriton()
        {
            
        }


        public virtual bool OnEntityCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }


        public virtual void OnEntitySeparation(Fixture fixtureA, Fixture fixtureB)
        {

        }

        /// <summary>
        /// This is called after Update, so if any variables need to be set after Update has been called by an overriding function,
        /// then they can be called here.
        /// </summary>
        protected internal override void PostUpdate()
        {
            base.PostUpdate();
            // Set the Velocity Here
            _preVelocity = Velocity;
        }

        /// <summary>
        /// Draw this instance.
        /// </summary>
        public void Draw (vxCamera camera, string renderpass)
		{

            // Smooth out the Alpha Value
            Alpha = vxMathHelper.Smooth(Alpha, Alpha_Req, AlphaChnageSteps);

            if (UsesSpriteSheet)
            {
                // Now draw the main Sprite
                vxGraphics.SpriteBatch.Draw(MainSpriteSheet,
                                        Position,
                                        SpriteSourceRectangle,
                                        DisplayColor * Alpha,
                                        Rotation,
                                        Origin,
                                        Scale,
                                        SpriteEffect,
                                        LayerDepth);
            }
            else
            {

                // Draw the texture
                if (Texture != null && PhysicCollider != null)
                    vxGraphics.SpriteBatch.Draw(Texture,
                        Position,
                        null,
                        DisplayColor * Alpha,
                        Rotation,
                        Origin,
                        Scale,
                        SpriteEffect,
                        LayerDepth);
            }
        }


        /// <summary>
        /// Begins or continues playback of an animation. Only one animation can be played by an entity at a time.
        /// If you need more than one animation, then create a seperate entity to handle that.
        /// </summary>
        /// <param name="animationSpriteSheet">Animation sprite sheet.</param>
        public void PlayAnimation(vxAnimationSprite2D animationSpriteSheet)
        {
            PlayAnimation(animationSpriteSheet, Vector2.Zero);
        }



        /// <summary>
        /// Begins or continues playback of an animation. Only one animation can be played by an entity at a time.
        /// If you need more than one animation, then create a seperate entity to handle that.
        /// </summary>
        /// <param name="animationSpriteSheet">Animation sprite sheet.</param>
        /// <param name="Offset">Offset to play the Animation.</param>
        public void PlayAnimation(vxAnimationSprite2D animationSpriteSheet, Vector2 Offset)
		{
			// If this animation is already running, do not restart it.
			if (AnimationSpriteSheet == animationSpriteSheet)
				return;


			// Start the new animation.
			AnimationSpriteSheet = animationSpriteSheet;
			FrameIndex = 0;
			this.time = 0.0f;


			AnimationOrigin = Offset + new Vector2(AnimationSpriteSheet.FrameWidth / 2.0f, AnimationSpriteSheet.FrameHeight / 2.0f);


             if (OnAnimationBegin != null)
				OnAnimationBegin(this, new EventArgs());
		}

		/// <summary>
		/// Stops the currently playing Animation.
		/// </summary>
		public void StopAnimation()
		{
			AnimationSpriteSheet = null;
		}

        public float AnimationScale = 1.0f;



		/// <summary>
		/// Advances the time position and draws the current frame of the animation.
		/// </summary>
		public virtual void DrawAnimation()
		{
			if (AnimationSpriteSheet != null)
			{
				// Get the Elapsed Time
				time += vxTime.DeltaTime;

				while (time > AnimationSpriteSheet.FrameTime)
				{
					time -= AnimationSpriteSheet.FrameTime;

					// Advance the frame index; looping or clamping as appropriate.
					if (AnimationSpriteSheet.IsLooping)
					{
						FrameIndex = (FrameIndex + 1) % AnimationSpriteSheet.FrameCount;
					}
					else {
						FrameIndex = Math.Min(FrameIndex + 1, AnimationSpriteSheet.FrameCount - 1);
					}
				}

				int row = (int)((float)FrameIndex / (float)AnimationSpriteSheet.NumOfCols);
				int column = FrameIndex % AnimationSpriteSheet.NumOfCols;

                var pos = AnimationSpriteSheet.SpriteSheetBounds.Location;
                // Calculate the source rectangle of the current frame.
                Rectangle source = new Rectangle(pos.X + column * AnimationSpriteSheet.FrameWidth, 
                    pos.Y+  row * AnimationSpriteSheet.FrameHeight, 
                    AnimationSpriteSheet.FrameWidth, 
                    AnimationSpriteSheet.FrameHeight);

                // Draw the current frame.
                //vxGraphics.SpriteBatch.Draw(AnimationSpriteSheet.Texture, this.Position + AnimationSpriteSheet.Offset,
                //	source, Color.White * AnimationSpriteSheet.Alpha, UseRotation ? this.Rotation:0, AnimationOrigin, 1.0f, SpriteEffect, 0.0f);
                
                vxGraphics.SpriteBatch.Draw(MainSpriteSheet,
                   Position + AnimationSpriteSheet.Offset,
                    source, 
                    Color.White * AnimationSpriteSheet.Alpha, 
                   AnimationSpriteSheet.MatchRotation ? this.Rotation : 0, 
                    AnimationOrigin, AnimationScale, SpriteEffect, 0.0f);

                

                //First Check if it's finished
                if (FrameIndex == AnimationSpriteSheet.FrameCount - 1 && AnimationSpriteSheet.IsLooping == false)
				{
					AnimationSpriteSheet = null;

					if (OnAnimationEnd != null)
						OnAnimationEnd(this, new EventArgs());
				}
			}
		}


        #region Utilties

        //public override void GetProperties(vxPropertiesControl propertyControl)
        //{
        //    base.GetProperties(propertyControl);

        //    propertyControl.AddPropertiesFromAttributes("General Properties", this.GetType(), typeof(vxBaseAttribute));
        //    propertyControl.AddPropertiesFromAttributes("Entity Properties", this.GetType(), typeof(vxEntity2DPropertyAttribute));
        //}

        /// <summary>
        /// Gets the body from texture.
        /// </summary>
        /// <returns>The body from texture.</returns>
        /// <param name="texture">Texture.</param>
        /// <param name="world">World.</param>
        /// <param name="bodyType">Body type.</param>
        public Body GetBodyFromTexture(Texture2D texture, World world, BodyType bodyType)
		{
			//Create an array to hold the data from the texture
			uint[] data = new uint[texture.Width * texture.Height];

			//Transfer the texture data to the array
			texture.GetData(data);

			//Find the vertices that makes up the outline of the shape in the texture
			Vertices textureVertices = PolygonTools.CreatePolygon(data, texture.Width, false);

			//The tool return vertices as they were found in the texture.
			//We need to find the real center (centroid) of the vertices for 2 reasons:

			//1. To translate the vertices so the polygon is centered around the centroid.
			Vector2 centroid = -textureVertices.GetCentroid();
			textureVertices.Translate(ref centroid);

			//2. To draw the texture the correct place.
			Origin = -centroid;

			//We simplify the vertices found in the texture.
			textureVertices = SimplifyTools.ReduceByDistance(textureVertices, 4f);

			//Since it is a concave polygon, we need to partition it into several smaller convex polygons
			List<Vertices> list = Triangulate.ConvexPartition(textureVertices, TriangulationAlgorithm.Bayazit);


			//scale the vertices from graphics space to sim space
			Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));
			foreach (Vertices vertices in list)
			{
				vertices.Scale(ref vertScale);
			}
            
            //Create a single body with multiple fixtures
            Body body = BodyFactory.CreateCompoundPolygon(world, list, 1f, bodyType);

            body.Position = ConvertUnits.ToSimUnits(Position);
            return body;
		}

		/// <summary>
		/// Gets the rotated vector.
		/// </summary>
		/// <returns>The rotated vector.</returns>
		/// <param name="rotation">Rotation.</param>
		public Vector2 GetRotatedVector(float rotation)
		{
			return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
		}





		#endregion
	}
}