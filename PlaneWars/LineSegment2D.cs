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
        public Point2D EndPoint1 { get; }
        public Point2D EndPoint2 { get; }

        private double xMin;
        private double xMax;
        private double yMin;
        private double yMax;

        public LineSegment2D(Point2D endPoint1, Point2D endPoint2)
            : base(endPoint1, endPoint2)
        {
            this.EndPoint1 = endPoint1;
            this.EndPoint2 = endPoint2;

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

        public override Point2D GetIntersectionWith(Line2D line2)
        {
            Point2D intersection = base.GetIntersectionWith(line2);

            if (intersection == null)
                return null;

            if ((xMin <= intersection.X) && (intersection.X <= xMax) &&
                (yMin <= intersection.Y) && (intersection.Y <= yMax))
                return intersection;
            else
                return null;
        }

        public Point2D GetIntersectionWith(LineSegment2D line2)
        {
            Point2D intersection = base.GetIntersectionWith(line2);
            if (intersection == null)
                return null;

            if ((this.xMin <= intersection.X) && (intersection.X <= this.xMax) &&
                (this.yMin <= intersection.Y) && (intersection.Y <= this.yMax) &&
                (line2.xMin <= intersection.X) && (intersection.X <= line2.xMax) &&
                (line2.yMin <= intersection.Y) && (intersection.Y <= line2.yMax))
                return intersection;
            else
                return null;
        }

        public override bool Contains(double x, double y)
        {
            if ((xMin <= x) && (x <= xMax) &&
                (yMin <= y) && (y <= yMax))
                return base.Contains(x, y);
            else
                return false;
        }

        public override string ToString()
        {
            return string.Format(base.ToString() + " with end point {0} and {1}", this.EndPoint1, this.EndPoint2);
        }
    }
}
