using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    /// <summary>
    /// The Line2D class represents a straight line in a 2-D surface whose equation is Ax + By + C = 0.
    /// </summary>
    public class Line2D
    {
        public double A { get; }
        public double B { get; }
        public double C { get; }

        public Line2D(double a, double b, double c)
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }

        public Line2D(Point2D point1, Point2D point2)
        {
            // If point1 and point2 have the same y (or there difference are close to 0), than we can just have line equation as if-branch.
            // Otherwise, we can obtain the line equation using code in else-branck.
            if (point1.X - point2.X < 1e-5)
            {
                this.A = 1;
                this.B = 0;
                this.C = -point1.X;
            }
            else
            {
                this.A = (point2.Y - point1.Y) / (point2.X - point1.X);
                this.B = -1;
                this.C = point1.Y - A * point1.X;
            }
        }

        /// <summary>
        /// Returns the intersection between this line and the given line, null if they have the same slope.
        /// </summary>
        /// <param name="line2">Another line.</param>
        /// <returns>The intersection between this line and the given line, null if they have the same slope.</returns>
        public virtual Point2D GetIntersectionWith(Line2D line2)
        {
            double a = this.A;
            double b = this.B;
            double c = line2.A;
            double d = line2.B;
            double e = -this.C;
            double f = -line2.C;

            double denominator = a * d - b * c;
            if (Math.Abs(denominator) < double.Epsilon)
                return null;

            double x = (e * d - b * f) / denominator;
            double y = (a * f - e * c) / denominator;
            return new Point2D(x, y);
        }

        /// <summary>
        /// Returns true if this line contains the given point, otherwise, false.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual bool Contains(double x, double y)
        {
            double delta = Math.Abs(this.A * x + this.B * y + this.C);
            return delta < 1e-5;
        }

        public override string ToString()
        {
            return string.Format("{0} * x + {1} * y + {2} = 0", this.A, this.B, this.C);
        }
    }
}
