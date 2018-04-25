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
    public abstract class BulletBase
    {
        public Image BulletImage { get; }

        public double WarheadX { get; private set; }
        public double WarheadY { get; private set; }

        public void MoveUp(double speed)
        {
            Canvas.SetTop(this.BulletImage, Canvas.GetTop(this.BulletImage) - speed);
        }


    }
}
