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

        private int imageIndex;
        
        private DateTime lastShootTime;

        public BulletKind BulletKind { get; set; }
        public double Top { get { return Canvas.GetTop(playerImage); } }
        public double Left { get { return Canvas.GetLeft(playerImage); } }
        public double Width { get { return playerImage.Width; } }
        public double Height { get { return playerImage.Height; } }

        public PlayerPlane(Image playerImage)
        {
            imageIndex = 0;
            this.BulletKind = BulletKind.Bullet1;
            lastShootTime = DateTime.Now;

            this.playerImage = playerImage;

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
            destroyImages = new BitmapImage[4];
            destroyImages[0] = new BitmapImage(new Uri("Images/me_destroy_1.png", UriKind.Relative));
            destroyImages[1] = new BitmapImage(new Uri("Images/me_destroy_2.png", UriKind.Relative));
            destroyImages[2] = new BitmapImage(new Uri("Images/me_destroy_3.png", UriKind.Relative));
            destroyImages[3] = new BitmapImage(new Uri("Images/me_destroy_4.png", UriKind.Relative));

        }

        public void MoveUp()
        {
            double top = Canvas.GetTop(playerImage);
            double nextTop = top - Settings.PlayerSpeed;
            nextTop = nextTop >= Settings.PlayerTopMin ? nextTop : Settings.PlayerTopMin;
            Canvas.SetTop(playerImage, nextTop);
        }

        public void MoveDown()
        {
            double top = Canvas.GetTop(playerImage);
            double nextTop = top + Settings.PlayerSpeed;
            nextTop = nextTop <= Settings.PlayerTopMax ? nextTop : Settings.PlayerTopMax;
            Canvas.SetTop(playerImage, nextTop);
        }

        public void MoveLeft()
        {
            double left = Canvas.GetLeft(playerImage);
            double nextLeft = left - Settings.PlayerSpeed;
            nextLeft = nextLeft >= Settings.PlayerLeftMin ? nextLeft : Settings.PlayerLeftMin;
            Canvas.SetLeft(playerImage, nextLeft);
        }

        public void MoveRight()
        {
            double left = Canvas.GetLeft(playerImage);
            double nextLeft = left + Settings.PlayerSpeed;
            nextLeft = nextLeft <= Settings.PlayerLeftMax ? nextLeft : Settings.PlayerLeftMax;
            Canvas.SetLeft(playerImage, nextLeft);
        }

        public void SwitchNormalImage()
        {
            imageIndex++;
            imageIndex = imageIndex % normalImages.Length;
            playerImage.Source = normalImages[imageIndex];
        }

        public void Shoot(Canvas mainScene, LinkedList<Image> bullets)
        {
            DateTime now = DateTime.Now;

            if (now - lastShootTime < Settings.PlayerShootInterval)
                return;

            lastShootTime = now;
            if (this.BulletKind == BulletKind.Bullet1)
            {
                Image bulletImage = new Image();
                bulletImage.Source = Settings.Bullet1;
                Canvas.SetLeft(bulletImage, this.Left + this.Width / 2);
                Canvas.SetTop(bulletImage, this.Top + Settings.BulletVerticalOffset);
                bullets.AddLast(bulletImage);
                mainScene.Children.Add(bulletImage);
            }
            else
            {
                Image bulletImage = new Image();
                bulletImage.Source = Settings.Bullet2;
                Canvas.SetLeft(bulletImage, this.Left + this.Width / 2 + Settings.Bullet2LeftHorizontalOffset);
                Canvas.SetTop(bulletImage, this.Top + Settings.BulletVerticalOffset);
                bullets.AddLast(bulletImage);
                mainScene.Children.Add(bulletImage);

                bulletImage = new Image();
                bulletImage.Source = Settings.Bullet2;
                Canvas.SetLeft(bulletImage, this.Left + this.Width / 2 + Settings.Bullet2RightHorizontalOffset);
                Canvas.SetTop(bulletImage, this.Top + Settings.BulletVerticalOffset);
                bullets.AddLast(bulletImage);
                mainScene.Children.Add(bulletImage);
            }
        }
    }
}
