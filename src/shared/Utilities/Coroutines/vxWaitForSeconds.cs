using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    public class vxWaitForSeconds
    {
        //float seconds = 0;
        float timeToWait = 0;


        public vxWaitForSeconds(float time)
        {
            timeToWait = vxTime.TotalGameTime + time;
        }

        public IEnumerator Wait()
        {
            while (vxTime.TotalGameTime < timeToWait)
                yield return null;
        }
    }
}
