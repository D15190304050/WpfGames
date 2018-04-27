using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PlaneWars
{
    public class Supply
    {
        public SupplyKind SupplyKind { get; }

        public Image SupplyImage { get; }

        private Rectangle2D rectangleCollider;

        private static readonly BitmapImage BulletSupplyImage;
        private static readonly BitmapImage BombSupplyImage;

        static Supply()
        {
            BulletSupplyImage = new BitmapImage(new Uri("Images/bullet_supply.png", UriKind.Relative));
            BombSupplyImage = new BitmapImage(new Uri("Images/bomb_supply.png", UriKind.Relative));
        }

        public Supply(SupplyKind supplyKind, int startX)
        {
            this.SupplyKind = supplyKind;
            this.SupplyImage = new Image();
            Canvas.SetLeft(this.SupplyImage, startX);
            Canvas.SetTop(this.SupplyImage, Settings.SupplyStartY);

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

        public void MoveDown()
        {
            Canvas.SetTop(this.SupplyImage, Canvas.GetTop(this.SupplyImage) + Settings.SupplySpeed);
            rectangleCollider.MoveDown(Settings.SupplySpeed);
        }

        public bool Collide(double x, double y)
        {
            return rectangleCollider.Collide(x, y);
        }
    }
}
