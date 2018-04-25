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
        private Random random;

        public MainWindow()
        {
            timer = new Timer(15);
            timer.Elapsed += ElapsedHandler;
            bullets = new LinkedList<Image>();
            enemies = new LinkedList<Enemy>();
            random = new Random();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            player = new PlayerPlane(playerImage);
            player.BulletKind = BulletKind.Bullet1;
            timer.Start();

            GenerateEnemy(EnemyKind.SmallEnemy);

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
            BulletsUpdate();
            EnemiesUpdate();
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

        private void BulletsUpdate()
        {
            BulletMoveUp();
        }

        private void BulletMoveUp()
        {
            LinkedList<Image> bulletsToRemove = new LinkedList<Image>();

            foreach (Image bullet in bullets)
            {
                Canvas.SetTop(bullet, Canvas.GetTop(bullet) - Settings.BulletSpeed);

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
            int leftMax = 0;
            switch (enemyKind)
            {
                case EnemyKind.SmallEnemy:
                    leftMax = Settings.SmallEnemyLeftMax;
                    break;
                case EnemyKind.MiddleEnemy:
                    leftMax = Settings.MiddleEnemyLeftMax;
                    break;
                case EnemyKind.LargeEnemy:
                    leftMax = Settings.LargeEnemyLeftMax;
                    break;
            }
            int startX = random.Next(Settings.EnemyLeftMin, leftMax);
            Enemy enemy = new Enemy(enemyKind, startX);
            enemies.AddLast(enemy);
            mainScene.Children.Add(enemy.EnemyImage);
        }

        private void EnemiesUpdate()
        {
            EnemiesMoveDown();
        }

        private void EnemiesMoveDown()
        {
            LinkedList<Enemy> enemiesToRemove = new LinkedList<Enemy>();

            foreach (Enemy e in enemies)
            {
                e.MoveDown();

                if (e.Top > Settings.EnemyTopMax)
                    enemiesToRemove.AddLast(e);
            }

            foreach (Enemy e in enemiesToRemove)
            {
                enemies.Remove(e);
                mainScene.Children.Remove(e.EnemyImage);
            }
        }

        private void EnemiesCollide()
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();
        }
    }
}
