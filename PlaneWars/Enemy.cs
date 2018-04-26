﻿using System;
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
    public class Enemy
    {
        public Image EnemyImage { get; set; }
        public int HP { get; set; }
        private double speed;
        private EnemyKind enemyKind;
        private BitmapImage[] destroyImages;
        private LinkedList<ICollider> colliders;
        private int destroyImageIndex;

        public double Top { get { return Canvas.GetTop(this.EnemyImage); } }
        public double Left { get { return Canvas.GetLeft(this.EnemyImage); } }

        public bool CanRemove { get; private set; }

        protected static BitmapImage SmallEnemyImage { get; private set; }
        protected static BitmapImage[] SmallEnemyDestroyImages { get; private set; }

        protected static BitmapImage MiddleEnemyImage { get; private set; }

        protected static BitmapImage[] MiddleEnemyDestroyImages { get; private set; }

        protected static BitmapImage LargeEnemyImage { get; private set; }

        protected static BitmapImage[] LargeEnemyDestroyImages { get; private set; }

        static Enemy()
        {
            Enemy.SmallEnemyImage = new BitmapImage(new Uri("Images/enemy1.png", UriKind.Relative));

            Enemy.SmallEnemyDestroyImages = new BitmapImage[8];
            Enemy.SmallEnemyDestroyImages[0] = new BitmapImage(new Uri("Images/enemy1_down1.png", UriKind.Relative));
            Enemy.SmallEnemyDestroyImages[1] = Enemy.SmallEnemyDestroyImages[0];
            Enemy.SmallEnemyDestroyImages[2] = new BitmapImage(new Uri("Images/enemy1_down2.png", UriKind.Relative));
            Enemy.SmallEnemyDestroyImages[3] = Enemy.SmallEnemyDestroyImages[2];
            Enemy.SmallEnemyDestroyImages[4] = new BitmapImage(new Uri("Images/enemy1_down3.png", UriKind.Relative));
            Enemy.SmallEnemyDestroyImages[5] = Enemy.SmallEnemyDestroyImages[4];
            Enemy.SmallEnemyDestroyImages[6] = new BitmapImage(new Uri("Images/enemy1_down4.png", UriKind.Relative));
            Enemy.SmallEnemyDestroyImages[7] = Enemy.SmallEnemyDestroyImages[6];

            Enemy.MiddleEnemyImage = new BitmapImage(new Uri("Images/enemy2.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages = new BitmapImage[8];
            Enemy.MiddleEnemyDestroyImages[0] = new BitmapImage(new Uri("Images/enemy2_down1.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages[1] = Enemy.MiddleEnemyDestroyImages[0];
            Enemy.MiddleEnemyDestroyImages[2] = new BitmapImage(new Uri("Images/enemy2_down2.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages[3] = Enemy.MiddleEnemyDestroyImages[2];
            Enemy.MiddleEnemyDestroyImages[4] = new BitmapImage(new Uri("Images/enemy2_down3.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages[5] = Enemy.MiddleEnemyDestroyImages[4];
            Enemy.MiddleEnemyDestroyImages[6] = new BitmapImage(new Uri("Images/enemy2_down4.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages[7] = Enemy.MiddleEnemyDestroyImages[6];

            Enemy.LargeEnemyImage = new BitmapImage(new Uri("Images/enemy3_n1.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages = new BitmapImage[8];
            Enemy.LargeEnemyDestroyImages[0] = new BitmapImage(new Uri("Images/enemy3_down1.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[1] = Enemy.LargeEnemyDestroyImages[0];
            Enemy.LargeEnemyDestroyImages[2] = new BitmapImage(new Uri("Images/enemy3_down2.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[3] = Enemy.LargeEnemyDestroyImages[2];
            Enemy.LargeEnemyDestroyImages[4] = new BitmapImage(new Uri("Images/enemy3_down3.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[5] = Enemy.LargeEnemyDestroyImages[4];
            Enemy.LargeEnemyDestroyImages[6] = new BitmapImage(new Uri("Images/enemy3_down4.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[7] = Enemy.LargeEnemyDestroyImages[6];
        }

        public Enemy(EnemyKind enemyKind, double startX)
        {
            this.EnemyImage = new Image();
            this.enemyKind = enemyKind;
            Canvas.SetLeft(this.EnemyImage, startX);
            Canvas.SetTop(this.EnemyImage, Settings.EnemyStartY);
            colliders = new LinkedList<ICollider>();
            destroyImageIndex = 0;
            this.CanRemove = false;
            double left = this.Left;
            double top = this.Top;

            if (enemyKind == EnemyKind.SmallEnemy)
            {
                destroyImages = SmallEnemyDestroyImages;
                this.EnemyImage.Source = SmallEnemyImage;
                speed = Settings.SmallEnemyInitialSpeed;
                this.HP = Settings.SmallEnemyInitialHP;
                
                Rectangle2D rectangleCollider = new Rectangle2D(left + Settings.SmallRectangleColliderLeftOffset,
                                                                top + Settings.SmallRectangleColliderTopOffset,
                                                                Settings.SmallRectangleColliderWidth,
                                                                Settings.SmallRectangleColliderHeight);
                Circle circleCollider = new Circle(left + Settings.SmallCircleColliderCenterXOffset,
                                                   top + Settings.SmallCircleColliderCenterYOffset,
                                                   Settings.SmallCircleColliderRadius);

                Point2D p1 = new Point2D(left + Settings.SmallLeftTriangleVertex1XOffset,
                                         top + Settings.SmallLeftTriangleVertex1YOffset);
                Point2D p2 = new Point2D(left + Settings.SmallLeftTriangleVertex2XOffset,
                                         top + Settings.SmallLeftTriangleVertex2YOffset);
                Point2D p3 = new Point2D(left + Settings.SmallLeftTriangleVertex3XOffset,
                                         top + Settings.SmallLeftTriangleVertex3YOffset);
                Point2D p4 = new Point2D(left + Settings.SmallRightTriangleVertex1XOffset,
                                         top + Settings.SmallRightTriangleVertex1YOffset);
                Point2D p5 = new Point2D(left + Settings.SmallRightTriangleVertex2XOffset,
                                         top + Settings.SmallRightTriangleVertex2YOffset);
                Point2D p6 = new Point2D(left + Settings.SmallRightTriangleVertex3XOffset,
                                         top + Settings.SmallRightTriangleVertex3YOffset);
                Triangle2D leftTriangleCollider = new Triangle2D(p1, p2, p3);
                Triangle2D rightTriangleCollider = new Triangle2D(p4, p5, p6);

                colliders.AddLast(rectangleCollider);
                colliders.AddLast(circleCollider);
                colliders.AddLast(leftTriangleCollider);
                colliders.AddLast(rightTriangleCollider);
            }
            else if (enemyKind == EnemyKind.MiddleEnemy)
            {
                destroyImages = MiddleEnemyDestroyImages;
                this.EnemyImage.Source = MiddleEnemyImage;
                speed = Settings.MiddleEnemyInitialSpeed;
                this.HP = Settings.MiddleEnemyInitialHP;

                Rectangle2D upRectangleCollider = new Rectangle2D(left + Settings.MiddleUpRectangleColliderLeftOffset,
                                                                  top + Settings.MiddleUpRectangleColliderTopOffset,
                                                                  Settings.MiddleUpRectangleColliderWidth,
                                                                  Settings.MiddleUpRectangleColliderHeight);
                Rectangle2D downRectangleCollider = new Rectangle2D(left + Settings.MiddleDownRectangleColliderLeftOffset,
                                                                    top + Settings.MiddleDownRectangleColliderTopOffset,
                                                                    Settings.MiddleDownRectangleColliderWidth,
                                                                    Settings.MiddleDownRectangleColliderHeight);

                colliders.AddLast(upRectangleCollider);
                colliders.AddLast(downRectangleCollider);
            }
            else
            {
                destroyImages = LargeEnemyDestroyImages;
                this.EnemyImage.Source = LargeEnemyImage;
                speed = Settings.LargeEnemyInitialSpeed;
                this.HP = Settings.LargeEnemyInitialHP;

                this.EnemyImage.Width = Settings.LargeEnemyWidth;

                Rectangle2D upRectangleCollider = new Rectangle2D(left + Settings.LargeUpRectangleColliderLeftOffset,
                                                                  top + Settings.LargeUpRectangleColliderTopOffset,
                                                                  Settings.LargeUpRectangleColliderWidth,
                                                                  Settings.LargeUpRectangleColliderHeight);
                Rectangle2D downRectangleCollider = new Rectangle2D(left + Settings.LargeDownRectangleColliderLeftOffset,
                                                                    top + Settings.LargeDownRectangleColliderTopOffset,
                                                                    Settings.LargeDownRectangleColliderWidth,
                                                                    Settings.LargeDownRectangleColliderHeight);
                Rectangle2D leftRectangleCollider = new Rectangle2D(left + Settings.LargeLeftRectangleColliderLeftOffset,
                                                                    top + Settings.LargeLeftRectangleColliderTopOffset,
                                                                    Settings.LargeLeftRectangleColliderWidth,
                                                                    Settings.LargeLeftRectangleColliderHeight);
                Rectangle2D rightRectangleCollider = new Rectangle2D(left + Settings.LargeRightRectangleColliderLeftOffset,
                                                                     top + Settings.LargeRightRectangleColliderTopOffset,
                                                                     Settings.LargeRightRectangleColliderWidth,
                                                                     Settings.LargeRightRectangleColliderHeight);
                colliders.AddLast(upRectangleCollider);
                colliders.AddLast(downRectangleCollider);
                colliders.AddLast(leftRectangleCollider);
                colliders.AddLast(rightRectangleCollider);
            }
        }

        public void MoveDown()
        {
            // Move enemy plane image.
            Canvas.SetTop(this.EnemyImage, this.Top + speed);

            // Move its collider component.
            foreach (ICollider collider in colliders)
                collider.MoveDown(speed);
        }

        public bool Collide(double x, double y)
        {
            foreach (ICollider collider in colliders)
            {
                if (collider.Collide(x, y))
                    return true;
            }
            return false;
        }

        public void Destroy()
        {
            if (destroyImageIndex == destroyImages.Length)
            {
                this.CanRemove = true;
                return;
            }
            else
                this.EnemyImage.Source = this.destroyImages[destroyImageIndex++];
        }
    }
}
