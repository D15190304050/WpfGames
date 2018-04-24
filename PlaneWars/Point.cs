using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    /// <summary>
    /// The Point2D class represents a point in a 2-D surface with coordinates (x,y).
    /// </summary>
    public class Point2D
    {
        /// <summary>
        /// X coordinate of this point.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Y coordinate of this point.
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Initializes a Point with given coordinate.
        /// </summary>
        /// <param name="x">X coordinate of this point.</param>
        /// <param name="y">Y coordinate of this point.</param>
        public Point2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
