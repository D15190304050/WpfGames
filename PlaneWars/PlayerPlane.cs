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

        public BitmapImage NormalImage1 { get; private set; }
        public BitmapImage NormalImage2 { get; private set; }
        public BitmapImage[] DestroyImages { get; private set; }

        public PlayerPlane(Image playerImage)
        {
            this.playerImage = playerImage;

            // Load normal images and show.
            this.NormalImage1 = new BitmapImage(new Uri("Images/me1.png", UriKind.Relative));
            this.NormalImage2 = new BitmapImage(new Uri("Images/me2.png", UriKind.Relative));
            playerImage.Source = this.NormalImage1;
            playerImage.Width = this.NormalImage1.Width * Settings.PlayerScaleFactor;
            playerImage.Height = this.NormalImage1.Height * Settings.PlayerScaleFactor;

            // Load destroy images.
            this.DestroyImages = new BitmapImage[4];
            this.DestroyImages[0] = new BitmapImage(new Uri("Images/me_destroy_1.png", UriKind.Relative));
            this.DestroyImages[1] = new BitmapImage(new Uri("Images/me_destroy_2.png", UriKind.Relative));
            this.DestroyImages[2] = new BitmapImage(new Uri("Images/me_destroy_3.png", UriKind.Relative));
            this.DestroyImages[3] = new BitmapImage(new Uri("Images/me_destroy_4.png", UriKind.Relative));


        }

        public void MoveUp()
        {
            double top = Canvas.GetTop(playerImage);
            double nextTop = top - Settings.PlayerSpeed;
            nextTop = nextTop <= Settings.PlayerTopMin ? nextTop : Settings.PlayerTopMin;

        }
    }
}
