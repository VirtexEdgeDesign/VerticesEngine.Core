﻿
//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

//using VerticesEngine;
////using VerticesEngine.PhysicsDrawer.Models;
//using BEPUphysics;
//using BEPUphysics.Vehicle;
//using BEPUphysics.CollisionShapes;
//using BEPUphysics.CollisionShapes.ConvexShapes;
//using BEPUphysics.Entities.Prefabs;
//using VerticesEngine.Cameras;


//#if XNA
//using VerticesEngine.PhysicsDrawer.Models;
//#else
//using BEPUphysicsDrawer.Models;
//#endif

//namespace VerticesEngine
//{
//    /// <summary>
//    /// Handles input and movement of a Vehicle in the game.
//    /// Acts as the 'front end' for the bookkeeping and math of the Vehicle within the physics Engine.
//    /// </summary>
//    public class VehicleInput
//    {
//        /// <summary>
//        /// Speed that the Vehicle tries towreach when moving backward.
//        /// </summary>
//        public float BackwardSpeed = -13;

//        /// <summary>
//        /// Camera to use for input.
//        /// </summary>
//        public vxCamera3D Camera;

//        /// <summary>
//        /// Current offset from the position of the Vehicle to the 'eyes.'
//        /// </summary>
//        public Vector3 CameraOffset;

//        /// <summary>
//        /// Speed that the Vehicle tries to reach when moving forward.
//        /// </summary>
//        public float ForwardSpeed = 30;

//        /// <summary>
//        /// Whether or not to use the Vehicle's input.
//        /// </summary>
//        public bool IsActive;


//        /// <summary>
//        /// Maximum turn angle of the wheels.
//        /// </summary>
//        public float MaximumTurnAngle = (float)Math.PI / 6;

//        /// <summary>
//        /// Draws the body and wheels.
//        /// </summary>
//        public ModelDrawer ModelDrawer;

//        /// <summary>
//        /// Owning space of the Vehicle.
//        /// </summary>
//        public Space Space;

//        /// <summary>
//        /// Turning speed of the wheels in radians per second.
//        /// </summary>
//        public float TurnSpeed = MathHelper.Pi;

//        /// <summary>
//        /// Physics representation of the Vehicle.
//        /// </summary>
//        public Vehicle Vehicle;


//#if XNA
//        /// <summary>
//        /// List of graphical representations of the wheels on the Vehicle.
//        /// </summary>
//        public List<DisplayModel> WheelModels;
//#endif

//        /// <summary>
//        /// Constructs the front end and the internal physics representation of the Vehicle.
//        /// </summary>
//        /// <param name="position">Position of the Vehicle.</param>
//        /// <param name="owningSpace">Space to add the Vehicle to.</param>
//        /// <param name="cameraToUse">Camera to attach to the Vehicle.</param>
//        /// <param name="drawer">Drawer used to draw the Vehicle.</param>
//        /// <param name="wheelModel">Model of the wheels.</param>
//        /// <param name="wheelTexture">Texture to use for the wheels.</param>
//        public VehicleInput(Vector3 position, Space owningSpace, vxCamera3D cameraToUse, ModelDrawer drawer, Model wheelModel, Texture2D wheelTexture)
//        {
//            var bodies = new List<CompoundShapeEntry>()
//            {
//                //new CompoundShapeEntry(new BoxShape(2.5f/2, .25f, 4.5f/2), new Vector3(0, 0, 0), 600),                
//                //new CompoundShapeEntry(new BoxShape(3.5f, .3f, 4.5f/5), new Vector3(0, 0.5f,0), 1),
//                //new CompoundShapeEntry(new SphereShape(0.5f),new Vector3(0,0,0), 1),
//                new CompoundShapeEntry(new CylinderShape(0.25f, 2.20f),new Vector3(0,0,0), 60),
//                new CompoundShapeEntry(new CylinderShape(0.25f, 1.5f),new Vector3(0,0.1f,0), 60),
//                //new CompoundShapeEntry(new CylinderShape(0.25f, 1.5f),new Vector3(0,0.5f,0), 60),
//            };

//            var body = new CompoundBody(bodies, 60);
//            body.CollisionInformation.LocalPosition = new Vector3(0, 0.25f, 0);
//            body.Position = position; //At first, just keep it out of the way.
//            Vehicle = new Vehicle(body);
//            body.AngularDamping = 0.25f;
//            body.Material.KineticFriction = 0;
//            body.Material.StaticFriction = 0;

//            body.Material.Bounciness = 0;

//            #region RaycastWheelShapes

//            float Wheel_Pos_W = 0.75f;
//            float Wheel_Pos_L = 1.5f;

//            float Wheel_RAD = 0.75f;

//            float Wheel_Friction = 1.0f;
//            float Wheel_Friction_Static = 2.5f;

//            float Susp_Stiff = 3500;

//            //The wheel model used is not aligned initially with how a wheel would normally look, so rotate them.
//            Matrix wheelGraphicRotation = Matrix.CreateFromAxisAngle(Vector3.Forward, MathHelper.PiOver2);

//            Vehicle.AddWheel(new Wheel(
//                                 new RaycastWheelShape(Wheel_RAD, wheelGraphicRotation),
//                                 new WheelSuspension(Susp_Stiff, 100f, Vector3.Down, .8f, new Vector3(-Wheel_Pos_W, 0, Wheel_Pos_L)),
//                                 new WheelDrivingMotor(2.5f, 30000, 10000),
//                                 new WheelBrake(1.5f, 2, .02f),
//                                 new WheelSlidingFriction(Wheel_Friction, Wheel_Friction_Static)));
//            Vehicle.AddWheel(new Wheel(
//                                 new RaycastWheelShape(Wheel_RAD, wheelGraphicRotation),
//                                 new WheelSuspension(Susp_Stiff, 100f, Vector3.Down, .8f, new Vector3(-Wheel_Pos_W, 0, -Wheel_Pos_L)),
//                                 new WheelDrivingMotor(2.5f, 30000, 10000),
//                                 new WheelBrake(1.5f, 2, .02f),
//                                 new WheelSlidingFriction(Wheel_Friction, Wheel_Friction_Static)));
//            Vehicle.AddWheel(new Wheel(
//                                 new RaycastWheelShape(Wheel_RAD, wheelGraphicRotation),
//                                 new WheelSuspension(Susp_Stiff, 100f, Vector3.Down, .8f, new Vector3(Wheel_Pos_W, 0, Wheel_Pos_L)),
//                                 new WheelDrivingMotor(2.5f, 30000, 10000),
//                                 new WheelBrake(1.5f, 2, .02f),
//                                 new WheelSlidingFriction(Wheel_Friction, Wheel_Friction_Static)));
//            Vehicle.AddWheel(new Wheel(
//                                 new RaycastWheelShape(Wheel_RAD, wheelGraphicRotation),
//                                 new WheelSuspension(Susp_Stiff, 100f, Vector3.Down, .8f, new Vector3(Wheel_Pos_W, 0, -Wheel_Pos_L)),
//                                 new WheelDrivingMotor(2.5f, 30000, 10000),
//                                 new WheelBrake(1.5f, 2, .02f),
//                                 new WheelSlidingFriction(Wheel_Friction, Wheel_Friction_Static)));


//            #endregion


//            foreach (Wheel wheel in Vehicle.Wheels)
//            {
//                //This is a cosmetic setting that makes it looks like the car doesn't have antilock brakes.
//                wheel.Shape.FreezeWheelsWhileBraking = true;

//                //By default, wheels use as many iterations as the space.  By lowering it,
//                //performance can be improved at the cost of a little accuracy.
//                //However, because the suspension and friction are not really rigid,
//                //the lowered accuracy is not so much of a problem.
//                wheel.Suspension.SolverSettings.MaximumIterations = 4;
//                wheel.Brake.SolverSettings.MaximumIterations = 1;
//                wheel.SlidingFriction.SolverSettings.MaximumIterations = 4;
//                wheel.DrivingMotor.SolverSettings.MaximumIterations = 1;
//            }

//            Space = owningSpace;

//            Space.Add(Vehicle);
//            ModelDrawer = drawer;
//            DisplayModel model;
//            WheelModels = new List<DisplayModel>();
//            for (int k = 0; k < 4; k++)
//            {
//                Vehicle.Wheels[k].Shape.Detector.Tag = "noDisplayObject";
//                model = new DisplayModel(wheelModel, ModelDrawer);
//                ModelDrawer.Add(model);
//                WheelModels.Add(model);
//                model.Texture = wheelTexture;
//            }


//            Camera = cameraToUse;
//        }

//        /// <summary>
//        /// Gives the Vehicle control over the camera and movement input.
//        /// </summary>
//        public void Activate(Vector3 Position, Vector3 linearVelocity, Quaternion Oreientation)
//        {
//            if (!IsActive)
//            {
//                IsActive = true;
//                Camera.UseMovementControls = false;
//                //Put the Vehicle where the camera is.
//                Vehicle.Body.Position = Position;// Camera.Position - CameraOffset;
//                Vehicle.Body.LinearVelocity = linearVelocity;
//                Vehicle.Body.AngularVelocity = Vector3.Zero;
//                Vehicle.Body.Orientation = Oreientation;
//                //Camera.ActivateChaseCameraMode(Vehicle.Body, new Vector3(0, .6f, 0), true, 10);
//            }
//        }

//        public void Activate()
//        {
//            Activate(Camera.Position - CameraOffset, Vector3.Zero, Quaternion.Identity);
//        }

//        /// <summary>
//        /// Returns input control to the camera.
//        /// </summary>
//        public void Deactivate()
//        {
//            if (IsActive)
//            {
//                IsActive = false;
//                Camera.UseMovementControls = true;
//                //Camera.DeactivateChaseCameraMode();
//            }
//        }


//        /// <summary>
//        /// Handles the input and movement of the character.
//        /// </summary>
//        /// <param name="dt">Time since last frame in simulation seconds.</param>
//        /// <param name="keyboardInput">Keyboard state.</param>
//        /// <param name="gamePadInput">Gamepad state.</param>
//        public void Update(float dt, KeyboardState keyboardInput, GamePadState gamePadInput)
//        {
//            //Update the wheel's graphics.
//            for (int k = 0; k < 4; k++)
//            {
//                WheelModels[k].WorldTransform = Vehicle.Wheels[k].Shape.WorldTransform;
//            }

//            if (IsActive)
//            {
//#if XBOX360
//                float speed = gamePadInput.Triggers.Right * ForwardSpeed + gamePadInput.Triggers.Left * BackwardSpeed;
//                Vehicle.Wheels[1].DrivingMotor.TargetSpeed = speed;
//                Vehicle.Wheels[3].DrivingMotor.TargetSpeed = speed;

//                if (gamePadInput.IsButtonDown(Buttons.LeftStick))
//                    foreach (Wheel wheel in Vehicle.Wheels)
//                    {
//                        wheel.Brake.IsBraking = true;
//                    }
//                else
//                    foreach (Wheel wheel in Vehicle.Wheels)
//                    {
//                        wheel.Brake.IsBraking = false;
//                    }
//                Vehicle.Wheels[1].Shape.SteeringAngle = (gamePadInput.ThumbSticks.Left.X * MaximumTurnAngle);
//                Vehicle.Wheels[3].Shape.SteeringAngle = (gamePadInput.ThumbSticks.Left.X * MaximumTurnAngle);
//#else

//                if (keyboardInput.IsKeyDown(Keys.W))
//                {
//                    //Drive
//                    Vehicle.Wheels[1].DrivingMotor.TargetSpeed = ForwardSpeed;
//                    Vehicle.Wheels[3].DrivingMotor.TargetSpeed = ForwardSpeed;
//                }
//                else if (keyboardInput.IsKeyDown(Keys.S))
//                {
//                    //Reverse
//                    Vehicle.Wheels[1].DrivingMotor.TargetSpeed = BackwardSpeed;
//                    Vehicle.Wheels[3].DrivingMotor.TargetSpeed = BackwardSpeed;
//                }
//                else
//                {
//                    //Idle
//                    Vehicle.Wheels[1].DrivingMotor.TargetSpeed = 0;
//                    Vehicle.Wheels[3].DrivingMotor.TargetSpeed = 0;
//                }
//                if (keyboardInput.IsKeyDown(Keys.Space))
//                {
//                    //Brake
//                    foreach (Wheel wheel in Vehicle.Wheels)
//                    {
//                        wheel.Brake.IsBraking = true;
//                    }
//                }
//                else
//                {
//                    //Release brake
//                    foreach (Wheel wheel in Vehicle.Wheels)
//                    {
//                        wheel.Brake.IsBraking = false;
//                    }
//                }
//                //Use smooth steering; while held down, move towards maximum.
//                //When not pressing any buttons, smoothly return to facing forward.
//                float angle;
//                bool steered = false;
//                if (keyboardInput.IsKeyDown(Keys.A))
//                {
//                    steered = true;
//                    angle = Math.Max(Vehicle.Wheels[1].Shape.SteeringAngle - TurnSpeed * dt, -MaximumTurnAngle);
//                    Vehicle.Wheels[1].Shape.SteeringAngle = angle;
//                    Vehicle.Wheels[3].Shape.SteeringAngle = angle;
//                }
//                if (keyboardInput.IsKeyDown(Keys.D))
//                {
//                    steered = true;
//                    angle = Math.Min(Vehicle.Wheels[1].Shape.SteeringAngle + TurnSpeed * dt, MaximumTurnAngle);
//                    Vehicle.Wheels[1].Shape.SteeringAngle = angle;
//                    Vehicle.Wheels[3].Shape.SteeringAngle = angle;
//                }
//                if (!steered)
//                {
//                    //Neither key was pressed, so de-steer.
//                    if (Vehicle.Wheels[1].Shape.SteeringAngle > 0)
//                    {
//                        angle = Math.Max(Vehicle.Wheels[1].Shape.SteeringAngle - TurnSpeed * dt, 0);
//                        Vehicle.Wheels[1].Shape.SteeringAngle = angle;
//                        Vehicle.Wheels[3].Shape.SteeringAngle = angle;
//                    }
//                    else
//                    {
//                        angle = Math.Min(Vehicle.Wheels[1].Shape.SteeringAngle + TurnSpeed * dt, 0);
//                        Vehicle.Wheels[1].Shape.SteeringAngle = angle;
//                        Vehicle.Wheels[3].Shape.SteeringAngle = angle;
//                    }
//                }


//#endif
//            }
//            else
//            {
//                //Parking brake
//                foreach (Wheel wheel in Vehicle.Wheels)
//                {
//                    wheel.Brake.IsBraking = true;
//                }
//                //Don't want the car to keep trying to drive.
//                Vehicle.Wheels[1].DrivingMotor.TargetSpeed = 0;
//                Vehicle.Wheels[3].DrivingMotor.TargetSpeed = 0;
//            }
//        }
//    }
//}
