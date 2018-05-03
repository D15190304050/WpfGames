using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaneWars;
using MySql.Data.MySqlClient;

namespace UnitTest
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //CircleTest();
            //Rectangle2DTest();
            //Line2DTest();
            //LineSegment2DTest();
            //Triangle2DTest();
            MySqlConnectionTest();

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

        private static void Rectangle2DTest()
        {
            Rectangle2D rect = new Rectangle2D(1, 1, 4, 2);
            Console.WriteLine("Rectangle rect is: " + rect);
            Console.WriteLine("Collision with (5, 3): {0}", rect.Collide(5, 3));
            Console.WriteLine("Collision with (5, 2): {0}", rect.Collide(5, 2));
            Console.WriteLine("Collision with (6, 4): {0}", rect.Collide(6, 4));
            Console.WriteLine("Collision with (2, 2): {0}", rect.Collide(2, 2));
        }

        private static void Line2DTest()
        {
            Line2D line1 = new Line2D(1, 1, 1);
            Line2D line2 = new Line2D(1, 1, 0);
            Line2D line3 = new Line2D(1, -1, -3);

            Console.WriteLine("Line1: {0}", line1);
            Console.WriteLine("Line2: {0}", line2);
            Console.WriteLine("Line3: {0}", line3);

            Point2D intersection = line1.GetIntersectionWith(line2);
            if (intersection == null)
                Console.WriteLine("No intersection between line1 and line2.");
            else
                Console.WriteLine("Intersection: {0}", intersection);

            intersection = line1.GetIntersectionWith(line3);
            if (intersection == null)
                Console.WriteLine("No intersection between line1 and line3.");
            else
                Console.WriteLine("Intersection: {0}", intersection);

            Point2D p1 = new Point2D(1, 5);
            Point2D p2 = new Point2D(2, 4);
            Line2D line4 = new Line2D(p1, p2);
            Console.WriteLine("Line4: {0}", line4);

            Line2D line5 = new Line2D(0, 1, -3);
            Console.WriteLine("line5: {0}", line5);
        }

        private static void LineSegment2DTest()
        {
            Point2D p1 = new Point2D(1, 1);
            Point2D p2 = new Point2D(5, 9);
            LineSegment2D lineSegment1 = new LineSegment2D(p1, p2);

            Point2D p3 = new Point2D(6, 9);
            Point2D p4 = new Point2D(10, 1);
            LineSegment2D lineSegment2 = new LineSegment2D(p3, p4);

            Point2D p5 = new Point2D(1, 9);
            Point2D p6 = new Point2D(5, 1);
            LineSegment2D lineSegment3 = new LineSegment2D(p5, p6);

            Console.WriteLine("LineSegment1: {0}", lineSegment1);
            Console.WriteLine("LineSegment2: {0}", lineSegment2);
            Console.WriteLine("LineSegment3: {0}", lineSegment3);

            Point2D intersection = lineSegment1.GetIntersectionWith(lineSegment2);
            if (intersection == null)
                Console.WriteLine("LineSegment1 and LineSegment2 has no intersection");
            else
                Console.WriteLine("Intersection of LineSegment1 and LineSegment2 is: {0}", intersection);

            intersection = lineSegment1.GetIntersectionWith(lineSegment3);
            if (intersection == null)
                Console.WriteLine("LineSegment1 and LineSegment3 has no intersection");
            else
                Console.WriteLine("Intersection of LineSegment1 and LineSegment3 is: {0}", intersection);
        }

        private static void Triangle2DTest()
        {
            // Vertices of triangle1.
            Point2D p1 = new Point2D(0, 0);
            Point2D p2 = new Point2D(1, 0);
            Point2D p3 = new Point2D(0, 1);

            // Test points for triangle1.
            Point2D testPoint1 = new Point2D(0.5, 0.5);
            Point2D testPoint2 = new Point2D(0, 0.5);
            Point2D testPoint3 = new Point2D(1, 0);
            Point2D testPoint4 = new Point2D(1, 1);

            // Collision detection.
            Triangle2D triangle1 = new Triangle2D(p1, p2, p3);
            Console.WriteLine("Triangle1: {0}", triangle1);
            Console.WriteLine("Triangle1 contains point {0}: {1}", testPoint1, triangle1.Collide(testPoint1.X, testPoint1.Y));
            Console.WriteLine("Triangle1 contains point {0}: {1}", testPoint2, triangle1.Collide(testPoint2.X, testPoint2.Y));
            Console.WriteLine("Triangle1 contains point {0}: {1}", testPoint3, triangle1.Collide(testPoint3.X, testPoint3.Y));
            Console.WriteLine("Triangle1 contains point {0}: {1}", testPoint4, triangle1.Collide(testPoint4.X, testPoint4.Y));

            // Vertices of triangle2.
            p1 = new Point2D(1, 1);
            p2 = new Point2D(2, 2);
            p3 = new Point2D(3, 0);

            // Print an empty line as a separator.
            Console.WriteLine();

            // Test points for triangle2.
            testPoint1 = new Point2D(2, 1);
            testPoint2 = new Point2D(2, 0.5);
            testPoint3 = new Point2D(2, 2);
            testPoint4 = new Point2D(2, 3);

            // Collision detection
            Triangle2D triangle2 = new Triangle2D(p1, p2, p3);
            Console.WriteLine("Triangle2: {0}", triangle1);
            Console.WriteLine("Triangle2 contains point {0}: {1}", testPoint1, triangle2.Collide(testPoint1.X, testPoint1.Y));
            Console.WriteLine("Triangle2 contains point {0}: {1}", testPoint2, triangle2.Collide(testPoint2.X, testPoint2.Y));
            Console.WriteLine("Triangle2 contains point {0}: {1}", testPoint3, triangle2.Collide(testPoint3.X, testPoint3.Y));
            Console.WriteLine("Triangle2 contains point {0}: {1}", testPoint4, triangle2.Collide(testPoint4.X, testPoint4.Y));
        }

        private static void MySqlConnectionTest()
        {
            string connectionString = "Server = localhost; User = DinoStark; Password = non-feeling; Database = PlaneWars;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            string query = "SELECT * FROM Users;";
            MySqlCommand cmd = new MySqlCommand(query, conn);

            try
            {
                conn.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    Console.WriteLine($"ID = {reader[0]}, Name = {reader[1]}, Password = {reader["Password"]}");

                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
