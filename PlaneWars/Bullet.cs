using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace PlaneWars
{
    /// <summary>
    /// The BulletBase class represents the abstract base class for Bullet1 and Bullet2. Making them hire from the same base class will make it easy to update in the game manager.
    /// </summary>
    public class Bullet
    {
        public Image BulletImage { get; }
        public double Top { get { return Canvas.GetTop(this.BulletImage); } }

        public double WarheadX { get; protected set; }
        public double WarheadY { get; protected set; }

        protected static BitmapImage Bullet1Image { get; private set; }
        protected static BitmapImage Bullet2Image { get; private set; }

        static Bullet()
        {
            Bullet.Bullet1Image = new BitmapImage(new Uri("Images/bullet1.png", UriKind.Relative));
            Bullet.Bullet2Image = new BitmapImage(new Uri("Images/bullet2.png", UriKind.Relative));
        }

        public Bullet(double startX, double startY, BulletKind bulletKind)
        {
            this.BulletImage = new Image();
            this.WarheadX = startX + Settings.BulletWarheadLeftOffset;
            this.WarheadY = startY + Settings.BulletWarheadTopOffset;

            if (bulletKind == BulletKind.Bullet1)
                this.BulletImage.Source = Bullet.Bullet1Image;
            else
                this.BulletImage.Source = Bullet.Bullet2Image;
        }

        public void MoveUp()
        {
            Canvas.SetTop(this.BulletImage, Canvas.GetTop(this.BulletImage) - Settings.BulletSpeed);
            this.WarheadY -= Settings.BulletSpeed;
        }
    }
}
