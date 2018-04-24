using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaneWars;

namespace UnitTest
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //CircleTest();
            RectangleTest();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return 0;
        }

        private static void CircleTest()
        {
            Circle c = new Circle(0, 0, 1);
            Console.WriteLine("Circle c is: " + c);
            Console.WriteLine("Collision with point (1, 1): {0}", c.Collide(1, 1));
            Console.WriteLine("Collision with point (1, 0): {0}", c.Collide(1, 0));
            Console.WriteLine("Collision with point (0.5, 0): {0}", c.Collide(0.5, 0));
        }

        private static void RectangleTest()
        {
            Rectangle rect = new Rectangle(1, 1, 4, 2);
            Console.WriteLine("Rectangle rect is: " + rect);
            Console.WriteLine("Collision with (5, 3): {0}", rect.Collide(5, 3));
            Console.WriteLine("Collision with (5, 2): {0}", rect.Collide(5, 2));
            Console.WriteLine("Collision with (6, 4): {0}", rect.Collide(6, 4));
            Console.WriteLine("Collision with (2, 2): {0}", rect.Collide(2, 2));
        }
    }
}
