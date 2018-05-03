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
    /// <summary>
    /// The PlayerPlane class represents the plane that player controls in the game plane wars.
    /// </summary>
    public class PlayerPlane
    {
        /// <summary>
        /// The Image of player plane.
        /// </summary>
        private Image playerImage;

        /// <summary>
        /// The image to show when player plane is alive.
        /// </summary>
        private static BitmapImage normalImage;

        /// <summary>
        /// The images to show when player plane is destroying.
        /// </summary>
        private static BitmapImage[] destroyImages;

        /// <summary>
        /// The index of next destroy image to show when this enemy is destroying.
        /// </summary>
        private int destroyImageIndex;

        /// <summary>
        /// Gets or sets the bullet kinds of player plane.
        /// </summary>
        public BulletKind BulletKind { get; set; }

        /// <summary>
        /// Gets the y-coordinate of the top-left point of player plane's image to show.
        /// </summary>
        public double Top { get { return Canvas.GetTop(playerImage); } }

        /// <summary>
        /// Gets the x-coordinate of the top-left point of player plane's image to show.
        /// </summary>
        public double Left { get { return Canvas.GetLeft(playerImage); } }

        /// <summary>
        /// Gets the width of player plane's image to show.
        /// </summary>
        public double Width { get { return playerImage.Width; } }

        /// <summary>
        /// Gets the width of player plane's image to show.
        /// </summary>
        public double Height { get { return playerImage.Height; } }

        /// <summary>
        /// Gets or sets player's HP.
        /// </summary>
        public int HP { get; set; }

        /// <summary>
        /// Gets or sets the bomb count of player.
        /// </summary>
        public int BombCount { get; set; }

        /// <summary>
        /// Gets a value that indicates whether player plane is destroying.
        /// </summary>
        public bool Destroying { get; set; }

        /// <summary>
        /// Points for collision detection.
        /// </summary>
        private List<Point2D> collisionPoints;

        /// <summary>
        /// Gets the points for collision detection.
        /// </summary>
        public IEnumerable<Point2D> CollisionPoints { get { return collisionPoints; } }

        /// <summary>
        /// Load BitmapImage variables only once.
        /// </summary>
        static PlayerPlane()
        {
            // Load normal image.
            normalImage = new BitmapImage(new Uri("Images/me1.png", UriKind.Relative));

            // Load destroy images.
            destroyImages = new BitmapImage[8];
            destroyImages[0] = new BitmapImage(new Uri("Images/me_destroy_1.png", UriKind.Relative));
            destroyImages[1] = destroyImages[0];
            destroyImages[2] = new BitmapImage(new Uri("Images/me_destroy_2.png", UriKind.Relative));
            destroyImages[3] = destroyImages[2];
            destroyImages[4] = new BitmapImage(new Uri("Images/me_destroy_3.png", UriKind.Relative));
            destroyImages[5] = destroyImages[4];
            destroyImages[6] = new BitmapImage(new Uri("Images/me_destroy_4.png", UriKind.Relative));
            destroyImages[7] = destroyImages[6];
        }

        /// <summary>
        /// Initializes a new instance of the PlayerPlane class.
        /// </summary>
        /// <param name="playerImage">The Image for player plane to show.</param>
        public PlayerPlane(Image playerImage)
        {
            // Configure player's HP, bomb count, bullet kind.
            this.HP = Settings.PlayerInitialHP;
            this.BombCount = Settings.PlayerBombMax;
            this.BulletKind = BulletKind.Bullet1;

            // Get the reference of the Image control.
            this.playerImage = playerImage;

            // Player is alive when initializing.
            this.Destroying = false;

            // Configure the source, size, coordinate of the image to show.
            playerImage.Source = normalImage;
            playerImage.Width = normalImage.Width * Settings.PlayerScaleFactor;
            playerImage.Height = normalImage.Height * Settings.PlayerScaleFactor;
            Canvas.SetTop(playerImage, Settings.PlayerTopMax);
            Canvas.SetLeft(playerImage, (Settings.PlayerLeftMin + Settings.PlayerLeftMax) / 2);

            // Generate collision points for player plane.
            collisionPoints = new List<Point2D>
            {
                new Point2D(this.Left + Settings.PlayerCollisionPoint1XOffset, this.Top + Settings.PlayerCollisionPoint1YOffset),
                new Point2D(this.Left + Settings.PlayerCollisionPoint2XOffset, this.Top + Settings.PlayerCollisionPoint2YOffset),
                new Point2D(this.Left + Settings.PlayerCollisionPoint3XOffset, this.Top + Settings.PlayerCollisionPoint3YOffset),
                new Point2D(this.Left + Settings.PlayerCollisionPoint4XOffset, this.Top + Settings.PlayerCollisionPoint4YOffset),
                new Point2D(this.Left + Settings.PlayerCollisionPoint5XOffset, this.Top + Settings.PlayerCollisionPoint5YOffset),
                new Point2D(this.Left + Settings.PlayerCollisionPoint6XOffset, this.Top + Settings.PlayerCollisionPoint6YOffset),
                new Point2D(this.Left + Settings.PlayerCollisionPoint7XOffset, this.Top + Settings.PlayerCollisionPoint7YOffset),
                new Point2D(this.Left + Settings.PlayerCollisionPoint8XOffset, this.Top + Settings.PlayerCollisionPoint8YOffset)
            };
        }

        /// <summary>
        /// Makes player plane move up.
        /// </summary>
        public void MoveUp()
        {
            double top = Canvas.GetTop(playerImage);
            double nextTop = top - Settings.PlayerSpeed;
            nextTop = nextTop >= Settings.PlayerTopMin ? nextTop : Settings.PlayerTopMin;
            Canvas.SetTop(playerImage, nextTop);

            // Update collision points after moving.
            UpdateCollisionPoints();
        }

        /// <summary>
        /// Makes player plane move down.
        /// </summary>
        public void MoveDown()
        {
            double top = Canvas.GetTop(playerImage);
            double nextTop = top + Settings.PlayerSpeed;
            nextTop = nextTop <= Settings.PlayerTopMax ? nextTop : Settings.PlayerTopMax;
            Canvas.SetTop(playerImage, nextTop);

            // Update collision points after moving.
            UpdateCollisionPoints();
        }

        /// <summary>
        /// Makes player plane move left.
        /// </summary>
        public void MoveLeft()
        {
            double left = Canvas.GetLeft(playerImage);
            double nextLeft = left - Settings.PlayerSpeed;
            nextLeft = nextLeft >= Settings.PlayerLeftMin ? nextLeft : Settings.PlayerLeftMin;
            Canvas.SetLeft(playerImage, nextLeft);

            // Update collision points after moving.
            UpdateCollisionPoints();
        }

        /// <summary>
        /// Makes player plane move right.
        /// </summary>
        public void MoveRight()
        {
            double left = Canvas.GetLeft(playerImage);
            double nextLeft = left + Settings.PlayerSpeed;
            nextLeft = nextLeft <= Settings.PlayerLeftMax ? nextLeft : Settings.PlayerLeftMax;
            Canvas.SetLeft(playerImage, nextLeft);

            // Update collision points after moving.
            UpdateCollisionPoints();
        }

        /// <summary>
        /// Updates collision points after player moves this plane.
        /// </summary>
        public void UpdateCollisionPoints()
        {
            collisionPoints[0].X = this.Left + Settings.PlayerCollisionPoint1XOffset;
            collisionPoints[0].Y = this.Top + Settings.PlayerCollisionPoint1YOffset;

            collisionPoints[1].X = this.Left + Settings.PlayerCollisionPoint2XOffset;
            collisionPoints[1].Y = this.Top + Settings.PlayerCollisionPoint2YOffset;

            collisionPoints[2].X = this.Left + Settings.PlayerCollisionPoint3XOffset;
            collisionPoints[2].Y = this.Top + Settings.PlayerCollisionPoint3YOffset;

            collisionPoints[3].X = this.Left + Settings.PlayerCollisionPoint4XOffset;
            collisionPoints[3].Y = this.Top + Settings.PlayerCollisionPoint4YOffset;

            collisionPoints[4].X = this.Left + Settings.PlayerCollisionPoint5XOffset;
            collisionPoints[4].Y = this.Top + Settings.PlayerCollisionPoint5YOffset;

            collisionPoints[5].X = this.Left + Settings.PlayerCollisionPoint6XOffset;
            collisionPoints[5].Y = this.Top + Settings.PlayerCollisionPoint6YOffset;

            collisionPoints[6].X = this.Left + Settings.PlayerCollisionPoint7XOffset;
            collisionPoints[6].Y = this.Top + Settings.PlayerCollisionPoint7YOffset;

            collisionPoints[7].X = this.Left + Settings.PlayerCollisionPoint8XOffset;
            collisionPoints[7].Y = this.Top + Settings.PlayerCollisionPoint8YOffset;
        }

        /// <summary>
        /// Player plane shoot.
        /// </summary>
        /// <param name="mainScene">The scene that this game runs.</param>
        /// <param name="bullets">The LinkedList that contains all the bullets to process in the main scene.</param>
        public void Shoot(Canvas mainScene, LinkedList<Bullet> bullets)
        {
            if (this.BulletKind == BulletKind.Bullet1)
            {
                // Calculate the top-left point of the bullet.
                double startX = this.Left + this.Width / 2;
                double startY = this.Top + Settings.BulletVerticalOffset;

                // Generate the new bullet instance.
                Bullet bullet = new Bullet(startX, startY, BulletKind.Bullet1);

                // Set its properties in the main scene.
                mainScene.Children.Add(bullet.BulletImage);
                Canvas.SetLeft(bullet.BulletImage, startX);
                Canvas.SetTop(bullet.BulletImage, startY);

                // Add it to the linked list of all bullets.
                bullets.AddLast(bullet);
            }
            else
            {
                /* Generate the bullet on the left. */

                // Calculate the top-left point of the left bullet.
                double startX = this.Left + this.Width / 2 + Settings.Bullet2LeftHorizontalOffset;
                double startY = this.Top + Settings.BulletVerticalOffset;

                // Generate the new bullet instance.
                Bullet bullet = new Bullet(startX, startY, BulletKind.Bullet2);

                // Set its properties in the main scene.
                mainScene.Children.Add(bullet.BulletImage);
                Canvas.SetLeft(bullet.BulletImage, startX);
                Canvas.SetTop(bullet.BulletImage, startY);

                // Add it to the linked list of all bullets.
                bullets.AddLast(bullet);

                /* Generate the bullet on the right. */

                // Calculate the top-left point of the right bullet.
                startX = this.Left + this.Width / 2 + Settings.Bullet2RightHorizontalOffset;
                startY = this.Top + Settings.BulletVerticalOffset;

                // Generate the new bullet instance.
                bullet = new Bullet(startX, startY, BulletKind.Bullet2);

                // Set its properties in the main scene.
                mainScene.Children.Add(bullet.BulletImage);
                Canvas.SetLeft(bullet.BulletImage, startX);
                Canvas.SetTop(bullet.BulletImage, startY);

                // Add it to the linked list of all bullets.
                bullets.AddLast(bullet);
            }
        }

        /// <summary>
        /// Plays destroy images when player is dead.
        /// </summary>
        public void Destroy()
        {
            // Show next destroy image if the index is less than the length of the array.
            // Re-initialize player plane when finishing showing destroy images.

            if (destroyImageIndex == destroyImages.Length)
            {
                // Set the index of next destroy image to show to 0.
                destroyImageIndex = 0;

                // Set the image to show.
                this.playerImage.Source = normalImage;

                // Player is alive now.
                this.Destroying = false;

                // Set its coordinate.
                Canvas.SetTop(playerImage, Settings.PlayerTopMax);
                Canvas.SetLeft(playerImage, (Settings.PlayerLeftMin + Settings.PlayerLeftMax) / 2);
            }
            else
                this.playerImage.Source = destroyImages[destroyImageIndex++];
        }
    }
}
