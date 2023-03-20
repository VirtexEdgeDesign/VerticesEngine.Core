using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VerticesEngine
{
    public class vxCoroutine
    {
        public IEnumerator routine;
        public vxCoroutine waitForCoroutine;
        public bool IsFinished = false;

        public Action callback;

        public vxCoroutine(IEnumerator routine) { this.routine = routine; }

        public vxCoroutine(IEnumerator routine, Action callback) { this.routine = routine; this.callback = callback; }
    }
}
