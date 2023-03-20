using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.InitSteps
{
    internal class PermissionsRequestStep : vxIInitializationStep
    {
        public bool IsComplete
        {
            get { return _isComplete; }
        }
        private bool _isComplete;

        public string Status
        {
            get { return _status; }
        }
        private string _status;

        public void Start()
        {
            vxEngine.Game.InitializationStage = GameInitializationStage.NotifyOfPermissions;
            _status = "Checking Permissions";

            // if we're not on android then let's just back out straight off
            if (vxEngine.PlatformOS != vxPlatformOS.Android)
            {
                _isComplete = true;
            }
        }
            int i = 0;
            public void Update()
            {
                i++;
                if (i == 30)
                {
                    if (vxEngine.PlatformOS == vxPlatformOS.Android)
                    {
                        if (vxEngine.Game.IsPermissionsRequestRequired())
                        {
                            vxEngine.Game.InitializationStage = GameInitializationStage.Waiting;
                            var permissionMsgBox = vxEngine.Game.OnShowPermissionRequestMessage();
                            permissionMsgBox.Accepted += delegate
                            {
                                vxEngine.Game.RequestPermissions();
                                //vxEngine.Game.InitializationStage = GameInitializationStage.SigningInUser;
                                _isComplete = true;
                                // we can now load any settings as well
                                vxSettings.Load();
                            };
                        }
                        else
                        {
                            _isComplete = true;
                            vxSettings.Load();
                        }
                    }
                    else
                    {
                        // If we're not on Android, then we're done
                        _isComplete = true;
                    }
                }
            }
        }
}
