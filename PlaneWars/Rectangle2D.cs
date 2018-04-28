using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    /// <summary>
    /// The Rectangle2D class represents a normal rectangle in a 2-D surface.
    /// </summary>
    public class Rectangle2D : ICollider
    {
        /// <summary>
        /// Gets x-coordinate of the top-left point of this rectangle.
        /// </summary>
        public double StartX { get; private set; }

        /// <summary>
        /// Gets y-coordinate of the top-left point of this rectangle.
        /// </summary>
        public double StartY { get; private set; }

        /// <summary>
        /// Gets width of this rectangle.
        /// </summary>
        public double Width { get; }

        /// <summary>
        /// Gets height of this rectangle.
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// Gets x-coordinate of the bottom-right point of this rectangle.
        /// </summary>
        public double EndX { get; private set; }

        /// <summary>
        /// Gets y-coordinate of the bottom-right point of this rectangle.
        /// </summary>
        public double EndY { get; private set; }

        /// <summary>
        /// Initializes a new instance of the the Rectangle2D class with given startX, startY, width, height.
        /// </summary>
        /// <param name="startX">X-coordinate of the top-left point of this rectangle.</param>
        /// <param name="startY">Y-coordinate of the top-left point of this rectangle.</param>
        /// <param name="width">Width of this rectangle.</param>
        /// <param name="height">Height of this rectangle.</param>
        public Rectangle2D(double startX, double startY, double width, double height)
        {
            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;
            this.EndX = startX + width;
            this.EndY = startY + height;
        }

        /// <summary>
        /// Makes this rectangle move down.
        /// </summary>
        /// <param name="speed">The distance to move.</param>
        public void MoveDown(double speed)
        {
            this.StartY += speed;
            this.EndY += speed;
        }

        /// <summary>
        /// Returns true if this rectangle collides with (contains) the given point, otherwise, false.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if this rectangle collides with (contains) the given point, otherwise, false.</returns>
        public bool Collide(double x, double y)
        {
            if ((this.StartX <= x) && (x <= this.EndX) &&
                (this.StartY <= y) && (y <= this.EndY))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns the string representation of this rectangle, i.e. the value of startX, startY, endX, endY.
        /// </summary>
        /// <returns>The string representation of this rectangle, i.e. the value of startX, startY, endX, endY.</returns>
        public override string ToString()
        {
            return string.Format("StartX = {0}, StartY = {1}, EndX = {2}, EndY = {3}", this.StartX, this.StartY, this.EndX, this.EndY);
        }
    }
}
