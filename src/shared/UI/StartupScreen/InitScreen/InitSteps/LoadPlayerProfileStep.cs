using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Plugins;

namespace VerticesEngine.InitSteps
{
    /// <summary>
    /// Loads a players profile
    /// </summary>
    internal class LoadPlayerProfileStep : vxIInitializationStep
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


        private IEnumerator contentLoader;

        public LoadPlayerProfileStep()
        {

        }


        public void Start()
        {
            contentLoader = vxEngine.Game.OnLoadPlayerProfile();
            _status = "Loading Player Profile";
        }

        public void Update() {

            if (contentLoader.MoveNext())
            {
                vxTime.ResetElapsedTime();
            }
            else if (_isComplete == false)
            {
                _isComplete = true;
            }
        }
    }
}
