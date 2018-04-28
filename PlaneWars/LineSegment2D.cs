using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    /// <summary>
    /// The LineSegment2D class represents a line segment with given end point in a 2-D surface.
    /// </summary>
    public class LineSegment2D : Line2D
    {
        /// <summary>
        /// Gets an end point of this line segment.
        /// </summary>
        public Point2D EndPoint1 { get; }

        /// <summary>
        /// Gets the other end point of this line segment.
        /// </summary>
        public Point2D EndPoint2 { get; }

        /// <summary>
        /// The min value that x can take.
        /// </summary>
        private double xMin;
        
        /// <summary>
        /// The max value that x can take.
        /// </summary>
        private double xMax;

        /// <summary>
        /// The min value that y can take.
        /// </summary>
        private double yMin;

        /// <summary>
        /// The max value that y can take.
        /// </summary>
        private double yMax;

        /// <summary>
        /// Initializes a new instance of the LineSegment2D class with given end points.
        /// </summary>
        /// <param name="endPoint1">An end point of this line segment.</param>
        /// <param name="endPoint2">The other end point of this line segment.</param>
        public LineSegment2D(Point2D endPoint1, Point2D endPoint2)
            : base(endPoint1, endPoint2)
        {
            // Save 2 end points.
            this.EndPoint1 = endPoint1;
            this.EndPoint2 = endPoint2;

            // Get the range of x on this line segment.
            if (endPoint1.X < endPoint2.X)
            {
                xMin = endPoint1.X;
                xMax = endPoint2.X;
            }
            else
            {
                xMin = endPoint2.X;
                xMax = endPoint1.X;
            }

            // Get the range of y on this line segment.
            if (endPoint1.Y < endPoint2.Y)
            {
                yMin = endPoint1.Y;
                yMax = endPoint2.Y;
            }
            else
            {
                yMin = endPoint2.Y;
                yMax = endPoint1.Y;
            }
        }

        /// <summary>
        /// Gets the intersection between this line segment and the given line, null if they don't have one.
        /// </summary>
        /// <param name="line2">The line.</param>
        /// <returns>The intersection between this line and the given line, null if they don't have one.</returns>
        public override Point2D GetIntersectionWith(Line2D line2)
        {
            // Try to get the intersection between the line containing this line segment and the given line.
            Point2D intersection = base.GetIntersectionWith(line2);

            // Return null if they don't have a intersection.
            if (intersection == null)
                return null;

            // Return the intersection if its x and y are in the range, otherwise, null.
            if ((xMin <= intersection.X) && (intersection.X <= xMax) &&
                (yMin <= intersection.Y) && (intersection.Y <= yMax))
                return intersection;
            else
                return null;
        }

        /// <summary>
        /// Gets the intersection between this line segment and the given line segment, null if the don't have one.
        /// </summary>
        /// <param name="line2">The other line segment.</param>
        /// <returns>the intersection between this line segment and the given line segment, null if the don't have one.</returns>
        public Point2D GetIntersectionWith(LineSegment2D line2)
        {
            // Try to get the intersection between the line containing this line segment and the line containing the given line segment.
            Point2D intersection = base.GetIntersectionWith(line2);

            // Return null if they don't have a intersection.
            if (intersection == null)
                return null;

            // Return the intersection if its x and y are in the range, otherwise, null.
            if ((this.xMin <= intersection.X) && (intersection.X <= this.xMax) &&
                (this.yMin <= intersection.Y) && (intersection.Y <= this.yMax) &&
                (line2.xMin <= intersection.X) && (intersection.X <= line2.xMax) &&
                (line2.yMin <= intersection.Y) && (intersection.Y <= line2.yMax))
                return intersection;
            else
                return null;
        }

        /// <summary>
        /// Returns true if this line segment contains the given point, otherwise, false.
        /// </summary>
        /// <param name="x">X-coordinate of the given point.</param>
        /// <param name="y">Y-coordinate of the given point.</param>
        /// <returns>True if this line collides with (contains) the given point, otherwise, false.</returns>
        public override bool Contains(double x, double y)
        {
            // Check range first, this will save some time.
            if ((xMin <= x) && (x <= xMax) &&
                (yMin <= y) && (y <= yMax))
                return base.Contains(x, y);
            else
                return false;
        }

        /// <summary>
        /// Returns the string representation of this line segment, i.e. the standard form equation of the line containing this line segment and 2 end points.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(base.ToString() + " with end point {0} and {1}", this.EndPoint1, this.EndPoint2);
        }
    }
}
