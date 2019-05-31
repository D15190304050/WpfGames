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
        /// <summary>
        /// Gets coefficient of x.
        /// </summary>
        public double A { get; }

        /// <summary>
        /// Gets coefficient of y.
        /// </summary>
        public double B { get; }

        /// <summary>
        /// Gets constant.
        /// </summary>
        public double C { get; }

        /// <summary>
        /// Initializes a new instance of the Line2D class with given line parameters.
        /// </summary>
        /// <param name="a">Coefficient of x.</param>
        /// <param name="b">Coefficient of y.</param>
        /// <param name="c">Constant.</param>
        public Line2D(double a, double b, double c)
        {
            this.A = a;
            this.B = b;
            this.C = c;
        }

        /// <summary>
        /// Initializes a new instance of the Line2D class across 2 given points.
        /// </summary>
        /// <param name="point1">A point that this line cross.</param>
        /// <param name="point2">Another point that this line cross.</param>
        public Line2D(Point2D point1, Point2D point2)
        {
            // If point1 and point2 have the same y (or there difference are close to 0), than we can just have line equation as if-branch.
            // Otherwise, we can obtain the line equation using code in else-branch.
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
        /// Returns the intersection between this line and the given line, null if they have the same slope (or their slope are close).
        /// </summary>
        /// <param name="line2">Another line.</param>
        /// <returns>The intersection between this line and the given line, null if they have the same slope (or their slope are close).</returns>
        public virtual Point2D GetIntersectionWith(Line2D line2)
        {
            // This implementation uses linear algebra formulas.
            // denominator here is just the determinant of the coefficient matrix.

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
        /// <param name="x">X-coordinate of the given point.</param>
        /// <param name="y">Y-coordinate of the given point.</param>
        /// <returns>True if this line contains the given point, otherwise, false.</returns>
        public virtual bool Contains(double x, double y)
        {
            // Here we calculate the result of the line equation.
            // And we think they are equal if the result is less than 1 * 10 ^ (-5).
            double delta = Math.Abs(this.A * x + this.B * y + this.C);
            return delta < 1e-5;
        }

        /// <summary>
        /// Returns the string representation of this line, i.e. the standard form equation of this line.
        /// </summary>
        /// <returns>The string representation of this line, i.e. the standard form equation of this line.</returns>
        public override string ToString()
        {
            return string.Format("{0} * x + {1} * y + {2} = 0", this.A, this.B, this.C);
        }
    }
}
