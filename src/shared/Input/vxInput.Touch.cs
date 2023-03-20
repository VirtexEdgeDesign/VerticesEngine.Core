using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace VerticesEngine.Input
{
    public static partial class vxInput
    {

        public static TouchCollection TouchCollection { get; private set; }

        public static TouchCollection PreviousTouchCollection { get; private set; }

#if __MOBILE__
        static Vector2 pos1;
        static Vector2 pos2;

        //static Vector2 pos1_strt;
        //static Vector2 pos2_strt;

        // Pinch Zoom Variables
        //static float initialZoom = 0;

        static float initialPinchDist = 0;

        static bool pinchBegin = true;


        // Touch Panning Variables
        //static bool panBegin = true;
        static Vector2 previousPan = Vector2.Zero;

        //static bool _isNewTouchDown = false;

        //static bool _isNewTouchRelease = false;
#endif
        static void InitTouchPanelState()
        {
            TouchPanel.EnabledGestures = GestureType.Tap;

        }

        static void UpdateTouchPanelState()
        {
            PreviousTouchCollection = TouchCollection;

            // Now Get Current Input States
            TouchCollection = TouchPanel.GetState();


#if __MOBILE__
            // Handle Touch Controls. This is because not every platform reliablly fires TouchState.Pressed. 
            // So instead, cound the touch collection between frames.
            //_isNewTouchDown = false;
            //_isNewTouchRelease = false;

            //if (PreviousTouchCollection.Count == 0 && TouchCollection.Count > 0)
            //    _isNewTouchDown = true;
            //else if (PreviousTouchCollection.Count > 0 && TouchCollection.Count == 0)
            //    _isNewTouchRelease = true;
#endif

            if (_gestures == null)
                _gestures = new System.Collections.Generic.List<GestureSample>();

            if (
                vxEngine.Instance == null || 
                vxEngine.Instance.CurrentScene == null || 
                vxEngine.Instance.CurrentScene.Cameras == null ||
                vxEngine.Instance.CurrentScene.Cameras.Count == 0 ||
                _gestures == null)
                return;

            _gestures.Clear();
            while (TouchPanel.IsGestureAvailable)
            {
                _gestures.Add(TouchPanel.ReadGesture());
            }

            if(!DidGamepadMoved)
            {
#if __IOS__ || __ANDROID__
                if (TouchCollection.Count > 0)
                {
                    //Only Fire Select Once it's been released
                    if (TouchCollection.Count == 1)
                    {
                        if (TouchCollection[0].State == TouchLocationState.Moved || TouchCollection[0].State == TouchLocationState.Pressed)
                        {
                            Cursor = TouchCollection[0].Position;
                        }
                    }

                    // Handle Pinch Zoom and Panning
                    if (TouchCollection.Count == 2)
                    {
                        // First get the currnet location of the touch positions
                        pos1 = TouchCollection[0].Position;
                        pos2 = TouchCollection[1].Position;

                        // if its the first loop with this pinch, then set the initial condisitons
                        if (pinchBegin == true)
                        {
                            pinchBegin = false;
                            initialPinchDist = Vector2.Subtract(pos2, pos1).Length();

                            // Get Average Position for panning
                            previousPan = (pos1 + pos2) / 2;
                            Cursor = previousPan;
                        }

                        // if not, then set the zoom based off of the pinch
                        else
                        {
                            float CamZoomDelta = Vector2.Subtract(pos2, pos1).Length() - initialPinchDist;

                            //_engine.Current2DSceneBase.Camera.Zoom += CamZoomDelta / 750;
                            initialPinchDist = Vector2.Subtract(pos2, pos1).Length();

                            Vector2 CamMov = previousPan - (pos1 + pos2) / 2;

                            if (vxEngine.Instance.CurrentScene is vxGameplayScene2D)
                            {
                                vxEngine.Instance.CurrentScene.Cameras[0].Zoom += CamZoomDelta / 750;
                                ((vxCamera2D)(vxEngine.Instance.CurrentScene).Cameras[0]).MoveCamera(CamMov / 20);
                            }

                            //_engine.Current2DSceneBase.Camera.MoveCamera(CamMov / 20);
                            previousPan = (pos1 + pos2) / 2;
                            Cursor = previousPan;
                        }
                    }
                    else
                    {
                        pinchBegin = true;
                    }
                }
#endif
            }
        }


        /// <summary>
        /// Is a New Screen Touch pressed
        /// </summary>
        /// <returns><c>true</c>, if new touch pressed was ised, <c>false</c> otherwise.</returns>
        public static bool IsNewTouchPressed()
        {
            return (PreviousTouchCollection.Count == 0 && TouchCollection.Count > 0);
        }

        /// <summary>
        /// Is a New Screen Touch released
        /// </summary>
        /// <returns><c>true</c>, if new touch released was ised, <c>false</c> otherwise.</returns>
        public static bool IsNewTouchReleased()
        {
            if (TouchCollection.Count == 0 && PreviousTouchCollection.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Is there at least one touch in the touch collection.
        /// </summary>
        /// <returns><c>true</c>, if touch pressed was ised, <c>false</c> otherwise.</returns>
        public static bool IsTouchPressed()
        {
            if (TouchCollection.Count > 0)
            {
                return (TouchCollection[0].State != TouchLocationState.Released);
            }
            else
                return false;
        }

        public static bool IsTouchReleased()
        {
            if (TouchCollection.Count > 0)
            {
                return (TouchCollection[0].State == TouchLocationState.Released);
            }
            else
                return false;
        }
    }
}
