using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    public class Triangle2D : ICollider
    {
        public Point2D Vertex1 { get; }
        public Point2D Vertex2 { get; }
        public Point2D Vertex3 { get; }

        public Triangle2D(Point2D point1, Point2D point2, Point2D point3)
        {
            this.Vertex1 = point1;
            this.Vertex2 = point2;
            this.Vertex3 = point3;

            
        }

        public bool Collide(double x, double y)
        {
            LineSegment2D lineSegment1 = new LineSegment2D(this.Vertex1, this.Vertex2);
            LineSegment2D lineSegment2 = new LineSegment2D(this.Vertex1, this.Vertex3);
            LineSegment2D lineSegment3 = new LineSegment2D(this.Vertex2, this.Vertex3);

            // Return true if there is a line segment of this triangle that contains the given point.
            if ((lineSegment1.Contains(x, y)) ||
                (lineSegment2.Contains(x, y)) ||
                (lineSegment3.Contains(x, y)))
                return true;

            // Get the horizontal line accross the given point.
            Line2D horizontalLine = new Line2D(0, 1, -y);

            // Switch references of Line objects to make sure that if there is a horizontal line that contains some segment of this triangle, then it must be line1.
            if (lineSegment2.A == 0)
                SwitchLine(lineSegment1, lineSegment2);
            else if (lineSegment3.A == 0)
                SwitchLine(lineSegment1, lineSegment3);

            if (lineSegment1.A == 0)
            {
                // If there is a line segment of this triangle is horizontal, then the horizontal line accross the given point will have at most 2 intersections with this triangle.

                Point2D intersection1 = lineSegment2.GetIntersectionWith(horizontalLine);
                Point2D intersection2 = lineSegment3.GetIntersectionWith(horizontalLine);
                return Collide(intersection1, intersection2, x);
            }
            else
            {
                // If there is no line segment of this triangle is horizontal, then the horizontal line accross the given point will have at most 3 intersections with this triangle.

                Point2D intersection1 = lineSegment1.GetIntersectionWith(horizontalLine);
                Point2D intersection2 = lineSegment2.GetIntersectionWith(horizontalLine);
                Point2D intersection3 = lineSegment3.GetIntersectionWith(horizontalLine);

                // If there is 1 of the intersections is null, then just check the other 2.
                // Otherwise, see comment from line 74 to line 76.

                if (intersection1 == null)
                    return Collide(intersection2, intersection3, x);
                else if (intersection2 == null)
                    return Collide(intersection1, intersection3, x);
                else if (intersection3 == null)
                    return Collide(intersection1, intersection2, x);
                else
                {
                    // If program reaches here, it means that the horizontal line accross the given point have 3 intersections with this triangle.
                    // In other words, 2 of them are the same point.
                    // So, you need to find out the 3rd point.
                    if (intersection1.Equals(intersection2))
                        return Collide(intersection1, intersection3, x);
                    else if (intersection1.Equals(intersection3))
                        return Collide(intersection1, intersection2, x);
                    else
                        return Collide(intersection1, intersection3, x);

                    // Or you can just merge the last 2 branches.
                }
            }

            // Note that program will never reach here.
        }

        public void MoveDown(double speed)
        {
            this.Vertex1.Y += speed;
            this.Vertex2.Y += speed;
            this.Vertex3.Y += speed;
        }

        private static void SwitchLine(Line2D line1, Line2D line2)
        {
            Line2D swap = line1;
            line1 = line2;
            line2 = swap;
        }

        /// <summary>
        /// Returns true if x is between 2 intersections, otherwise, false.
        /// </summary>
        /// <param name="intersection1"></param>
        /// <param name="intersection2"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private static bool Collide(Point2D intersection1, Point2D intersection2, double x)
        {
            if ((intersection1 == null) || (intersection2 == null))
                return false;

            double xMin;
            double xMax;
            if (intersection1.X < intersection2.X)
            {
                xMin = intersection1.X;
                xMax = intersection2.X;
            }
            else
            {
                xMin = intersection2.X;
                xMax = intersection1.X;
            }

            if ((xMin <= x) && (x <= xMax))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return string.Format("Triangle with vertex1 = {0}, vertex2 = {1}, vertex3 = {2}", this.Vertex1, this.Vertex2, this.Vertex3);
        }
    }
}
