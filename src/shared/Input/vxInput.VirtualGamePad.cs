using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace VerticesEngine.Input
{
    public static partial class vxInput
    {
        private static bool _handleVirtualStick;

        public static GamePadState VirtualState { get; private set; }
        public static GamePadState PreviousVirtualState { get; private set; }

        public static bool EnableVirtualStick;

        static void InitVirtualGamePad()
        {
            VirtualState = new GamePadState();
            PreviousVirtualState = new GamePadState();

#if WINDOWS_PHONE
			// virtual stick content
			_phoneStick = new VirtualStick(_manager.Content.Load<Texture2D>("Common/socket"),
			_manager.Content.Load<Texture2D>("Common/stick"), new Vector2(80f, 400f));

			Texture2D temp = _manager.Content.Load<Texture2D>("Common/buttons");
			_phoneA = new VirtualButton(temp, new Vector2(695f, 380f), new Rectangle(0, 0, 40, 40), new Rectangle(0, 40, 40, 40));
			_phoneB = new VirtualButton(temp, new Vector2(745f, 360f), new Rectangle(40, 0, 40, 40), new Rectangle(40, 40, 40, 40));
#endif
            _handleVirtualStick = false;
        }

        static void UpdateVirtualGamePad()
        {
            if (_handleVirtualStick)
                PreviousVirtualState = VirtualState;

            if (_handleVirtualStick)
            {
#if XBOX
				VirtualState = GamePad.GetState(PlayerIndex.One);
#elif WINDOWS
				VirtualState = GamePad.GetState(PlayerIndex.One).IsConnected ? GamePad.GetState(PlayerIndex.One) : HandleVirtualStickWin();
#elif WINDOWS_PHONE
				VirtualState = HandleVirtualStickWP7();
#endif
            }

        }


        private static GamePadState HandleVirtualStickWin()
        {
            Vector2 leftStick = Vector2.Zero;
            List<Buttons> buttons = new List<Buttons>();

            if (KeyboardState.IsKeyDown(Keys.A))
                leftStick.X -= 1f;
            if (KeyboardState.IsKeyDown(Keys.S))
                leftStick.Y -= 1f;
            if (KeyboardState.IsKeyDown(Keys.D))
                leftStick.X += 1f;
            if (KeyboardState.IsKeyDown(Keys.W))
                leftStick.Y += 1f;
            if (KeyboardState.IsKeyDown(Keys.Space))
                buttons.Add(Buttons.A);
            if (KeyboardState.IsKeyDown(Keys.LeftControl))
                buttons.Add(Buttons.B);
            if (leftStick != Vector2.Zero)
                leftStick.Normalize();

            //return new GamePadState(leftStick, Vector2.Zero, 0f, 0f, buttons.ToArray());
            return new GamePadState();
        }

        private static GamePadState HandleVirtualStickWP7()
        {
            List<Buttons> buttons = new List<Buttons>();
            Vector2 stick = Vector2.Zero;

#if WINDOWS_PHONE
			_phoneA.Pressed = false;
			_phoneB.Pressed = false;
			TouchCollection touchLocations = TouchPanel.GetState();
			foreach (TouchLocation touchLocation in touchLocations)
			{
			_phoneA.Update(touchLocation);
			_phoneB.Update(touchLocation);
			_phoneStick.Update(touchLocation);
			}
			if (_phoneA.Pressed)
			{
			buttons.Add(Buttons.A);
			}
			if (_phoneB.Pressed)
			{
			buttons.Add(Buttons.B);
			}
			stick = _phoneStick.StickPosition;
#endif
            //return new GamePadState(stick, Vector2.Zero, 0f, 0f, buttons.ToArray());
            return new GamePadState();
        }
    }
}
