using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace PlaneWars
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer timer;
        private PlayerPlane player;
        private LinkedList<Image> bullets;
        private LinkedList<Enemy> enemies;

        public MainWindow()
        {
            timer = new Timer(15);
            timer.Elapsed += ElapsedHandler;
            bullets = new LinkedList<Image>();
            enemies = new LinkedList<Enemy>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            player = new PlayerPlane(playerImage);
            player.BulletKind = BulletKind.Bullet2;
            timer.Start();
        }

        private void ElapsedHandler(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(Update));
        }

        private void Update()
        {
            PlayerMove();
            player.SwitchNormalImage();
            player.Shoot(mainScene, bullets);
            BulletUpdate();
        }

        private void PlayerMove()
        {
            if (Keyboard.IsKeyDown(Key.Up) && Keyboard.IsKeyDown(Key.Left))
            {
                player.MoveUp();
                player.MoveLeft();
            }
            else if (Keyboard.IsKeyDown(Key.Up) && (Keyboard.IsKeyDown(Key.Right)))
            {
                player.MoveUp();
                player.MoveRight();
            }
            else if (Keyboard.IsKeyDown(Key.Down) && (Keyboard.IsKeyDown(Key.Left)))
            {
                player.MoveDown();
                player.MoveLeft();
            }
            else if (Keyboard.IsKeyDown(Key.Down) && (Keyboard.IsKeyDown(Key.Right)))
            {
                player.MoveDown();
                player.MoveRight();
            }
            else if (Keyboard.IsKeyDown(Key.Up))
                player.MoveUp();
            else if (Keyboard.IsKeyDown(Key.Down))
                player.MoveDown();
            else if (Keyboard.IsKeyDown(Key.Left))
                player.MoveLeft();
            else if (Keyboard.IsKeyDown(Key.Right))
                player.MoveRight();
        }

        private void BulletUpdate()
        {
            BulletMoveUp();
            RemoveOutBullets();
        }

        private void BulletMoveUp()
        {
            foreach (Image bullet in bullets)
                Canvas.SetTop(bullet, Canvas.GetTop(bullet) - Settings.BulletSpeed);
        }

        private void RemoveOutBullets()
        {
            LinkedList<Image> bulletsToRemove = new LinkedList<Image>();
            foreach (Image bullet in bullets)
            {
                if (Canvas.GetTop(bullet) < 0)
                    bulletsToRemove.AddLast(bullet);
            }

            foreach (Image bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
                mainScene.Children.Remove(bullet);
            }
        }

        private void GenerateEnemy(EnemyKind enemyKind)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
        }
    }
}
