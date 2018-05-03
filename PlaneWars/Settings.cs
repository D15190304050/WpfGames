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
        public const int PlayerBombMax = 3;

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
        public const int Bullet2Total = 100;

        // Common constants for enemies.
        public const double EnemyStartY = -5;
        public const int EnemyLeftMin = 0;
        public const int EnemyTopMax = 770;

        // Constants for SmallEnemy.
        public const int SmallEnemyInitialHP = 1;
        public const int SmallEnemyLeftMax = 450;
        public const double SmallEnemyInitialSpeed = 2;
        public const int SmallEnemyScore = 100;

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
        public const double MiddleEnemyInitialSpeed = 0;
        public const int MiddleEnemyScore = 1000;

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
        public const double LargeEnemyInitialSpeed = -2;
        public const int LargeEnemyScore = 2500;

        public const double LargeUpLeftRectangleColliderLeftOffset = 15;
        public const double LargeUpLeftRectangleColliderTopOffset = 3;
        public const double LargeUpLeftRectangleColliderWidth = 12;
        public const double LargeUpLeftRectangleColliderHeight = 50;

        public const double LargeUpRightRectangleColliderLeftOffset = 124;
        public const double LargeUpRightRectangleColliderTopOffset = 3;
        public const double LargeUpRightRectangleColliderWidth = 12;
        public const double LargeUpRightRectangleColliderHeight = 50;

        public const double LargeMiddleRectangleColliderLeftOffset = 15;
        public const double LargeMiddleRectangleColliderTopOffset = 52;
        public const double LargeMiddleRectangleColliderWidth = 120;
        public const double LargeMiddleRectangleColliderHeight = 132;

        public const double LargeLeftRectangleColliderLeftOffset = 3;
        public const double LargeLeftRectangleColliderTopOffset = 153;
        public const double LargeLeftRectangleColliderWidth = 13;
        public const double LargeLeftRectangleColliderHeight = 31;

        public const double LargeRightRectangleColliderLeftOffset = 135;
        public const double LargeRightRectangleColliderTopOffset = 153;
        public const double LargeRightRectangleColliderWidth = 13;
        public const double LargeRightRectangleColliderHeight = 31;

        public const double LargeDownRectangleColliderLeftOffset = 23;
        public const double LargeDownRectangleColliderTopOffset = 183;
        public const double LargeDownRectangleColliderWidth = 105;
        public const double LargeDownRectangleColliderHeight = 25;

        // Common parameters for supplies.
        public const double SupplyStartY = -5;
        public const double SupplySpeed = 8;
        public const int SupplyInterval = 1000;

        public const int SupplyLeftMin = 0;
        public const int SupplyLeftMax = 450;

        // Rectangle collider parameters for supplies.
        public const double BulletSupplyRectangleColliderLeftOffset = 18;
        public const double BulletSupplyRectangleColliderTopOffset = 75;
        public const double BulletSupplyRectangleColliderWidth = 52;
        public const double BulletSupplyRectangleColliderHeight = 37;

        public const double BombSupplyRectangleColliderLeftOffset = 5;
        public const double BombSupplyRectangleColliderTopOffset = 95;
        public const double BombSupplyRectangleColliderWidth = 63;
        public const double BombSupplyRectangleColliderHeight = 42;

        /// <summary>
        /// Connection string for this game.
        /// </summary>
        public const string ConnectionString = "Server = localhost; User = DinoStark; Password = non-feeling; Database = PlaneWars;";

        public const string LoginHint = "Please Login";
        public const string RegisterHint = "Please Register";

        public static GenerationInterval[] GenerationIntervals { get; private set; }

        public static int[] LevelScores { get; private set; }

        static Settings()
        {
            GenerationIntervals = new GenerationInterval[7];
            GenerationIntervals[0] = new GenerationInterval(60, int.MaxValue, int.MaxValue);
            GenerationIntervals[1] = new GenerationInterval(58, 600, int.MaxValue);
            GenerationIntervals[2] = new GenerationInterval(55, 580, int.MaxValue);
            GenerationIntervals[3] = new GenerationInterval(50, 550, 600);
            GenerationIntervals[4] = new GenerationInterval(45, 500, 550);
            GenerationIntervals[5] = new GenerationInterval(40, 480, 500);
            GenerationIntervals[6] = new GenerationInterval(30, 450, 450);

            LevelScores = new int[6];
            LevelScores[0] = 5000;
            LevelScores[1] = 10000;
            LevelScores[2] = 25000;
            LevelScores[3] = 50000;
            LevelScores[4] = 80000;
            LevelScores[5] = 120000;
        }
    }
}
