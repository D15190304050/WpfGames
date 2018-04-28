using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PlaneWars
{
    /// <summary>
    /// The supply class represents the supply in the game plane wars, either double-bullet or bomb.
    /// </summary>
    public class Supply
    {
        /// <summary>
        /// Gets the kind of this supply.
        /// </summary>
        public SupplyKind SupplyKind { get; }

        /// <summary>
        /// Gets the Image of this supply.
        /// </summary>
        public Image SupplyImage { get; }

        /// <summary>
        /// Rectangle collider of this supply.
        /// </summary>
        private Rectangle2D rectangleCollider;

        /// <summary>
        /// Supply image of double-bullet.
        /// </summary>
        private static readonly BitmapImage BulletSupplyImage;

        /// <summary>
        /// Supply image of bomb.
        /// </summary>
        private static readonly BitmapImage BombSupplyImage;

        /// <summary>
        /// Load BitmapImage variables only once.
        /// </summary>
        static Supply()
        {
            BulletSupplyImage = new BitmapImage(new Uri("Images/bullet_supply.png", UriKind.Relative));
            BombSupplyImage = new BitmapImage(new Uri("Images/bomb_supply.png", UriKind.Relative));
        }

        /// <summary>
        /// Initializes a new instance of the Supply class.
        /// </summary>
        /// <param name="supplyKind">Specifies whether this supply is double-bullet supply or bomb supply.</param>
        /// <param name="startX">X-coordinate of top-left point of the Image to show.</param>
        public Supply(SupplyKind supplyKind, int startX)
        {
            // Get the kind of this supply.
            this.SupplyKind = supplyKind;

            // Initialize a new Image to show.
            this.SupplyImage = new Image();

            // Set the coordinate of the image to show.
            Canvas.SetLeft(this.SupplyImage, startX);
            Canvas.SetTop(this.SupplyImage, Settings.SupplyStartY);

            // Configure image source and rectangle collider for this supply.
            if (supplyKind == SupplyKind.BulletSupply)
            {
                this.SupplyImage.Source = BulletSupplyImage;
                rectangleCollider = new Rectangle2D(startX + Settings.BulletSupplyRectangleColliderLeftOffset,
                                                    Settings.SupplyStartY + Settings.BulletSupplyRectangleColliderTopOffset,
                                                    Settings.BulletSupplyRectangleColliderWidth,
                                                    Settings.BulletSupplyRectangleColliderHeight);
            }
            else
            {
                this.SupplyImage.Source = BombSupplyImage;
                rectangleCollider = new Rectangle2D(startX + Settings.BombSupplyRectangleColliderLeftOffset,
                                                    Settings.SupplyStartY + Settings.BombSupplyRectangleColliderTopOffset,
                                                    Settings.BombSupplyRectangleColliderWidth,
                                                    Settings.BombSupplyRectangleColliderHeight);
            }
        }

        /// <summary>
        /// Makes this supply move down.
        /// </summary>
        public void MoveDown()
        {
            Canvas.SetTop(this.SupplyImage, Canvas.GetTop(this.SupplyImage) + Settings.SupplySpeed);
            rectangleCollider.MoveDown(Settings.SupplySpeed);
        }

        /// <summary>
        /// Returns true if this supply collides with (contains) the given point, otherwise, false.
        /// </summary>
        /// <param name="x">X-coordinate of the given point.</param>
        /// <param name="y">Y-coordinate of the given point.</param>
        /// <returns>True if this supply collides with (contains) the given point, otherwise, false.</returns>
        public bool Collide(double x, double y)
        {
            return rectangleCollider.Collide(x, y);
        }
    }
}
