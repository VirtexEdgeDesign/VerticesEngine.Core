using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerticesEngine
{
    // Scheduler
    public class vxCoroutineManager
    {
        /// <summary>
        /// The instance of the game coroutine manager
        /// </summary>
        public static vxCoroutineManager Instance
        {
            get { return _instance; }
        }
        private static readonly vxCoroutineManager _instance = new vxCoroutineManager();


        private readonly List<vxCoroutine> _coroutines = new List<vxCoroutine>();

        private vxCoroutineManager() { }

        /// <summary>
        /// Starts a coroutine for the given IEnumerator
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public vxCoroutine StartCoroutine(IEnumerator routine)
        {
            vxCoroutine coroutine = new vxCoroutine(routine);
            _coroutines.Add(coroutine);

            return coroutine;
        }

        public vxCoroutine StartCoroutine(IEnumerator routine, Action callback)
        {
            vxCoroutine coroutine = new vxCoroutine(routine, callback);
            _coroutines.Add(coroutine);

            return coroutine;
        }

        public void Update()
        {
            foreach (vxCoroutine coroutine in _coroutines.Reverse<vxCoroutine>())
            {
                if (coroutine.routine.Current is vxCoroutine)
                    coroutine.waitForCoroutine = coroutine.routine.Current as vxCoroutine;

                if (coroutine.waitForCoroutine != null && coroutine.waitForCoroutine.IsFinished)
                    coroutine.waitForCoroutine = null;

                if (coroutine.waitForCoroutine != null)
                    continue;

                // update coroutine

                if (coroutine.routine.MoveNext())
                {
                    coroutine.IsFinished = false;
                }
                else
                {
                    _coroutines.Remove(coroutine);
                    coroutine.IsFinished = true;
                    coroutine.callback?.Invoke();
                }
            }
        }

    }
}
