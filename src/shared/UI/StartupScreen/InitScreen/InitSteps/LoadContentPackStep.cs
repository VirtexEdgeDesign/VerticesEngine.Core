using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using VerticesEngine.Plugins;

namespace VerticesEngine.InitSteps
{
    /// <summary>
    /// Loads a given content pack
    /// </summary>
    internal class LoadContentPackStep : vxIInitializationStep
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

        private vxIPlugin contentPack;

        private bool isMain = false;

        private vxPluginType contentPackType;

        private IEnumerator contentLoader;

        public LoadContentPackStep(vxIPlugin contentPack)
        {
            this.contentPack = contentPack;
            isMain = true;
        }


        public LoadContentPackStep(vxIPlugin contentPack, vxPluginType contentPackType)
        {
            this.contentPack = contentPack;
            this.contentPackType = contentPackType;
        }

        public void Start()
        {
            vxEngine.Game.InitializationStage = GameInitializationStage.LoadingGlobalContent;

            if(isMain)
                _status = "Loading Game";
            else
                _status = "Loading " + contentPackType;

            contentLoader = contentPack.LoadContent();
        }

        public void Update() {

            if (contentLoader.MoveNext())
            {
                // let's move next in the pseudo coroutine
                vxTime.ResetElapsedTime();
            }
            else if(_isComplete == false)
            {
                vxEntityRegister.RegisterAssemblyEntityTypes(contentPack);
                _isComplete = contentPack.IsLoaded;
            }
        }
    }
}
