using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.InitSteps
{
    internal class LoadGlobalContentStep : vxIInitializationStep
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
            vxEngine.Game.InitializationStage = GameInitializationStage.LoadingGlobalContent;
            _status = "Loading Core Content";

            vxCoroutineManager.Instance.StartCoroutine(LoadContentAsync());
        }

        IEnumerator globalLoader;

        bool isLangSet = false;
        public void Update()
        {
            if (isLangSet==false)
            {
                vxLocalizer.SetLocalization(vxSettings.Language);
                isLangSet= true;
            }           
        }


        public IEnumerator LoadContentAsync()
        {
            if (globalLoader == null)
                globalLoader = vxEngine.Game.LoadGlobalContent();

            while (globalLoader.MoveNext())
            {
                vxTime.ResetElapsedTime();
                yield return null;
            }

            _isComplete = true;
        }
    }
}
