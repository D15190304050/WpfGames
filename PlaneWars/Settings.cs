using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PlaneWars
{
    public static class Settings
    {
        // Constants for PlayerPlane.
        public const double PlayerTopMin = 50;
        public const double PlayerTopMax = 650;
        public const double PlayerLeftMin = 0;
        public const double PlayerLeftMax = 430;
        public const double PlayerScaleFactor = 0.7;
        public const double PlayerSpeed = 8;

        public static TimeSpan PlayerShootInterval { get; }

        // Constants for bullets.
        public const double BulletSpeed = 10;
        public const double BulletVerticalOffset = 20;
        public const double Bullet2LeftHorizontalOffset = -32;
        public const double Bullet2RightHorizontalOffset = 28;

        public static BitmapImage Bullet1 { get; }
        public static BitmapImage Bullet2 { get; }

        // Constants for Enemy1.


        static Settings()
        {
            PlayerShootInterval = TimeSpan.FromMilliseconds(200);
            Bullet1 = new BitmapImage(new Uri("Images/bullet1.png", UriKind.Relative));
            Bullet2 = new BitmapImage(new Uri("Images/bullet2.png", UriKind.Relative));
        }
    }
}
