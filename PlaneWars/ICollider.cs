using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICollider
    {
        /// <summary>
        /// Returns true if the collider collides with (contains) the given point, otherwise, false.
        /// </summary>
        /// <param name="x">X-coordinate of the given point.</param>
        /// <param name="y">Y-coordinate of the given point.</param>
        /// <returns>True if the collider collide with (contains) the given point, otherwise, false.</returns>
        bool Collide(double x, double y);

        /// <summary>
        /// Makes the collider move down.
        /// </summary>
        /// <param name="speed">The distance to move.</param>
        void MoveDown(double speed);
    }
}
