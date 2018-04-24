using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneWars
{
    public class Rectangle : ICollide
    {
        public double StartX { get; private set; }
        public double StartY { get; private set; }
        public double Width { get; }
        public double Height { get; }

        public double EndX { get; private set; }
        public double EndY { get; private set; }

        public Rectangle(double startX, double startY, double width, double height)
        {
            this.StartX = startX;
            this.StartY = startY;
            this.Width = width;
            this.Height = height;
            this.EndX = startX + width;
            this.EndY = startY + height;
        }

        public void MoveDown(double speed)
        {
            this.StartY += speed;
            this.EndY += speed;
        }

        public bool Collide(double x, double y)
        {
            if ((this.StartX < x) && (x < this.EndX) &&
                (this.StartY < y) && (y < this.EndY))
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            return string.Format("StartX = {0}, StartY = {1}, EndX = {2}, EndY = {3}", this.StartX, this.StartY, this.EndX, this.EndY);
        }
    }
}
