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
        /// <summary>
        /// Gets the image of this bullet to show.
        /// </summary>
        public Image BulletImage { get; }

        /// <summary>
        /// Returns the y-coordinate of the top-left point of the image to show.
        /// </summary>
        public double Top { get { return Canvas.GetTop(this.BulletImage); } }

        /// <summary>
        /// Gets the x-coordinate of the warhead of this bullet.
        /// </summary>
        public double WarheadX { get; protected set; }

        /// <summary>
        /// Gets the y-coordinate of the warhead of this bullet.
        /// </summary>
        public double WarheadY { get; protected set; }

        /// <summary>
        /// Image of bullet 1.
        /// </summary>
        protected static BitmapImage Bullet1Image { get; private set; }

        /// <summary>
        /// Image of bullet 2.
        /// </summary>
        protected static BitmapImage Bullet2Image { get; private set; }

        //// <summary>
        /// Load BitmapImage variables only once.
        /// </summary>
        static Bullet()
        {
            Bullet.Bullet1Image = new BitmapImage(new Uri("Images/bullet1.png", UriKind.Relative));
            Bullet.Bullet2Image = new BitmapImage(new Uri("Images/bullet2.png", UriKind.Relative));
        }

        /// <summary>
        /// Initializes a new instance of the Bullet class with given top-left point coordinate and bullet kind.
        /// </summary>
        /// <param name="startX">X-coordinate of the top-left point of the bullet image to show.</param>
        /// <param name="startY">Y-coordinate of the top-left point of the bullet image to show.<</param>
        /// <param name="bulletKind">Specifies which kind of bullet it is.</param>
        public Bullet(double startX, double startY, BulletKind bulletKind)
        {
            // Initialize the image to show.
            this.BulletImage = new Image();

            // Calculate the coordiate of the warhead.
            this.WarheadX = startX + Settings.BulletWarheadLeftOffset;
            this.WarheadY = startY + Settings.BulletWarheadTopOffset;

            // Save the bullet kind.
            if (bulletKind == BulletKind.Bullet1)
                this.BulletImage.Source = Bullet.Bullet1Image;
            else
                this.BulletImage.Source = Bullet.Bullet2Image;
        }

        /// <summary>
        /// Makes this bullet move up.
        /// </summary>
        public void MoveUp()
        {
            Canvas.SetTop(this.BulletImage, Canvas.GetTop(this.BulletImage) - Settings.BulletSpeed);
            this.WarheadY -= Settings.BulletSpeed;
        }
    }
}
