using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine
{
    public class vxGameAttributeException : Exception
    {
        /// <summary>
        /// Throws a game attribute exception
        /// </summary>
        /// <param name="type"></param>
        public vxGameAttributeException(Type type) :
            base(string.Format("vxGame class is missing required vxGameInfo Attribute of type '{0}'", type.ToString()))
        {

        }
    }
}
