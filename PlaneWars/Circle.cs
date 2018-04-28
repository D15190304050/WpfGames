using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    /// <summary>
    /// The Circle class represents a circle in a 2-D surface.
    /// </summary>
    public class Circle : ICollider
    {
        /// <summary>
        /// X-coordinate of its center.
        /// </summary>
        public double CenterX { get; private set; }

        /// <summary>
        /// Y-coordinate of its center.
        /// </summary>
        public double CenterY { get; private set; }

        /// <summary>
        /// Radius of this circle.
        /// </summary>
        public double Radius { get; }

        /// <summary>
        /// Initializes a new instance of the Circle class with given center coordinate and radius.
        /// </summary>
        /// <param name="centerX">X-coordinate of its center.</param>
        /// <param name="centerY">Y-coordinate of its center.</param>
        /// <param name="radius">Radius of this circle.</param>
        /// <exception cref="ArgumentException">If given radius less than or equal to 0.</exception>
        public Circle(double centerX, double centerY, double radius)
        {
            this.CenterX = centerX;
            this.CenterY = centerY;

            if (radius <= 0)
                throw new ArgumentException("Radius of a circle must be greater than 0.");
            this.Radius = radius;
        }

        /// <summary>
        /// Returns the distance between given point and center of this circle.
        /// </summary>
        /// <param name="x">X-coordinate of the given point.</param>
        /// <param name="y">Y-coordinate of the given point.</param>
        /// <returns>The distance between given point and center of this circle.</returns>
        public double DistanceToCenter(double x, double y)
        {
            double deltaX = x - this.CenterX;
            double deltaY = y - this.CenterY;
            double distanceSquare = deltaX * deltaX + deltaY * deltaY;
            return Math.Sqrt(distanceSquare);
        }

        /// <summary>
        /// Returns true if this circle collides with (contains) the given point, otherwise, false.
        /// </summary>
        /// <param name="x">X-coordinate of the given point.</param>
        /// <param name="y">Y-coordinate of the given point.</param>
        /// <returns>True if this circle collide with (contains) the given point, otherwise, false.</returns>
        public bool Collide(double x, double y)
        {
            double deltaX = x - this.CenterX;
            double deltaY = y - this.CenterY;
            double distanceSquare = deltaX * deltaX + deltaY * deltaY;

            return distanceSquare <= this.Radius * this.Radius;
        }

        /// <summary>
        /// Makes this circle move down.
        /// </summary>
        /// <param name="speed">The distance to move.</param>
        public void MoveDown(double speed)
        {
            this.CenterY += speed;
        }

        /// <summary>
        /// Returns the string representation of this circle, i.e. the standard form equation of this circle.
        /// </summary>
        /// <returns>the string representation of this circle, i.e. the standard form equation of this circle.</returns>
        public override string ToString()
        {
            return string.Format("(x - {0})^2 + (y - {1})^2 = {2}^2", this.CenterX, this.CenterY, this.Radius);
        }
    }
}
