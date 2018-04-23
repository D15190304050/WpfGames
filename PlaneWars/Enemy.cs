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
    public abstract class Enemy
    {
        protected Image enemyImage;
        public int HP { get; set; }
        private double speed;
        private EnemyKind enemyKind;
        private BitmapImage[] destroyImages;

        public double Top { get { return Canvas.GetTop(enemyImage); } }

        protected static BitmapImage SmallEnemyImage { get; private set; }
        protected static BitmapImage[] SmallEnemyDestroyImages { get; private set; }

        protected static BitmapImage MiddleEnemyImage { get; private set; }

        protected static BitmapImage[] MiddleEnemyDestroyImages { get; private set; }

        protected static BitmapImage LargeEnemyImage { get; private set; }

        protected static BitmapImage[] LargeEnemyDestroyImages { get; private set; }

        static Enemy()
        {
            Enemy.SmallEnemyImage = new BitmapImage(new Uri("Images/enemy1.png", UriKind.Relative));

            Enemy.SmallEnemyDestroyImages = new BitmapImage[4];
            Enemy.SmallEnemyDestroyImages[0] = new BitmapImage(new Uri("Images/enemy1_down1.png", UriKind.Relative));
            Enemy.SmallEnemyDestroyImages[1] = new BitmapImage(new Uri("Images/enemy1_down2.png", UriKind.Relative));
            Enemy.SmallEnemyDestroyImages[2] = new BitmapImage(new Uri("Images/enemy1_down3.png", UriKind.Relative));
            Enemy.SmallEnemyDestroyImages[3] = new BitmapImage(new Uri("Images/enemy1_down4.png", UriKind.Relative));

            Enemy.MiddleEnemyImage = new BitmapImage(new Uri("Images/enemy2.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages = new BitmapImage[4];
            Enemy.MiddleEnemyDestroyImages[0] = new BitmapImage(new Uri("Images/enemy2_down1.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages[1] = new BitmapImage(new Uri("Images/enemy2_down2.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages[2] = new BitmapImage(new Uri("Images/enemy2_down3.png", UriKind.Relative));
            Enemy.MiddleEnemyDestroyImages[3] = new BitmapImage(new Uri("Images/enemy2_down4.png", UriKind.Relative));

            Enemy.LargeEnemyImage = new BitmapImage(new Uri("Images/enemy3.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[0] = new BitmapImage(new Uri("Images/enemy3_down1.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[1] = new BitmapImage(new Uri("Images/enemy3_down2.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[2] = new BitmapImage(new Uri("Images/enemy3_down3.png", UriKind.Relative));
            Enemy.LargeEnemyDestroyImages[3] = new BitmapImage(new Uri("Images/enemy3_down4.png", UriKind.Relative));
        }

        public Enemy(EnemyKind enemyKind, double startX)
        {
            if (enemyKind == EnemyKind.SmallEnemy)
            {
                destroyImages = SmallEnemyDestroyImages;
                enemyImage.Source = SmallEnemyImage;
                speed = Settings.EnemyInitialSpeed;
                this.HP = Settings.SmallEnemyInitialHP;
            }
            else if (enemyKind == EnemyKind.MiddleEnemy)
            {
                destroyImages = MiddleEnemyDestroyImages;
                enemyImage.Source = MiddleEnemyImage;
                speed = Settings.EnemyInitialSpeed;
                this.HP = Settings.MiddleEnemyInitialHP;
            }
            else
            {
                destroyImages = LargeEnemyDestroyImages;
                enemyImage.Source = LargeEnemyImage;
                speed = Settings.EnemyInitialSpeed;
                this.HP = Settings.LargeEnemyInitialHP;
            }

            this.enemyKind = enemyKind;
            Canvas.SetLeft(enemyImage, startX);
            Canvas.SetTop(enemyImage, Settings.EnemyStartY);
        }

        public void MoveDown()
        {
            Canvas.SetTop(enemyImage, this.Top + speed);
        }

        public void Update()
        {

        }
    }
}
