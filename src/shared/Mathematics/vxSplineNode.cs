using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerticesEngine.Mathematics
{
    /// <summary>
    /// A node along a <see cref="VerticesEngine.Mathematics.vxSpline" Curve/>
    /// </summary>
    public class vxSplineNode
    {
        /// <summary>
        /// The positon of this spline node
        /// </summary>
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
            }
        }
        private Vector3 _position = Vector3.Zero;

        /// <summary>
        /// The next node along the spline 
        /// </summary>
        public vxSplineNode Next
        {
            get { return _next; }
            set
            {
                _next = value;
            }
        }
        private vxSplineNode _next;

        /// <summary>
        /// The previous node along the spline
        /// </summary>
        public vxSplineNode Previous
        {
            get { return _previous; }
            set
            {
                _previous = value;
            }
        }
        private vxSplineNode _previous;

        /// <summary>
        /// The spline that this node belongs to
        /// </summary>
        public vxSpline Spline
        {
            get { return _spline; }
        }
        private vxSpline _spline;

        public vxSplineNode(vxSpline spline)
        {
            _spline = spline;
        }
    }
}
