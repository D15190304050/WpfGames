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
    public class PlayerPlane
    {
        private Image playerImage;

        private BitmapImage[] normalImages;
        private BitmapImage[] destroyImages;

        private int normalImageIndex;
        private int destroyImageIndex;

        public BulletKind BulletKind { get; set; }
        public double Top { get { return Canvas.GetTop(playerImage); } }
        public double Left { get { return Canvas.GetLeft(playerImage); } }
        public double Width { get { return playerImage.Width; } }
        public double Height { get { return playerImage.Height; } }

        public int HP { get; set; }

        public int BombCount { get; set; }

        public bool CanMove { get; private set; }

        /// <summary>
        /// Points for collision detection.
        /// </summary>
        private List<Point2D> collisionPoints;

        public IEnumerable<Point2D> CollisionPoints { get { return collisionPoints; } }

        public PlayerPlane(Image playerImage)
        {
            normalImageIndex = 0;
            this.HP = Settings.PlayerInitialHP;
            this.BombCount = Settings.PlayerBombMax;
            this.BulletKind = BulletKind.Bullet1;

            this.playerImage = playerImage;
            this.CanMove = true;

            // Load normal images and show.
            normalImages = new BitmapImage[2];
            normalImages[0] = new BitmapImage(new Uri("Images/me1.png", UriKind.Relative));
            normalImages[1] = new BitmapImage(new Uri("Images/me2.png", UriKind.Relative));
            playerImage.Source = normalImages[0];
            playerImage.Width = normalImages[0].Width * Settings.PlayerScaleFactor;
            playerImage.Height = normalImages[0].Height * Settings.PlayerScaleFactor;
            Canvas.SetTop(playerImage, Settings.PlayerTopMax);
            Canvas.SetLeft(playerImage, (Settings.PlayerLeftMin + Settings.PlayerLeftMax) / 2);

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

            collisionPoints = new List<Point2D>();
            collisionPoints.Add(new Point2D(this.Left + Settings.PlayerCollisionPoint1XOffset, this.Top + Settings.PlayerCollisionPoint1YOffset));
            collisionPoints.Add(new Point2D(this.Left + Settings.PlayerCollisionPoint2XOffset, this.Top + Settings.PlayerCollisionPoint2YOffset));
            collisionPoints.Add(new Point2D(this.Left + Settings.PlayerCollisionPoint3XOffset, this.Top + Settings.PlayerCollisionPoint3YOffset));
            collisionPoints.Add(new Point2D(this.Left + Settings.PlayerCollisionPoint4XOffset, this.Top + Settings.PlayerCollisionPoint4YOffset));
            collisionPoints.Add(new Point2D(this.Left + Settings.PlayerCollisionPoint5XOffset, this.Top + Settings.PlayerCollisionPoint5YOffset));
            collisionPoints.Add(new Point2D(this.Left + Settings.PlayerCollisionPoint6XOffset, this.Top + Settings.PlayerCollisionPoint6YOffset));
            collisionPoints.Add(new Point2D(this.Left + Settings.PlayerCollisionPoint7XOffset, this.Top + Settings.PlayerCollisionPoint7YOffset));
            collisionPoints.Add(new Point2D(this.Left + Settings.PlayerCollisionPoint8XOffset, this.Top + Settings.PlayerCollisionPoint8YOffset));
        }

        public void MoveUp()
        {
            double top = Canvas.GetTop(playerImage);
            double nextTop = top - Settings.PlayerSpeed;
            nextTop = nextTop >= Settings.PlayerTopMin ? nextTop : Settings.PlayerTopMin;
            Canvas.SetTop(playerImage, nextTop);

            UpdateCollisionPoints();
        }

        public void MoveDown()
        {
            double top = Canvas.GetTop(playerImage);
            double nextTop = top + Settings.PlayerSpeed;
            nextTop = nextTop <= Settings.PlayerTopMax ? nextTop : Settings.PlayerTopMax;
            Canvas.SetTop(playerImage, nextTop);

            UpdateCollisionPoints();
        }

        public void MoveLeft()
        {
            double left = Canvas.GetLeft(playerImage);
            double nextLeft = left - Settings.PlayerSpeed;
            nextLeft = nextLeft >= Settings.PlayerLeftMin ? nextLeft : Settings.PlayerLeftMin;
            Canvas.SetLeft(playerImage, nextLeft);

            UpdateCollisionPoints();
        }

        public void MoveRight()
        {
            double left = Canvas.GetLeft(playerImage);
            double nextLeft = left + Settings.PlayerSpeed;
            nextLeft = nextLeft <= Settings.PlayerLeftMax ? nextLeft : Settings.PlayerLeftMax;
            Canvas.SetLeft(playerImage, nextLeft);

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

        public void SwitchNormalImage()
        {
            normalImageIndex++;
            normalImageIndex = normalImageIndex % normalImages.Length;
            playerImage.Source = normalImages[normalImageIndex];
        }

        public void Shoot(Canvas mainScene, LinkedList<Bullet> bullets)
        {
            if (this.BulletKind == BulletKind.Bullet1)
            {
                double startX = this.Left + this.Width / 2;
                double startY = this.Top + Settings.BulletVerticalOffset;
                Bullet bullet = new Bullet(startX, startY, BulletKind.Bullet1);
                mainScene.Children.Add(bullet.BulletImage);
                Canvas.SetLeft(bullet.BulletImage, startX);
                Canvas.SetTop(bullet.BulletImage, startY);
                bullets.AddLast(bullet);
            }
            else
            {
                double startX = this.Left + this.Width / 2 + Settings.Bullet2LeftHorizontalOffset;
                double startY = this.Top + Settings.BulletVerticalOffset;
                Bullet bullet = new Bullet(startX, startY, BulletKind.Bullet2);
                mainScene.Children.Add(bullet.BulletImage);
                Canvas.SetLeft(bullet.BulletImage, startX);
                Canvas.SetTop(bullet.BulletImage, startY);
                bullets.AddLast(bullet);

                startX = this.Left + this.Width / 2 + Settings.Bullet2RightHorizontalOffset;
                startY = this.Top + Settings.BulletVerticalOffset;
                bullet = new Bullet(startX, startY, BulletKind.Bullet2);
                mainScene.Children.Add(bullet.BulletImage);
                Canvas.SetLeft(bullet.BulletImage, startX);
                Canvas.SetTop(bullet.BulletImage, startY);
                bullets.AddLast(bullet);
            }
        }

        public void Destroy()
        {
            if (destroyImageIndex == destroyImages.Length)
            {
                destroyImageIndex = 0;
                this.playerImage.Source = this.normalImages[0];
                this.HP = 3;
            }
            else
                this.playerImage.Source = this.destroyImages[destroyImageIndex++];
        }
    }
}
