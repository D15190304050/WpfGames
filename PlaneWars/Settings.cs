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
        public const int PlayerInitialHP = 3;

        public const double PlayerCollisionPoint1XOffset = 48;
        public const double PlayerCollisionPoint1YOffset = 0;

        public const double PlayerCollisionPoint2XOffset = 18;
        public const double PlayerCollisionPoint2YOffset = 45;

        public const double PlayerCollisionPoint3XOffset = 78;
        public const double PlayerCollisionPoint3YOffset = 45;

        public const double PlayerCollisionPoint4XOffset = 5;
        public const double PlayerCollisionPoint4YOffset = 72;

        public const double PlayerCollisionPoint5XOffset = 5;
        public const double PlayerCollisionPoint5YOffset = 78;

        public const double PlayerCollisionPoint6XOffset = 90;
        public const double PlayerCollisionPoint6YOffset = 72;

        public const double PlayerCollisionPoint7XOffset = 90;
        public const double PlayerCollisionPoint7YOffset = 78;

        public const double PlayerCollisionPoint8XOffset = 48;
        public const double PlayerCollisionPoint8YOffset = 78;

        public const int PlayerShootInterval = 15;

        // Constants for bullets.
        public const double BulletSpeed = 10;
        public const double BulletVerticalOffset = 20;
        public const double Bullet2LeftHorizontalOffset = -32;
        public const double Bullet2RightHorizontalOffset = 28;
        public const double BulletWarheadTopOffset = 1;
        public const double BulletWarheadLeftOffset = 3;

        public static BitmapImage Bullet1 { get; }
        public static BitmapImage Bullet2 { get; }

        // Common constants for enemies.
        public const double EnemyStartY = -5;
        public const int EnemyLeftMin = 0;
        public const int EnemyTopMax = 770;
        public const int EnemyGenerationInterval1 = 60;
        public const int EnemyGenerationInterval2 = 50;
        public const int EnemyGenerationInterval3 = 40;

        // Constants for SmallEnemy.
        public const int SmallEnemyInitialHP = 1;
        public const int SmallEnemyLeftMax = 450;
        public const double SmallEnemyInitialSpeed = 2;

        public const double SmallRectangleColliderLeftOffset = 33;
        public const double SmallRectangleColliderTopOffset = 8;
        public const double SmallRectangleColliderWidth = 11;
        public const double SmallRectangleColliderHeight = 39;
        public const double SmallCircleColliderCenterXOffset = 38.5;
        public const double SmallCircleColliderCenterYOffset = 45.5;
        public const double SmallCircleColliderRadius = 5.5;
        public const double SmallLeftTriangleVertex1XOffset = 5;
        public const double SmallLeftTriangleVertex1YOffset = 20;
        public const double SmallLeftTriangleVertex2XOffset = 34;
        public const double SmallLeftTriangleVertex2YOffset = 20;
        public const double SmallLeftTriangleVertex3XOffset = 34;
        public const double SmallLeftTriangleVertex3YOffset = 48;
        public const double SmallRightTriangleVertex1XOffset = 73;
        public const double SmallRightTriangleVertex1YOffset = 20;
        public const double SmallRightTriangleVertex2XOffset = 44;
        public const double SmallRightTriangleVertex2YOffset = 20;
        public const double SmallRightTriangleVertex3XOffset = 44;
        public const double SmallRightTriangleVertex3YOffset = 48;

        // Constants for MiddleEnemy.
        public const int MiddleEnemyInitialHP = 8;
        public const int MiddleEnemyLeftMax = 440;
        public const double MiddleEnemyInitialSpeed = 2;

        public const double MiddleUpRectangleColliderTopOffset = 40;
        public const double MiddleUpRectangleColliderLeftOffset = 0;
        public const double MiddleUpRectangleColliderWidth = 92;
        public const double MiddleUpRectangleColliderHeight = 45;
        public const double MiddleDownRectangleColliderTopOffset = 85;
        public const double MiddleDownRectangleColliderLeftOffset = 28;
        public const double MiddleDownRectangleColliderWidth = 35;
        public const double MiddleDownRectangleColliderHeight = 35;

        // Constants for LargeEnemy.
        public const int LargeEnemyInitialHP = 20;
        public const int LargeEnemyLeftMax = 308;
        public const int LargeEnemyWidth = 150;
        public const double LargeEnemyInitialSpeed = 1;

        public const double LargeUpRectangleColliderTopOffset = 5;
        public const double LargeUpRectangleColliderLeftOffset = 15;
        public const double LargeUpRectangleColliderWidth = 120;
        public const double LargeUpRectangleColliderHeight = 180;

        public const double LargeDownRectangleColliderTopOffset = 185;
        public const double LargeDownRectangleColliderLeftOffset = 20;
        public const double LargeDownRectangleColliderWidth = 110;
        public const double LargeDownRectangleColliderHeight = 20;

        public const double LargeLeftRectangleColliderTopOffset = 155;
        public const double LargeLeftRectangleColliderLeftOffset = 3;
        public const double LargeLeftRectangleColliderWidth = 13;
        public const double LargeLeftRectangleColliderHeight = 35;

        public const double LargeRightRectangleColliderTopOffset = 155;
        public const double LargeRightRectangleColliderLeftOffset = 135;
        public const double LargeRightRectangleColliderWidth = 13;
        public const double LargeRightRectangleColliderHeight = 35;

        static Settings()
        {
            //PlayerShootInterval = TimeSpan.FromMilliseconds(200);
            Bullet1 = new BitmapImage(new Uri("Images/bullet1.png", UriKind.Relative));
            Bullet2 = new BitmapImage(new Uri("Images/bullet2.png", UriKind.Relative));
        }
    }
}
