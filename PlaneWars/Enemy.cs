using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PlaneWars
{
    /// <summary>
    /// The Enemy class represents enemies in the game plane wars.
    /// </summary>
    public class Enemy
    {
        /// <summary>
        /// Gets the Image of this enemy.
        /// </summary>
        public Image EnemyImage { get; }

        /// <summary>
        /// Gets HP of this enemy.
        /// </summary>
        public int HP { get; set; }

        /// <summary>
        /// Gets the score that the player can obtain when this enemis dies.
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Speed of this enemy.
        /// </summary>
        public double speed;

        /// <summary>
        /// Gets the kind of this enemy.
        /// </summary>
        public EnemyKind EnemyKind { get; }

        /// <summary>
        /// A sequence images for explosions.
        /// </summary>
        private BitmapImage[] destroyImages;

        /// <summary>
        /// Gets the colliders of this enemy.
        /// </summary>
        public LinkedList<ICollider> Colliders { get; private set; }

        /// <summary>
        /// The index of next destroy image to show when this enemy is destroying.
        /// </summary>
        private int destroyImageIndex;

        /// <summary>
        /// Gets the y-coordinate of the top-left point of this enemy's image to show.
        /// </summary>
        public double Top { get { return Canvas.GetTop(this.EnemyImage); } }

        /// <summary>
        /// Gets the x-coordinate of the top-left point of this enemy's image to show.
        /// </summary>
        public double Left { get { return Canvas.GetLeft(this.EnemyImage); } }

        /// <summary>
        /// Gets the value that indicates whether the main scence can remove this enemy.
        /// </summary>
        public bool CanBeRemoved { get; private set; }

        /// <summary>
        /// Image of small enemy.
        /// </summary>
        protected static BitmapImage SmallEnemyImage { get; private set; }
        
        /// <summary>
        /// Images to show when small enemies is destroying.
        /// </summary>
        protected static BitmapImage[] SmallEnemyDestroyImages { get; private set; }

        /// <summary>
        /// Image of middle enemy.
        /// </summary>
        protected static BitmapImage MiddleEnemyImage { get; private set; }

        /// <summary>
        /// Images to show when middle enemies is destroying.
        /// </summary>
        protected static BitmapImage[] MiddleEnemyDestroyImages { get; private set; }

        /// <summary>
        /// Image of large enemy.
        /// </summary>
        protected static BitmapImage LargeEnemyImage { get; private set; }

        /// <summary>
        /// Images to show when large enemies is destroying.
        /// </summary>
        protected static BitmapImage[] LargeEnemyDestroyImages { get; private set; }

        /// <summary>
        /// Load BitmapImage variables only once.
        /// </summary>
        static Enemy()
        {
            // Load images.

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

        /// <summary>
        /// Initializes a new instance of the Enemy class with given enemy kind, startX and level.
        /// </summary>
        /// <param name="enemyKind">Specifies which kind of enemy it is.</param>
        /// <param name="startX">The x-coordinate of the top-left point of this enemy's image to show.</param>
        /// <param name="level">Currrent difficulty level.</param>
        public Enemy(EnemyKind enemyKind, double startX, int level)
        {
            // Initialize a new Image for this enemy to show.
            this.EnemyImage = new Image();

            // Save the kind.
            this.EnemyKind = enemyKind;

            // Set its coordinate.
            Canvas.SetLeft(this.EnemyImage, startX);
            Canvas.SetTop(this.EnemyImage, Settings.EnemyStartY);

            // Initialize the collection of colliders of this enemy.
            Colliders = new LinkedList<ICollider>();

            // Set the image to display when destroying to 0.
            destroyImageIndex = 0;

            // This enemy is alive when initializing, so it can't be removed.
            this.CanBeRemoved = false;

            // Get the coordinate of the top-left point of this enemy's image.
            // This will be re-used for multiple times.
            double left = this.Left;
            double top = this.Top;

            if (enemyKind == EnemyKind.SmallEnemy)
            {
                // Configure the flying and destroying images, speed, HP, and score of small enemy.
                destroyImages = SmallEnemyDestroyImages;
                this.EnemyImage.Source = SmallEnemyImage;
                speed = Settings.SmallEnemyInitialSpeed + level;
                this.HP = Settings.SmallEnemyInitialHP;
                this.Score = Settings.SmallEnemyScore;
                
                // Generate colliders of this enemy.
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

                // Add colliders to the collider collection.
                Colliders.AddLast(rectangleCollider);
                Colliders.AddLast(circleCollider);
                Colliders.AddLast(leftTriangleCollider);
                Colliders.AddLast(rightTriangleCollider);
            }
            else if (enemyKind == EnemyKind.MiddleEnemy)
            {
                // Configure the flying and destroying images, speed, HP, and score of middle enemy.
                destroyImages = MiddleEnemyDestroyImages;
                this.EnemyImage.Source = MiddleEnemyImage;
                speed = Settings.MiddleEnemyInitialSpeed + level;
                this.HP = Settings.MiddleEnemyInitialHP;
                this.Score = Settings.MiddleEnemyScore;

                // Generate colliders of this enemy.
                Rectangle2D upRectangleCollider = new Rectangle2D(left + Settings.MiddleUpRectangleColliderLeftOffset,
                                                                  top + Settings.MiddleUpRectangleColliderTopOffset,
                                                                  Settings.MiddleUpRectangleColliderWidth,
                                                                  Settings.MiddleUpRectangleColliderHeight);
                Rectangle2D downRectangleCollider = new Rectangle2D(left + Settings.MiddleDownRectangleColliderLeftOffset,
                                                                    top + Settings.MiddleDownRectangleColliderTopOffset,
                                                                    Settings.MiddleDownRectangleColliderWidth,
                                                                    Settings.MiddleDownRectangleColliderHeight);

                // Add colliders to the collider collection.
                Colliders.AddLast(upRectangleCollider);
                Colliders.AddLast(downRectangleCollider);
            }
            else
            {
                // Configure the flying and destroying images, speed, HP, and score of large enemy.
                destroyImages = LargeEnemyDestroyImages;
                this.EnemyImage.Source = LargeEnemyImage;
                speed = Settings.LargeEnemyInitialSpeed + level;
                this.HP = Settings.LargeEnemyInitialHP;
                this.Score = Settings.LargeEnemyScore;

                this.EnemyImage.Width = Settings.LargeEnemyWidth;

                // Generate colliders of this enemy.
                Rectangle2D upLeftRectangleCollider = new Rectangle2D(left + Settings.LargeUpLeftRectangleColliderLeftOffset,
                                                                      top + Settings.LargeUpLeftRectangleColliderTopOffset,
                                                                      Settings.LargeUpLeftRectangleColliderWidth,
                                                                      Settings.LargeUpLeftRectangleColliderHeight);

                Rectangle2D upRightRectangleCollider = new Rectangle2D(left + Settings.LargeUpRightRectangleColliderLeftOffset,
                                                                       top + Settings.LargeUpRightRectangleColliderTopOffset,
                                                                       Settings.LargeUpRightRectangleColliderWidth,
                                                                       Settings.LargeUpRightRectangleColliderHeight);

                Rectangle2D middleRectangleCollider = new Rectangle2D(left + Settings.LargeMiddleRectangleColliderLeftOffset,
                                                                      top + Settings.LargeMiddleRectangleColliderTopOffset,
                                                                      Settings.LargeMiddleRectangleColliderWidth,
                                                                      Settings.LargeMiddleRectangleColliderHeight);

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

                // Add colliders to the collider collection.
                Colliders.AddLast(upLeftRectangleCollider);
                Colliders.AddLast(upRightRectangleCollider);
                Colliders.AddLast(middleRectangleCollider);
                Colliders.AddLast(downRectangleCollider);
                Colliders.AddLast(leftRectangleCollider);
                Colliders.AddLast(rightRectangleCollider);

                
            }
        }

        /// <summary>
        /// Draws all the rectangle colliders of this enemy.
        /// </summary>
        /// <param name="canvas">The canvas to draw.</param>
        public void DrawRectangleColliders(Canvas canvas)
        {
            // Find rectangles to remove, leave other UIElements alone.
            LinkedList<Rectangle> rectanglesToRemove = new LinkedList<Rectangle>();
            foreach (UIElement e in canvas.Children)
            {
                if (e is Rectangle)
                    rectanglesToRemove.AddLast(e as Rectangle);
            }

            // Remove all old rectangles.
            foreach (Rectangle r in rectanglesToRemove)
                canvas.Children.Remove(r);

            // Draw new rectangles.
            foreach (ICollider collider in Colliders)
            {
                Rectangle2D r2D = collider as Rectangle2D;
                if (r2D == null)
                    continue;

                Rectangle r = new Rectangle();
                Canvas.SetLeft(r, r2D.StartX);
                Canvas.SetTop(r, r2D.StartY);
                r.Width = r2D.Width;
                r.Height = r2D.Height;
                r.Stroke = Brushes.Red;
                r.StrokeThickness = 1;

                canvas.Children.Add(r);
            }
        }

        // Makes this enemy move down.
        public void MoveDown()
        {
            // Move enemy plane image.
            Canvas.SetTop(this.EnemyImage, this.Top + this.speed);

            // Move its collider component.
            foreach (ICollider collider in Colliders)
                collider.MoveDown(this.speed);
        }

        /// <summary>
        /// Returns true if this enemy collides with (contains) the given point, otherwise, false.
        /// </summary>
        /// <param name="x">X-coordinate of the given point.</param>
        /// <param name="y">Y-coordinate of the given point.</param>
        /// <returns>True if this enemy collides with (contains) the given point, otherwise, false.</returns>
        public bool Collide(double x, double y)
        {
            // Note: Collidsion occurs if any collider contains the given point.
            foreach (ICollider collider in Colliders)
            {
                if (collider.Collide(x, y))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Plays destroy images when this enemy is dead.
        /// </summary>
        public void Destroy()
        {
            // Show next destroy image if the index is less than the length of the array.
            // Set CanBeRemoved to true when finishing showing destroy images.

            if (destroyImageIndex == destroyImages.Length)
            {
                this.CanBeRemoved = true;
                return;
            }
            else
                this.EnemyImage.Source = this.destroyImages[destroyImageIndex++];
        }
    }
}
