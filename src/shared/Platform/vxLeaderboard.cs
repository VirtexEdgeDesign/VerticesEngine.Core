using System;
using Microsoft.Xna.Framework.Graphics;

namespace VerticesEngine.Profile
{
    /// <summary>
    /// Vx leaderboard.
    /// </summary>
    public class vxLeaderboard
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string ID
        {
            get
            {
                return _id;
            }
        }
        private readonly string _id = "xxxxx-xxxxxx";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:VerticesEngine.Profile.vxLeaderboard"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        public vxLeaderboard(string id)
        {
            this._id = id;
        }
    }
}
