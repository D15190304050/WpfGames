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
    public class Enemy
    {
        public Image EnemyImage { get; set; }
        public int HP { get; set; }
        private double speed;
        private EnemyKind enemyKind;
        private BitmapImage[] destroyImages;
        private LinkedList<ICollide> colliders;

        public double Top { get { return Canvas.GetTop(this.EnemyImage); } }
        public double Left { get { return Canvas.GetLeft(this.EnemyImage); } }

        protected static BitmapImage SmallEnemyImage { get; private set; }
        protected static BitmapImage[] SmallEnemyDestroyImages { get; private set; }

        protected static BitmapImage MiddleEnemyImage { get; private set; }

        protected static BitmapImage[] MiddleEnemyDestroyImages { get; private set; }

        protected static BitmapImage LargeEnemyImage { get; private set; }

        protected static BitmapImage[] LargeEnemyDestroyImages { get; private set; }

        static Enemy()
        {
            Enemy.SmallEnemyImage = new BitmapImage(new Uri("Images/enemy1.png", UriKind.Relative));

            Enemy.SmallEnemyDestroyImages = new BitmapImage[4];
            Enemy.SmallEnemyDestroyImages[0] = new BitmapImage(new Uri("Images/enemy1_down1.png", UriKind.Relative));
            Enemy.SmallEnemyDestroyImages[1] = new BitmapImage(new Uri("Images/enemy1_down2.png", UriKind.Relative));
            Enemy.SmallEnemyDestroyImages[2] = new BitmapImage(new Uri("Images/enemy1_down3.png", UriKind.Relative));
            Enemy.SmallEnemyDestroyImages[3] = new BitmapImage(new Uri("Images/enemy1_down4.png", UriKind.Relative));

            Enemy.MiddleEnemyImage = new BitmapImage(new Uri("Images/enemy2.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages = new BitmapImage[4];
            Enemy.MiddleEnemyDestroyImages[0] = new BitmapImage(new Uri("Images/enemy2_down1.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages[1] = new BitmapImage(new Uri("Images/enemy2_down2.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages[2] = new BitmapImage(new Uri("Images/enemy2_down3.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages[3] = new BitmapImage(new Uri("Images/enemy2_down4.png", UriKind.Relative));

            Enemy.LargeEnemyImage = new BitmapImage(new Uri("Images/enemy3_n1.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages = new BitmapImage[4];
            Enemy.LargeEnemyDestroyImages[0] = new BitmapImage(new Uri("Images/enemy3_down1.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[1] = new BitmapImage(new Uri("Images/enemy3_down2.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[2] = new BitmapImage(new Uri("Images/enemy3_down3.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[3] = new BitmapImage(new Uri("Images/enemy3_down4.png", UriKind.Relative));
        }

        public Enemy(EnemyKind enemyKind, double startX)
        {
            this.EnemyImage = new Image();
            this.enemyKind = enemyKind;
            Canvas.SetLeft(this.EnemyImage, startX);
            Canvas.SetTop(this.EnemyImage, Settings.EnemyStartY);
            colliders = new LinkedList<ICollide>();

            if (enemyKind == EnemyKind.SmallEnemy)
            {
                destroyImages = SmallEnemyDestroyImages;
                this.EnemyImage.Source = SmallEnemyImage;
                speed = Settings.EnemyInitialSpeed;
                this.HP = Settings.SmallEnemyInitialHP;

                double left = this.Left;
                double top = this.Top;
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
                speed = Settings.EnemyInitialSpeed;
                this.HP = Settings.MiddleEnemyInitialHP;
            }
            else
            {
                destroyImages = LargeEnemyDestroyImages;
                this.EnemyImage.Source = LargeEnemyImage;
                speed = Settings.EnemyInitialSpeed;
                this.HP = Settings.LargeEnemyInitialHP;
            }
        }

        public void MoveDown()
        {
            // Move enemy plane image.
            Canvas.SetTop(this.EnemyImage, this.Top + speed);

            // Move its collider component.
            foreach (ICollide collider in colliders)
                collider.MoveDown(speed);
        }

        public bool Collide(double x, double y)
        {
            foreach (ICollide collider in colliders)
            {
                if (collider.Collide(x, y))
                {
                    MessageBox.Show("Collide");
                    return true;
                }
            }
            return false;
        }
    }
}
