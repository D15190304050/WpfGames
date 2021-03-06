﻿using System;
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
        /// Gets x-coordinate of this point.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets y-coordinate of this point.
        /// </summary>
        public double Y { get; set; }

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

        /// <summary>
        /// Returns the string representation of this point, i.e. (x,y).
        /// </summary>
        /// <returns>The string representation of this point, i.e. (x,y).</returns>
        public override string ToString()
        {
            return string.Format("({0},{1})", this.X, this.Y);
        }

        /// <summary>
        /// Returns true if the given point equals this point (or very close), otherwise, false.
        /// </summary>
        /// <param name="point2">Another point.</param>
        /// <returns>true if the given point equals this point (or very close), otherwise, false.</returns>
        public bool Equals(Point2D point2)
        {
            // Here we think they are equal if their horizontal and vertical distance are all less than 1 * 10 ^ (-5).
            if ((Math.Abs(this.X - point2.X) < 1e-5) &&
                (Math.Abs(this.Y - point2.Y) < 1e-5))
                return true;
            else
                return false;
        }
    }
}
