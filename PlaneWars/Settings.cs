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

        // Common constants for enemies.
        public const double EnemyStartY = -5;
        public const double EnemyInitialSpeed = 2;
        public const int EnemyLeftMin = 0;
        public const int EnemyTopMax = 770;

        // Constants for SmallEnemy.
        public const int SmallEnemyInitialHP = 1;
        public const int SmallEnemyLeftMax = 450;
        public const double SmallRectangleColliderLeftOffset = 33;
        public const double SmallRectangleColliderTopOffset = 8;
        public const double SmallRectangleColliderWidth = 11;
        public const double SmallRectangleColliderHeight = 39;
        public const double SmallCircleColliderCenterX = 38.5;

        // Constants for MiddleEnemy.
        public const int MiddleEnemyInitialHP = 8;
        public const int MiddleEnemyLeftMax = 440;

        // Constants for LargeEnemy.
        public const int LargeEnemyInitialHP = 20;
        public const int LargeEnemyLeftMax = 308;

        static Settings()
        {
            PlayerShootInterval = TimeSpan.FromMilliseconds(200);
            Bullet1 = new BitmapImage(new Uri("Images/bullet1.png", UriKind.Relative));
            Bullet2 = new BitmapImage(new Uri("Images/bullet2.png", UriKind.Relative));
        }
    }
}
