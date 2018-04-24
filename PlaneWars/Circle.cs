using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    public class Circle : ICollide
    {
        public double CenterX { get; }
        public double CenterY { get; }
        public double Radius { get; }

        public Circle(double centerX, double centerY, double radius)
        {
            this.CenterX = centerX;
            this.CenterY = centerY;

            if (radius <= 0)
                throw new ArgumentException("Radius of a circle must be greater than 0.");
            this.Radius = radius;
        }

        public double DistanceToCenter(double x, double y)
        {
            double deltaX = x - this.CenterX;
            double deltaY = y - this.CenterY;
            double distanceSquare = deltaX * deltaX + deltaY * deltaY;
            return Math.Sqrt(distanceSquare);
        }

        public bool Collide(double x, double y)
        {
            double deltaX = x - this.CenterX;
            double deltaY = y - this.CenterY;
            double distanceSquare = deltaX * deltaX + deltaY * deltaY;

            return distanceSquare < this.Radius * this.Radius;
        }

        public override string ToString()
        {
            return string.Format("(x - {0})^2 + (y - {1})^2 = {2}^2", this.CenterX, this.CenterY, this.Radius);
        }
    }
}
